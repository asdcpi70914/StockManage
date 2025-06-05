using SRC.DB.Models.EFMSSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRC.DB.Interfaces.Equipment
{
    public interface IDF_Material
    {
        List<equipment_maintain> SearchMaterialMaintain(string name, int? price, DateTime? start_time, DateTime? end_time, string state, int? page, int? take, out int rowtotal);

        equipment_maintain GetMaterialMaintain(long pid);

        bool CheckSameName(long? pid, string Name);

        Task Create(equipment_maintain data);

        Task Edit(equipment_maintain data);

        Task Delete(List<long> pids, string account);
    }
}
