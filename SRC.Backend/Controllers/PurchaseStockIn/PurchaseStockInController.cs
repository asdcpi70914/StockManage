using Microsoft.AspNetCore.Mvc;
using SRC.Backend.Models.Brain;
using SRC.Backend.Models.Config;
using SRC.Backend.Models.Filters;
using SRC.Backend.Models.Pages.Equipment;
using SRC.Backend.Models.Pages;
using SRC.Backend.Models.Pages.PurchaseStockIn;
using SRC.Backend.Models.System;
using SRC.DB.Interfaces.Equipment;
using SRC.DB.Interfaces.PurchaseStockIn;
using SRC.DB.Interfaces.UnitApply;
using SRC.DB.Responsibility.Equipment;
using SRC.DB.Responsibility.PurchaseStockIn;
using SRC.DB.Responsibility.UnitApply;

namespace SRC.Backend.Controllers.PurchaseStockIn
{
    public class PurchaseStockInController : BaseController
    {
        private SysAppsetting Setting { get; set; }
        private IDF_PurchaseStockIn DF_PurchaseStockIn { get; set; }
        private IDF_UnitApply DF_UnitApply { get; set; }
        private PurchaseStockInLogic _PurchaseStockInLogic { get; set; }

        public PurchaseStockInController(
            SysAppsetting setting,
            IHttpContextAccessor cxtAccessor,
            IConfiguration config,
            IDF_PurchaseStockIn dF_PurchaseStockIn,
            IDF_UnitApply dF_UnitApply,
            Serilog.ILogger slog) : base(cxtAccessor, config, slog)
        {
            Setting = setting;
            DF_PurchaseStockIn = dF_PurchaseStockIn;
            DF_UnitApply = dF_UnitApply;
            _PurchaseStockInLogic = new PurchaseStockInLogic(slog, DF_PurchaseStockIn);
        }

        public IActionResult Index()
        {
            PurchaseStockInIndex Model = new PurchaseStockInIndex();

            int page = 1;
            int take = Setting.PaginationTake;

            Model.SearchResultPage = _PurchaseStockInLogic.Search(new PurchaseStockInIndex.SearchModel(), page, take);

            return View(Model);
        }

        public IActionResult Search(PurchaseStockInIndex.SearchModel condition,int? page,int? take)
        {
            PurchaseStockInSearch Model = new PurchaseStockInSearch();

            Model = _PurchaseStockInLogic.Search(condition, page, take);

            return PartialView(Model);
        }

        [HttpGet]
        public IActionResult StockIn(long? pid)
        {
            if (!pid.HasValue)
            {
                return PartialAOE_EditFail(UIMessage.SYS.ArgumentEmpty);
            }

            var data = DF_PurchaseStockIn.GetPurchaseStockIn(pid.Value);

            SRCLayoutAOE lyAoe = new SRCLayoutAOE();
            lyAoe.formId = "edit_form";
            lyAoe.Action = "StockIn";
            lyAoe.Controller = "PurchaseStockIn";
            lyAoe.View = "Edit";
            lyAoe.Title = "採購入庫作業";
            lyAoe.AOEObject = new PurchaseStockInEdit
            {
                pid = data.pid,
                old_stock = data.stock,
                Action = ActionMode.EDIT
            };

            return PartialAOE(lyAoe);
        }

        [HttpPost]
        public async Task<IActionResult> StockIn(PurchaseStockInEdit model)
        {
            SRCLoginMeta meta = User.Identity.LoginMeta();

            var Ok = await _PurchaseStockInLogic.StockIn(model.pid, model.stock,meta.Account);

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
                return PartialAOE_EditFail(_PurchaseStockInLogic.InnerMessage);
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
