using SRC.Backend.Models.System;
using SRC.DB.Models.Complex;

namespace SRC.Backend.Models.Pages.PayTreasury
{
    public class PayTreasurySearch
    {
        public List<PayTreasuryComplex> Data { get; set; }
        public SRCUIPagination Pagination { get; set; } = new SRCUIPagination();
    }
}
