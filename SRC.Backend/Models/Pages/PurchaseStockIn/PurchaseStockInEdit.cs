using SRC.Backend.Models.System;

namespace SRC.Backend.Models.Pages.PurchaseStockIn
{
    public class PurchaseStockInEdit
    {
        public ActionMode Action { get; set; }
        public long pid { get; set; }
        public int stock { get; set; }
        public int old_stock { get; set; }
    }
}
