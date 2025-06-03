using SRC.DB.Models.EFMSSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRC.DB.Interfaces.SubscribePoint
{
    public interface IDF_SubscribePoint
    {
        List<subscribepoint_maintain> ListSubscribePoint(string name, DateTime? start_time, DateTime? end_time, int? page, int? take, out int rowtotal);
        subscribepoint_maintain GetSubscribePoint(long pid);
        Task Create(subscribepoint_maintain data);
        Task Edit(subscribepoint_maintain data);
        bool CheckSameName(long? pid, string name);
        Task Delete(List<long> pids);
        Dictionary<long, string> SubscribepointDic();
    }
}
