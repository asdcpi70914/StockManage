using Azure;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Mysqlx.Crud;
using SRC.DB.Abstract;
using SRC.DB.Interfaces.Authority;
using SRC.DB.Models.Complex;
using SRC.DB.Models.EFMSSQL;
using SRC.DB.Models.Funcs;
using SRC.DB.Responsibility.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Mysqlx.Crud.Find.Types;
using static SRC.DB.HardCodes.ROLE_TYPE;

namespace SRC.DB.Responsibility.Authority
{
    public class DF_Role : ADF, IDF_Role
    {
        public DF_Role(IDataBaseFactory dataBaseService, EFContext db) : base(dataBaseService, db)
        {
        }

        public List<string> GetRoleProgramCode(Guid userID)
        {
            List<int> rolesID = DB.backend_users_roles.Where(m => m.user_id == userID).Select(m => m.role_id).ToList();

            return DB.roles.Where(m => rolesID.Contains(m.pid) && !string.IsNullOrWhiteSpace(m.programe_code)).Select(m => m.programe_code).ToList();

        }

        public async Task<List<MemberItem>> SearchBackendUserInRole(int RolePID)
        {
            List<backend_users_role> mbRoleJson = await DB.backend_users_roles.AsNoTracking().Where(m => m.role_id == RolePID).ToListAsync();
            //await QueryMemberRoleByRoleId(RolePID);

            IList<Guid> selectedUsersPid = mbRoleJson.Select(m => m.user_id).ToList();

            List<backend_user> users = await DB.backend_users.AsNoTracking().ToListAsync();

            List<MemberItem> backendUsers = new List<MemberItem>();

            foreach (var each in users)
            {
                backendUsers.Add(new MemberItem()
                {
                    account = each.account,
                    name_ch = each.name_ch,
                    user_id = each.user_id,
                    pid = each.pid,
                    IsSelected = selectedUsersPid.Where(m => m == each.user_id).Count() > 0
                }); ;
            }

            return backendUsers;
        }

        public string GetRoleFuncsJson(string type, int? RolePID)
        {
            string qryResult = GetRoleFuncFull(RolePID);

            IList<SRCMenu> funcItems = JsonSerializer.Deserialize<IList<SRCMenu>>(qryResult);

            IList<RoleFuncFull> parsingResult = ParseFuncItems(type, funcItems);

            return JsonSerializer.Serialize(parsingResult);

        }


        public string GetRoleFuncFull(int? roleID)
        {
            //var action = DataBaseService
            //.Use("usp_get_role_func_all")
            //.Add(m => m.Model("@role_id", roleID))
            //.Execute<RoleFuncFull>();

            //return JsonSerializer.Serialize(action.Data);

            DynamicParameters prms = new DynamicParameters();
            StringBuilder sql = new StringBuilder();
            System.Data.Common.DbConnection conn = DB.Database.GetDbConnection();
            sql.AppendLine("select rf.* ,");

            sql.AppendLine("f.pid , f.[name] , f.[url] , f.parentid , f.[type] , f.icon , f.[weight],");

            sql.AppendLine("case");
            sql.AppendLine("when rf.func_id is null then 0");

            sql.AppendLine("else 1");

            sql.AppendLine("end as isChecked");

            sql.AppendLine("from");
            sql.AppendLine("(");
            sql.AppendLine(" select func_id, role_id from Role_Func with (nolock)");
            sql.AppendLine("                         where role_id = @roleID");

            sql.AppendLine(") as rf");

            sql.AppendLine("right join Func f with(nolock)");

            sql.AppendLine(" on rf.func_id = f.pid");

            sql.AppendLine("where f.[state] = 1");

            sql.AppendLine("order by f.[weight] asc");

            prms.Add("@roleID", roleID);

            List<RoleFuncFull> PermissionList = Pagination<RoleFuncFull>(
               sql.ToString(),"", prms,null,null,out int rowtotal);


            return JsonSerializer.Serialize(PermissionList);



        }

        private IList<RoleFuncFull> ParseFuncItems(string type, IList<SRCMenu> funcItems)
        {
            //string type = "menu";
            SRCMenu parserFunc = new SRCMenu();
            funcItems = parserFunc.ParseTypeItems(type, funcItems, true);

            IList<RoleFuncFull> result = JsonSerializer.Deserialize<IList<RoleFuncFull>>(JsonSerializer.Serialize(funcItems));

            return result;
        }



        //將以下語法改為MYSQL
        public List<Permission> SearchRole(string? Role_Name, int? pid, int? page, int? take, out int rowsCount)
        {
            DynamicParameters prms = new DynamicParameters();
            StringBuilder sql = new StringBuilder();
            System.Data.Common.DbConnection conn = DB.Database.GetDbConnection();
            sql.AppendLine("Select pid");
            sql.AppendLine(",pid as Role_Id");
            sql.AppendLine(",name as Role_Name");
            sql.AppendLine("From Role");
            sql.AppendLine("Where State = 1");

            if (!string.IsNullOrWhiteSpace(Role_Name))
            {
                sql.AppendLine("And name Like @Role_Name");
                prms.Add("@Role_Name", "%" + Role_Name + "%");
            }

            if (pid.HasValue)
            {
                sql.AppendLine("And pid = @pid");
                prms.Add("@pid", pid);
            }

            List<Permission> PermissionList = Pagination<Permission>(
                sql.ToString(), " order by create_time ", prms, page, take, out rowsCount);// conn.Query<Permission>(sql.ToString(), prms).ToList();

            return PermissionList;
        }

        public async Task<role> GetRole(int rolePid)
        {
            return await DB.roles.Where(m => m.pid == rolePid).FirstOrDefaultAsync();
        }
        public bool AddRole(role data)
        {
            DB.roles.Add(data);

            DB.SaveChanges();

            return true;
        }

        public async Task<bool> EditRole(int pid, string newName, string editor, DateTime editTime)
        {
            List<role> roles = await DB.roles.Where(m => m.pid == pid).ToListAsync();

            if (roles == null) throw new Exception("查無角色資料");

            if (roles.Count > 1)
            {
                InnerMessage = "角色名稱重複";
                return false;
            }
            role role = roles.First();
            role.name = newName;
            role.editor = editor;
            role.edit_time = editTime;

            DB.SaveChanges();

            return true;
        }

        public async Task<bool> DeleteRole(List<int> pid, string userAccount)
        {

            if (DB.backend_users_roles.Where(m => pid.Contains(m.role_id)).Count() > 0)
            {
                InnerMessage = "角色使用中無法刪除";
                return false;
            }

            //role role = DB.roles.Where(m => pid.Contains(m.pid) && m.state == 1).FirstOrDefault();

            //if (role == null)
            //{
            //    InnerMessage = "角色已被刪除";
            //    return false;
            //}
            foreach (var each in pid)
            {
                role role = DB.roles.Where(m => m.pid == each).FirstOrDefault();
                role_del backup = new role_del()
                {
                    role_pid = role.pid,
                    create_time = role.create_time,
                    creator = role.creator,
                    editor = userAccount,
                    edit_time = DateTime.Now,
                    name = role.name,
                    programe_code = role.programe_code,
                    state = role.state
                };

                DB.role_dels.Add(backup);
                DB.roles.Remove(role);
            }


            await DB.SaveChangesAsync();

            return true;

        }

        public async Task EditFunc(int RolePID, IList<Guid> userGuid, IList<int> Funcs)
        {

            List<role_func> toDelFuncs = await DB.role_funcs.Where(m => m.role_id == RolePID).ToListAsync();

            DB.role_funcs.RemoveRange(toDelFuncs);

            //func_id , role_id , [weight]

            List<role_func> newFuncs = new List<role_func>();
            foreach (var each in Funcs)
            {
                newFuncs.Add(new role_func()
                {
                    func_id = each,
                    role_id = RolePID,
                    weight = 10
                });
            }

            await DB.role_funcs.AddRangeAsync(newFuncs);

            List<backend_users_role> toDelUserRole = await DB.backend_users_roles.Where(m => m.role_id == RolePID).ToListAsync();

            DB.backend_users_roles.RemoveRange(toDelUserRole);

            if (userGuid != null)
            {
                List<backend_users_role> newUserRole = new List<backend_users_role>();
                foreach (var each in userGuid)
                {
                    newUserRole.Add(new backend_users_role()
                    {
                        role_id = RolePID,
                        user_id = each
                    });
                }

                DB.backend_users_roles.AddRange(newUserRole);
            }

            await DB.SaveChangesAsync();


            //RoleFuncMember beAddData = new RoleFuncMember()
            //    {
            //        RoleID = RolePID,
            //        MembersID = membsPID,
            //        Funcs = new List<role_func>()
            //    };

            //    if (Funcs != null)
            //    {
            //        foreach (int eachFuncPid in Funcs)
            //        {
            //            beAddData.Funcs.Add(
            //                new role_func()
            //                {
            //                    func_id = eachFuncPid,
            //                    weight = 0
            //                }
            //                );
            //        }
            //    }



            //    DataTable funcs = new DataTable();
            //    funcs.Columns.Add("PID");
            //    funcs.Columns.Add("weight");
            //    if (beAddData != null)
            //    {
            //        foreach (role_func eachFunc in beAddData.Funcs)
            //        {
            //            DataRow dr = funcs.NewRow();
            //            dr["PID"] = eachFunc.func_id;
            //            dr["weight"] = eachFunc.weight;
            //            funcs.Rows.Add(dr);
            //        }
            //    }

            //    DataTable membersId = new DataTable();
            //    membersId.Columns.Add("PID");

            //    if (beAddData.MembersID != null)
            //    {
            //        foreach (Guid eachGuid in beAddData.MembersID)
            //        {
            //            DataRow dr = membersId.NewRow();
            //            dr["PID"] = eachGuid;
            //            membersId.Rows.Add(dr);
            //        }
            //    }


            //    var action = DataBaseService
            //    .Use("usp_role_func_member_insertDelete")
            //    .Add(m => m.Model("@rolePid", RolePID))
            //    .Add(m => m.Model("@funcs", funcs))
            //    .Add(m => m.Model("@membersId", membersId))
            //    .Execute();

            //return true;

        }

        public async Task<List<role>> ListRole()
        {
            return await DB.roles.AsNoTracking().ToListAsync();
        }

        public async Task<func> GetFunc(string name)
        {
            return await DB.funcs.Where(x => x.name == name).FirstOrDefaultAsync();
        }

        public async Task UpdateRoleInUser(Guid userId, List<int> userRoles)
        {

            List<backend_users_role> toDel = await DB.backend_users_roles.Where(m => m.user_id == userId).ToListAsync();

            DB.backend_users_roles.RemoveRange(toDel);

            List<backend_users_role> newRoles = new List<backend_users_role>();
            foreach (var each in userRoles)
            {
                newRoles.Add(new backend_users_role()
                {
                    user_id = userId,
                    role_id = each
                });
            }

            await DB.backend_users_roles.AddRangeAsync(newRoles);

            await DB.SaveChangesAsync();

        }

        public void AddRoleFunc(List<role_func> Data)
        {
            DB.role_funcs.AddRange(Data);

            DB.SaveChanges();
        }
    }
}
