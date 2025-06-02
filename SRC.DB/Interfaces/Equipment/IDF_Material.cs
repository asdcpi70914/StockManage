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
        List<material_maintain> SearchMaterialMaintain(string name, int? price, DateTime? start_time, DateTime? end_time, string state, int? page, int? take, out int rowtotal);

        material_maintain GetMaterialMaintain(long pid);

        bool CheckSameName(long? pid, string Name);

        Task Create(material_maintain data);

        Task Edit(material_maintain data);

        Task Delete(List<long> pids, string account);
    }
}
