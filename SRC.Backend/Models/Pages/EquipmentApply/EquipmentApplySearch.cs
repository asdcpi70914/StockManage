using SRC.Backend.Models.System;
using SRC.DB.Models.Complex;

namespace SRC.Backend.Models.Pages.EquipmentApply
{
    public class EquipmentApplySearch
    {
        public List<UnitApplyComplex> data { get; set; } = new List<UnitApplyComplex>();
        public SRCUIPagination Pagination { get; set; } = new SRCUIPagination();
    }
}
