using Microsoft.AspNetCore.Mvc;
using SRC.Backend.Models.Brain;
using SRC.Backend.Models.Config;
using SRC.Backend.Models.Filters;
using SRC.Backend.Models.Pages;
using SRC.Backend.Models.Pages.Distribute;
using SRC.Backend.Models.Pages.EquipmentApply;
using SRC.Backend.Models.System;
using SRC.DB.HardCodes;
using SRC.DB.Interfaces.Distribute;
using SRC.DB.Interfaces.Equipment;
using SRC.DB.Interfaces.SubscribePoint;
using SRC.DB.Interfaces.UnitApply;
using SRC.DB.Responsibility.SubscribePoint;
using SRC.DB.Responsibility.UnitApply;

namespace SRC.Backend.Controllers.Distribute
{
    public class DistributeController : BaseController
    {
        private SysAppsetting Setting { get; set; }
        private DistributeLogic _DistributeLogic { get; set; }
        private IDF_Distribute DF_Distribute {  get; set; }
        private IDF_SubscribePoint DF_SubscribePoint { get; set; }

        private IDF_UnitApply DF_UnitApply { get; set; }

        public DistributeController(
            SysAppsetting setting,
            IHttpContextAccessor cxtAccessor,
            IConfiguration config,
            IDF_Distribute dF_Distribute,
            IDF_SubscribePoint dF_SubscribePoint,
            IDF_UnitApply dF_UnitApply,
            Serilog.ILogger slog) : base(cxtAccessor, config, slog)
        {
            Setting = setting;
            DF_Distribute = dF_Distribute;
            DF_SubscribePoint = dF_SubscribePoint;
            DF_UnitApply = dF_UnitApply;
            _DistributeLogic = new DistributeLogic(slog, DF_Distribute);
        }

        public IActionResult Index()
        {
            DistributeIndex Model = new DistributeIndex();
            int page = 1;
            int take = Setting.PaginationTake;

            Model.subscribepointDic = DF_SubscribePoint.SubscribepointDic();
            Model.SearchResultPage = _DistributeLogic.Search(new DistributeIndex.SearchModel() { state = UNITAPPLY_STATE.STATE.REVIEW_OK.ToString()}, page, take);

            return View(Model);
        }

        public IActionResult Search(DistributeIndex.SearchModel condition,int? page,int? take)
        {
            DistributeSearch Model = new DistributeSearch();

            condition.state = UNITAPPLY_STATE.STATE.REVIEW_OK.ToString();

            Model = _DistributeLogic.Search(condition, page, take);

            return PartialView(Model);
        }

        [HttpGet]
        public IActionResult Distribute(long? pid)
        {
            if (!pid.HasValue)
            {
                return PartialAOE_EditFail(UIMessage.SYS.ArgumentEmpty);
            }

            var data = DF_Distribute.GetDistributeUnitApply(pid.Value);

            SRCLayoutAOE lyAoe = new SRCLayoutAOE();
            lyAoe.formId = "edit_form";
            lyAoe.Action = "Distribute";
            lyAoe.Controller = "Distribute";
            lyAoe.View = "Edit";
            lyAoe.Title = "器材/裝備撥發";
            lyAoe.ViewSize = ModalSize.ExtraLarge;
            lyAoe.SaveButtonDesc = "撥發";
            lyAoe.AOEObject = new DistributeEdit
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
        public async Task<IActionResult> Distribute(DistributeEdit model)
        {
            SRCLoginMeta meta = User.Identity.LoginMeta();

            var Ok = await _DistributeLogic.Distribute(model.pid, meta.Account);

            if (Ok)
            {
                return PartialSrcvalleyPageMessage(new Models.System.SRCPageMessage()
                {
                    IsSuccess = true,
                    Message = UIMessage.SYS.Success_Distribute
                });
            }
            else
            {
                return PartialAOE_EditFail(_DistributeLogic.InnerMessage);
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
