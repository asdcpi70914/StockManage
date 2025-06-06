using SRC.Backend.Models.System;
using SRC.DB.Models.EFMSSQL;

namespace SRC.Backend.Models.Pages.ExistingStock
{
    public class ExistingStockSearch
    {
        public List<equipment_maintain> Data { get; set; }
        public SRCUIPagination Pagination { get; set; }
    }
}
