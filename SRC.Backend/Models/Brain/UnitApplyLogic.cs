using SRC.Backend.Models.Pages.UnitApply;
using SRC.Backend.Models.System;
using SRC.DB.HardCodes;
using SRC.DB.Interfaces.UnitApply;
using SRC.DB.Models.EFMSSQL;

namespace SRC.Backend.Models.Brain
{
    public class UnitApplyLogic
    {
        private Serilog.ILogger Logger { get; set; }
        private IDF_UnitApply DF_UnitApply { get; set; }
        public string InnerMessage { get; set; }
        public UnitApplyLogic(Serilog.ILogger logger, IDF_UnitApply dF_UnitApply)
        {
            Logger = logger;
            DF_UnitApply = dF_UnitApply;
        }

        public UnitApplySearch Search(UnitApplyIndex.SearchModel condition,int? page,int? take)
        {
            UnitApplySearch Model = new UnitApplySearch();
            try
            {
                Model.data = DF_UnitApply.ListUnitApply(condition.type, condition.sub_pid, condition.subscribepoint_pid, condition.start_time, condition.end_time, condition.state, page, take, out int rowtotal);

                Model.Pagination = new System.SRCUIPagination(page, take, rowtotal);

            }
            catch(Exception ex)
            {
                Logger.Fatal(ex, $"取得使用單位器材申請及撤銷作業資料發生異常，{ex.Message}");
            }

            return Model;
        }

        public async Task<bool> Create(UnitApplyEdit model,string account,string unit)
        {

            try
            {
                unit_apply data = new unit_apply()
                {
                    setting_pid = model.setting_pid.Value,
                    create_time = DateTime.Now,
                    creator = account,
                    unit = unit,
                    apply_amount = model.apply_amount,
                    state = UNITAPPLY_STATE.STATE.INIT.ToString()
                };

                data.unit_apply_review_logs.Add(new unit_apply_review_log()
                {
                    ori_state = "",
                    new_state = UNITAPPLY_STATE.STATE.INIT.ToString(),
                    create_time = DateTime.Now,
                    creator = account,
                    memo = ""
                });

                await DF_UnitApply.Create(data);

                return true;
            }
            catch(Exception ex)
            {
                Logger.Fatal(ex, $"新增使用單位器材申請及撤銷作業發生異常，{ex.Message}");
                InnerMessage = $"{UIMessage.SYS.Fail_Add}，{ex.Message}";
                return false;
            }
        }

        public async Task<bool> Edit(UnitApplyEdit model, string account)
        {

            try
            {
                unit_apply data = new unit_apply()
                {
                    pid = model.pid,
                    setting_pid = model.setting_pid.Value,
                    apply_amount = model.apply_amount,
                    state = UNITAPPLY_STATE.STATE.INIT.ToString(),
                    editor = account,
                    edit_time = DateTime.Now
                };

                unit_apply_review_log log = new unit_apply_review_log()
                {
                    unit_apply_pid = model.pid,
                    ori_state = "",
                    new_state = UNITAPPLY_STATE.STATE.INIT.ToString(),
                    create_time = DateTime.Now,
                    creator = account,
                    memo = ""
                };

                await DF_UnitApply.Edit(data,log);

                return true;
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex, $"編輯使用單位器材申請及撤銷作業發生異常，{ex.Message}");
                InnerMessage = $"{UIMessage.SYS.Fail_Add}，{ex.Message}";
                return false;
            }
        }
    }
}
