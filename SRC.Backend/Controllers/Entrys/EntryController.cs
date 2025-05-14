using DocumentFormat.OpenXml.InkML;
using SRC.Backend.Models.System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IISIntegration;
using SRC.Backend.Models.Brain;
using SRC.Backend.Models.Config;
using SRC.Backend.Models.Pages;
using SRC.DB.Interfaces.Authority;
using SRC.DB.Interfaces.Settings;
using SRC.DB.Interfaces.Users;
using SRC.DB.Models.EFMSSQL;
using System.DirectoryServices;
using System.Net;
using System.Security.Claims;

namespace SRC.Backend.Controllers.Entrys
{


    public class EntryController : Controller
    {
        private IDF_BackendUser BackendUserDF { get; set; }
        public IDF_SystemCode SystemCodeDF { get; set; }
        public Serilog.ILogger SLog { get; set; }
        private IDF_Role RoleDF { get; set; }
        private SysAppsetting SysAppsetting { get; set; }
        private BackendUserLogin _Login { get; set; }

        private ADLogin _ADLogin { get; set; }

        EFContext DB;

        public EntryController(
            IDF_Role roleDF,
            IDF_BackendUser backendUserDF,
            IDF_SystemCode systemCodeDF,
            Serilog.ILogger slog,
            SysAppsetting sysAppsetting)
        {
            BackendUserDF = backendUserDF;
            RoleDF = roleDF;
            SLog = slog;
            _Login = new BackendUserLogin(backendUserDF, SLog);
            SysAppsetting = sysAppsetting;
            SystemCodeDF = systemCodeDF;

            _ADLogin = new ADLogin(SLog, SysAppsetting);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {

            if (HttpContext.Request.Cookies["ClickAdLoginBtn"] != null)
            {
                HttpContext.Response.Cookies.Delete("ClickAdLoginBtn");
            }

            if (HttpContext.Request.Cookies["ClickLoginOrCancelBtn"] != null)
            {
                HttpContext.Response.Cookies.Delete("ClickLoginOrCancelBtn");
            }

            ViewData["LoginMessage"] = string.Empty;
            return View();


        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {


            bool success = _Login.LoginFunc(model.Account, model.Password, SystemCodeDF);


            if (!success)
            {
                ViewData["LoginMessage"] = _Login.InnerMessage;
                return View("Login");
            }


            //可以直接在這用DF Object抓資料 但不能在這處理複雜邏輯,比如登入或驗證密碼等功能邏輯
            backend_user user = BackendUserDF.GetUser(model.Account);


            List<string> roleCode = RoleDF.GetRoleProgramCode(user.user_id);

            ClaimsIdentity claimsIdentity = new UserClaims().Create(new SRCLoginMeta
            {
                Account = user.account,
                UserId = user.user_id,
                UserName = user.name_ch,
                RoleCode = roleCode,
                Unit = user.unit ?? ""
            });

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,// Refreshing the authentication session should be allowed.
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddDays(1)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View("Login");
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> LoginAD(LoginModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Account) || string.IsNullOrWhiteSpace(model.Password))
            {
                ViewData["LoginMessage"] = "帳號密碼不可空白";
                return View("Login");
            }

            var AuthOk = _ADLogin.GetADLoginAuth(model.Account, model.Password);

            if (AuthOk)
            {
                backend_user user = BackendUserDF.GetUserByAdAccount(model.Account);

                if (user != null)
                {
                    List<string> roleCode = RoleDF.GetRoleProgramCode(user.user_id);

                    ClaimsIdentity claimsIdentity = new UserClaims().Create(new SRCLoginMeta
                    {
                        Account = user.account,
                        UserId = user.user_id,
                        UserName = user.name_ch,
                        RoleCode = roleCode,
                        Unit = user.unit ?? ""
                    });

                    var authProperties = new AuthenticationProperties
                    {
                        AllowRefresh = true,// Refreshing the authentication session should be allowed.
                        IsPersistent = true,
                        ExpiresUtc = DateTime.UtcNow.AddDays(1)
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    return RedirectToAction("Index", "ProductionSchedule");
                }
                else
                {
                    ViewData["LoginMessage"] = "此AD帳號尚未綁定至一般帳號，請先進行綁定後再次進行AD登入";
                    return View("Login");
                }
            }
            else
            {
                ViewData["LoginMessage"] = "AD驗證不通過，請確認輸入的帳號密碼或者使用一般登入!";
                return View("Login");
            }

            //var UserIdentity = User.Identity;

            //if (UserIdentity != null && UserIdentity.IsAuthenticated)
            //{
            //    var UserName = "";
            //    SLog.Fatal($"UserIdentityName：{UserIdentity.Name}");
            //    if (UserIdentity.Name.Split(new[] { "\\" }, StringSplitOptions.None).Length == 2)
            //    {
            //        UserName = UserIdentity.Name.Split(new[] { "\\" }, StringSplitOptions.None)[1];
            //    }
            //    else
            //    {

            //        UserName = UserIdentity.Name;
            //    }

            //    var AuthOk = _ADLogin.GetDirectoryEntryByAccount(UserName);

            //    if(AuthOk)
            //    {
            //        backend_user user = BackendUserDF.GetUserByAdAccount(UserName);

            //        if (user != null)
            //        {
            //            List<string> roleCode = RoleDF.GetRoleProgramCode(user.user_id);

            //            ClaimsIdentity claimsIdentity = new UserClaims().Create(new SRCLoginMeta
            //            {
            //                Account = user.account,
            //                UserId = user.user_id,
            //                UserName = user.name_ch,
            //                RoleCode = roleCode,
            //                Unit = user.unit ?? ""
            //            });

            //            var authProperties = new AuthenticationProperties
            //            {
            //                AllowRefresh = true,// Refreshing the authentication session should be allowed.
            //                IsPersistent = true,
            //                ExpiresUtc = DateTime.UtcNow.AddDays(1)
            //            };

            //            await HttpContext.SignInAsync(
            //                CookieAuthenticationDefaults.AuthenticationScheme,
            //                new ClaimsPrincipal(claimsIdentity),
            //                authProperties);

            //            return RedirectToAction("Index", "ProductionSchedule");
            //        }
            //        else
            //        {
            //            ViewData["LoginMessage"] = "此AD帳號尚未綁定至一般帳號，請先進行綁定後再次進行AD登入";
            //            return View("Login");
            //        }
            //    }
            //    else
            //    {
            //        ViewData["LoginMessage"] = "AD驗證不通過，請確認輸入的帳號密碼或者使用一般登入!";
            //        return View("Login");
            //    }
            //}

            //return View("Login");
        }
    }
}
