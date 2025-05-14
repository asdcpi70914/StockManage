using Microsoft.AspNetCore.Mvc;
using SRC.Backend.Controllers;
using SRC.Backend.Models.Brain;
using SRC.Backend.Models.Config;
using SRC.Backend.Models.Pages;
using SRC.Backend.Models.Pages.BackendUser;
using SRC.Backend.Models.System;
using SRC.DB.Interfaces.Authority;
using SRC.DB.Interfaces.Settings;
using SRC.DB.Interfaces.Users;
using SRC.DB.Models.EFMSSQL;

namespace SRC.Backend.Controllers.Users
{
    public class BackendUserRoleController : BaseController
    {
        private Serilog.ILogger SLog { get; set; }
        private IDF_BackendUser BackendUserDF { get; set; }

        private IDF_Role RoleDF { get; set; }
        private IDF_SystemCode SysCodeDF { get; set; }

        private SysAppsetting Setting;

        private UserInfoSecurity security { get; set; }

        public BackendUserRoleController(
            IDF_Role roleDF,
            IDF_SystemCode sysCodeDF,
            IDF_BackendUser db,
            SysAppsetting sysAppsetting,
            Serilog.ILogger slog,
            IHttpContextAccessor cxtAccessor,
            IConfiguration config)
            : base(cxtAccessor, config, slog)
        {
            RoleDF = roleDF;
            SLog = slog;
            SysCodeDF = sysCodeDF;
            BackendUserDF = db;
            Setting = sysAppsetting;
            security = new UserInfoSecurity(SLog, SysCodeDF);
        }

        public IActionResult Index()
        {
            BackendUserRoleIndex index = new BackendUserRoleIndex();

            if (
                (index.SearchResultPage =
                    new BackendUserRoleIndex.SearchModel().GetSearchViewModel(SLog, BackendUserDF, security, 1, Setting.PaginationTake, out string innerMessage))
                == null)
            {
                return SRCPageMessage(false, $"{UIMessage.SYS.Fail_Search},{innerMessage}");
            }


            return View(index);
        }


        public IActionResult Search(BackendUserRoleIndex.SearchModel cdt, int? page, int? take)
        {
            BackendUserRoleSearch result;

            if (
                (result = cdt.GetSearchViewModel(SLog, BackendUserDF, security, page, take, out string innerMessage))
                == null)
            {
                return PartialSrcvalleyPageMessage(false, $"{UIMessage.SYS.Fail_Search},{innerMessage}");
            }

            return PartialView(result);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(Guid? pid)
        {
            if (!pid.HasValue) return PartialSrcvalleyPageMessage(false, UIMessage.SYS.ArgumentEmpty);

            List<backend_users_role> userOwnRoles = null;

            try
            {
                userOwnRoles = await BackendUserDF.GetUserRolesMap(pid.Value);
            }
            catch (Exception ex)
            {
                SLog.Fatal(ex, $"查詢帳號擁有的角色異常,{ex.Message}");
                return PartialSrcvalleyPageMessage(false, $"{UIMessage.SYS.Fail_Search},{ex.Message}");
            }

            List<role> roles = await RoleDF.ListRole();
            try
            {
                roles = await RoleDF.ListRole();
            }
            catch (Exception ex)
            {
                SLog.Fatal(ex, $"查詢角色清單異常,{ex.Message}");
                return PartialSrcvalleyPageMessage(false, $"{UIMessage.SYS.Fail_Search},{ex.Message}");
            }


            List<int> rolePidInUser = userOwnRoles.Select(m => m.role_id).ToList();

            BackendUserRoleEdit editData = new BackendUserRoleEdit()
            {
                UserPid = pid.Value,
                UserRoles = roles.Select(m => new BackendUserRoleEdit.UserRole()
                {
                    RoleId = m.pid,
                    RoleName = m.name,
                    IsSelected = rolePidInUser.Contains(m.pid)
                }).ToList()
            };

            SRCLayoutAOE lyAoe = new SRCLayoutAOE();
            lyAoe.formId = "edit_form";
            lyAoe.Action = "Edit";
            lyAoe.Controller = "BackendUserRole";
            lyAoe.View = "Edit";
            lyAoe.Title = "修改使用者帳號的角色設定";
            lyAoe.AOEObject = editData;
            return PartialAOE(lyAoe);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid userPid, List<BackendUserRoleEdit.UserRole> userRoles)
        {
            SRCPageMessage page = new SRCPageMessage();
            try
            {
                var NewuserRoles = userRoles.Where(x => x.IsSelected).Select(x => x.RoleId).ToList();



                await RoleDF.UpdateRoleInUser(userPid, NewuserRoles);
                page.IsSuccess = true;
                page.Message = UIMessage.SYS.Success_Edit;
            }
            catch (Exception ex)
            {
                SLog.Fatal(ex, $"更新使用者帳號擁有的角色異常,{ex.Message}");
                page.IsSuccess = false;
                page.Message = $"{UIMessage.SYS.Fail_Edit},{ex.Message}";
            }

            return PartialSrcvalleyPageMessage(page);

        }
    }
}
