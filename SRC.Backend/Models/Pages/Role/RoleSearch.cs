using SRC.Backend.Models.System;
using SRC.DB.Models.Complex;

namespace SRC.Backend.Models.Pages.Role
{
    public class RoleSearch
    {
        public List<Permission> RoleList { get; set; }

        public SRCUIPagination Pagination { get; set; }
    }
}
