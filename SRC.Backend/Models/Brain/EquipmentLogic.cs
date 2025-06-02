using SRC.Backend.Models.Pages.Equipment;
using SRC.Backend.Models.System;
using SRC.DB.HardCodes;
using SRC.DB.Interfaces.Equipment;
using SRC.DB.Models.EFMSSQL;

namespace SRC.Backend.Models.Brain
{
    public class EquipmentLogic
    {
        private Serilog.ILogger _logger;
        private IDF_Equipment DF_Equipment;
        public string InnerMessage { get; set; }

        public EquipmentLogic(Serilog.ILogger logger,IDF_Equipment dF_Equipment)
        {
            _logger = logger;
            DF_Equipment = dF_Equipment;
        }

        public string CheckEquipmentParameter(EquipmentEdit model)
        {
            List<string> ErrorMsg = new List<string>();
            if (string.IsNullOrWhiteSpace(model.name))
            {
                ErrorMsg.Add("請輸入裝備名稱");
            }

            if (!model.price.HasValue)
            {
                ErrorMsg.Add("請輸入價格");
            }

            if (!model.stock.HasValue)
            {
                ErrorMsg.Add("請輸入庫存量");
            }

            if(model.Action == ActionMode.ADD)
            {
                if (DF_Equipment.CheckSameName(null, model.name))
                {
                    ErrorMsg.Add("裝備名稱不能重複，請重新確認");
                }
            }
            else
            {
                if (DF_Equipment.CheckSameName(model.pid, model.name))
                {
                    ErrorMsg.Add("裝備名稱不能重複，請重新確認");
                }
            }

            return string.Join("，", ErrorMsg);
        }

        public async Task<bool> Create(EquipmentEdit model,string account)
        {
            try
            {
                equipment_maintain data = new equipment_maintain()
                {
                    name = model.name,
                    price = model.price.Value,
                    stock = model.stock.Value,
                    create_time = DateTime.Now,
                    creator = account,
                    state = EQUIPMENT_STATE.STATE.ENABLE.ToString()
                };

                await DF_Equipment.Create(data);

                return true;
            }
            catch(Exception ex)
            {
                InnerMessage = $"{UIMessage.SYS.Fail_Add}，{ex.Message}";
                _logger.Fatal(ex, $"新增裝備主檔資料發生異常，{ex.Message}");
                return false;
            }
        }

        public async Task<bool> Edit(EquipmentEdit model, string account)
        {
            try
            {
                equipment_maintain data = new equipment_maintain()
                {
                    pid = model.pid,
                    name = model.name,
                    price = model.price.Value,
                    stock = model.stock.Value,
                    editor = account,
                    edit_time = DateTime.Now
                };

                await DF_Equipment.Edit(data);

                return true;
            }
            catch (Exception ex)
            {
                InnerMessage = $"{UIMessage.SYS.Fail_Add}，{ex.Message}";
                _logger.Fatal(ex, $"編輯裝備主檔資料發生異常，{ex.Message}");
                return false;
            }
        }
    }
}
