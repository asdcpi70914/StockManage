using SRC.Backend.Models.Pages.MinBaseStock;
using SRC.Backend.Models.System;
using SRC.DB.Interfaces.MinBaseStock;
using SRC.DB.Models.EFMSSQL;

namespace SRC.Backend.Models.Brain
{
    public class MinBaseStockLogic
    {
        private Serilog.ILogger Logger {  get; set; }
        private IDF_MinBaseStock DF_MinBaseStock { get; set; }
        public string InnerMessage { get; set; }
        public MinBaseStockLogic(Serilog.ILogger logger,IDF_MinBaseStock dF_MinBaseStock) 
        {
            Logger = logger;
            DF_MinBaseStock = dF_MinBaseStock;
        }

        public MinBaseStockSearch Search(MinBaseStockIndex.SearchModel condition,int? page,int? take) 
        {
            MinBaseStockSearch Model = new MinBaseStockSearch();
            try
            {
                Model.Data = DF_MinBaseStock.ListMinBaseStockSubscribeSetting(condition.type, condition.sub_pid, condition.subscribepoint_pid, condition.start_time, condition.end_time, page, take, out int rowtotal);
                Model.Pagination = new System.SRCUIPagination(page, take, rowtotal);
            }
            catch(Exception ex)
            {
                Logger.Fatal(ex, $"取得基準存量與申購點設定資料發生異常，{ex.Message}");
            }

            return Model;
        }

        public string CheckParameter(MinBaseStockEdit model)
        {
            List<string> ErrorMsg = new List<string>();

            if (string.IsNullOrWhiteSpace(model.type))
            {
                ErrorMsg.Add("請選擇類別");
            }

            if (!model.subscribepoint_pid.HasValue)
            {
                ErrorMsg.Add("請選擇申請點");
            }

            if (!model.sub_pid.HasValue)
            {
                ErrorMsg.Add("請選擇裝備/器材");
            }

            if (!model.min_base_stock.HasValue)
            {
                ErrorMsg.Add("請輸入最低基準存量");
            }

            if (!model.stock.HasValue)
            {
                ErrorMsg.Add("請輸入庫存數量");
            }

            if(!string.IsNullOrWhiteSpace(model.type) && model.subscribepoint_pid.HasValue && model.sub_pid.HasValue)
            {
                if (DF_MinBaseStock.CheckSameData(model.pid,model.type,model.sub_pid.Value, model.subscribepoint_pid.Value))
                {
                    ErrorMsg.Add("已存在相同裝備/器材與申請點");
                }
            }


            return string.Join("，", ErrorMsg);
        }

        public async Task<bool> Create(MinBaseStockEdit model,string account)
        {
            try
            {
                var check = CheckParameter(model);

                if (!string.IsNullOrWhiteSpace(check))
                {
                    InnerMessage = $"{UIMessage.SYS.Fail_Add}，{check}";

                    return false;
                }


                min_base_stock_subscribe_setting data = new min_base_stock_subscribe_setting()
                {
                    type = model.type,
                    subscribepoint_pid = model.subscribepoint_pid.Value,
                    sub_pid = model.sub_pid.Value,
                    create_time = DateTime.Now,
                    creator = account,
                    stock = model.stock.Value,
                    min_base_stock = model.min_base_stock.Value,
                };

                await DF_MinBaseStock.Create(data);

                return true;
            }
            catch(Exception ex)
            {
                Logger.Fatal(ex, $"新增基準存量與申購點設定發生異常，{ex.Message}");
                InnerMessage = $"{UIMessage.SYS.Fail_Add}，{ex.Message}";
                return false;
            }
        }

        public async Task<bool> Edit(MinBaseStockEdit model, string account)
        {
            try
            {
                var check = CheckParameter(model);

                if (!string.IsNullOrWhiteSpace(check))
                {
                    InnerMessage = $"{UIMessage.SYS.Fail_Edit}，{check}";

                    return false;
                }


                min_base_stock_subscribe_setting data = new min_base_stock_subscribe_setting()
                {
                    pid = model.pid.Value,
                    type = model.type,
                    subscribepoint_pid = model.subscribepoint_pid.Value,
                    sub_pid = model.sub_pid.Value,
                    edit_time = DateTime.Now,
                    editor = account,
                    min_base_stock = model.min_base_stock.Value,
                };

                await DF_MinBaseStock.Edit(data);

                return true;
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex, $"編輯基準存量與申購點設定發生異常，{ex.Message}");
                InnerMessage = $"{UIMessage.SYS.Fail_Add}，{ex.Message}";
                return false;
            }
        }
    }
}
