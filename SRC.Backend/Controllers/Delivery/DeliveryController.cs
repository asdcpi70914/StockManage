using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI.Common;
using SRC.Backend.Models.Brain;
using SRC.Backend.Models.Config;
using SRC.Backend.Models.Filters;
using SRC.Backend.Models.Pages;
using SRC.Backend.Models.Pages.Delivery;
using SRC.Backend.Models.Pages.SupplyRoomApply;
using SRC.Backend.Models.System;
using SRC.DB.HardCodes;
using SRC.DB.Interfaces.Delivery;
using SRC.DB.Interfaces.Settings;
using SRC.DB.Interfaces.SubscribePoint;
using SRC.DB.Interfaces.UnitApply;
using SRC.DB.Responsibility.Settings;
using SRC.DB.Responsibility.SupplyRoomApply;
using SRC.DB.Responsibility.UnitApply;

namespace SRC.Backend.Controllers.Delivery
{
    public class DeliveryController : BaseController
    {
        private SysAppsetting Setting { get; set; }
        private IDF_SubscribePoint DF_SubscribePoint { get; set; }
        private IDF_Delivery DF_Delivery {  get; set; }
        private IDF_UnitApply DF_UnitApply { get; set; }
        private IDF_SystemCode DF_SystemCode { get; set; }
        private DeliveryLogic _DelivaeyLogic {  get; set; }
        public DeliveryController(
            SysAppsetting setting,
            IHttpContextAccessor cxtAccessor,
            IConfiguration config,
            IDF_SubscribePoint dF_SubscribePoint,
            IDF_Delivery dF_Delivery,
            IDF_UnitApply dF_UnitApply,
            IDF_SystemCode dF_SystemCode,
            Serilog.ILogger slog) : base(cxtAccessor, config, slog)
        {
            Setting = setting;
            DF_SubscribePoint = dF_SubscribePoint;
            DF_Delivery = dF_Delivery;
            DF_UnitApply = dF_UnitApply;
            DF_SystemCode = dF_SystemCode;
            _DelivaeyLogic = new DeliveryLogic(slog, DF_Delivery);
        }

        private List<string> states = new List<string>()
        {
                UNITAPPLY_STATE.STATE.DISTRIBUTE_OK.ToString(),
                UNITAPPLY_STATE.STATE.DELIVERY.ToString()
        };

        public IActionResult Index()
        {
            DeliveryIndex Model = new DeliveryIndex();
            int page = 1;
            int take = Setting.PaginationTake;

            Model.subscribepointDic = DF_SubscribePoint.SubscribepointDic();
            Model.SearchResultPage = _DelivaeyLogic.Search(new DeliveryIndex.SearchModel() { state = states }, page, take);

            return View(Model);
        }

        public IActionResult Search(DeliveryIndex.SearchModel condition, int? page, int? take)
        {
            DeliverySearch Model = new DeliverySearch();

            condition.state = states;

            Model = _DelivaeyLogic.Search(condition, page, take);

            return PartialView(Model);
        }

        [HttpGet]
        public IActionResult Delivery(long? pid)
        {
            if (!pid.HasValue)
            {
                return PartialAOE_EditFail(UIMessage.SYS.ArgumentEmpty);
            }

            var data = DF_Delivery.GetDeliveryUnitApply(pid.Value);

            SRCLayoutAOE lyAoe = new SRCLayoutAOE();
            lyAoe.formId = "edit_form";
            lyAoe.Action = "Delivery";
            lyAoe.Controller = "Delivery";
            lyAoe.View = "Edit";
            lyAoe.Title = "交貨處理作業";
            lyAoe.ViewSize = ModalSize.ExtraLarge;
            lyAoe.SaveButtonDesc = "出貨";
            lyAoe.AOEObject = new DeliveryEdit
            {
                pid = data.pid,
                subscribepoint = data.subscribepoint,
                sub_name = data.sub_name,
                RemainingStock = data.RemainingStock,
                apply_amount = data.apply_amount,
                min_base_stock = data.min_base_stock,
                type = data.type,
                Action = ActionMode.EDIT
            };

            return PartialAOE(lyAoe);
        }

        [HttpPost]
        public async Task<IActionResult> Delivery(SupplyRoomApplyEdit model)
        {
            SRCLoginMeta meta = User.Identity.LoginMeta();

            var Ok = await _DelivaeyLogic.Delivery(model.pid, meta.Account);

            if (Ok)
            {
                return PartialSrcvalleyPageMessage(new Models.System.SRCPageMessage()
                {
                    IsSuccess = true,
                    Message = UIMessage.SYS.Success_Delivery
                });
            }
            else
            {
                return PartialAOE_EditFail(_DelivaeyLogic.InnerMessage);
            }
        }

        [HttpPost]
        [FuncAlias(AliasName = "Search")]
        public IActionResult ChangeType(string type)
        {
            try
            {
                var result = DF_UnitApply.DicMinBaseStoc(type);

                return Json(result);
            }
            catch (Exception ex)
            {
                SLog.Fatal(ex, $"取得裝備/器材基準存量與申購點設定發生異常，{ex.Message}");
                return Json(new Dictionary<long, string>());
            }
        }

        [HttpPost]
        public IActionResult Export(DeliveryIndex.SearchModel condition, int? page, int? take)
        {
            condition.state = new List<string>()
            {
                UNITAPPLY_STATE.STATE.DELIVERY.ToString()
            };

            var data = _DelivaeyLogic.Search(condition, page, take);

            ExcelLogic excelLogic = new ExcelLogic(SLog);

            byte[] excelResult = excelLogic.MaterialReceipt(data.data);

            if (excelResult != null)
            {
                return File(excelResult, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"取料單_{DateTime.Now.ToString("yyyy_MMdd")}.xlsx");
            }
            else
            {
                return PartialSrcvalleyPageMessage(false, UIMessage.SYS.ServerBusy);
            }
        }
    }
}
