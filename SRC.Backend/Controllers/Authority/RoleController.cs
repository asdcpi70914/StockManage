using Microsoft.AspNetCore.Mvc;
using SRC.Backend.Controllers;
using SRC.Backend.Models.Config;
using SRC.Backend.Models.Pages;
using SRC.Backend.Models.Pages.Role;
using SRC.Backend.Models.System;
using SRC.DB.Interfaces.Authority;
using SRC.DB.Interfaces.Users;
using SRC.DB.Models.EFMSSQL;

namespace SRC.Backend.Controllers.Authority
{
    public class RoleController : BaseController
    {

        private IDF_Role RoleDF { get; set; }
        private IDF_BackendUser BackendUserDF { get; set; }

        private SysAppsetting Setting { get; set; }

        public RoleController(
            SysAppsetting setting,
            IHttpContextAccessor cxtAccessor,
            IDF_Role roleDF,
            IDF_BackendUser backendUserDF,
            IConfiguration config,
            Serilog.ILogger slog)
            : base(cxtAccessor, config, slog)
        {
            RoleDF = roleDF;
            Setting = setting;
            BackendUserDF = backendUserDF;
        }

        public string ViewName => "Index";

        public IActionResult Index()
        {

            RoleViewModel ViewModel = new RoleViewModel()
            {
                SearchPage = new RoleSearch()
                {
                    RoleList = RoleDF.SearchRole(
                    null,
                    null,
                1, Setting.PaginationTake, out int rowscount)
                }
            };


            return View(ViewModel);
        }

        [HttpPost]
        public IActionResult Search(RoleQuery roleQuery, int? page, int? take)
        {
            page = page ?? 1;
            take = take ?? Setting.PaginationTake;

            RoleSearch searchPage = new RoleSearch();

            searchPage.RoleList = RoleDF.SearchRole(roleQuery.name, roleQuery.pid, page, take, out int rowsCount);
            searchPage.Pagination = new SRCUIPagination(page, take, rowsCount);

            return PartialView(searchPage);
        }


        public ActionResult ADD()
        {
            return PartialView("Role_Add");

        }

        [HttpPost]
        public IActionResult ADD(RoleAdd roleMode)
        {
            SRCLoginMeta loginMeta = User.Identity.LoginMeta();

            SRCPageMessage page = new SRCPageMessage();
            try
            {
                role Data = new role
                {
                    name = roleMode.Name,
                    creator = loginMeta.Account,
                    create_time = DateTime.Now,
                    state = 1
                };

                page.IsSuccess = RoleDF.AddRole(Data);
                page.Message = UIMessage.SYS.Success_Add;
                //先暫時加這段
                List<role_func> role_func_data = new List<role_func>();
                role_func_data.Add(new role_func()
                {
                    role_id = Data.pid,
                    func_id = RoleDF.GetFunc("首頁").Result.pid,
                    weight = 10
                });

                role_func_data.Add(new role_func()
                {
                    role_id = Data.pid,
                    func_id = RoleDF.GetFunc("修改個人密碼").Result.pid,
                    weight = 10
                });

                RoleDF.AddRoleFunc(role_func_data);
            }
            catch (Exception ex)
            {
                page.IsSuccess = false;
                page.Message = $"{UIMessage.SYS.Fail_Add},{ex.Message},";
                SLog.Fatal(ex, $"新增角色發生異常,{ex.Message}");

            }

            return PartialSrcvalleyPageMessage(page);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? pid)
        {
            if (!pid.HasValue)
            {
                return PartialSrcvalleyPageMessage(false, UIMessage.SYS.ArgumentEmpty);
            }

            role roleInfo = await RoleDF.GetRole(pid.Value);

            if (roleInfo == null)
            {
                return PartialSrcvalleyPageMessage(false, UIMessage.SYS.ServerBusy);
            }

            SRCLayoutAOE lyAoe = new SRCLayoutAOE();
            lyAoe.formId = "edit_form";
            lyAoe.Action = "Edit";
            lyAoe.Controller = "Role";
            lyAoe.View = "Role_Edit";
            lyAoe.Title = "修改角色資料";
            lyAoe.AOEObject = new RoleEdit
            {
                pid = roleInfo.pid,
                Name = roleInfo.name
            };

            return PartialAOE(lyAoe);


        }

        [HttpPost]
        public async Task<IActionResult> EDIT(RoleEdit roleModel)
        {

            SRCLoginMeta loginMeta = User.Identity.LoginMeta();

            SRCPageMessage page = new SRCPageMessage();

            try
            {
                page.IsSuccess = await RoleDF.EditRole(roleModel.pid, roleModel.Name, loginMeta.Account, DateTime.Now);
                page.Message = page.IsSuccess ? UIMessage.SYS.Success_Edit : $"{UIMessage.SYS.Fail_Edit},{RoleDF.InnerMessage}";

            }
            catch (Exception ex)
            {
                page.IsSuccess = false;
                page.Message = $"{UIMessage.SYS.Fail_Edit},{ex.Message},";
                SLog.Fatal(ex, $"修改角色發生異常,{ex.Message}");
            }

            return PartialSrcvalleyPageMessage(page);
        }


        [HttpPost]
        public async Task<IActionResult> DELETE(List<int> pid)
        {

            if (pid == null)
            {
                return PartialSrcvalleyPageMessage(false, UIMessage.SYS.ArgumentEmpty);
            }

            SRCLoginMeta loginMeta = User.Identity.LoginMeta();

            SRCPageMessage page = new SRCPageMessage();
            try
            {
                page.IsSuccess = await RoleDF.DeleteRole(pid, loginMeta.Account);
                page.Message = page.IsSuccess ? UIMessage.SYS.Success_Delete : $"{UIMessage.SYS.Fail_Delete},{RoleDF.InnerMessage}";
            }
            catch (Exception ex)
            {
                page.IsSuccess = false;
                page.Message = $"{UIMessage.SYS.Fail_Delete},{ex.Message},";
                SLog.Fatal(ex, $"刪除角色發生異常,{ex.Message}");
            }

            return PartialSrcvalleyPageMessage(page);
        }

        [HttpGet]
        public async Task<IActionResult> EDIT_FUNC(int? pid)
        {
            if (!pid.HasValue)
            {
                return PartialSrcvalleyPageMessage(false, UIMessage.SYS.ArgumentEmpty);
            }


            RoleFunc roleFunc = new RoleFunc();
            roleFunc.RolePID = pid.Value;

            try
            {
                roleFunc.UserOwnRole = await RoleDF.SearchBackendUserInRole(pid.Value);
            }
            catch (Exception ex)
            {
                SLog.Fatal(ex, $"取得角色清單發生異常,{ex.Message}");
                return PartialSrcvalleyPageMessage(false, UIMessage.SYS.ServerBusy);
            }

            try
            {
                roleFunc.FuncListForJson = RoleDF.GetRoleFuncsJson(string.Empty, pid.Value);

            }
            catch (Exception ex)
            {
                SLog.Fatal(ex, $"取得角色功能清單發生異常,{ex.Message}");
                return PartialSrcvalleyPageMessage(false, UIMessage.SYS.ServerBusy);
            }


            SRCLayoutAOE lyAoe = new SRCLayoutAOE();
            lyAoe.ViewSize = ModalSize.ExtraLarge;
            lyAoe.formId = "editfunc_form";
            lyAoe.Controller = "Role";
            lyAoe.Action = "EDIT_FUNC";
            lyAoe.View = "Role_Func";
            lyAoe.Title = "修改後台使用者帳號權限";
            lyAoe.AOEObject = roleFunc;

            return PartialAOE(lyAoe);
        }

        [HttpPost]
        public async Task<IActionResult> EDIT_FUNC(RoleFuncAdd roleEditFunc)
        {

            SRCPageMessage page = new SRCPageMessage();
            try
            {
                await RoleDF.EditFunc(roleEditFunc.RolePID, roleEditFunc.membsPID, roleEditFunc.funcsPID);
                page.IsSuccess = true;
                page.Message = UIMessage.SYS.Success_Edit;
            }
            catch (Exception ex)
            {
                page.IsSuccess = false;
                page.Message = $"{UIMessage.SYS.Fail_Edit},{ex.Message},";
                SLog.Fatal(ex, $"更新角色權限發生異常,{ex.Message}");
            }


            return PartialSrcvalleyPageMessage(page);
        }
    }
}
