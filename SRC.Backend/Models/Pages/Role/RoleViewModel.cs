using SRC.Backend.Models.System;
using SRC.DB.Models.Complex;

namespace SRC.Backend.Models.Pages.Role
{
    public class RoleViewModel
    {
        public List<Permission> RoleList { get; set; }

        public RoleSearch SearchPage { get; set; }
    }

    public class RoleQuery
    {
        public string name { get; set; }
        public int? pid { get; set; }
    }

    public class RoleAdd
    {
        public ActionMode Action { get; set; }
        public string Name { get; set; }
    }

    public class RoleEdit
    {
        public int pid { get; set; }
        public string Name { get; set; }
    }

    public class RoleDelete
    {
        public int pid { get; set; }
    }

    public class RoleFunc
    {
        public int RolePID { get; set; }
        public List<MemberItem> UserOwnRole { get; set; }

        public string FuncListForJson { get; set; }
    }

    public class RoleFuncAdd
    {
        public int RolePID { get; set; }
        public IList<Guid> membsPID { get; set; }

        public IList<int> funcsPID { get; set; }
    }

}
