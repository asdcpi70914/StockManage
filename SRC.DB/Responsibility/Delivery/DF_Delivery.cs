using Microsoft.EntityFrameworkCore;
using SRC.DB.Abstract;
using SRC.DB.HardCodes;
using SRC.DB.Interfaces.Delivery;
using SRC.DB.Models.Complex;
using SRC.DB.Models.EFMSSQL;
using SRC.DB.Responsibility.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRC.DB.Responsibility.Delivery
{
    public class DF_Delivery:ADF, IDF_Delivery
    {
        public DF_Delivery(IDataBaseFactory dataBaseService, EFContext db) : base(dataBaseService, db)
        {
        }

        public List<UnitApplyComplex> ListUnitApplyComplex(string type, long? setting_pid, long? subscribepoint_pid, DateTime? start_time, DateTime? end_time, List<string> state, int? page, int? take, out int rowtotal)
        {
            IQueryable<unit_apply> data = DB.unit_applies.AsNoTracking();
            IQueryable<min_base_stock_subscribe_setting> query = DB.min_base_stock_subscribe_settings.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(type))
            {
                query = query.Where(x => x.type == type);
            }

            if (setting_pid.HasValue)
            {
                query = query.Where(x => x.pid == setting_pid);
            }

            if (subscribepoint_pid.HasValue)
            {
                query = query.Where(x => x.subscribepoint_pid == subscribepoint_pid);
            }

            var queryResults = query.ToList();

            if (start_time.HasValue)
            {
                data = data.Where(x => x.create_time >= start_time.Value);
            }

            if (end_time.HasValue)
            {
                data = data.Where(x => x.create_time <= end_time.Value.AddDays(1).AddSeconds(-1));
            }

            if (state != null && state.Count() > 0)
            {
                data = data.Where(x => state.Contains(x.state));
            }

            var queryResultPids = queryResults.Select(x => x.pid).ToList();
            data = data.Where(x => queryResultPids.Contains(x.setting_pid));

            var dataResult = Pagination(data, page, take, out rowtotal);

            List<UnitApplyComplex> result = new List<UnitApplyComplex>();

            var subscribepointList = query.Select(x => x.subscribepoint_pid).ToList();

            var subscribepoint = DB.subscribepoint_maintains.AsNoTracking().Where(x => subscribepointList.Contains(x.pid)).ToList();
            var EquipmentList = query.Where(x => x.type == MINBASESTOCK_TYPE.STATE.EQUIPMENT.ToString()).Select(x => x.sub_pid).ToList();
            var equipment = DB.equipment_maintains.AsNoTracking().AsNoTracking().Where(x => EquipmentList.Contains(x.pid)).ToList();
            var MaterialList = query.Where(x => x.type == MINBASESTOCK_TYPE.STATE.MATERIAL.ToString()).Select(x => x.sub_pid).ToList();
            var material = DB.equipment_maintains.AsNoTracking().AsNoTracking().Where(x => MaterialList.Contains(x.pid)).ToList();
            var AccountList = dataResult.Select(x => x.creator).ToList();
            var Users = DB.backend_users.AsNoTracking().Where(x => AccountList.Contains(x.account)).ToList();

            var SystemCodeList = DB.system_codes.AsNoTracking().Where(x => x.code == "UNIT").ToList();

            foreach (var each in dataResult)
            {
                var queryResult = queryResults.Where(x => x.pid == each.setting_pid).FirstOrDefault();
                var User = Users.Where(x => x.account == each.creator).FirstOrDefault();
                var SystemCode = SystemCodeList.Where(x => x.data == each.unit).FirstOrDefault();
                result.Add(new UnitApplyComplex()
                {
                    pid = each.pid,
                    subscribepoint = subscribepoint.Where(m => m.pid == queryResult?.subscribepoint_pid).FirstOrDefault()?.name,
                    sub_name = queryResult?.type == MINBASESTOCK_TYPE.STATE.EQUIPMENT.ToString() ? equipment.Where(m => m.pid == queryResult?.sub_pid).FirstOrDefault()?.name : material.Where(m => m.pid == queryResult?.sub_pid).FirstOrDefault()?.name,
                    type = queryResult?.type,
                    apply_amount = each.apply_amount,
                    create_time = each.create_time,
                    state = each.state,
                    unit = SystemCode?.description,
                    Apply_Name = User?.name_ch
                });
            }

            return result;
        }

        public UnitApplyComplex GetDeliveryUnitApply(long pid)
        {
            UnitApplyComplex result = new UnitApplyComplex();
            var data = DB.unit_applies.AsNoTracking().Where(x => x.pid == pid).FirstOrDefault();

            if (data == null)
            {
                throw new Exception("查無器材/裝備撥發資料");
            }

            var setting = DB.min_base_stock_subscribe_settings.AsNoTracking().Where(x => x.pid == data.setting_pid).FirstOrDefault();

            if (setting == null)
            {
                throw new Exception("查無基準存量與申購點設定資料");
            }

            var subscribepoint = DB.subscribepoint_maintains.AsNoTracking().Where(x => x.pid == setting.subscribepoint_pid).FirstOrDefault();
            var RemainingStock = 0;

            var sub = DB.equipment_maintains.AsNoTracking().Where(x => x.pid == setting.sub_pid).FirstOrDefault();
            result.sub_name = $"{sub.name}【{subscribepoint?.name}】";
            result.RemainingStock = sub.stock;

            var User = DB.backend_users.AsNoTracking().Where(x => x.account == data.creator).FirstOrDefault();

            result.pid = data.pid;
            result.apply_amount = data.apply_amount;
            result.subscribepoint = subscribepoint?.name;
            result.state = data.state;
            result.type = setting.type;
            result.Apply_Name = User?.name_ch;

            return result;
        }

        public async Task Delivery(long pid, string account)
        {
            var data = await DB.unit_applies.Where(x => x.pid == pid).FirstOrDefaultAsync();

            if(data == null)
            {
                throw new Exception($"查無出貨作業資料，pid：{pid}");
            }

            var setting = await DB.min_base_stock_subscribe_settings.AsNoTracking().Where(x => x.pid == data.setting_pid).FirstOrDefaultAsync();

            if(setting == null)
            {
                throw new Exception($"查無基準存量與申購點設定資料，setting：{data.setting_pid}");
            }

            var equipmentMaintain = await DB.equipment_maintains.Where(x => x.pid == setting.sub_pid).FirstOrDefaultAsync();

            if (equipmentMaintain == null)
            {
                throw new Exception($"查無裝備/器材資料，sub_pid：{setting.sub_pid}");
            }

            unit_apply_review_log log = new unit_apply_review_log()
            {
                unit_apply_pid = pid,
                ori_state = data.state,
                new_state = UNITAPPLY_STATE.STATE.DELIVERY.ToString(),
                memo = "",
                create_time = DateTime.Now,
                creator = account,
            };

            data.state = UNITAPPLY_STATE.STATE.DELIVERY.ToString();
            equipmentMaintain.stock = equipmentMaintain.stock - data.apply_amount;

            await DB.unit_apply_review_logs.AddAsync(log);

            await DB.SaveChangesAsync();
        }
    }
}
