using Microsoft.EntityFrameworkCore;
using SRC.DB.Abstract;
using SRC.DB.HardCodes;
using SRC.DB.Interfaces.Equipment;
using SRC.DB.Interfaces.Settings;
using SRC.DB.Models.EFMSSQL;
using SRC.DB.Responsibility.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRC.DB.Responsibility.Equipment
{
    public class DF_Equipment : ADF, IDF_Equipment
    {
        public DF_Equipment(IDataBaseFactory dataBaseService, EFContext db) : base(dataBaseService, db)
        {
        }

        public List<equipment_maintain> SearchEquipmentMaintain(string name, int? price, DateTime? start_time, DateTime? end_time, string state, int? page, int? take, out int rowtotal)
        {
            IQueryable<equipment_maintain> data = DB.equipment_maintains.AsNoTracking().Where(x => x.type == MINBASESTOCK_TYPE.STATE.EQUIPMENT.ToString());

            if (!string.IsNullOrWhiteSpace(name))
            {
                data = data.Where(x => x.name.Contains(name));
            }

            if (price.HasValue)
            {
                data = data.Where(x => x.price == price.Value);
            }

            if (start_time.HasValue)
            {
                data = data.Where(x => x.create_time >= start_time.Value);
            }

            if (end_time.HasValue)
            {
                data = data.Where(x => x.create_time <= end_time.Value.AddDays(1).AddSeconds(-1));
            }

            if (!string.IsNullOrWhiteSpace(state))
            {
                data = data.Where(x => x.state == state);
            }

            return Pagination<equipment_maintain>(data, page, take,out rowtotal);
        }

        public equipment_maintain GetEquipmentMaintain(long pid)
        {
            return DB.equipment_maintains.AsNoTracking().Where(x => x.pid == pid).FirstOrDefault();
        }

        public bool CheckSameName(long? pid, string Name)
        {
            if (pid.HasValue)
            {
                return DB.equipment_maintains.AsNoTracking().Where(x => x.pid != pid.Value && x.name == Name && x.type == MINBASESTOCK_TYPE.STATE.EQUIPMENT.ToString()).Any();
            }
            else
            {
                return DB.equipment_maintains.AsNoTracking().Where(x => x.name == Name && x.type == MINBASESTOCK_TYPE.STATE.EQUIPMENT.ToString()).Any();
            }
        }

        public async Task Create(equipment_maintain data)
        {
            await DB.equipment_maintains.AddAsync(data);
            await DB.SaveChangesAsync();
        }

        public async Task Edit(equipment_maintain data)
        {
            var editData = await DB.equipment_maintains.Where(x => x.pid == data.pid).FirstOrDefaultAsync();

            if(editData == null)
            {
                throw new Exception($"查無編輯資料，pid：{data.pid}");
            }

            editData.name = data.name;
            //editData.stock = data.stock;
            editData.price = data.price;
            editData.editor = data.editor;
            editData.edit_time = data.edit_time;

            await DB.SaveChangesAsync();
        }

        public async Task Delete(List<long> pids, string account)
        {
            var Data = await DB.equipment_maintains.Where(x => pids.Contains(x.pid)).ToListAsync();

            foreach(var each in Data)
            {
                each.state = EQUIPMENT_STATE.STATE.INVALID.ToString();
                each.editor = account;
                each.edit_time = DateTime.Now;
            }

            await DB.SaveChangesAsync();
        }
    }
}
