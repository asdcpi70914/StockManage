using Microsoft.AspNetCore.Mvc;
using SRC.Backend.Models.Brain;
using SRC.Backend.Models.Config;
using SRC.Backend.Models.Filters;
using SRC.Backend.Models.Pages.EquipmentApply;
using SRC.Backend.Models.Pages;
using SRC.Backend.Models.Pages.UnitApply;
using SRC.Backend.Models.System;
using SRC.DB.Interfaces.SubscribePoint;
using SRC.DB.Interfaces.UnitApply;
using SRC.DB.Responsibility.Equipment;
using SRC.DB.Responsibility.MinBaseStock;

namespace SRC.Backend.Controllers.UnitApply
{
    public class UnitApplyController : BaseController
    {
        private SysAppsetting Setting { get; set; }
        private IDF_SubscribePoint DF_SubscribePoint { get; set; }
        private UnitApplyLogic _UnitApplyLogic { get; set; }
        private IDF_UnitApply DF_UnitApply { get; set; }

        public UnitApplyController(
            SysAppsetting setting,
            IHttpContextAccessor cxtAccessor,
            IConfiguration config,
            IDF_SubscribePoint dF_SubscribePoint,
            IDF_UnitApply dF_UnitApply,
            Serilog.ILogger slog) : base(cxtAccessor, config, slog)
        {
            Setting = setting;
            DF_SubscribePoint = dF_SubscribePoint;
            DF_UnitApply = dF_UnitApply;
            _UnitApplyLogic = new UnitApplyLogic(slog, DF_UnitApply);
        }

        public IActionResult Index()
        {
            UnitApplyIndex Model = new UnitApplyIndex();
            SRCLoginMeta meta = User.Identity.LoginMeta();
            int page = 1;
            int take = Setting.PaginationTake;

            Model.SearchResultPage = _UnitApplyLogic.Search(new UnitApplyIndex.SearchModel() { unit = meta.Unit }, page, take);
            Model.subscribepointDic = DF_SubscribePoint.SubscribepointDic();

            return View(Model);
        }

        public IActionResult Search(UnitApplyIndex.SearchModel condition,int? page,int? take)
        {
            SRCLoginMeta meta = User.Identity.LoginMeta();

            condition.unit = meta.Unit;

            var result = _UnitApplyLogic.Search(condition, page, take);

            return PartialView(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UnitApplyEdit model)
        {
            SRCLoginMeta meta = User.Identity.LoginMeta();

            var Ok = await _UnitApplyLogic.Create(model, meta.Account, meta.Unit);

            if (Ok)
            {
                return PartialSrcvalleyPageMessage(new Models.System.SRCPageMessage()
                {
                    IsSuccess = true,
                    Message = UIMessage.SYS.Success_Add
                });
            }
            else
            {
                return PartialAOE_ADDFail(_UnitApplyLogic.InnerMessage);
            }
        }

        [HttpGet]
        public IActionResult Edit(long? pid)
        {
            if (!pid.HasValue)
            {
                return PartialAOE_EditFail(UIMessage.SYS.ArgumentEmpty);
            }

            var data = DF_UnitApply.GetUnitApply(pid.Value);

            SRCLayoutAOE lyAoe = new SRCLayoutAOE();
            lyAoe.formId = "edit_form";
            lyAoe.Action = "Edit";
            lyAoe.Controller = "UnitApply";
            lyAoe.View = "Edit";
            lyAoe.Title = "審核器材申請";
            lyAoe.ViewSize = ModalSize.ExtraLarge;
            lyAoe.AOEObject = new UnitApplyEdit
            {
                pid = data.pid,
                subscribepoint = data.subscribepoint,
                setting_pid = data.setting_pid,
                RemainingStock = data.RemainingStock,
                apply_amount = data.apply_amount,
                type = data.type,
                Action = ActionMode.EDIT
            };

            return PartialAOE(lyAoe);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UnitApplyEdit model)
        {
            SRCLoginMeta meta = User.Identity.LoginMeta();

            var Ok = await _UnitApplyLogic.Edit(model, meta.Account);

            if (Ok)
            {
                return PartialSrcvalleyPageMessage(new Models.System.SRCPageMessage()
                {
                    IsSuccess = true,
                    Message = UIMessage.SYS.OK_Edit
                });
            }
            else
            {
                return PartialAOE_EditFail(_UnitApplyLogic.InnerMessage);
            }
        }


        [HttpPost]
        [FuncAlias(AliasName = "Search,Create,Edit")]
        public IActionResult ChangeType(string type)
        {
            SRCLoginMeta meta = User.Identity.LoginMeta();
            try
            {
                var result = DF_UnitApply.DicMinBaseStoc(type, meta.Unit);

                return Json(result);
            }
            catch (Exception ex)
            {
                SLog.Fatal(ex, $"取得裝備/器材基準存量與申購點設定發生異常，{ex.Message}");
                return Json(new Dictionary<long, string>());
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(List<long> pid)
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
                await DF_UnitApply.Delete(pid, meta.Account);

                page.IsSuccess = true;
                page.Message = UIMessage.SYS.Success_Cancel;
            }
            catch (Exception ex)
            {
                page.IsSuccess = false;
                page.Message = UIMessage.SYS.Fail_Cancel + "，" + ex.Message;
                SLog.Fatal(ex, $"{page.Message},{ex.Message}");
            }


            return PartialSrcvalleyPageMessage(page);
        }

        [HttpPost]
        [FuncAlias(AliasName = "Create,Edit")]
        public IActionResult GetRemainingStock(string type,long pid)
        {
            try
            {
                var result = DF_UnitApply.GetRemainingStock(type, pid);

                return Json(result);
            }
            catch (Exception ex)
            {
                SLog.Fatal(ex, $"取得裝備/器材剩餘庫存發生異常發生異常，{ex.Message}");
                return Json(new Dictionary<long, string>());
            }
        }
    }
}
