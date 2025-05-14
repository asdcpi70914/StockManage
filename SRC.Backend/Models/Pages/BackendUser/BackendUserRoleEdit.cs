namespace SRC.Backend.Models.Pages.BackendUser
{
    public class BackendUserRoleEdit
    {
        public class UserRole
        {
            public int RoleId { get; set; }
            public string RoleName { get; set; }
            public bool IsSelected { get; set; }
        }

        public Guid UserPid { get; set; }
        public List<UserRole> UserRoles { get; set; }
    }
}
