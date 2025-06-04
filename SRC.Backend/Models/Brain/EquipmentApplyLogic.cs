using SRC.Backend.Models.Pages.EquipmentApply;
using SRC.Backend.Models.Pages.UnitApply;
using SRC.Backend.Models.System;
using SRC.DB.Interfaces.UnitApply;

namespace SRC.Backend.Models.Brain
{
    public class EquipmentApplyLogic
    {
        private Serilog.ILogger Logger { get; set; }
        private IDF_UnitApply DF_UnitApply { get; set; }
        public string InnerMessage { get; set; }

        public EquipmentApplyLogic(Serilog.ILogger logger,IDF_UnitApply dF_UnitApply)
        {
            DF_UnitApply = dF_UnitApply;
            Logger = logger;
        }

        public EquipmentApplySearch Search(EquipmentApplyIndex.SearchModel condition, int? page, int? take)
        {
            EquipmentApplySearch Model = new EquipmentApplySearch();
            try
            {
                Model.data = DF_UnitApply.ListUnitApply(condition.type, condition.sub_pid, condition.subscribepoint_pid, condition.start_time, condition.end_time, condition.state, page, take, out int rowtotal);

                Model.Pagination = new System.SRCUIPagination(page, take, rowtotal);

            }
            catch (Exception ex)
            {
                Logger.Fatal(ex, $"取得使用單位器材申請資料發生異常，{ex.Message}");
            }

            return Model;
        }

        public async Task<bool> Review(EquipmentApplyEdit model,string account)
        {
            try
            {

                await DF_UnitApply.Review(model.pid, model.state,model.memo,account);

                return true;
            }
            catch(Exception ex)
            {
                Logger.Fatal(ex, $"審核器材裝備申請資料發生異常，{ex.Message}");
                InnerMessage = $"{UIMessage.SYS.Fail_Review}，{ex.Message}";
                return false;
            }
        }
    }
}
