using SRC.Backend.Models.System;

namespace SRC.Backend.Models.Pages.BackendUser
{
    public class BackendUserRoleSearch
    {
        public class SearchView
        {
            public Guid UserID { get; set; }
            public string Account { get; set; }
            public string Email { get; set; }
            public string UserName { get; set; }
            public List<string> RoleNameList { get; set; }
        }

        public List<SearchView> Users { get; set; }
        public SRCUIPagination Pagination { get; set; }
    }
}
