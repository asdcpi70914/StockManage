using Microsoft.EntityFrameworkCore;
using SRC.DB.Abstract;
using SRC.DB.HardCodes;
using SRC.DB.Interfaces.PayTreasury;
using SRC.DB.Models.Complex;
using SRC.DB.Models.EFMSSQL;
using SRC.DB.Responsibility.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRC.DB.Responsibility.PayTreasury
{
    public class DF_PayTreasury:ADF, IDF_PayTreasury
    {
        public DF_PayTreasury(IDataBaseFactory dataBaseService, EFContext db) : base(dataBaseService, db)
        {
        }

        public List<PayTreasuryComplex> ListPayTreasuryComplex(string type, long? setting_pid, long? subscribepoint_pid, DateTime? start_time, DateTime? end_time, int? page, int? take, out int rowtotal)
        {
            IQueryable<unit_apply> data = DB.unit_applies.AsNoTracking().Where(x => x.state == UNITAPPLY_STATE.STATE.DELIVERY.ToString());
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

            //if (!string.IsNullOrWhiteSpace(state))
            //{
            //    data = data.Where(x => x.state == state);
            //}

            var queryResultPids = queryResults.Select(x => x.pid).ToList();
            data = data.Where(x => queryResultPids.Contains(x.setting_pid));

            var dataResult = Pagination(data, page, take, out rowtotal);

            List<PayTreasuryComplex> result = new List<PayTreasuryComplex>();

            var subscribepointList = query.Select(x => x.subscribepoint_pid).ToList();

            var subscribepoint = DB.subscribepoint_maintains.AsNoTracking().Where(x => subscribepointList.Contains(x.pid)).ToList();
            var EquipmentList = query.Select(x => x.sub_pid).ToList();
            var equipments = DB.equipment_maintains.AsNoTracking().AsNoTracking().Where(x => EquipmentList.Contains(x.pid)).ToList();
            //var MaterialList = query.Where(x => x.type == MINBASESTOCK_TYPE.STATE.MATERIAL.ToString()).Select(x => x.sub_pid).ToList();
            //var material = DB.equipment_maintains.AsNoTracking().Where(x => MaterialList.Contains(x.pid)).ToList();
            var ApplyUser = dataResult.Select(x => x.creator).ToList();
            var Users = DB.backend_users.AsNoTracking().Where(x => ApplyUser.Contains(x.account)).ToList();
            var UnitCodes = dataResult.Select(x => x.unit).ToList();
            var Units = DB.system_codes.AsNoTracking().Where(x => UnitCodes.Contains(x.data));

            foreach (var each in dataResult)
            {
                var queryResult = queryResults.Where(x => x.pid == each.setting_pid).FirstOrDefault();
                var subscribepoint_name = subscribepoint.Where(m => m.pid == queryResult?.subscribepoint_pid).FirstOrDefault()?.name;
                var User = Users.Where(x => x.account == each.creator).FirstOrDefault();
                var Unit = Units.Where(x => x.data == each.unit).FirstOrDefault();
                var equipment = equipments.Where(x => x.pid == queryResult?.sub_pid).FirstOrDefault();
                result.Add(new PayTreasuryComplex()
                {
                    pid = each.pid,
                    name = $"{equipment?.name}【{subscribepoint_name}】",
                    apply_amount = each.apply_amount,
                    apply_name = User?.name_ch,
                    apply_time = each.create_time,
                    already_pay_amount = each.pay_treasury.HasValue ? each.pay_treasury.Value : 0,
                    stock = equipment != null ? equipment.stock : 0,
                    unit = Unit?.description
                });
            }

            return result;
        }


        public PayTreasuryComplex GetPayTreasury(long pid)
        {
            PayTreasuryComplex result = new PayTreasuryComplex();

            unit_apply data = DB.unit_applies.AsNoTracking().Where(x => x.pid == pid).FirstOrDefault();
            backend_user User = DB.backend_users.AsNoTracking().Where(x => x.account == data.creator).FirstOrDefault();
            min_base_stock_subscribe_setting setting = DB.min_base_stock_subscribe_settings.AsNoTracking().Where(x => x.pid == data.setting_pid).FirstOrDefault();
            equipment_maintain equipment = DB.equipment_maintains.AsNoTracking().Where(x => x.pid == setting.sub_pid).FirstOrDefault();

            result.pid = data.pid;
            result.apply_time = data.create_time;
            result.apply_amount = data.apply_amount;
            result.apply_name = User?.name_ch;
            result.already_pay_amount = data.pay_treasury.HasValue ? data.pay_treasury.Value : 0;
            result.stock = equipment.stock;


            return result;
        }

        public async Task Edit(long pid, int pay_amount, string account)
        {
            var data = DB.unit_applies.Where(x => x.pid == pid).FirstOrDefault();

            if(data == null)
            {
                throw new Exception($"查無繳庫資料，pid：{pid}");
            }

            if(data.apply_amount < pay_amount || data.pay_treasury + pay_amount > data.apply_amount)
            {
                throw new Exception("繳庫數量超過申請數量，請重新確認");
            }


            var setting = DB.min_base_stock_subscribe_settings.AsNoTracking().Where(x => x.pid == data.setting_pid).FirstOrDefault();

            if(setting == null)
            {
                throw new Exception($"查無基準存量與申購點設定，setting_pid：{data.setting_pid}");
            }

            var equipmentMaintain = DB.equipment_maintains.Where(x => x.pid == setting.sub_pid).FirstOrDefault();

            if(equipmentMaintain == null)
            {
                throw new Exception($"查無器材/裝備設定，equipmentMaintain_pid：{setting.sub_pid}");
            }

            equipmentMaintain.stock = equipmentMaintain.stock + pay_amount;

            if (!data.pay_treasury.HasValue)
            {
                data.pay_treasury = 0;
            }

            data.pay_treasury += pay_amount;
            data.editor = account;
            data.edit_time = DateTime.Now;


            await DB.SaveChangesAsync();
        }

        
    }
}
