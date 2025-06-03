using SRC.Backend.Models.System;
using SRC.DB.Models.Complex;
using SRC.DB.Models.EFMSSQL;

namespace SRC.Backend.Models.Pages.MinBaseStock
{
    public class MinBaseStockSearch
    {
        public List<MinBaseStockComplex> Data { get; set; }
        public SRCUIPagination Pagination { get; set; }

        public Dictionary<long, string> subscribepointDic { get; set; }
    }
}
