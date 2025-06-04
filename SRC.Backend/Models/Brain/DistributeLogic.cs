using DocumentFormat.OpenXml.Bibliography;
using SRC.Backend.Models.Pages.Distribute;
using SRC.Backend.Models.System;
using SRC.DB.Interfaces.Distribute;

namespace SRC.Backend.Models.Brain
{
    public class DistributeLogic
    {
        private Serilog.ILogger Logger { get; set; }
        private IDF_Distribute DF_Distribute {  get; set; }
        public string InnerMessage {  get; set; }
        public DistributeLogic(Serilog.ILogger logger,IDF_Distribute dF_Distribute)
        {
            Logger = logger;
            DF_Distribute = dF_Distribute;
        }

        public DistributeSearch Search(DistributeIndex.SearchModel model,int? page,int? take)
        {
            DistributeSearch result = new DistributeSearch();
            try
            {
                result.data = DF_Distribute.ListUnitApplyComplex(model.type, model.sub_pid, model.subscribepoint_pid, model.start_time, model.end_time, model.state, page, take, out int rowtotal);

                result.Pagination = new System.SRCUIPagination(page,take,rowtotal);


            }
            catch(Exception ex)
            {
                Logger.Fatal(ex, $"取得器材/裝備撥發資料發生異常，{ex.Message}");
            }

            return result;
        }

        public async Task<bool> Distribute(long pid,string account)
        {
            try
            {
                await DF_Distribute.Distribute(pid, account);

                return true;
            }
            catch(Exception ex)
            {
                Logger.Fatal(ex, $"器材/裝備撥發處理發生異常，{ex.Message}");
                InnerMessage = $"{UIMessage.SYS.Fail_Distribute}，{ex.Message}";
                return false;
            }
        }
    }
}
