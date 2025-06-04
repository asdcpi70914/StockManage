using SRC.DB.Models.Complex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRC.DB.Interfaces.Delivery
{
    public interface IDF_Delivery
    {
        List<UnitApplyComplex> ListUnitApplyComplex(string type, long? setting_pid, long? subscribepoint_pid, DateTime? start_time, DateTime? end_time, List<string> state, int? page, int? take, out int rowtotal);

        UnitApplyComplex GetDeliveryUnitApply(long pid);

        Task Delivery(long pid, string account);
    }
}
