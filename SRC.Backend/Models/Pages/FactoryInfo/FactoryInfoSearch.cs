using SRC.Backend.Models.System;
using SRC.DB.Models.EFMSSQL;

namespace SRC.Backend.Models.Pages.FactoryInfo
{
    public class FactoryInfoSearch
    {
        public List<factoryinfo_maintain> Data { get; set; }
        public SRCUIPagination Pagination { get; set; }
    }
}
