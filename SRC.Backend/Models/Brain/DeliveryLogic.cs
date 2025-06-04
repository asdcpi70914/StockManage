using SRC.Backend.Models.Pages.Delivery;
using SRC.Backend.Models.System;
using SRC.DB.Interfaces.Delivery;

namespace SRC.Backend.Models.Brain
{
    public class DeliveryLogic
    {
        private Serilog.ILogger Logger { get; set; }
        private IDF_Delivery DF_Delivery {  get; set; }
        public string InnerMessage { get; set; }

        public DeliveryLogic(Serilog.ILogger logger, IDF_Delivery dF_Delivery)
        {
            Logger = logger;
            DF_Delivery = dF_Delivery;
        }

        public DeliverySearch Search(DeliveryIndex.SearchModel condition,int? page,int? take)
        {
            DeliverySearch Model = new DeliverySearch();

            try
            {
                Model.data = DF_Delivery.ListUnitApplyComplex(condition.type, condition.sub_pid, condition.subscribepoint_pid, condition.start_time, condition.end_time, condition.state, page, take, out int rowtotal);

                Model.Pagination = new System.SRCUIPagination(page, take, rowtotal);
            }
            catch(Exception ex)
            {
                Logger.Fatal(ex, $"取得出貨作業資料發生異常，{ex.Message}");
            }

            return Model;
        }

        public async Task<bool> Delivery(long pid,string account)
        {
            try
            {
                await DF_Delivery.Delivery(pid, account);

                return true;
            }
            catch(Exception ex)
            {
                Logger.Fatal(ex, $"交貨處理作業資料發生異常，{ex.Message}");
                InnerMessage = $"{UIMessage.SYS.Fail_Delivery}，{ex.Message}";
                return false;
            }
        }
    }
}
