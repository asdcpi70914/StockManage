using Microsoft.AspNetCore.Mvc;
using SRC.Backend.Models.Brain;
using SRC.Backend.Models.Config;
using SRC.Backend.Models.Filters;
using SRC.Backend.Models.Pages.Equipment;
using SRC.Backend.Models.Pages;
using SRC.Backend.Models.Pages.FactoryInfo;
using SRC.Backend.Models.System;
using SRC.DB.Interfaces.Equipment;
using SRC.DB.Interfaces.FactoryInfo;
using SRC.DB.Interfaces.Settings;
using SRC.DB.Responsibility.Equipment;
using SRC.DB.Responsibility.Settings;

namespace SRC.Backend.Controllers.FactoryInfo
{
    public class FactoryInfoController : BaseController
    {
        private SysAppsetting Setting { get; set; }
        private FactoryInfoLogic _FactoryInfoLogic { get; set; }

        private IDF_FactoryInfo DF_FactoryInfo { get; set; }

        private IDF_SystemCode DF_SystemCode { get; set; }

        public FactoryInfoController(
            SysAppsetting setting,
            IHttpContextAccessor cxtAccessor,
            IConfiguration config,
            IDF_FactoryInfo dF_FactoryInfo,
            IDF_SystemCode dF_SystemCode,
            Serilog.ILogger slog) : base(cxtAccessor, config, slog)
        {
            Setting = setting;
            DF_FactoryInfo = dF_FactoryInfo;
            DF_SystemCode = dF_SystemCode;
            _FactoryInfoLogic = new FactoryInfoLogic(slog, DF_FactoryInfo);
        }

        public IActionResult Index()
        {
            FactoryInfoIndex Model = new FactoryInfoIndex();

            int page = 1;
            int take = Setting.PaginationTake;

            Model.SearchResultPage = _FactoryInfoLogic.Search(new FactoryInfoIndex.SearchModel(), page, take);

            return View(Model);
        }

        public IActionResult Search(FactoryInfoIndex.SearchModel condition,int? page,int? take)
        {
            FactoryInfoSearch Model = new FactoryInfoSearch();

            Model = _FactoryInfoLogic.Search(condition, page, take);

            return PartialView(Model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(FactoryInfoEdit model)
        {
            SRCLoginMeta LoginMeta = User.Identity.LoginMeta();

            var Ok = await _FactoryInfoLogic.Create(model, LoginMeta.Account);

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
                return PartialAOE_ADDFail(_FactoryInfoLogic.InnerMessage);
            }
        }

        [HttpGet]
        public IActionResult Edit(long? pid)
        {
            if (!pid.HasValue)
            {
                return PartialAOE_EditFail(UIMessage.SYS.ArgumentEmpty);
            }

            var data = DF_FactoryInfo.GetFactoryInfo(pid.Value);

            SRCLayoutAOE lyAoe = new SRCLayoutAOE();
            lyAoe.formId = "edit_form";
            lyAoe.Action = "Edit";
            lyAoe.Controller = "FactoryInfo";
            lyAoe.View = "Edit";
            lyAoe.Title = "修改廠商基本資料";
            lyAoe.AOEObject = new FactoryInfoEdit
            {
                pid = data.pid,
                name = data.name,
                company_phone = data.contact_phone,
                company_number = data.company_number,
                city = data.city,
                town = data.town,
                address = data.address,  
                Action = ActionMode.EDIT,
                Towns = DF_SystemCode.ListTownCode(data.city).ToDictionary(x => x.TownCode,x => x.TownName)
            };

            return PartialAOE(lyAoe);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(FactoryInfoEdit model)
        {
            SRCLoginMeta LoginMeta = User.Identity.LoginMeta();
            model.Action = ActionMode.EDIT;

            var Ok = await _FactoryInfoLogic.Edit(model, LoginMeta.Account);

            if (Ok)
            {
                return PartialSrcvalleyPageMessage(new Models.System.SRCPageMessage()
                {
                    IsSuccess = true,
                    Message = UIMessage.SYS.Success_Edit
                });
            }
            else
            {
                return PartialAOE_EditFail(_FactoryInfoLogic.InnerMessage);
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
                await DF_FactoryInfo.Delete(pid);

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
        public IActionResult ChangeTown(string code)
        {
            var result = DF_SystemCode.ListTownCode(code);

            return Json(result.ToDictionary(x => x.TownCode, x => x.TownName));
        }
    }
}
