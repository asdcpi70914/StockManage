using Dapper;
using SRC.DB.Models.Complex;
using SRC.DB.Models.EFMSSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRC.DB.Interfaces.Authority
{
    public interface IDF_Role
    {
        string InnerMessage { get; }
        List<Permission> SearchRole(string? Role_Name, int? pid, int? page, int? take, out int rowsCount);
        Task<List<role>> ListRole();
        Task<func> GetFunc(string name);
        List<string> GetRoleProgramCode(Guid userID);
        Task<List<MemberItem>> SearchBackendUserInRole(int Role_ID);

        string GetRoleFuncFull(int? roleID);

        string GetRoleFuncsJson(string type, int? RolePID);
        Task<role> GetRole(int rolePid);

        bool AddRole(role data);

        Task<bool> EditRole(int pid, string newName, string editor, DateTime editTime);

        Task<bool> DeleteRole(List<int> pid, string userAccount);

        Task EditFunc(int RolePID, IList<Guid> userGuid, IList<int> Funcs);
        Task UpdateRoleInUser(Guid userId, List<int> userRoles);

        void AddRoleFunc(List<role_func> Data);
    }
}
