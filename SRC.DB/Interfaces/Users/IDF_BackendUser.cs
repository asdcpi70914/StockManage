using SRC.DB.Models.EFMSSQL;

namespace SRC.DB.Interfaces.Users
{
    public interface IDF_BackendUser
    {
        string InnerMessage { get; }

        List<backend_user> SearchRoleInUser(string account, string userName, string email, int? page, int? take, out int rowCount);

        bool Create(backend_user user);
        bool UpdatePassword(string account, Guid userID, string oldPwd, string newPwd);
        bool UpdateInfo(long pid, backend_user newInfo);

        Guid? ForgetPassword(string account, string email, string newPwd);
        backend_user? GetUser(long pid);
        List<backend_user> ListUser(List<long> pid);
        backend_user? GetUser(Guid uuid);
        backend_user? GetUser(string Account);
        backend_user? GetUserByAdAccount(string ADAccount);

        Task<bool> ExistUser(string Account);

        Task<List<backend_users_role>> GetUserRolesMap(Guid userId);

        bool GetUserByLoginPwd(string account, string passwordHash, out backend_user user);

        bool IncrementAccountFailCount(string account);
        bool ResetAccountFailCount(string account);
        bool UnlockAccount(string account);
        bool LockAccount(string account);

        bool ResetPassword(long pid, string newPasswordHash, string editor);

        List<backend_user> Query(string? account, string? email, string? name, string? phoneNumber, bool? enable, int? page, int? take, out int rowCount);

        bool Delete(List<long> pids, string account);

        backend_dept GetBackendDept(long backend_user_pid);

        List<backend_user> ListBackendUserForUnit(long unit);

        #region Backend_Unit
        List<backend_unit> List_Backend_Unit();
        #endregion

        List<backend_user> AllBackendUser();

        List<backend_user> ListBackUserByAccount(List<string> Account);
    }
}
