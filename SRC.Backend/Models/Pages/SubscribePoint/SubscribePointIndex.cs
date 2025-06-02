namespace SRC.Backend.Models.Pages.SubscribePoint
{
    public class SubscribePointIndex
    {
        public class SearchModel
        {
            public string name { get; set; }
            public DateTime? start_time { get; set; }
            public DateTime? end_time { get; set; }

        }

        public SubscribePointSearch SearchResultModel { get; set; }
    }
}
