namespace SRC.Backend.Models.Pages.UnitUseReport
{
    public class UnitUseReportIndex
    {
        public class SearchModel
        {
            public string type { get; set; }
            public long? sub_pid { get; set; }
            public string unit { get; set; }
            public DateTime? start_time { get; set; }
            public DateTime? end_time { get; set; }
        }

        public UnitUseReportSearch SearchResultPage { get; set; }
    }
}
