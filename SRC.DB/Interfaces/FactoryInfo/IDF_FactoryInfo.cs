using SRC.DB.Models.EFMSSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRC.DB.Interfaces.FactoryInfo
{
    public interface IDF_FactoryInfo
    {
        List<factoryinfo_maintain> ListFactoryInfo(string name, string city, string town, string address, DateTime? start_time, DateTime? end_time, int? page, int? take, out int rowtotal);

        factoryinfo_maintain GetFactoryInfo(long pid);

        Task Create(factoryinfo_maintain data);

        Task Edit(factoryinfo_maintain data);

        Task Delete(List<long> pids);
    }
}
