using SRC.Backend.Models.System;
using SRC.DB.Models.EFMSSQL;

namespace SRC.Backend.Models.Pages.BackendUser
{
    public class BackendUserSearch
    {
        public List<backend_user> Users { get; set; }
        public SRCUIPagination Pagination { get; set; }
    }
}
