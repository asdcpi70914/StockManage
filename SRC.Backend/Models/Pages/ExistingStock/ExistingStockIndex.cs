namespace SRC.Backend.Models.Pages.ExistingStock
{
    public class ExistingStockIndex
    {
        public class SearchModel
        {
            public string name { get; set; }
            public DateTime? start_time { get; set; }
            public DateTime? end_time { get; set; }

        }

        public ExistingStockSearch SearchResultPage { get; set; }
    }
}
