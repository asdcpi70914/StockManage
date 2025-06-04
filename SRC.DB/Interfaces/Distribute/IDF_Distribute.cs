using SRC.DB.HardCodes;
using SRC.DB.Models.Complex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRC.DB.Interfaces.Distribute
{
    public interface IDF_Distribute
    {
        List<UnitApplyComplex> ListUnitApplyComplex(string type, long? setting_pid, long? subscribepoint_pid, DateTime? start_time, DateTime? end_time, string state, int? page, int? take, out int rowtotal);

        UnitApplyComplex GetDistributeUnitApply(long pid);

        Task Distribute(long pid, string account);
    }
}
