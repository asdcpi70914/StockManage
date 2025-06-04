using Microsoft.AspNetCore.Mvc;
using SRC.Backend.Models.Brain;
using SRC.Backend.Models.Config;
using SRC.Backend.Models.Pages.Distribute;
using SRC.Backend.Models.Pages;
using SRC.Backend.Models.Pages.SupplyRoomApply;
using SRC.Backend.Models.System;
using SRC.DB.HardCodes;
using SRC.DB.Interfaces.SubscribePoint;
using SRC.DB.Interfaces.SupplyRoomApply;
using SRC.DB.Responsibility.Distribute;
using SRC.DB.Responsibility.SupplyRoomApply;
using SRC.Backend.Models.Filters;
using SRC.DB.Responsibility.UnitApply;
using SRC.DB.Interfaces.UnitApply;

namespace SRC.Backend.Controllers.SupplyRoomApply
{
    public class SupplyRoomApplyController : BaseController
    {
        private SysAppsetting Setting { get; set; }
        private IDF_SubscribePoint DF_SubscribePoint { get; set; }
        private IDF_SupplyRoomApply DF_SupplyRoomApply { get; set; }
        private IDF_UnitApply DF_UnitApply {  get; set; }
        private SupplyRoomApplyLogic _SupplyRoomApplyLogic { get; set; }

        public SupplyRoomApplyController(
            SysAppsetting setting,
            IHttpContextAccessor cxtAccessor,
            IConfiguration config,
            IDF_SupplyRoomApply dF_SupplyRoomApply,
            IDF_SubscribePoint dF_SubscribePoint,
            IDF_UnitApply dF_UnitApply,
            Serilog.ILogger slog) : base(cxtAccessor, config, slog)
        {
            Setting = setting;
            DF_SubscribePoint = dF_SubscribePoint;
            DF_SupplyRoomApply = dF_SupplyRoomApply;
            DF_UnitApply = dF_UnitApply;
            _SupplyRoomApplyLogic = new SupplyRoomApplyLogic(slog, DF_SupplyRoomApply);
        }

        public IActionResult Index()
        {
            SupplyRoomApplyIndex Model = new SupplyRoomApplyIndex();
            int page = 1;
            int take = Setting.PaginationTake;

            Model.subscribepointDic = DF_SubscribePoint.SubscribepointDic();
            Model.SearchResultPage = _SupplyRoomApplyLogic.Search(new SupplyRoomApplyIndex.SearchModel() { state = UNITAPPLY_STATE.STATE.DISTRIBUTE.ToString() }, page, take);

            return View(Model);
        }

        public IActionResult Search(SupplyRoomApplyIndex.SearchModel condition, int? page, int? take)
        {
            SupplyRoomApplySearch Model = new SupplyRoomApplySearch();

            condition.state = UNITAPPLY_STATE.STATE.DISTRIBUTE.ToString();

            Model = _SupplyRoomApplyLogic.Search(condition, page, take);

            return PartialView(Model);
        }

        [HttpGet]
        public IActionResult Edit(long? pid)
        {
            if (!pid.HasValue)
            {
                return PartialAOE_EditFail(UIMessage.SYS.ArgumentEmpty);
            }

            var data = DF_SupplyRoomApply.GetDistributeUnitApply(pid.Value);

            SRCLayoutAOE lyAoe = new SRCLayoutAOE();
            lyAoe.formId = "edit_form";
            lyAoe.Action = "Edit";
            lyAoe.Controller = "SupplyRoomApply";
            lyAoe.View = "Edit";
            lyAoe.Title = "供應室申請及撤銷";
            lyAoe.ViewSize = ModalSize.ExtraLarge;
            lyAoe.SaveButtonDesc = "通過";
            lyAoe.AOEObject = new SupplyRoomApplyEdit
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
        public async Task<IActionResult> Edit(SupplyRoomApplyEdit model)
        {
            SRCLoginMeta meta = User.Identity.LoginMeta();

            var Ok = await _SupplyRoomApplyLogic.Edit(model.pid, meta.Account);

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
                return PartialAOE_EditFail(_SupplyRoomApplyLogic.InnerMessage);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(List<long> pid)
        {
            SRCLoginMeta meta = User.Identity.LoginMeta();


            if (pid == null || pid.Count < 0)
            {
                return PartialSrcvalleyPageMessage(new SRCPageMessage()
                {
                    IsSuccess = false,
                    Message = UIMessage.SYS.ArgumentEmpty
                });
            }


            SRCPageMessage page = new SRCPageMessage();
            try
            {
                await DF_SupplyRoomApply.Cancel(pid, meta.Account);

                page.IsSuccess = true;
                page.Message = UIMessage.SYS.Success_Cancel;
            }
            catch (Exception ex)
            {
                page.IsSuccess = false;
                page.Message = UIMessage.SYS.Fail_Cancel;
                SLog.Fatal(ex, $"{page.Message},{ex.Message}");
            }


            return PartialSrcvalleyPageMessage(page);
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
