using SRC.Backend.Models.System;

namespace SRC.Backend.Models.Pages.MinBaseStock
{
    public class MinBaseStockEdit
    {
        public ActionMode Action { get; set; }
        public long? pid { get; set; }
        public string type { get; set; }
        public long? sub_pid { get; set; }
        public long? subscribepoint_pid { get; set; }
        public int? min_base_stock { get; set; }
    }
}
