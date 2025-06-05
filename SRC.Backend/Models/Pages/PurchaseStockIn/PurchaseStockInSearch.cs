using SRC.Backend.Models.System;
using SRC.DB.Models.EFMSSQL;

namespace SRC.Backend.Models.Pages.PurchaseStockIn
{
    public class PurchaseStockInSearch
    {
        public List<equipment_maintain> data { get; set; }
        public SRCUIPagination Pagination { get; set; }
    }
}
