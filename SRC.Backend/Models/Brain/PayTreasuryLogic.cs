using SRC.Backend.Models.Pages.PayTreasury;
using SRC.Backend.Models.System;
using SRC.DB.Interfaces.PayTreasury;

namespace SRC.Backend.Models.Brain
{
    public class PayTreasuryLogic
    {
        private Serilog.ILogger Logger { get; set; }
        private IDF_PayTreasury DF_PayTreasury { get; set; }
        public string InnerMessage { get; set; }
        public PayTreasuryLogic(Serilog.ILogger logger,IDF_PayTreasury dF_PayTreasury)
        {
            DF_PayTreasury = dF_PayTreasury;
            Logger = logger;
        }

        public PayTreasurySearch Search(PayTreasuryIndex.SearchModel condition,int? page,int? take)
        {
            PayTreasurySearch Model = new PayTreasurySearch();

            try
            {
                Model.Data = DF_PayTreasury.ListPayTreasuryComplex(condition.type, condition.sub_pid, condition.subscribepoint_pid, condition.start_time, condition.end_time, page, take, out int rowtotal);

                Model.Pagination = new System.SRCUIPagination(page, take, rowtotal);
            }
            catch(Exception ex)
            {
                Logger.Fatal(ex, $"查詢器材繳庫作業資料發生異常，{ex.Message}");
            }

            return Model;
        }

        public async Task<bool>Edit(PayTreasuryEdit model,string account)
        {
            try
            {
                await DF_PayTreasury.Edit(model.pid, model.pay_amount, account);

                return true;
            }
            catch(Exception ex)
            {
                Logger.Fatal(ex, $"繳庫作業發生異常，{ex.Message}");
                InnerMessage = $"{UIMessage.SYS.Fail_Edit}，{ex.Message}";
                return false;
            }
        }
    }
}
