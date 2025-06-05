namespace SRC.Backend.Models.Pages.PurchaseStockIn
{
    public class PurchaseStockInIndex
    {
        public class SearchModel
        {
            public string type { get; set; }
            public long? pid { get; set; }
            public DateTime? start_time { get; set; }
            public DateTime? end_time { get; set; }
        }

        public PurchaseStockInSearch SearchResultPage { get; set; }
    }
}
