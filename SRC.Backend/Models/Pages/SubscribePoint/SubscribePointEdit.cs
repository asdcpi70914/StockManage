using SRC.Backend.Models.System;

namespace SRC.Backend.Models.Pages.SubscribePoint
{
    public class SubscribePointEdit
    {
        public ActionMode Action { get; set; }
        public long pid { get; set; }
        public string name { get; set; }
    }
}
