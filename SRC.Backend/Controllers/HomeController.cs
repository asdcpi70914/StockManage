using SRC.Backend.Models.Brain;
using SRC.Backend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace SRC.Backend.Controllers
{
    public class HomeController : BaseController
    {

        public HomeController(
            Serilog.ILogger slog,
            IHttpContextAccessor cxtAccessor,
            IConfiguration config)
             : base(cxtAccessor, config, slog)
        {

        }

        public async Task<IActionResult> Index()
        {
            return View();
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