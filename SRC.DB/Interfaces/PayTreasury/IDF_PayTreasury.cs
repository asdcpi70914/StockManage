using SRC.DB.Models.Complex;
using SRC.DB.Models.EFMSSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRC.DB.Interfaces.PayTreasury
{
    public interface IDF_PayTreasury
    {
        List<PayTreasuryComplex> ListPayTreasuryComplex(string type, long? setting_pid, long? subscribepoint_pid, DateTime? start_time, DateTime? end_time, int? page, int? take, out int rowtotal);

        PayTreasuryComplex GetPayTreasury(long pid);

        Task Edit(long pid, int pay_amount, string account);
    }
}
