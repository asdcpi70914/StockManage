using SRC.Backend.Models.Pages.SupplyRoomApply;
using SRC.Backend.Models.System;
using SRC.DB.Interfaces.SupplyRoomApply;

namespace SRC.Backend.Models.Brain
{
    public class SupplyRoomApplyLogic
    {
        private Serilog.ILogger Logger { get; set; }
        private IDF_SupplyRoomApply DF_SupplyRoomApply {  get; set; }
        public string InnerMessage { get; set; }

        public SupplyRoomApplyLogic(Serilog.ILogger logger,IDF_SupplyRoomApply dF_SupplyRoomApply)
        {
            Logger = logger;
            DF_SupplyRoomApply = dF_SupplyRoomApply;
        }

        public SupplyRoomApplySearch Search(SupplyRoomApplyIndex.SearchModel condition,int? page,int? take)
        {
            SupplyRoomApplySearch Model = new SupplyRoomApplySearch();
            try
            {
                Model.data = DF_SupplyRoomApply.ListUnitApplyComplex(condition.type, condition.sub_pid, condition.subscribepoint_pid, condition.start_time, condition.end_time, condition.state, page, take, out int rowtotal);

                Model.Pagination = new System.SRCUIPagination(page,take,rowtotal);
            }
            catch(Exception ex) 
            {
                Logger.Fatal(ex, $"取得供應室申請及撤銷資料發生異常，{ex.Message}");
            }

            return Model;
        }

        public async Task<bool> Edit(long pid,string account)
        {
            try
            {
                await DF_SupplyRoomApply.Edit(pid, account);

                return true;
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex, $"供應室申請及撤銷作業發生異常，{ex.Message}");
                InnerMessage = $"{UIMessage.SYS.Fail_Review}，{ex.Message}";
                return false;
            }
        }
    }
}
