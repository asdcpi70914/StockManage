using Microsoft.AspNetCore.Mvc;
using SRC.Backend.Models.Brain;
using SRC.Backend.Models.Config;
using SRC.Backend.Models.Pages;
using SRC.Backend.Models.Pages.Equipment;
using SRC.Backend.Models.System;
using SRC.DB.Interfaces.Equipment;

namespace SRC.Backend.Controllers.Equipment
{
    public class MaterialController : BaseController
    {
        private SysAppsetting Setting { get; set; }
        private IDF_Material DF_Material { get; set; }
        private MaterialLogic _MaterialLogic {  get; set; }

        public MaterialController(
            SysAppsetting setting,
            IHttpContextAccessor cxtAccessor,
            IConfiguration config,
            IDF_Material dF_Material,
            Serilog.ILogger slog) : base(cxtAccessor, config, slog)
        {
            Setting = setting;
            DF_Material = dF_Material;
            _MaterialLogic = new MaterialLogic(slog,DF_Material);
        }

        public IActionResult Index()
        {
            int page = 1;
            int take = Setting.PaginationTake;

            var result = DF_Material.SearchMaterialMaintain("", null, null, null, "", page, take, out int rowtotal);


            MaterialIndex Model = new MaterialIndex()
            {
                SearchResultPage = new MaterialSearch()
                {
                    Data = result,
                    Pagination = new Models.System.SRCUIPagination(page, take, rowtotal)
                }
            };

            return View(Model);
        }

        public IActionResult Search(MaterialIndex.SearchModel condition, int? page, int? take)
        {
            var result = DF_Material.SearchMaterialMaintain(condition.name, condition.price, condition.start_time, condition.end_time, condition.state, page, take, out int rowtotal);

            return PartialView(new MaterialSearch()
            {
                Data = result,
                Pagination = new Models.System.SRCUIPagination(page, take, rowtotal)
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(MaterialEdit model)
        {
            SRCLoginMeta LoginMeta = User.Identity.LoginMeta();
            model.Action = ActionMode.ADD;

            var check = _MaterialLogic.CheckMaterialParameter(model);

            if (!string.IsNullOrWhiteSpace(check))
            {
                return PartialAOE_ADDFail(check);
            }

            var Ok = await _MaterialLogic.Create(model, LoginMeta.Account);

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
                return PartialAOE_ADDFail(_MaterialLogic.InnerMessage);
            }
        }

        [HttpGet]
        public IActionResult Edit(long? pid)
        {
            if (!pid.HasValue)
            {
                return PartialAOE_EditFail(UIMessage.SYS.ArgumentEmpty);
            }

            var data = DF_Material.GetMaterialMaintain(pid.Value);

            SRCLayoutAOE lyAoe = new SRCLayoutAOE();
            lyAoe.formId = "edit_form";
            lyAoe.Action = "Edit";
            lyAoe.Controller = "Material";
            lyAoe.View = "Edit";
            lyAoe.Title = "修改裝備主檔資料";
            lyAoe.AOEObject = new MaterialEdit
            {
                pid = data.pid,
                name = data.name,
                price = data.price,
                stock = data.stock,
                Action = ActionMode.EDIT
            };

            return PartialAOE(lyAoe);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(MaterialEdit model)
        {
            SRCLoginMeta LoginMeta = User.Identity.LoginMeta();
            model.Action = ActionMode.EDIT;

            var check = _MaterialLogic.CheckMaterialParameter(model);

            if (!string.IsNullOrWhiteSpace(check))
            {
                return PartialAOE_EditFail(check);
            }

            var Ok = await _MaterialLogic.Edit(model, LoginMeta.Account);

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
                return PartialAOE_EditFail(_MaterialLogic.InnerMessage);
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
                await DF_Material.Delete(pid, meta.Account);

                page.IsSuccess = true;
                page.Message = UIMessage.SYS.Success_Abandon;
            }
            catch (Exception ex)
            {
                page.IsSuccess = false;
                page.Message = UIMessage.SYS.Fail_Abandon;
                SLog.Fatal(ex, $"{page.Message},{ex.Message}");
            }


            return PartialSrcvalleyPageMessage(page);
        }
    }
}
