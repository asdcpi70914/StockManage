using Microsoft.EntityFrameworkCore;
using SRC.DB.Abstract;
using SRC.DB.HardCodes;
using SRC.DB.Interfaces.UnitApply;
using SRC.DB.Models.Complex;
using SRC.DB.Models.EFMSSQL;
using SRC.DB.Responsibility.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRC.DB.Responsibility.UnitApply
{
    public class DF_UnitApply:ADF, IDF_UnitApply
    {
        public DF_UnitApply(IDataBaseFactory dataBaseService, EFContext db) : base(dataBaseService, db)
        {
        }

        public List<UnitApplyComplex> ListUnitApply(string type, long? setting_pid, long? subscribepoint_pid, DateTime? start_time, DateTime? end_time, List<string> state, long? unit, int? page, int? take, out int rowtotal)
        {
            IQueryable<unit_apply> data = DB.unit_applies.AsNoTracking();
            IQueryable<min_base_stock_subscribe_setting> query = DB.min_base_stock_subscribe_settings.AsNoTracking();

            if (unit.HasValue)
            {
                data = data.Where(x => x.unit == unit);
            }

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

            foreach(var each in dataResult)
            {
                var queryResult = queryResults.Where(x => x.pid == each.setting_pid).FirstOrDefault();

                result.Add(new UnitApplyComplex()
                {
                    pid = each.pid,
                    subscribepoint = subscribepoint.Where(m => m.pid == queryResult?.subscribepoint_pid).FirstOrDefault()?.name,
                    sub_name = queryResult?.type == MINBASESTOCK_TYPE.STATE.EQUIPMENT.ToString() ? equipment.Where(m => m.pid == queryResult?.sub_pid).FirstOrDefault()?.name : material.Where(m => m.pid == queryResult?.sub_pid).FirstOrDefault()?.name,
                    type = queryResult?.type,
                    apply_amount = each.apply_amount,
                    create_time = each.create_time,
                    state = each.state,
                    unit = subscribepoint.Where(m => m.pid == each?.unit).FirstOrDefault()?.name,
                    Apply_Name = each.creator,
                });
            }

            return result;
        }

        public UnitApplyEditComplex GetUnitApply(long pid)
        {
            UnitApplyEditComplex result = new UnitApplyEditComplex();
            var data = DB.unit_applies.AsNoTracking().Where(x => x.pid == pid).FirstOrDefault();

            if (data == null)
            {
                throw new Exception("查無器材申請資料");
            }

            var setting = DB.min_base_stock_subscribe_settings.AsNoTracking().Where(x => x.pid == data.setting_pid).FirstOrDefault();

            if (setting == null)
            {
                throw new Exception("查無基準存量與申購點設定資料");
            }

            var RemainingStock = 0;

            var sub = DB.equipment_maintains.AsNoTracking().Where(x => x.pid == setting.sub_pid).FirstOrDefault();
            //result.RemainingStock = sub.stock;

            result.pid = data.pid;
            result.apply_amount = data.apply_amount;
            result.subscribepoint = setting.subscribepoint_pid;
            result.setting_pid = setting.pid;
            result.state = data.state;
            result.type = setting.type;

            return result;
        }

        public UnitApplyComplex GetReviewUnitApply(long pid)
        {
            UnitApplyComplex result = new UnitApplyComplex();
            var data = DB.unit_applies.AsNoTracking().Where(x => x.pid == pid).FirstOrDefault();

            if(data == null)
            {
                throw new Exception("查無器材申請資料");
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
            result.RemainingStock = setting.stock;

            result.pid = data.pid;
            result.apply_amount = data.apply_amount;
            result.subscribepoint = subscribepoint?.name;
            result.state = data.state;
            result.type = setting.type;

            return result;
        }

        public async Task Create(unit_apply data)
        {
            var setting = await DB.min_base_stock_subscribe_settings.AsNoTracking().Where(x => x.pid == data.setting_pid).FirstOrDefaultAsync();

            if(setting == null)
            {
                throw new Exception($"查無裝備存量與申購點設定，setting_pid：{data.setting_pid}");
            }


            var equipmentData = await DB.equipment_maintains.AsNoTracking().Where(x => x.pid == setting.sub_pid).FirstOrDefaultAsync();

            if (equipmentData == null)
            {
                throw new Exception($"查無裝備/器材設定，equipmentData：{equipmentData.pid}");
            }

            //if(equipmentData.stock < data.apply_amount)
            //{
            //    throw new Exception($"申請數量超過庫存數量，請重新確認");
            //}

            if(data.creator == "admin")
            {
                data.unit = setting.subscribepoint_pid;            
            }

            await DB.unit_applies.AddAsync(data);

            await DB.SaveChangesAsync();
        }

        public async Task Edit(unit_apply data, unit_apply_review_log log)
        {
            var editData = await DB.unit_applies.Where(x => x.pid == data.pid).FirstOrDefaultAsync();

            if(editData == null)
            {
                throw new Exception("查無器材申請資料");
            }

            List<string> canEditState = new List<string>()
            {
                UNITAPPLY_STATE.STATE.REVIEW_FAIL.ToString()
            };

            if (!canEditState.Contains(editData.state))
            {
                throw new Exception("申請狀態已變更，請重新新確認");
            }


            log.ori_state = editData.state;
            editData.state = data.state;
            editData.apply_amount = data.apply_amount;
            editData.setting_pid = data.setting_pid;
            editData.edit_time = data.edit_time;
            editData.editor = data.editor;
            

            await DB.unit_apply_review_logs.AddAsync(log);
            await DB.SaveChangesAsync();
        }

        public async Task Delete(List<long> pids,string account)
        {
            var deleteData = await DB.unit_applies.Where(x => pids.Contains(x.pid)).ToListAsync();

            List<unit_apply_review_log> logs = new List<unit_apply_review_log>();

            List<string> canDeleteState = new List<string>()
            {
                UNITAPPLY_STATE.STATE.REVIEW_FAIL.ToString(),
                UNITAPPLY_STATE.STATE.INIT.ToString()
            };

            foreach (var each in deleteData)
            {
                if (!canDeleteState.Contains(each.state))
                {
                    throw new Exception("申請狀態已變更，請重新新確認");
                }

                each.state = UNITAPPLY_STATE.STATE.CANCEL.ToString();
                each.editor = account;
                each.edit_time = DateTime.Now;

                logs.Add(new unit_apply_review_log()
                {
                    unit_apply_pid = each.pid,
                    ori_state = each.state,
                    new_state = UNITAPPLY_STATE.STATE.CANCEL.ToString(),
                    create_time = DateTime.Now,
                    creator = account,
                    memo = ""
                });
            }

            await DB.unit_apply_review_logs.AddRangeAsync(logs);
            await DB.SaveChangesAsync();
        }

        public async Task Review(long pid, string state, string memo, string account)
        {
            var data = await DB.unit_applies.Where(x => x.pid == pid).FirstOrDefaultAsync();

            if(data == null)
            {
                throw new Exception("");
            }

            unit_apply_review_log log = new unit_apply_review_log()
            {
                unit_apply_pid = pid,
                ori_state = data.state,
                new_state = state,
                create_time = DateTime.Now,
                creator = account,
                memo = memo ?? ""
            };

            if(state == UNITAPPLY_STATE.STATE.REVIEW_OK.ToString())
            {
                data.state = UNITAPPLY_STATE.STATE.REVIEW_OK.ToString();
            }
            else
            {
                data.state = UNITAPPLY_STATE.STATE.REVIEW_FAIL.ToString();
            }

            await DB.unit_apply_review_logs.AddAsync(log);

            await DB.SaveChangesAsync();
        }

        public Dictionary<long, string> DicMinBaseStoc(string type, long? unit)
        {
            List<min_base_stock_subscribe_setting> data = new List<min_base_stock_subscribe_setting>();
            if (unit.HasValue)
            {
                data = DB.min_base_stock_subscribe_settings.AsNoTracking().Where(x => x.type == type && x.subscribepoint_pid == unit).ToList();
            }
            else
            {
                data = DB.min_base_stock_subscribe_settings.AsNoTracking().Where(x => x.type == type).ToList();
            }


            var SubscribepointPids = data.Select(x => x.subscribepoint_pid).ToList();
            var SubPids = data.Select(x => x.sub_pid).ToList();
            var SubscribepointList = DB.subscribepoint_maintains.AsNoTracking().Where(x => SubscribepointPids.Contains(x.pid)).ToList();
            var EquipmentList = new List<equipment_maintain>();

            EquipmentList = DB.equipment_maintains.AsNoTracking().Where(x => SubPids.Contains(x.pid)).ToList();

            Dictionary<long, string> result = new Dictionary<long, string>();
            string subName = "";
            foreach(var each in data)
            {
                subName = EquipmentList.Where(x => x.pid == each.sub_pid).FirstOrDefault()?.name;

                var Subscribepoint = SubscribepointList.Where(x => x.pid == each.subscribepoint_pid).FirstOrDefault();

                result.Add(each.pid,$"{subName}【{Subscribepoint?.name}】");
            }

            return result;
        }

        public min_base_stock_subscribe_setting GetRemainingStock(string type, long pid)
        {
            return DB.min_base_stock_subscribe_settings.AsNoTracking().Where(x => x.pid == pid).FirstOrDefault();
        }

        public List<UnitApplyReviewLogComplex> ListReviewLogs(long pid)
        {
            List<UnitApplyReviewLogComplex> data = new List<UnitApplyReviewLogComplex>();

            List<unit_apply_review_log> logs = DB.unit_apply_review_logs.AsNoTracking().Where(x => x.unit_apply_pid == pid).ToList();

            var UnitApplyPids = logs.Select(x => x.unit_apply_pid).ToList();
            var ReviewAccount = logs.Select(x => x.creator).ToList();

            var UnitApplyList = DB.unit_applies.AsNoTracking().Where(x => UnitApplyPids.Contains(x.pid)).ToList();

            var SettingPidList = UnitApplyList.Select(x => x.setting_pid).ToList();

            var Setting = DB.min_base_stock_subscribe_settings.AsNoTracking().Where(x => SettingPidList.Contains(x.pid)).FirstOrDefault();

            if(Setting == null)
            {
                throw new Exception("");
            }

            string sub_name = "";
            string subscribepoint = DB.subscribepoint_maintains.AsNoTracking().Where(x => x.pid == Setting.subscribepoint_pid).FirstOrDefault()?.name;
            sub_name = DB.equipment_maintains.AsNoTracking().Where(x => x.pid == Setting.sub_pid).FirstOrDefault()?.name;

            var Users = DB.backend_users.AsNoTracking().Where(x => ReviewAccount.Contains(x.account)).ToList();

            foreach (var each in logs)
            {
                var User = Users.Where(x => x.account == each.creator).FirstOrDefault();
                data.Add(new UnitApplyReviewLogComplex()
                {
                    sub_name = $"{sub_name}【{subscribepoint}】",
                    ori_state = !string.IsNullOrWhiteSpace(each.ori_state) ? new UNITAPPLY_STATE(each.ori_state).Desc : "",
                    new_state = !string.IsNullOrWhiteSpace(each.new_state) ? new UNITAPPLY_STATE(each.new_state).Desc : "",
                    memo = each.memo,
                    review_time = each.create_time,
                    review_name = User?.name_ch
                });
            }

            return data;
        }
    }
}
