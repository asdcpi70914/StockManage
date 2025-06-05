using Microsoft.AspNetCore.Mvc;
using SRC.Backend.Models.Brain;
using SRC.Backend.Models.Config;
using SRC.Backend.Models.Filters;
using SRC.Backend.Models.Pages;
using SRC.Backend.Models.Pages.PayTreasury;
using SRC.Backend.Models.Pages.UnitApply;
using SRC.Backend.Models.System;
using SRC.DB.Interfaces.PayTreasury;
using SRC.DB.Interfaces.PurchaseStockIn;
using SRC.DB.Interfaces.SubscribePoint;
using SRC.DB.Interfaces.UnitApply;
using SRC.DB.Responsibility.SubscribePoint;
using SRC.DB.Responsibility.UnitApply;

namespace SRC.Backend.Controllers.PayTreasury
{
    public class PayTreasuryController : BaseController
    {
        private SysAppsetting Setting { get; set; }
        private PayTreasuryLogic _PayTreasuryLogic { get; set; }
        private IDF_PayTreasury DF_PayTreasury { get; set; }
        private IDF_SubscribePoint DF_SubscribePoint { get; set; }
        private IDF_UnitApply DF_UnitApply { get; set; }

        public PayTreasuryController(
            SysAppsetting setting,
            IHttpContextAccessor cxtAccessor,
            IConfiguration config,
            IDF_PayTreasury dF_PayTreasury,
            IDF_SubscribePoint dF_SubscribePoint,
            IDF_UnitApply dF_UnitApply,
            Serilog.ILogger slog) : base(cxtAccessor, config, slog)
        {
            Setting = setting;
            DF_PayTreasury = dF_PayTreasury;
            _PayTreasuryLogic = new PayTreasuryLogic(slog, DF_PayTreasury);
            DF_SubscribePoint = dF_SubscribePoint;
            DF_UnitApply = dF_UnitApply;
        }

        public IActionResult Index()
        {
            PayTreasuryIndex Model = new PayTreasuryIndex();

            int page = 1;
            int take = Setting.PaginationTake;

            Model.SearchResultPage = _PayTreasuryLogic.Search(new PayTreasuryIndex.SearchModel(), page, take);
            Model.subscribepointDic = DF_SubscribePoint.SubscribepointDic();

            return View(Model);
        }

        public IActionResult Search(PayTreasuryIndex.SearchModel condition,int? page,int? take)
        {
            PayTreasurySearch Model = new PayTreasurySearch();

            Model = _PayTreasuryLogic.Search(condition, page, take);

            return PartialView(Model);
        }

        [HttpGet]
        public IActionResult Edit(long? pid)
        {
            if (!pid.HasValue)
            {
                return PartialAOE_EditFail(UIMessage.SYS.ArgumentEmpty);
            }

            var data = DF_PayTreasury.GetPayTreasury(pid.Value);

            SRCLayoutAOE lyAoe = new SRCLayoutAOE();
            lyAoe.formId = "edit_form";
            lyAoe.Action = "Edit";
            lyAoe.Controller = "PayTreasury";
            lyAoe.View = "Edit";
            lyAoe.Title = "器材繳庫作業";
            lyAoe.AOEObject = new PayTreasuryEdit
            {
                pid = data.pid,
                apply_amount = data.apply_amount,
                stock = data.stock,
                already_pay_amount = data.already_pay_amount,
                Action = ActionMode.EDIT
            };

            return PartialAOE(lyAoe);
        }

        [HttpPost]
        public async Task<IActionResult>Edit(PayTreasuryEdit model)
        {
            SRCLoginMeta meta = User.Identity.LoginMeta();

            var Ok = await _PayTreasuryLogic.Edit(model, meta.Account);

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
                return PartialAOE_EditFail(_PayTreasuryLogic.InnerMessage);
            }
        }

        [HttpPost]
        public IActionResult Export(PayTreasuryIndex.SearchModel condition, int? page, int? take)
        {
            var data = _PayTreasuryLogic.Search(condition, page, take);

            ExcelLogic excelLogic = new ExcelLogic(SLog);

            byte[] excelResult = excelLogic.EquipmentConsumptionList(data.Data);

            if (excelResult != null)
            {
                return File(excelResult, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"器材消耗清冊_{DateTime.Now.ToString("yyyy_MMdd")}.xlsx");
            }
            else
            {
                return PartialSrcvalleyPageMessage(false, UIMessage.SYS.ServerBusy);
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
