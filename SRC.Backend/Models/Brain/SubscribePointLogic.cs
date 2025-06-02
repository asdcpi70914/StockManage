using SRC.Backend.Models.Pages.SubscribePoint;
using SRC.Backend.Models.System;
using SRC.DB.Interfaces.SubscribePoint;
using SRC.DB.Models.EFMSSQL;

namespace SRC.Backend.Models.Brain
{
    public class SubscribePointLogic
    {
        private IDF_SubscribePoint DF_SubscribePoint { get; set; }
        private Serilog.ILogger Logger { get; set; }
        public string InnerMessage { get; set; }

        public SubscribePointLogic(Serilog.ILogger logger,IDF_SubscribePoint dF_SubscribePoint)
        {
            Logger = logger;
            DF_SubscribePoint = dF_SubscribePoint;
        }

        public SubscribePointSearch Search(SubscribePointIndex.SearchModel condition,int? page,int? take,out int rowtotal)
        {
            SubscribePointSearch Model = new SubscribePointSearch();


            try
            {
                Model.Data = DF_SubscribePoint.ListSubscribePoint(condition.name, condition.start_time, condition.end_time, page, take, out rowtotal);
                Model.Pagination = new System.SRCUIPagination(page, take, rowtotal);
            }
            catch(Exception ex)
            {
                rowtotal = 0;
                Logger.Fatal(ex, $"查詢申購點資料發生異常，{ex.Message}");
            }


            return Model;
        }

        public string CheckParameter(SubscribePointEdit model)
        {
            List<string> ErrorMsg = new List<string>();

            if (string.IsNullOrWhiteSpace(model.name))
            {
                ErrorMsg.Add("請輸入申購點名稱");
            }
            else
            {
                if(model.Action == ActionMode.ADD)
                {
                    if (DF_SubscribePoint.CheckSameName(null, model.name))
                    {
                        ErrorMsg.Add("申購點名稱不能重複，請重新確認");
                    }
                }
                else
                {
                    if (DF_SubscribePoint.CheckSameName(model.pid, model.name))
                    {
                        ErrorMsg.Add("申購點名稱不能重複，請重新確認");
                    }
                }
            }

            return string.Join("，", ErrorMsg);
        }

        public async Task<bool> Create(SubscribePointEdit model,string account)
        {
            try
            {
                var check = CheckParameter(model);

                if (!string.IsNullOrWhiteSpace(check))
                {
                    InnerMessage = $"{UIMessage.SYS.Fail_Add}，{check}";
                    return false;
                }

                subscribepoint_maintain data = new subscribepoint_maintain()
                {
                    name = model.name,
                    create_time = DateTime.Now,
                    creator = account
                };

               await DF_SubscribePoint.Create(data);

                return true;
            }
            catch(Exception ex)
            {
                Logger.Fatal(ex, $"新增申購點發生異常，{ex.Message}");
                InnerMessage = $"{UIMessage.SYS.Fail_Add}，{ex.Message}";
                return false;
            }
        }

        public async Task<bool> Edit(SubscribePointEdit model,string account)
        {
            try
            {
                var check = CheckParameter(model);

                if (!string.IsNullOrWhiteSpace(check))
                {
                    InnerMessage = $"{UIMessage.SYS.Fail_Edit}，{check}";
                    return false;
                }

                subscribepoint_maintain data = new subscribepoint_maintain()
                {
                    pid = model.pid,
                    name = model.name,
                    edit_time = DateTime.Now,
                    editor = account
                };

                await DF_SubscribePoint.Edit(data);

                return true;
            }
            catch(Exception ex)
            {
                Logger.Fatal(ex, $"編輯申購點發生異常，{ex.Message}");
                InnerMessage = $"{UIMessage.SYS.Fail_Edit}，{ex.Message}";
                return false;
            }
        }
    }
}
