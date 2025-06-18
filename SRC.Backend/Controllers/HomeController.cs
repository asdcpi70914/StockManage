using SRC.Backend.Models.Brain;
using SRC.Backend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using SRC.DB.Interfaces.Home;
using SRC.Backend.Models.Pages.Home;
using SRC.Backend.Models.System;
using SRC.Backend.Models.Config;
using System.Drawing;
using SRC.Backend.Models.Filters;

namespace SRC.Backend.Controllers
{
    public class HomeController : BaseController
    {
        private IDF_Home DF_Home { get; set; }
        private SysAppsetting SysAppsetting { get; set; }
        private HomeLogic _HomeLogic { get; set; }

        public HomeController(
            Serilog.ILogger slog,
            IHttpContextAccessor cxtAccessor,
            IDF_Home dF_Home,
            IConfiguration config,
            SysAppsetting sysAppsetting)
             : base(cxtAccessor, config, slog)
        {
            DF_Home = dF_Home;
            SysAppsetting = sysAppsetting;
            _HomeLogic = new HomeLogic(slog);
        }

        public async Task<IActionResult> Index()
        {
            HomeIndex Model = new HomeIndex();

            SRCLoginMeta meta = User.Identity.LoginMeta();

            long? unit = null;

            if(meta.Account != SysAppsetting.AdminAccount)
            {
                unit = meta.Unit;
            }


            var UpData = DF_Home.ListHomeUpData(unit).GroupBy(x => x.subscribepoint_name).ToList();

            Model.UpData = UpData;

            Model.MidData = DF_Home.ListHomeMidData(unit);

            Model.DownData = DF_Home.ListHomeDownData(unit);


            return View(Model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}