using SRC.Backend.Models.Brain;
using SRC.Backend.Models.Config;
using SRC.Backend.Models.System;
using SRC.DB.HardCodes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using Serilog;
using SRC.DB.Interfaces.Settings;

namespace SRC.Backend.Controllers
{
    [Authorize]
    public class DynamicController : Controller
    {
        //test

        private readonly Serilog.ILogger _logger;


        public DynamicController(
            IDF_SystemCode sysCodeDF,
            Serilog.ILogger slog,
            IHttpContextAccessor contextAccessor
            )
        {
            _logger = slog;
        }
    }
}
