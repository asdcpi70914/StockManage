using SRC.Backend.Models.System;

namespace SRC.Backend.Models.Pages.UnitApply
{
    public class UnitApplyEdit
    {
        public ActionMode Action { get; set; }
        public long pid { get; set; }
        public long? setting_pid { get; set; }
        public long subscribepoint { get; set; }
        public int RemainingStock { get; set; }
        public int apply_amount { get; set; }
        public string type { get; set; }
    }
}
