using SRC.Backend.Models.System;

namespace SRC.Backend.Models.Pages.FactoryInfo
{
    public class FactoryInfoEdit
    {
        public ActionMode Action { get;set; }
        public long pid { get; set; }
        public string name { get; set; }
        public string company_phone { get; set; }
        public string company_number { get; set; }
        public string city { get; set; }
        public string town { get; set; }
        public string address { get; set; }

        public Dictionary<string, string> Towns { get; set; }
    }
}
