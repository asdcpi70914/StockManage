namespace SRC.Backend.Models.System
{

    public class SRCTools
    {
        public string id { get; set; }
        public string mainkey { get; set; }
        public string maininfo { get; set; }
        public string type { get; set; }

        public string icon { get; set; }

        public string iconOut { get; set; }

        public string customClass { get; set; }
        public string AuthorityUrl { get; set; }

        public string UserFuncList { get; set; }
        public SRCUIAuthority btnAuth { get; set; }

        public string Dismiss = "modal";

        public string Title { get; set; }
        public string Desc { get; set; }

        //public string Style { get; set; }
        public string btnClass { get; set; }

        public string Url { get; set; }

        public bool IgnoreAuthUrl { get; set; }

        public string ConfirmPageModelID { get; set; }

        public SRCTools()
        {
            type = "button";
        }

        public void InitToButton()
        {
            type = "button";
        }

        public void InitToSubmit()
        {
            type = "submit";
        }
    }
}
