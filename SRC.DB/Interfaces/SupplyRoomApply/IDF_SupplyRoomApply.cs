using SRC.DB.Models.Complex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRC.DB.Interfaces.SupplyRoomApply
{
    public interface IDF_SupplyRoomApply
    {
        List<UnitApplyComplex> ListUnitApplyComplex(string type, long? sub_pid, long? subscribepoint_pid, DateTime? start_time, DateTime? end_time, string state, int? page, int? take, out int rowtotal);

        UnitApplyComplex GetDistributeUnitApply(long pid);

        Task Edit(long pid, string account);

        Task Cancel(List<long> pids,string account);
    }
}
