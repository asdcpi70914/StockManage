using SRC.Backend.Models.System;
using SRC.DB.Models.EFMSSQL;

namespace SRC.Backend.Models.Pages.Equipment
{
    public class MaterialSearch
    {
        public List<material_maintain> Data { get; set; }
        public SRCUIPagination Pagination { get; set; }
    }
}
