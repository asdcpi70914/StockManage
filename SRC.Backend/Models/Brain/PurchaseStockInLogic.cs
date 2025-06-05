using SRC.Backend.Models.Pages.PurchaseStockIn;
using SRC.Backend.Models.System;
using SRC.DB.Interfaces.PurchaseStockIn;

namespace SRC.Backend.Models.Brain
{
    public class PurchaseStockInLogic
    {
        private Serilog.ILogger Logger { get; set; }
        private IDF_PurchaseStockIn DF_PurchaseStockIn { get; set; }
        public string InnerMessage { get; set; }

        public PurchaseStockInLogic(Serilog.ILogger logger,IDF_PurchaseStockIn dF_PurchaseStockIn)
        {
            Logger = logger;
            DF_PurchaseStockIn = dF_PurchaseStockIn;
        }

        public PurchaseStockInSearch Search (PurchaseStockInIndex.SearchModel condition,int? page,int? take)
        {
            PurchaseStockInSearch Model = new PurchaseStockInSearch();
            try
            {
                Model.data = DF_PurchaseStockIn.ListPurchaseStockIn(condition.type, condition.pid, condition.start_time, condition.end_time, page, take, out int rowtotal);

                Model.Pagination = new System.SRCUIPagination(page, take, rowtotal);
            }
            catch(Exception ex)
            {
                Logger.Fatal(ex, $"查詢器材/裝備庫存數量發生異常，{ex.Message}");
            }

            return Model;
        }

        public async Task<bool> StockIn(long pid,int stock,string acccount)
        {
            try
            {
                await DF_PurchaseStockIn.StockIn(pid,stock,acccount);

                return true;
            }
            catch(Exception ex)
            {
                Logger.Fatal(ex, $"採購入庫發生異常，{ex.Message}");
                InnerMessage = $"{UIMessage.SYS.Fail_Edit}，{ex.Message}";
                return false;
            }
        }
    }
}
