using SRC.DB.Models.EFMSSQL;

namespace SRC.DB.Models.Complex
{
    public class RoleFuncMember
    {
        public int RoleID { get; set; }

        public IList<role_func> Funcs { get; set; }

        public IList<Guid> MembersID { get; set; }
    }
}
