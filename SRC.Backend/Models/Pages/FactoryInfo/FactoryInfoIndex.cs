namespace SRC.Backend.Models.Pages.FactoryInfo
{
    public class FactoryInfoIndex
    {
        public class SearchModel
        {
            public string name { get; set; }
            public string city {  get; set; }
            public string town {  get; set; }
            public string address { get; set; }
            public DateTime? start_time { get; set; }
            public DateTime? end_time { get; set; }
        }

        public FactoryInfoSearch SearchResultPage { get; set; }
    }
}
