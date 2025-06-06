using SRC.DB.Models.EFMSSQL;
using Microsoft.EntityFrameworkCore;
using SRC.DB.Abstract;
using SRC.DB.Interfaces.Users;
using SRC.DB.Responsibility.Components;

namespace SRC.DB.Responsibility.Users
{
    public class DF_BackendUser : ADF, IDF_BackendUser
    {
        public DF_BackendUser(IDataBaseFactory dataBaseService, EFContext db)
            : base(dataBaseService, db)
        {
        }

        //public (string ResCode, backend_user? Data) Login(string account, string passwordHash)
        public bool GetUserByLoginPwd(string account, string passwordHash, out backend_user? user)
        {
            user = null;
            List<backend_user> users = DB.backend_users.Where(m => m.account == account).ToList();

            if (users.Count != 1)
            {
                return false;
            }

            user = users.First();

            if (user.password_hash != passwordHash)
            {
                return false;
            }


            return true;
        }


        public bool Create(backend_user user, backend_dept dept)
        {

            if (DB.backend_users.Where(m => m.account == user.account).Count() > 0)
            {
                throw new Exception("帳號已存在");
            }

            if (!string.IsNullOrWhiteSpace(user.ad_account) && DB.backend_users.Where(x => x.ad_account == user.ad_account).Count() > 0)
            {
                throw new Exception("AD連動帳號已存在");
            }

            backend_user newUser = new backend_user()
            {
                user_id = user.user_id,
                account = user.account,
                ad_account = user.ad_account,
                password_hash = user.password_hash,
                name_ch = user.name_ch,
                name_en = user.name_en,
                email = user.email,
                phone_number = user.phone_number,
                enabled = user.enabled,
                unit = user.unit,
                creator = user.creator,
                create_time = DateTime.Now,
                person_in_charge = user.person_in_charge
            };

            DB.backend_users.Add(newUser);

            DB.SaveChanges();

            return true;
        }

        public bool UpdatePassword(string account, Guid userID, string oldPwd, string newPwd)
        {

            backend_user? user =
                DB.backend_users
                .AsTracking()
                .Where(
                    m =>
                    m.account == account
                    && m.user_id == userID
                    && m.password_hash == oldPwd
                    ).FirstOrDefault();

            if (user == null) return false;

            user.password_hash = newPwd;

            DB.SaveChanges();

            return true;
        }


        public bool UpdateInfo(long pid, backend_user newInfo, backend_dept backend_Dept)
        {
            backend_user? user =
               DB.backend_users
               .Where(
                   m =>
                   m.pid == pid
                   ).FirstOrDefault();

            if (user == null) throw new Exception("查無使用者資料");

            if (!string.IsNullOrWhiteSpace(newInfo.ad_account) && DB.backend_users.Where(x => x.ad_account == newInfo.ad_account && x.pid != user.pid).Count() > 0)
            {
                throw new Exception("AD連動帳號已存在");
            }

            backend_dept? dept = DB.backend_depts.Where(x => x.backend_user_pid == pid).FirstOrDefault();

            if (dept != null)
            {
                DB.backend_depts.Remove(dept);
            }

            user.name_en = newInfo.name_en;
            user.name_ch = newInfo.name_ch;
            user.phone_number = newInfo.phone_number;
            user.email = newInfo.email;
            user.unit = newInfo.unit;
            user.enabled = newInfo.enabled;
            user.editor = newInfo.editor;
            user.edit_time = DateTime.Now;
            user.ad_account = newInfo.ad_account;
            user.person_in_charge = newInfo.person_in_charge;

            DB.SaveChanges();

            return true;

        }


        public Guid? ForgetPassword(string account, string email, string newPwd)
        {
            backend_user? user =
               DB.backend_users
               .Where(
                   m =>
                   m.account == account
                   && m.email == email
                   ).FirstOrDefault();

            if (user == null) return null;

            user.password_hash = newPwd;

            DB.SaveChanges();

            return user.user_id;
        }


        public backend_user? GetUser(long pid)
        {
            IQueryable<backend_user>? form = DB.backend_users;

            return form.Where(m => m.pid == pid).FirstOrDefault();
        }

        public List<backend_user> ListUser(List<long> pid)
        {
            return DB.backend_users.AsNoTracking().Where(x => pid.Contains(x.pid)).ToList();
        }
        public backend_user? GetUser(Guid uuid)
        {
            IQueryable<backend_user>? form = DB.backend_users;

            return form.Where(m => m.user_id == uuid).FirstOrDefault();
        }

        public backend_user? GetUser(string account)
        {
            IQueryable<backend_user>? form = DB.backend_users;

            return form.Where(x => x.account == account).FirstOrDefault();
        }

        public backend_user? GetUserByAdAccount(string ADAccount)
        {
            IQueryable<backend_user>? form = DB.backend_users;

            return form.Where(x => x.ad_account == ADAccount).FirstOrDefault();
        }

        public async Task<bool> ExistUser(string Account)
        {
            int count = await DB.backend_users.AsNoTracking().Where(m => m.account == Account).CountAsync();

            return count > 0;
        }

        public bool IncrementAccountFailCount(string account)
        {
            List<backend_user> users = DB.backend_users.Where(m => m.account == account).ToList();

            if (users.Count != 1) return false;

            backend_user user = users.First();

            user.access_failed_count += 1;

            DB.SaveChanges();

            return true;
        }

        public bool ResetAccountFailCount(string account)
        {
            List<backend_user> users = DB.backend_users.Where(m => m.account == account).ToList();

            if (users.Count != 1) return false;

            backend_user user = users.First();

            user.access_failed_count = 0;

            DB.SaveChanges();

            return true;
        }

        public bool UnlockAccount(string account)
        {
            List<backend_user> users = DB.backend_users.Where(m => m.account == account).ToList();

            if (users.Count != 1) return false;

            backend_user user = users.First();

            user.access_failed_count = 0;
            user.lockout_end = null;

            DB.SaveChanges();

            return true;
        }

        public bool LockAccount(string account)
        {
            List<backend_user> users = DB.backend_users.Where(m => m.account == account).ToList();

            if (users.Count != 1) return false;

            backend_user user = users.First();

            user.lockout_end = DateTime.Now;

            DB.SaveChanges();

            return true;
        }

        public bool ResetPassword(long pid, string newPasswordHash, string editor)
        {
            backend_user user = DB.backend_users.Where(m => m.pid == pid).FirstOrDefault();

            if (user == null) throw new Exception("使用者不存在");

            user.password_hash = newPasswordHash;
            user.editor = editor;
            user.edit_time = DateTime.Now;

            DB.SaveChanges();
            return true;
        }

        public List<backend_user> Query(string account, string email, string name, string phoneNumber, bool? enable, int? page, int? take, out int rowCount)
        {
            IQueryable<backend_user> searchUsers = DB.backend_users;


            if (!string.IsNullOrWhiteSpace(account))
            {
                searchUsers = searchUsers.Where(m => !string.IsNullOrWhiteSpace(m.account) && m.account.Contains(account));
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                searchUsers = searchUsers.Where(m => !string.IsNullOrWhiteSpace(m.name_ch) && m.name_ch.Contains(name));
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                searchUsers = searchUsers.Where(m => !string.IsNullOrWhiteSpace(m.email) && m.email.Contains(email));
            }

            if (!string.IsNullOrWhiteSpace(phoneNumber))
            {
                searchUsers = searchUsers.Where(m => !string.IsNullOrWhiteSpace(m.phone_number) && m.phone_number.Contains(phoneNumber));
            }

            if (enable.HasValue)
            {
                //sbyte enval = (sbyte)(enable.Value ? 1 : 0);
                searchUsers = searchUsers.Where(m => m.enabled == enable.Value);
            }

            return Pagination(searchUsers, page, take, out rowCount);
        }




        public bool Delete(List<long> pids, string account)
        {


            foreach (var each in pids)
            {

                backend_user user = DB.backend_users.Where(m => m.pid == each).FirstOrDefault();

                if (user == null)
                {
                    throw new Exception("帳號資料已被異動，請新操作");
                }

                if (user.account.ToLower() == "admin")
                {
                    throw new Exception("Admin系統管理員帳號無法刪除");
                }



                DB.backend_users_dels.Add(new backend_users_del()
                {
                    user_id = user.user_id,
                    backend_user_pid = user.pid,
                    state = user.state,
                    first_login = user.first_login,
                    access_failed_count = user.access_failed_count,
                    device_code = user.device_code,
                    apply_date = user.apply_date,
                    account = account,
                    changed_password_time = user.changed_password_time,
                    create_time = user.create_time,
                    creator = user.creator,
                    editor = account,
                    edit_time = user.edit_time,
                    email = user.email,
                    email_confirmed = user.email_confirmed,
                    email_confirmed_time = user.email_confirmed_time,
                    enabled = user.enabled,
                    first_login_time = user.first_login_time,
                    jwt_code = user.jwt_code,
                    limit_time = user.limit_time,
                    lockout_end = user.lockout_end,
                    name_ch = user.name_ch,
                    name_en = user.name_en,
                    password_hash = user.password_hash,
                    phone_number = user.phone_number,
                    unit = user.unit,
                    verification_code = user.verification_code,
                });

                List<backend_users_role>
                    toRemoveRoleRelate =
                    DB.backend_users_roles.Where(m => m.user_id == user.user_id).ToList();

                if (toRemoveRoleRelate.Count > 0)
                {
                    DB.backend_users_roles.RemoveRange(toRemoveRoleRelate);
                }

                List<backend_dept> toRemoveDept = DB.backend_depts.Where(x => x.backend_user_pid == user.pid).ToList();

                if (toRemoveDept.Count() > 0)
                {
                    DB.backend_depts.RemoveRange(toRemoveDept);
                }

                //取得backend_dept 的 parent_id是自己的

                List<backend_dept> toUpdateDept = DB.backend_depts.Where(x => x.parent_pid == user.pid).ToList();

                foreach (var item in toUpdateDept)
                {
                    item.parent_pid = null;
                }

                DB.backend_depts.UpdateRange(toUpdateDept);

                DB.backend_users.Remove(user);
            }


            DB.SaveChanges();

            return true;

        }

        public List<backend_user> SearchRoleInUser(string account, string userName, string email, int? page, int? take, out int rowCount)
        {
            IQueryable<backend_user> preQry = DB.backend_users
                .Include(m => m.backend_users_roles)
                .ThenInclude(m => m.role).AsNoTracking();

            if (!string.IsNullOrWhiteSpace(account))
            {
                preQry = preQry.Where(m => m.account.Contains(account));
            }

            if (!string.IsNullOrWhiteSpace(userName))
            {
                preQry = preQry.Where(m => m.name_ch.Contains(userName));
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                preQry = preQry.Where(m => m.email.Contains(email));
            }

            return Pagination(preQry, page, take, out rowCount);
        }

        public async Task<List<backend_users_role>> GetUserRolesMap(Guid userId)
        {
            return await DB.backend_users_roles.AsNoTracking().Where(m => m.user_id == userId).ToListAsync();
        }

        public backend_dept GetBackendDept(long backend_user_pid)
        {
            return DB.backend_depts.AsNoTracking().Where(x => x.backend_user_pid == backend_user_pid).FirstOrDefault();
        }

        public List<backend_unit> List_Backend_Unit()
        {
            return DB.backend_units.AsNoTracking().ToList();
        }

        public List<backend_user> ListBackendUserForUnit(string unit)
        {
            return DB.backend_users.AsNoTracking().Where(x => x.unit == unit).ToList();
        }

        public List<backend_user> AllBackendUser()
        {
            return DB.backend_users.AsNoTracking().ToList();
        }

        public List<backend_user> ListBackUserByAccount(List<string> Account)
        {
            return DB.backend_users.AsNoTracking().Where(x => Account.Contains(x.account)).ToList();
        }
    }
}
