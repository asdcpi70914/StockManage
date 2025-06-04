using SRC.Backend.Models.System;
using SRC.DB.Models.Complex;
using SRC.DB.Models.EFMSSQL;

namespace SRC.Backend.Models.Pages.UnitApply
{
    public class UnitApplySearch
    {
        public List<UnitApplyComplex> data { get; set; } = new List<UnitApplyComplex>();
        public SRCUIPagination Pagination { get; set; } = new SRCUIPagination();
    }
}
