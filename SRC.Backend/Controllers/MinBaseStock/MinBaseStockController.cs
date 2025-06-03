using Microsoft.AspNetCore.Mvc;
using SRC.Backend.Models.Brain;
using SRC.Backend.Models.Config;
using SRC.Backend.Models.Filters;
using SRC.Backend.Models.Pages;
using SRC.Backend.Models.Pages.MinBaseStock;
using SRC.Backend.Models.Pages.SubscribePoint;
using SRC.Backend.Models.System;
using SRC.DB.Interfaces.MinBaseStock;
using SRC.DB.Interfaces.SubscribePoint;
using SRC.DB.Responsibility.MinBaseStock;
using SRC.DB.Responsibility.SubscribePoint;

namespace SRC.Backend.Controllers.MinBaseStock
{
    public class MinBaseStockController : BaseController
    {
        private SysAppsetting Setting { get; set; }
        private IDF_MinBaseStock DF_MinBaseStock { get; set; }
        private IDF_SubscribePoint DF_SubscribePoint { get; set; }
        private MinBaseStockLogic _MinBaseStockLogic { get; set; }

        public MinBaseStockController(
            SysAppsetting setting,
            IHttpContextAccessor cxtAccessor,
            IConfiguration config,
            IDF_MinBaseStock dF_MinBaseStock,
            IDF_SubscribePoint dF_SubscribePoint,
            Serilog.ILogger slog) : base(cxtAccessor, config, slog)
        {
            Setting = setting;
            DF_MinBaseStock = dF_MinBaseStock;
            _MinBaseStockLogic = new MinBaseStockLogic(slog, DF_MinBaseStock);
            DF_SubscribePoint = dF_SubscribePoint;
        }

        public IActionResult Index()
        {
            MinBaseStockIndex Model = new MinBaseStockIndex();
            int page = 1;
            int take = Setting.PaginationTake;

            Model.SearchResultPage = _MinBaseStockLogic.Search(new MinBaseStockIndex.SearchModel(), page, take);
            Model.subscribepointDic = DF_SubscribePoint.SubscribepointDic();

            return View(Model);
        }

        public IActionResult Search(MinBaseStockIndex.SearchModel condition,int? page,int? take)
        {

            var result = _MinBaseStockLogic.Search(condition, page, take);

            return PartialView(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(MinBaseStockEdit Model)
        {
            SRCLoginMeta meta = User.Identity.LoginMeta();

            var Ok = await _MinBaseStockLogic.Create(Model, meta.Account);

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
                return PartialAOE_ADDFail(_MinBaseStockLogic.InnerMessage);
            }

        }

        [HttpGet]
        public IActionResult Edit(long? pid)
        {
            if (!pid.HasValue)
            {
                return PartialAOE_EditFail(UIMessage.SYS.ArgumentEmpty);
            }

            var data = DF_MinBaseStock.GetMinBaseStockSubscribeSetting(pid.Value);

            SRCLayoutAOE lyAoe = new SRCLayoutAOE();
            lyAoe.formId = "edit_form";
            lyAoe.Action = "Edit";
            lyAoe.Controller = "MinBaseStock";
            lyAoe.View = "Edit";
            lyAoe.Title = "修改基準存量與申購點設定資料";
            lyAoe.AOEObject = new MinBaseStockEdit
            {
                pid = data.pid,
                subscribepoint_pid = data.subscribepoint_pid,
                sub_pid = data.sub_pid,
                min_base_stock = data.min_base_stock,
                type = data.type,
                Action = ActionMode.EDIT
            };

            return PartialAOE(lyAoe);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(MinBaseStockEdit Model)
        {
            SRCLoginMeta meta = User.Identity.LoginMeta();

            var Ok = await _MinBaseStockLogic.Edit(Model, meta.Account);

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
                return PartialAOE_EditFail(_MinBaseStockLogic.InnerMessage);
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
                await DF_MinBaseStock.Delete(pid);

                page.IsSuccess = true;
                page.Message = UIMessage.SYS.Success_Delete;
            }
            catch (Exception ex)
            {
                page.IsSuccess = false;
                page.Message = UIMessage.SYS.Fail_Delete;
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
                var result = DF_MinBaseStock.MinBaseStockSubscribeSettingDropDown(type);

                return Json(result);
            }
            catch(Exception ex)
            {
                SLog.Fatal(ex, $"取得裝備/器材下拉選單發生異常，{ex.Message}");
                return Json(new Dictionary<long, string>());
            }
        }
    }
}
