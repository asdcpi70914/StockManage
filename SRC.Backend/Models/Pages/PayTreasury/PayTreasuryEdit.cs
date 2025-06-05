using SRC.Backend.Models.System;

namespace SRC.Backend.Models.Pages.PayTreasury
{
    public class PayTreasuryEdit
    {
        public ActionMode Action { get; set; }

        public long pid { get; set; }

        public int pay_amount { get; set; }
        public int already_pay_amount { get; set; }

        public int apply_amount { get; set; }

        public int stock { get; set; }
    }
}
