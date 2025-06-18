using Microsoft.AspNetCore.Mvc;
using SRC.Backend.Models.Brain;
using SRC.Backend.Models.Config;
using SRC.Backend.Models.Pages.ExistingStock;
using SRC.Backend.Models.Pages.UnitUseReport;
using SRC.Backend.Models.System;
using SRC.DB.Interfaces.ExistingStock;
using SRC.DB.Interfaces.MinBaseStock;
using SRC.DB.Interfaces.Settings;
using SRC.DB.Interfaces.SubscribePoint;
using SRC.DB.Interfaces.UnitApply;
using SRC.DB.Interfaces.Users;
using SRC.DB.Responsibility.ExistingStock;
using SRC.DB.Responsibility.Settings;
using SRC.DB.Responsibility.SubscribePoint;
using SRC.DB.Responsibility.Users;

namespace SRC.Backend.Controllers.ExistingStockList
{
    public class ExistingStockListController : BaseController
    {
        private SysAppsetting Setting { get; set; }
        private ExistingStockLogic _ExistingStockLogic { get; set; }
        private IDF_ExistingStock DF_ExistingStock { get; set; }
        private IDF_MinBaseStock DF_MinBaseStock { get; set; }
        private IDF_SubscribePoint DF_SubscribePoint { get; set; }

        public ExistingStockListController(
            SysAppsetting setting,
            IHttpContextAccessor cxtAccessor,
            IConfiguration config,
            IDF_ExistingStock dF_ExistingStock,
            IDF_MinBaseStock dF_MinBaseStock,
            IDF_SubscribePoint dF_SubscribePoint,
            Serilog.ILogger slog) : base(cxtAccessor, config, slog)
        {
            Setting = setting;
            DF_ExistingStock = dF_ExistingStock;
            _ExistingStockLogic = new ExistingStockLogic(slog, DF_ExistingStock);
            DF_MinBaseStock = dF_MinBaseStock;
            DF_SubscribePoint = dF_SubscribePoint;
        }

        public IActionResult Index()
        {
            ExistingStockIndex Model = new ExistingStockIndex();

            int page = 1;
            int take = Setting.PaginationTake;

            Model.SearchResultPage = _ExistingStockLogic.Search(new ExistingStockIndex.SearchModel(),page,take);

            return View(Model);
        }

        public IActionResult Search(ExistingStockIndex.SearchModel condition,int? page,int? take)
        {
            ExistingStockSearch Model = new ExistingStockSearch();

            Model = _ExistingStockLogic.Search(condition, page, take);

            return PartialView(Model);
        }

        [HttpPost]
        public IActionResult Export(ExistingStockIndex.SearchModel condition, int? page, int? take)
        {
            var data = _ExistingStockLogic.Search(condition, page, take);

            ExcelLogic excelLogic = new ExcelLogic(SLog);

            SRCLoginMeta meta = User.Identity.LoginMeta();

            byte[] excelResult = excelLogic.ExistingStockReport(data.Data,DF_MinBaseStock,DF_SubscribePoint, meta.Unit);

            if (excelResult != null)
            {
                return File(excelResult, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"現有存量清冊_{DateTime.Now.ToString("yyyy_MMdd")}.xlsx");
            }
            else
            {
                return PartialSrcvalleyPageMessage(false, UIMessage.SYS.ServerBusy);
            }
        }
    }
}
