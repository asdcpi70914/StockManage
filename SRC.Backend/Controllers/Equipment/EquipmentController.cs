using Microsoft.AspNetCore.Mvc;
using SRC.Backend.Models.Brain;
using SRC.Backend.Models.Config;
using SRC.Backend.Models.Pages.BackendUser;
using SRC.Backend.Models.Pages;
using SRC.Backend.Models.Pages.Equipment;
using SRC.Backend.Models.System;
using SRC.DB.Interfaces.Authority;
using SRC.DB.Interfaces.Equipment;
using SRC.DB.Interfaces.Users;

namespace SRC.Backend.Controllers.Equipment
{
    public class EquipmentController : BaseController
    {
        private SysAppsetting Setting { get; set; }
        private IDF_Equipment DF_Equipment { get; set; }
        private EquipmentLogic _EquipmentLogic { get; set; }

        public EquipmentController(
            SysAppsetting setting,
            IHttpContextAccessor cxtAccessor,
            IConfiguration config,
            IDF_Equipment dF_Equipment,
            Serilog.ILogger slog): base(cxtAccessor, config, slog)
        {
            Setting = setting;
            DF_Equipment = dF_Equipment;
            _EquipmentLogic = new EquipmentLogic(slog, DF_Equipment);
        }

        public IActionResult Index()
        {
            int page = 1;
            int take = Setting.PaginationTake;

            var result = DF_Equipment.SearchEquipmentMaintain("", null, null, null,"", page, take, out int rowtotal);


            EquipmentIndex Model = new EquipmentIndex()
            {
                SearchResultPage = new EquipmentSearch()
                {
                    Data = result,
                    Pagination = new Models.System.SRCUIPagination(page,take,rowtotal)
                }
            };

            return View(Model);
        }

        public IActionResult Search(EquipmentIndex.SearchModel condition,int? page,int? take)
        {
            var result = DF_Equipment.SearchEquipmentMaintain(condition.name, condition.price, condition.start_time, condition.end_time, condition.state, page, take, out int rowtotal);

            return PartialView(new EquipmentSearch()
            {
                Data = result,
                Pagination = new Models.System.SRCUIPagination(page, take, rowtotal)
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(EquipmentEdit model)
        {
            SRCLoginMeta LoginMeta = User.Identity.LoginMeta();
            model.Action = ActionMode.ADD;

            var check = _EquipmentLogic.CheckEquipmentParameter(model);

            if (!string.IsNullOrWhiteSpace(check))
            {
                return PartialAOE_ADDFail(check);
            }

            var Ok = await _EquipmentLogic.Create(model, LoginMeta.Account);

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
                return PartialAOE_ADDFail(_EquipmentLogic.InnerMessage);
            }
        }

        [HttpGet]
        public IActionResult Edit(long? pid)
        {
            if (!pid.HasValue)
            {
                return PartialAOE_EditFail(UIMessage.SYS.ArgumentEmpty);
            }

            var data = DF_Equipment.GetEquipmentMaintain(pid.Value);

            SRCLayoutAOE lyAoe = new SRCLayoutAOE();
            lyAoe.formId = "edit_form";
            lyAoe.Action = "Edit";
            lyAoe.Controller = "Equipment";
            lyAoe.View = "Edit";
            lyAoe.Title = "修改裝備主檔資料";
            lyAoe.AOEObject = new EquipmentEdit
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
        public async Task<IActionResult> Edit(EquipmentEdit model)
        {
            SRCLoginMeta LoginMeta = User.Identity.LoginMeta();
            model.Action = ActionMode.EDIT;

            var check = _EquipmentLogic.CheckEquipmentParameter(model);

            if (!string.IsNullOrWhiteSpace(check))
            {
                return PartialAOE_EditFail(check);
            }

            var Ok = await _EquipmentLogic.Edit(model, LoginMeta.Account);

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
                return PartialAOE_EditFail(_EquipmentLogic.InnerMessage);
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
                await DF_Equipment.Delete(pid, meta.Account);

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
