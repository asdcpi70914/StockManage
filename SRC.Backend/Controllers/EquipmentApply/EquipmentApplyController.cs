using Microsoft.AspNetCore.Mvc;
using SRC.Backend.Models.Brain;
using SRC.Backend.Models.Config;
using SRC.Backend.Models.Filters;
using SRC.Backend.Models.Pages;
using SRC.Backend.Models.Pages.EquipmentApply;
using SRC.Backend.Models.Pages.MinBaseStock;
using SRC.Backend.Models.Pages.UnitApply;
using SRC.Backend.Models.System;
using SRC.DB.HardCodes;
using SRC.DB.Interfaces.Equipment;
using SRC.DB.Interfaces.SubscribePoint;
using SRC.DB.Interfaces.UnitApply;
using SRC.DB.Responsibility.MinBaseStock;
using SRC.DB.Responsibility.SubscribePoint;

namespace SRC.Backend.Controllers.EquipmentApply
{
    public class EquipmentApplyController : BaseController
    {
        private SysAppsetting Setting { get; set; }
        private IDF_UnitApply DF_UnitApply { get; set; }
        private IDF_SubscribePoint DF_SubscribePoint { get; set; }
        private EquipmentApplyLogic _EquipmentApplyLogic { get; set; }

        public EquipmentApplyController(
            SysAppsetting setting,
            IHttpContextAccessor cxtAccessor,
            IDF_UnitApply dF_UnitApply,
            IDF_SubscribePoint dF_SubscribePoint,
            IConfiguration config,
            Serilog.ILogger slog) : base(cxtAccessor, config, slog)
        {
            Setting = setting;
            DF_UnitApply = dF_UnitApply;
            DF_SubscribePoint = dF_SubscribePoint;
            _EquipmentApplyLogic = new EquipmentApplyLogic(slog, DF_UnitApply);
        }

        public IActionResult Index()
        {
            EquipmentApplyIndex Model = new EquipmentApplyIndex();
            int page = 1;
            int take = Setting.PaginationTake;

            Model.SearchResultPage = _EquipmentApplyLogic.Search(new EquipmentApplyIndex.SearchModel() { state = UNITAPPLY_STATE.STATE.INIT.ToString()}, page, take);
            Model.subscribepointDic = DF_SubscribePoint.SubscribepointDic();

            return View(Model);
        }

        public IActionResult Search(EquipmentApplyIndex.SearchModel condition, int? page, int? take)
        {
            condition.state = UNITAPPLY_STATE.STATE.INIT.ToString();

            var result = _EquipmentApplyLogic.Search(condition, page, take);

            return PartialView(result);
        }

        [HttpGet]
        public IActionResult Review(long? pid)
        {
            if (!pid.HasValue)
            {
                return PartialAOE_EditFail(UIMessage.SYS.ArgumentEmpty);
            }

            var data = DF_UnitApply.GetReviewUnitApply(pid.Value);

            SRCLayoutAOE lyAoe = new SRCLayoutAOE();
            lyAoe.formId = "edit_form";
            lyAoe.Action = "Review";
            lyAoe.Controller = "EquipmentApply";
            lyAoe.View = "Review";
            lyAoe.Title = "審核器材申請";
            lyAoe.ViewSize = ModalSize.ExtraLarge;
            lyAoe.AOEObject = new EquipmentApplyEdit
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
        public async Task<IActionResult> Review(EquipmentApplyEdit model)
        {
            SRCLoginMeta meta = User.Identity.LoginMeta();

            var Ok = await _EquipmentApplyLogic.Review(model, meta.Account);

            if (Ok)
            {
                return PartialSrcvalleyPageMessage(new Models.System.SRCPageMessage()
                {
                    IsSuccess = true,
                    Message = UIMessage.SYS.Success_Review
                });
            }
            else
            {
                return PartialAOE_EditFail(_EquipmentApplyLogic.InnerMessage);
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
    }
}
