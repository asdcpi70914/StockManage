using SRC.DB.Models.Complex;
using SRC.DB.Models.EFMSSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRC.DB.Interfaces.PurchaseStockIn
{
    public interface IDF_PurchaseStockIn
    {
        List<equipment_maintain> ListPurchaseStockIn(string type, long? sub_pid, DateTime? start_time, DateTime? end_time, int? page, int? take, out int rowtotal);

        equipment_maintain GetPurchaseStockIn(long pid);

        Task StockIn(long pid, int stock, string account);
    }
}
