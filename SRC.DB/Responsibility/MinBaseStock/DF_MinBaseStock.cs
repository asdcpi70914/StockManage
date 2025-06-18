using Microsoft.EntityFrameworkCore;
using SRC.DB.Abstract;
using SRC.DB.HardCodes;
using SRC.DB.Interfaces.MinBaseStock;
using SRC.DB.Models.Complex;
using SRC.DB.Models.EFMSSQL;
using SRC.DB.Responsibility.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRC.DB.Responsibility.MinBaseStock
{
    public class DF_MinBaseStock :ADF, IDF_MinBaseStock
    {
        public DF_MinBaseStock(IDataBaseFactory dataBaseService, EFContext db) : base(dataBaseService, db)
        {
        }

        public List<MinBaseStockComplex> ListMinBaseStockSubscribeSetting(string type, long? sub_pid, long? subscribepoint_pid, DateTime? start_time, DateTime? end_time, int? page, int? take, out int rowtotal)
        {
            IQueryable<min_base_stock_subscribe_setting> data = DB.min_base_stock_subscribe_settings.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(type))
            {
                data = data.Where(x => x.type == type);
            }

            if (sub_pid.HasValue)
            {
                data = data.Where(x => x.sub_pid == sub_pid.Value);
            }

            if (subscribepoint_pid.HasValue)
            {
                data = data.Where(x => x.subscribepoint_pid == subscribepoint_pid.Value);
            }

            if (start_time.HasValue)
            {
                data = data.Where(x => x.create_time >= start_time.Value);
            }

            if (end_time.HasValue)
            {
                data = data.Where(x => x.create_time <= end_time.Value.AddDays(1).AddSeconds(-1));
            }

            var query = Pagination(data, page, take, out rowtotal);

            List<MinBaseStockComplex> result = new List<MinBaseStockComplex>();

            var subscribepoint = DB.subscribepoint_maintains.AsNoTracking().Where(x => query.Select(x => x.subscribepoint_pid).ToList().Contains(x.pid)).ToList();
            var EquipmentList = query.Where(x => x.type == MINBASESTOCK_TYPE.STATE.EQUIPMENT.ToString()).Select(x => x.sub_pid).ToList();
            var equipment = DB.equipment_maintains.AsNoTracking().AsNoTracking().Where(x => EquipmentList.Contains(x.pid)).ToList();
            var MaterialList = query.Where(x => x.type == MINBASESTOCK_TYPE.STATE.MATERIAL.ToString()).Select(x => x.sub_pid).ToList();
            var material = DB.equipment_maintains.AsNoTracking().AsNoTracking().Where(x => MaterialList.Contains(x.pid)).ToList();

            result = query.Select(x => new MinBaseStockComplex()
            {
                pid = x.pid,
                create_time = x.create_time,
                sub_name = x.type == MINBASESTOCK_TYPE.STATE.EQUIPMENT.ToString() ? equipment.Where(m => m.pid == x.sub_pid).FirstOrDefault()?.name : material.Where(m => m.pid == x.sub_pid).FirstOrDefault()?.name,
                min_base_stock = x.min_base_stock,
                subscribepoint = subscribepoint.Where(m => m.pid == x.subscribepoint_pid).FirstOrDefault()?.name,
                type = x.type,
                stock = x.stock

            }).ToList();

            return result;
        }

        public min_base_stock_subscribe_setting GetMinBaseStockSubscribeSetting(long pid)
        {
            return DB.min_base_stock_subscribe_settings.AsNoTracking().Where(x => x.pid == pid).FirstOrDefault();
        }

        public Dictionary<long, string> MinBaseStockSubscribeSettingDropDown(string type)
        {

            if(!string.IsNullOrWhiteSpace(type))
            {
                return DB.equipment_maintains.AsNoTracking().Where(x => x.type == type && x.state != EQUIPMENT_STATE.STATE.INVALID.ToString()).ToDictionary(x => x.pid, x => x.name);
            }
            else
            {
                return new Dictionary<long, string>();
            }
        }

        public async Task Create(min_base_stock_subscribe_setting data)
        {
            await DB.min_base_stock_subscribe_settings.AddAsync(data);

            await DB.SaveChangesAsync();
        }

        public async Task Edit(min_base_stock_subscribe_setting data)
        {
            var editData = await DB.min_base_stock_subscribe_settings.Where(x => x.pid == data.pid).FirstOrDefaultAsync();

            if(editData == null)
            {
                throw new Exception($"查無基準存量與申購點設定編輯資料，pid：{data.pid}");
            }

            editData.type = data.type;
            editData.sub_pid = data.sub_pid;
            editData.subscribepoint_pid = data.subscribepoint_pid;
            editData.min_base_stock = data.min_base_stock;
            editData.editor = data.editor;
            editData.edit_time = data.edit_time;

            await DB.SaveChangesAsync();
        }

        public async Task Delete(List<long> pids)
        {
            var deleteData = await DB.min_base_stock_subscribe_settings.Where(x => pids.Contains(x.pid)).ToListAsync();

            DB.min_base_stock_subscribe_settings.RemoveRange(deleteData);

            await DB.SaveChangesAsync();
        }

        public bool CheckSameData(long? pid, string type,long sub_pid, long subscribepoint_pid)
        {
            if (pid.HasValue)
            {
                return DB.min_base_stock_subscribe_settings.AsNoTracking().Where(x => x.pid != pid.Value && x.type == type && x.sub_pid == sub_pid && x.subscribepoint_pid == subscribepoint_pid).Any();
            }
            else
            {
                return DB.min_base_stock_subscribe_settings.AsNoTracking().Where(x => x.type == type && x.sub_pid == sub_pid && x.subscribepoint_pid == subscribepoint_pid).Any();
            }
            
        }

        public List<min_base_stock_subscribe_setting> ListStockSetting(List<long> pids)
        {
            return DB.min_base_stock_subscribe_settings.AsNoTracking().Where(x => pids.Contains(x.sub_pid)).ToList();
        }
    }
}
