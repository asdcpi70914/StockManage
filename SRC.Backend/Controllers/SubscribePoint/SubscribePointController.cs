using Microsoft.AspNetCore.Mvc;
using SRC.Backend.Models.Brain;
using SRC.Backend.Models.Config;
using SRC.Backend.Models.Pages.Equipment;
using SRC.Backend.Models.Pages;
using SRC.Backend.Models.Pages.SubscribePoint;
using SRC.Backend.Models.System;
using SRC.DB.Interfaces.Equipment;
using SRC.DB.Interfaces.SubscribePoint;
using SRC.DB.Responsibility.Equipment;
using SRC.DB.Responsibility.SubscribePoint;

namespace SRC.Backend.Controllers.SubscribePoint
{
    public class SubscribePointController : BaseController
    {
        private SysAppsetting Setting { get; set; }
        private SubscribePointLogic _SubscribePointLogic { get; set; }
        private IDF_SubscribePoint DF_SubscribePoint { get; set; }

        public SubscribePointController(
            SysAppsetting setting,
            IHttpContextAccessor cxtAccessor,
            IConfiguration config,
            IDF_SubscribePoint dF_SubscribePoint,
            Serilog.ILogger slog) : base(cxtAccessor, config, slog)
        {
            Setting = setting;
            DF_SubscribePoint = dF_SubscribePoint;
            _SubscribePointLogic = new SubscribePointLogic(slog, DF_SubscribePoint);
        }

        public IActionResult Index()
        {
            SubscribePointIndex Model = new SubscribePointIndex();
            int page = 1;
            int take = Setting.PaginationTake;

            Model.SearchResultModel = _SubscribePointLogic.Search(new SubscribePointIndex.SearchModel(), page, take, out int rowtotal);


            return View(Model);
        }

        public IActionResult Search(SubscribePointIndex.SearchModel condition,int? page,int? take)
        {

            var result = _SubscribePointLogic.Search(condition, page, take, out int rowtotal);


            return PartialView(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SubscribePointEdit model)
        {
            SRCLoginMeta meta = User.Identity.LoginMeta();

            var Ok = await _SubscribePointLogic.Create(model, meta.Account);

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
                return PartialAOE_ADDFail(_SubscribePointLogic.InnerMessage);
            }
        }

        [HttpGet]
        public IActionResult Edit(long? pid)
        {
            if (!pid.HasValue)
            {
                return PartialAOE_EditFail(UIMessage.SYS.ArgumentEmpty);
            }

            var data = DF_SubscribePoint.GetSubscribePoint(pid.Value);

            SRCLayoutAOE lyAoe = new SRCLayoutAOE();
            lyAoe.formId = "edit_form";
            lyAoe.Action = "Edit";
            lyAoe.Controller = "SubscribePoint";
            lyAoe.View = "Edit";
            lyAoe.Title = "修改申購點主檔資料";
            lyAoe.AOEObject = new SubscribePointEdit
            {
                pid = data.pid,
                name = data.name,
                Action = ActionMode.EDIT
            };

            return PartialAOE(lyAoe);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SubscribePointEdit model)
        {

            SRCLoginMeta meta = User.Identity.LoginMeta();

            var Ok = await _SubscribePointLogic.Edit(model, meta.Account);

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
                return PartialAOE_EditFail(_SubscribePointLogic.InnerMessage);
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
                await DF_SubscribePoint.Delete(pid);

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
    }
}
