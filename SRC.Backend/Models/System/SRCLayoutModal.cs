namespace SRC.Backend.Models.System
{
    public class SRCLayoutModal
    {
        public string formId { get; set; }

        public bool IsDivPage { get; set; }

        public string Action { get; set; }
        public string Controller { get; set; }

        public string Enctype { get; set; }

        public string MaxModelWidth { get; set; }

        public string Title { get; set; }

        public string View { get; set; }

        public object ViewObject { get; set; }

        public bool HideHeader { get; set; }
        public bool NoAutoHide { get; set; }

        public bool NoSaveButton { get; set; }
        public bool NoCancelButton { get; set; }
        public bool HasCloseButton { get; set; }
    }
}
