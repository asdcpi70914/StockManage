namespace SRC.Backend.Models.Pages.PayTreasury
{
    public class PayTreasuryIndex
    {
        public class SearchModel
        {
            public string type { get; set; }
            public long? sub_pid { get; set; }
            public long? subscribepoint_pid { get; set; }
            public DateTime? start_time { get; set; }
            public DateTime? end_time { get; set; }
        }

        public Dictionary<long, string> subscribepointDic { get; set; }
        public PayTreasurySearch SearchResultPage { get; set; }
    }
}
