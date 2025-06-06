using Microsoft.AspNetCore.Mvc;
using SRC.Backend.Models.Brain;
using SRC.Backend.Models.Config;
using SRC.Backend.Models.Filters;
using SRC.Backend.Models.Pages.PayTreasury;
using SRC.Backend.Models.Pages.UnitUseReport;
using SRC.Backend.Models.System;
using SRC.DB.Interfaces.Settings;
using SRC.DB.Interfaces.SubscribePoint;
using SRC.DB.Interfaces.UnitApply;
using SRC.DB.Interfaces.Users;
using SRC.DB.Responsibility.Users;

namespace SRC.Backend.Controllers.UnitUseReport
{
    public class UnitUseReportController : BaseController
    {
        private SysAppsetting Setting { get; set; }
        private IDF_SubscribePoint DF_SubscribePoint { get; set; }
        private UnitUseReportLogic _UnitUseReportLogic { get; set; }
        private IDF_UnitApply DF_UnitApply { get; set; }
        private IDF_BackendUser DF_BackendUser { get; set; }
        private IDF_SystemCode DF_SystemCode { get; set; }

        public UnitUseReportController(
            SysAppsetting setting,
            IHttpContextAccessor cxtAccessor,
            IConfiguration config,
            IDF_SubscribePoint dF_SubscribePoint,
            IDF_UnitApply dF_UnitApply,
            IDF_BackendUser dF_BackendUser,
            IDF_SystemCode dF_SystemCode,
            Serilog.ILogger slog) : base(cxtAccessor, config, slog)
        {
            Setting = setting;
            DF_SubscribePoint = dF_SubscribePoint;
            DF_UnitApply = dF_UnitApply;
            DF_BackendUser = dF_BackendUser;
            DF_SystemCode = dF_SystemCode;
            _UnitUseReportLogic = new UnitUseReportLogic(slog, DF_UnitApply);
        }

        public IActionResult Index()
        {
            UnitUseReportIndex Model = new UnitUseReportIndex();

            int page = 1;
            int take = Setting.PaginationTake;

            Model.SearchResultPage = _UnitUseReportLogic.Search(new UnitUseReportIndex.SearchModel(), page, take);

            return View(Model);
        }

        public IActionResult Search(UnitUseReportIndex.SearchModel condition,int? page,int? take)
        {
            UnitUseReportSearch Model = new UnitUseReportSearch();

            Model = _UnitUseReportLogic.Search(condition, page, take);

            return PartialView(Model);
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
        public IActionResult Export(UnitUseReportIndex.SearchModel condition, int? page, int? take)
        {
            var data = _UnitUseReportLogic.Search(condition, page, take);

            ExcelLogic excelLogic = new ExcelLogic(SLog);

            byte[] excelResult = excelLogic.UnitUseReport(data.data,DF_SystemCode, DF_BackendUser);

            if (excelResult != null)
            {
                return File(excelResult, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"使用單位年報表_{DateTime.Now.ToString("yyyy_MMdd")}.xlsx");
            }
            else
            {
                return PartialSrcvalleyPageMessage(false, UIMessage.SYS.ServerBusy);
            }
        }
    }
}
