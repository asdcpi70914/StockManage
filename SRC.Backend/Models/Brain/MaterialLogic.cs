using SRC.Backend.Models.Pages.Equipment;
using SRC.Backend.Models.System;
using SRC.DB.HardCodes;
using SRC.DB.Interfaces.Equipment;
using SRC.DB.Models.EFMSSQL;
using SRC.DB.Responsibility.Equipment;

namespace SRC.Backend.Models.Brain
{
    public class MaterialLogic
    {
        private Serilog.ILogger _logger;
        private IDF_Material DF_Material;
        public string InnerMessage { get; set; }

        public MaterialLogic(Serilog.ILogger logger, IDF_Material dF_Material)
        {
            _logger = logger;
            DF_Material = dF_Material;
        }

        public string CheckMaterialParameter(MaterialEdit model)
        {
            List<string> ErrorMsg = new List<string>();
            if (string.IsNullOrWhiteSpace(model.name))
            {
                ErrorMsg.Add("請輸入器材名稱");
            }

            if (!model.price.HasValue)
            {
                ErrorMsg.Add("請輸入價格");
            }

            //if (!model.stock.HasValue)
            //{
            //    ErrorMsg.Add("請輸入庫存量");
            //}

            if (model.Action == ActionMode.ADD)
            {
                if (DF_Material.CheckSameName(null, model.name))
                {
                    ErrorMsg.Add("器材名稱不能重複，請重新確認");
                }
            }
            else
            {
                if (DF_Material.CheckSameName(model.pid, model.name))
                {
                    ErrorMsg.Add("器材名稱不能重複，請重新確認");
                }
            }

            return string.Join("，", ErrorMsg);
        }

        public async Task<bool> Create(MaterialEdit model, string account)
        {
            try
            {
                equipment_maintain data = new equipment_maintain()
                {
                    name = model.name,
                    price = model.price.Value,
                    //stock = model.stock.Value,
                    create_time = DateTime.Now,
                    creator = account,
                    state = EQUIPMENT_STATE.STATE.ENABLE.ToString(),
                    type = MINBASESTOCK_TYPE.STATE.MATERIAL.ToString(),
                };

                await DF_Material.Create(data);

                return true;
            }
            catch (Exception ex)
            {
                InnerMessage = $"{UIMessage.SYS.Fail_Add}，{ex.Message}";
                _logger.Fatal(ex, $"新增器材主檔資料發生異常，{ex.Message}");
                return false;
            }
        }

        public async Task<bool> Edit(MaterialEdit model, string account)
        {
            try
            {
                equipment_maintain data = new equipment_maintain()
                {
                    pid = model.pid,
                    name = model.name,
                    price = model.price.Value,
                    //stock = model.stock.Value,
                    editor = account,
                    edit_time = DateTime.Now
                };

                await DF_Material.Edit(data);

                return true;
            }
            catch (Exception ex)
            {
                InnerMessage = $"{UIMessage.SYS.Fail_Add}，{ex.Message}";
                _logger.Fatal(ex, $"編輯器材主檔資料發生異常，{ex.Message}");
                return false;
            }
        }
    }
}
