using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System;

namespace SRC.Backend.Controllers.Systems
{
    public class ErrorController : Controller
    {
        private readonly ILogger Log;

        public ErrorController(
            ILogger<ErrorController> log,
            IConfiguration config)
        {
            Log = log;

        }


        public IActionResult Index()
        {


            IExceptionHandlerPathFeature iExceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (iExceptionHandlerFeature != null)
            {
                string path = iExceptionHandlerFeature.Path;
                Exception exception = iExceptionHandlerFeature.Error;

                Log.LogError(exception, $"沒處理到的例外,{exception.Message}");
                //Write code here to log the exception details
                return View();
            }

            return View("Index");

        }

        public IActionResult NotFound(int? statusCode = null)
        {
            //

            IStatusCodeReExecuteFeature iStatusCodeReExecuteFeature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            if (iStatusCodeReExecuteFeature != null)
            {
                if (HttpContext.Response.StatusCode == 401 && iStatusCodeReExecuteFeature.OriginalPath == "/Entry/LoginAd")
                {
                    return View("WindowsAuthReturnLogin");
                }
            }

            return View("Index");
        }

        public IActionResult WindowsAuthReturnLogin()
        {

            ViewData["LoginMessage"] = string.Empty;
            return View();
        }

        [Authorize]
        public IActionResult NoAuth()
        {
            return View();
        }


    }
}
