namespace SRC.Backend.Models.System
{
    public class SRCPageMessage
    {

        /// <summary>
        /// 目標ID(引動後傳遞用)
        /// </summary>
        public string TargetID { get; set; }
        public bool IsSuccess { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string JsonData { get; set; }

    }
}
