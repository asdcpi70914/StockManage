using Microsoft.EntityFrameworkCore;
using SRC.DB.Abstract;
using SRC.DB.HardCodes;
using SRC.DB.Interfaces.ExistingStock;
using SRC.DB.Models.EFMSSQL;
using SRC.DB.Responsibility.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRC.DB.Responsibility.ExistingStock
{
    public class DF_ExistingStock:ADF, IDF_ExistingStock
    {
        public DF_ExistingStock(IDataBaseFactory dataBaseService, EFContext db) : base(dataBaseService, db)
        {
        }

        public List<equipment_maintain> ListEquipmentMaintain(string name, DateTime? start_time, DateTime? end_time, int? page, int? take, out int rowtotal)
        {
            IQueryable<equipment_maintain> data = DB.equipment_maintains.AsNoTracking().Where(x => x.state == EQUIPMENT_STATE.STATE.ENABLE.ToString());

            if (!string.IsNullOrWhiteSpace(name))
            {
                data = data.Where(x => x.name.Contains(name));
            }

            if (start_time.HasValue)
            {
                data = data.Where(x => x.create_time >= start_time.Value);
            }

            if (end_time.HasValue)
            {
                data = data.Where(x => x.create_time <= end_time.Value.AddDays(1).AddSeconds(-1));
            }

            return Pagination(data, page, take, out rowtotal);
        }
    }
}
