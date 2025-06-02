using SRC.Backend.Models.System;

namespace SRC.Backend.Models.Pages.Equipment
{
    public class MaterialEdit
    {
        public ActionMode Action { get; set; }
        public long pid { get; set; }
        public string name { get; set; }
        public decimal? price { get; set; }
        public int? stock { get; set; }

        public string state { get; set; }
    }
}
