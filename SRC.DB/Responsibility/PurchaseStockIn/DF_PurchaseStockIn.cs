using Microsoft.EntityFrameworkCore;
using SRC.DB.Abstract;
using SRC.DB.HardCodes;
using SRC.DB.Interfaces.PurchaseStockIn;
using SRC.DB.Models.Complex;
using SRC.DB.Models.EFMSSQL;
using SRC.DB.Responsibility.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRC.DB.Responsibility.PurchaseStockIn
{
    public class DF_PurchaseStockIn : ADF, IDF_PurchaseStockIn
    {
        public DF_PurchaseStockIn(IDataBaseFactory dataBaseService, EFContext db) : base(dataBaseService, db)
        {
        }

        public List<equipment_maintain> ListPurchaseStockIn(string type, long? pid, DateTime? start_time, DateTime? end_time, int? page, int? take, out int rowtotal)
        {
            IQueryable<equipment_maintain> data = DB.equipment_maintains.AsNoTracking().Where(x => x.state == EQUIPMENT_STATE.STATE.ENABLE.ToString());

            if (!string.IsNullOrWhiteSpace(type))
            {
                data = data.Where(x => x.type == type);
            }

            if (pid.HasValue)
            {
                data = data.Where(x => x.pid == pid.Value);
            }

            if (start_time.HasValue)
            {
                data = data.Where(x => x.create_time >= start_time.Value);
            }

            if (end_time.HasValue)
            {
                data = data.Where(x => x.create_time <= end_time.Value.AddDays(1).AddSeconds(-1));
            }

            return Pagination(data,page,take,out rowtotal);
        }

        public equipment_maintain GetPurchaseStockIn(long pid)
        {
            return DB.equipment_maintains.AsNoTracking().Where(x => x.pid == pid).FirstOrDefault();
        }

        public async Task StockIn(long pid, int stock, string account)
        {
            var data = await DB.equipment_maintains.Where(x => x.pid == pid).FirstOrDefaultAsync();

            if(data == null)
            {
                throw new Exception($"查無此裝備/器材資料，pid：{pid}");
            }

            stockin_log log = new stockin_log()
            {
                equipment_pid = pid,
                old_stock = data.stock,
                new_stock  = data.stock + stock,
                create_time = DateTime.Now,
                creator = account
            };

            data.stock = data.stock + stock;
            data.editor = account;
            data.edit_time = DateTime.Now;

            await DB.SaveChangesAsync();
        }
    }
}
