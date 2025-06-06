using SRC.DB.Models.EFMSSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRC.DB.Interfaces.ExistingStock
{
    public interface IDF_ExistingStock
    {
        List<equipment_maintain> ListEquipmentMaintain(string name, DateTime? start_time, DateTime? end_time, int? page, int? take,out int rowtotal);
    }
}
