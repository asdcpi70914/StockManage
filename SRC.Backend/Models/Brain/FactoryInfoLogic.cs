using SRC.Backend.Models.Pages.FactoryInfo;
using SRC.Backend.Models.System;
using SRC.DB.Interfaces.FactoryInfo;
using SRC.DB.Models.EFMSSQL;

namespace SRC.Backend.Models.Brain
{
    public class FactoryInfoLogic
    {
        private Serilog.ILogger Logger { get; set; }
        private IDF_FactoryInfo DF_FactoryInfo { get; set; }
        public string InnerMessage { get; set; }

        public FactoryInfoLogic(Serilog.ILogger logger,IDF_FactoryInfo dF_FactoryInfo)
        {
            Logger = logger;
            DF_FactoryInfo = dF_FactoryInfo;
        }

        public FactoryInfoSearch Search(FactoryInfoIndex.SearchModel condition,int? page,int? take)
        {
            FactoryInfoSearch Model = new FactoryInfoSearch();

            try
            {
                Model.Data = DF_FactoryInfo.ListFactoryInfo(condition.name, condition.city, condition.town, condition.address, condition.start_time, condition.end_time, page, take, out int rowtotal);

                Model.Pagination = new System.SRCUIPagination(page,take,rowtotal);
            }
            catch(Exception ex)
            {
                Logger.Fatal(ex, $"查詢廠商基本資料發生異常，{ex.Message}");
            }

            return Model;
        }

        public string CheckParameter(FactoryInfoEdit model)
        {
            List<string> ErrorMsg = new List<string>();
            if (string.IsNullOrWhiteSpace(model.name))
            {
                ErrorMsg.Add("請輸入廠商名稱");
            }

            if (string.IsNullOrWhiteSpace(model.company_phone))
            {
                ErrorMsg.Add("請輸入廠商電話");
            }

            if (string.IsNullOrWhiteSpace(model.company_number))
            {
                ErrorMsg.Add("請輸入統一編號");
            }

            if (string.IsNullOrWhiteSpace(model.city))
            {
                ErrorMsg.Add("請選擇縣市");
            }

            if (string.IsNullOrWhiteSpace(model.town))
            {
                ErrorMsg.Add("請選擇鄉鎮市");
            }

            if (string.IsNullOrWhiteSpace(model.address))
            {
                ErrorMsg.Add("請輸入廠商地址");
            }

            return string.Join("，", ErrorMsg);
        }

        public async Task<bool> Create(FactoryInfoEdit model,string account)
        {
            try
            {
                var check = CheckParameter(model);

                if (!string.IsNullOrWhiteSpace(check))
                {
                    InnerMessage = $"{UIMessage.SYS.Fail_Add}，{check}";
                    return false;
                }


                factoryinfo_maintain data = new factoryinfo_maintain()
                {
                    name = model.name,
                    contact_phone = model.company_phone,
                    company_number = model.company_number,
                    city = model.city,
                    town = model.town,
                    address = model.address,
                    create_time = DateTime.Now,
                    creator = account
                };

                await DF_FactoryInfo.Create(data);

                return true;
            }
            catch(Exception ex)
            {
                Logger.Fatal(ex, $"新增廠商基本資料發生異常，{ex.Message}");

                InnerMessage = $"{UIMessage.SYS.Fail_Add}，{ex.Message}";

                return false;
            }
        }

        public async Task<bool> Edit(FactoryInfoEdit model, string account)
        {
            try
            {
                var check = CheckParameter(model);

                if (!string.IsNullOrWhiteSpace(check))
                {
                    InnerMessage = $"{UIMessage.SYS.Fail_Edit}，{check}";
                    return false;
                }


                factoryinfo_maintain data = new factoryinfo_maintain()
                {
                    pid = model.pid,
                    name = model.name,
                    contact_phone = model.company_phone,
                    company_number = model.company_number,
                    city = model.city,
                    town = model.town,
                    address = model.address,
                    edit_time = DateTime.Now,
                    editor = account
                };

                await DF_FactoryInfo.Edit(data);

                return true;
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex, $"編輯廠商基本資料發生異常，{ex.Message}");

                InnerMessage = $"{UIMessage.SYS.Fail_Edit}，{ex.Message}";

                return false;
            }
        }
    }
}
