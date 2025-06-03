using SRC.DB.Models.Complex;
using SRC.DB.Models.EFMSSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRC.DB.Interfaces.MinBaseStock
{
    public interface IDF_MinBaseStock
    {
        List<MinBaseStockComplex> ListMinBaseStockSubscribeSetting(string type, long? sub_pid, long? subscribepoint_pid, DateTime? start_time, DateTime? end_time,int? page, int? take, out int rowtotal);

        min_base_stock_subscribe_setting GetMinBaseStockSubscribeSetting(long pid);

        Dictionary<long, string> MinBaseStockSubscribeSettingDropDown(string type);

        Task Create(min_base_stock_subscribe_setting data);

        Task Edit(min_base_stock_subscribe_setting data);

        Task Delete(List<long> pids);

        bool CheckSameData(long? pid,string type,long sub_pid, long subscribepoint_pid);
    }
}
