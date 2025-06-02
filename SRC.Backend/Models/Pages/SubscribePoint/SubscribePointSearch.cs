using SRC.Backend.Models.System;
using SRC.DB.Models.EFMSSQL;

namespace SRC.Backend.Models.Pages.SubscribePoint
{
    public class SubscribePointSearch
    {
        public List<subscribepoint_maintain> Data { get; set; }
        public SRCUIPagination Pagination { get; set; }

    }
}
