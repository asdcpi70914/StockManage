using SRC.Backend.Models.Filters;
using Microsoft.AspNetCore.Mvc;
using SRC.Backend.Controllers;
using SRC.Backend.Models.Brain;
using SRC.Backend.Models.Config;
using SRC.Backend.Models.Pages;
using SRC.Backend.Models.Pages.BackendUser;
using SRC.Backend.Models.System;
using SRC.DB.Interfaces.Settings;
using SRC.DB.Interfaces.Users;
using SRC.DB.Models.EFMSSQL;
using SRC.ST.Security;

namespace SRC.Backend.Controllers.Users
{
    public class BackendUserController : BaseController
    {
        private Serilog.ILogger SLog { get; set; }
        private IDF_BackendUser _backendUserDF { get; set; }
        private IDF_SystemCode _SysCodeDF { get; set; }

        private SysAppsetting _setting;

        private UserInfoSecurity security { get; set; }

        //private IAccountDb _accountDB { get; set; }

        public BackendUserController(
            Serilog.ILogger slog,
            IDF_SystemCode sysCodeDF,
            SysAppsetting sysAppsetting,
            IDF_BackendUser db,
                IHttpContextAccessor cxtAccessor,
                ILogger<BackendUserController> _logger,
                IConfiguration config)
                 : base(cxtAccessor, config, slog)
        {
            SLog = slog;
            _SysCodeDF = sysCodeDF;
            _backendUserDF = db;
            _setting = sysAppsetting;
            security = new UserInfoSecurity(SLog, _SysCodeDF);
        }

        [HttpGet]
        public IActionResult ChangePassword() { return View(); }


        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordModel model)
        {
            SRCLoginMeta meta = User.Identity.LoginMeta();
            if (meta == null)
            {
                return RedirectToAction("Login", "Entry");
            }

            bool ok = false;
            bool canUpdate = model.NewPassword == model.ConfirmPassword;
            string message = string.Empty;

            if (canUpdate)
            {
                string salt = _SysCodeDF.GetBackendUserSalt();
                var userId = meta.UserId;

                string oldPasswordHash = new SecuritySalt(salt).Encrypt(model.OldPassword);
                string newPasswordHash = new SecuritySalt(salt).Encrypt(model.NewPassword);

                ok = _backendUserDF.UpdatePassword(meta.Account, userId.Value, oldPasswordHash, newPasswordHash);
            }
            else
            {
                message = "請確認新密碼是否正確";
            }


            model.ResultView = new SRCPageMessage()
            {
                Message = ok ? UIMessage.SYS.UpdateOK : $"{UIMessage.SYS.UpdateFail},{message}",
                IsSuccess = ok,
            };

            return View("ChangePassword", model);
        }

        [HttpGet]
        public IActionResult Index()
        {
            int page = 1;
            int take = _setting.PaginationTake;

            int rowsCount = 0;

            List<backend_user>? result = _backendUserDF.Query(null, null, null, null, null,
                page, take, out rowsCount);

            foreach (var Each in result)
            {
                Each.email = Each.email;
                Each.phone_number = Each.phone_number;
            }

            return View(new BackendUserIndex
            {
                SearchResultPage = new BackendUserSearch()
                {
                    Pagination = new SRCUIPagination(page, take, rowsCount),
                    Users = result
                }
            });
        }

        [HttpPost]
        public IActionResult Search(BackendUserIndex.SearchModel cdt, int? page, int? take)
        {
            List<backend_user>? result = new List<backend_user>();
            int rowsCount = 0;

            try
            {
            result = _backendUserDF.Query(
                cdt.Account,
                cdt.Email,
                cdt.UserName,
                cdt.PhoneNumber,
                cdt.Enabled,
                page, take, out rowsCount);
            }
            catch (Exception ex)
            {

            }
            if (result == null)
            {
                return PartialSrcvalleyPageMessage(_backendUserDF.InnerMessage);
            }

            foreach (var Each in result)
            {
                Each.email = Each.email;
                Each.phone_number = Each.phone_number;
            }


            SRCUIPagination pagination = new SRCUIPagination(page, take, rowsCount);

            return PartialView(new BackendUserSearch
            {
                Pagination = pagination,
                Users = result
            });

        }


        [HttpPost]
        public async Task<IActionResult> Create(BackendUserEdit model)
        {
            SRCLoginMeta opUser = User.Identity.LoginMeta();

            BackendUserDataCheck dataCheck = new BackendUserDataCheck();

            model.Action = ActionMode.ADD;

            if (!dataCheck.CheckCreateBackendUser(model))
            {
                return PartialAOE_ADDFail(dataCheck.InnerMessage);
            }

            if (await _backendUserDF.ExistUser(model.Account))
            {
                return PartialAOE_ADDFail(UIMessage.SYS.AccountExist);
            }

            string salt = _SysCodeDF.GetBackendUserSalt();

            backend_user newUser = new backend_user()
            {
                creator = opUser.Account,
                create_time = DateTime.Now,
                user_id = Guid.NewGuid(),
                account = model.Account,
                name_ch = model.UserName,
                phone_number = model.PhoneNumber,
                ad_account = model.AD_Account,
                unit = model.Unit,
                email = model.Email,
                enabled = model.Enabled,
                password_hash = new SecuritySalt(salt).Encrypt(model.Password),
                person_in_charge = model.person_in_charge
            };

            ////新增上下屬關聯表

            //backend_dept backend_Dept = new backend_dept()
            //{
            //    parent_pid = model.superior,
            //    code = model.Unit,
            //    create_time = DateTime.Now,
            //};

            SRCPageMessage page = new SRCPageMessage();

            try
            {
                bool Ok = _backendUserDF.Create(newUser);
                page.IsSuccess = Ok;
                page.Message = Ok ? UIMessage.SYS.Success_Add : UIMessage.SYS.Fail_Add;
            }
            catch (Exception ex)
            {
                SLog.Fatal(ex, $"新增帳號發生異常,{ex.Message}");
                page.Message = UIMessage.SYS.ServerBusy;
            }

            if (page.IsSuccess)
            {
                return PartialSrcvalleyPageMessage(page);
            }
            else
            {
                return PartialAOE_ADDFail(page.Message);
            }
        }


        [HttpGet]
        public IActionResult Edit(long pid)
        {
            backend_user getUser = _backendUserDF.GetUser(pid);// new AccountFactory(_backendUserDF).(user.UserId.Value);

            if (getUser == null)
            {
                return PartialSrcvalleyPageMessage(new SRCPageMessage()
                {
                    IsSuccess = false,
                    Message = UIMessage.SYS.NoDataToProcess
                });
            }

            backend_dept dept = _backendUserDF.GetBackendDept(pid);

            SRCLayoutAOE lyAoe = new SRCLayoutAOE();
            lyAoe.formId = "edit_form";
            lyAoe.Action = "Edit";
            lyAoe.Controller = "BackendUser";
            lyAoe.View = "Edit";
            lyAoe.Title = "修改後台使用者帳號資料";
            lyAoe.AOEObject = new BackendUserEdit
            {
                UserId = getUser.pid,
                Account = getUser.account,
                Email = getUser.email,
                UserName = getUser.name_ch,
                Unit = getUser.unit,
                AD_Account = getUser.ad_account,
                Enabled = getUser.enabled,
                PhoneNumber = getUser.phone_number,
                Action = ActionMode.EDIT,
                person_in_charge = getUser.person_in_charge,
                superior = dept != null ? dept.parent_pid : null
            };

            return PartialAOE(lyAoe);
        }

        [HttpPost]
        public IActionResult Edit(BackendUserEdit model)
        {
            SRCLoginMeta meta = User.Identity.LoginMeta();

            SRCPageMessage page = new SRCPageMessage();

            BackendUserDataCheck dataCheck = new BackendUserDataCheck();

            model.Action = ActionMode.EDIT;

            if (!dataCheck.CheckCreateBackendUser(model))
            {
                return PartialAOE_EditFail(dataCheck.InnerMessage);
            }

            //新增上下屬關聯表
            //backend_dept backend_Dept = new backend_dept();
            //backend_Dept.parent_pid = model.superior;
            //backend_Dept.code = model.Unit;
            //backend_Dept.create_time = DateTime.Now;
            //backend_Dept.backend_user_pid = model.UserId.Value;

            try
            {
                bool Ok = _backendUserDF.UpdateInfo(model.UserId.Value, new backend_user()
                {
                    name_ch = model.UserName,
                    email = model.Email,
                    phone_number = model.PhoneNumber,
                    unit = model.Unit,
                    ad_account = model.AD_Account,
                    enabled = model.Enabled,
                    editor = meta.Account,
                    edit_time = DateTime.Now,
                    person_in_charge = model.person_in_charge
                });

                page.IsSuccess = Ok;
                page.Message = Ok ? UIMessage.SYS.Success_Edit : UIMessage.SYS.Fail_Edit;
            }
            catch (Exception ex)
            {
                page.Message = $"{UIMessage.SYS.Fail_Edit},{ex.Message}";
            }

            if (page.IsSuccess)
            {
                return PartialSrcvalleyPageMessage(page);
            }
            else
            {
                return PartialAOE_EditFail(page.Message);
            }
        }

        [HttpPost]
        public IActionResult Delete(List<long> pid)
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
                page.IsSuccess = _backendUserDF.Delete(pid, meta.Account);
                page.Message = page.IsSuccess ? UIMessage.SYS.Success_Delete : UIMessage.SYS.Fail_Delete;
            }
            catch (Exception ex)
            {
                page.IsSuccess = false;
                page.Message = "刪除帳號發生異常";
                SLog.Fatal(ex, $"{page.Message},{ex.Message}");
            }


            return PartialSrcvalleyPageMessage(page);
        }

        [HttpGet]
        public IActionResult ResetPassword(long id)
        {
            backend_user user = _backendUserDF.GetUser(id);

            if (user == null)
            {
                return PartialSrcvalleyPageMessage(false, "資料不正確");
            }

            SRCLayoutAOE lyAoe = new SRCLayoutAOE();
            lyAoe.formId = "resetpwd_form";
            lyAoe.Action = "ResetPassword";
            lyAoe.Controller = "BackendUser";
            lyAoe.View = "ResetPassword";
            lyAoe.Title = "重設密碼";
            lyAoe.AOEObject = new ResetPassword()
            {
                Pid = user.pid,
                Account = user.account
            };
            return PartialAOE(lyAoe);
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPassword model)
        {
            SRCLoginMeta meta = User.Identity.LoginMeta();

            BackendUserDataCheck dataCheck = new BackendUserDataCheck();

            if (!dataCheck.CheckResetPasswordData(model))
            {
                return PartialSrcvalleyPageMessage(false, dataCheck.InnerMessage);
            }

            string salt = _SysCodeDF.GetBackendUserSalt();

            string hashPwd = new SecuritySalt(salt).Encrypt(model.Password);

            SRCPageMessage page = new SRCPageMessage();
            page.IsSuccess = _backendUserDF.ResetPassword(model.Pid, hashPwd, meta.Account);
            page.Message = page.IsSuccess ? UIMessage.SYS.Success_Edit : $"{UIMessage.SYS.Fail_Edit},{_backendUserDF.InnerMessage}";

            return PartialSrcvalleyPageMessage(page);
        }

        [HttpPost]
        [FuncAlias(AliasName = "Create,Edit")]
        public async Task<IActionResult> InitSuperiorSelect(long unit)
        {
            SRCLoginMeta meta = User.Identity.LoginMeta();

            var AllUnitUser = _backendUserDF.ListBackendUserForUnit(unit);

            var DropDown = AllUnitUser.Where(x => x.user_id != meta.UserId.Value).ToDictionary(x => x.pid, x => x.name_ch);

            return Json(DropDown);
        }
    }
}
