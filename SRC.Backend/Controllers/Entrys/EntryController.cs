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
using System.Drawing;
using Serilog;

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
            var vaildCode = "";

            if (TempData["code"] == null)
            {
                SLog.Fatal($"驗證碼資料異常，TempData：{TempData["code"]}");
                ViewData["LoginMessage"] = "驗證碼錯誤";
                return View("Login");
            }
            else
            {
                vaildCode = TempData["code"].ToString();
            }

            bool success = _Login.LoginFunc(model.Account, model.Password,model.Code,vaildCode, SystemCodeDF);


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
                Unit = user.unit
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
        public IActionResult GetValidateCode()
        {
            byte[] data = null;
            string code = _Login.RandomCode(5);
            TempData["code"] = code;
            //定義一個畫板
            MemoryStream ms = new MemoryStream();
            using (Bitmap map = new Bitmap(100, 40))
            {
                //畫筆,在指定畫板畫板上畫圖
                //g.Dispose();
                using (Graphics g = Graphics.FromImage(map))
                {
                    g.Clear(Color.White);
                    g.DrawString(code, new Font("黑體", 18.0F), Brushes.Blue, new Point(10, 8));
                    //繪製干擾線(數字代表幾條)
                    _Login.PaintInterLine(g, 5, map.Width, map.Height);
                }

                map.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            data = ms.GetBuffer();
            return File(data, "image/jpeg");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View("Login");
        }
    }
}
