

namespace SRC.Backend.Models.Pages.Delivery
{
    public class DeliveryIndex
    {
        public class SearchModel
        {
            public string type { get; set; }
            public long? sub_pid { get; set; }
            public long? subscribepoint_pid { get; set; }
            public List<string> state { get; set; }
            public DateTime? start_time { get; set; }
            public DateTime? end_time { get; set; }
        }

        public Dictionary<long, string> subscribepointDic { get; set; }

        public DeliverySearch SearchResultPage { get; set; }
    }
}
