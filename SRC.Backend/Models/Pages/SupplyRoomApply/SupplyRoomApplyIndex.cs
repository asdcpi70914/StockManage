

namespace SRC.Backend.Models.Pages.SupplyRoomApply
{
    public class SupplyRoomApplyIndex
    {
        public class SearchModel
        {
            public string type { get; set; }
            public long? sub_pid { get; set; }
            public long? subscribepoint_pid { get; set; }
            public string state { get; set; }
            public DateTime? start_time { get; set; }
            public DateTime? end_time { get; set; }
        }

        public Dictionary<long, string> subscribepointDic { get; set; }

        public SupplyRoomApplySearch SearchResultPage { get; set; }
    }
}
