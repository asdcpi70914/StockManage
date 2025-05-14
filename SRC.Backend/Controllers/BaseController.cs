using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SRC.Backend.Models.Filters;
using SRC.Backend.Models.Pages;
using SRC.Backend.Models.System;
using System.Security.Claims;

namespace SRC.Backend.Controllers
{
    [Authorize]
    [TypeFilter(typeof(SRCMenusFilter))]
    public class BaseController : Controller
    {
        public IConfiguration Config { get; set; }
        public IHttpContextAccessor ContextAccessor { get; set; }
        public Serilog.ILogger SLog { get; set; }

        public BaseController(IHttpContextAccessor cxtAccessor, IConfiguration config, Serilog.ILogger log)
        {
            //this.LogCfg = logConfig.Value;
            ContextAccessor = cxtAccessor;
            Config = config;
            SLog = log;
        }

        protected IActionResult SRCPageMessage(bool success, string message)
        {
            return View("Component/_src_pg_message", new SRCPageMessage { IsSuccess = success, Message = message });
        }




        protected IActionResult PartialSrcvalleyPageMessage(SRCPageMessage page)
        {
            return PartialView("Component/_src_pg_message", page);
        }

        protected IActionResult PartialSrcvalleyPageMessage(SRCAoePageMessage page)
        {
            return PartialAOEBackMessage(page);
        }

        protected IActionResult PartialSrcvalleyPageMessage(string message)
        {
            return PartialView("Component/_src_pg_message", new SRCPageMessage { Message = message });
        }

        protected IActionResult PartialSrcvalleyPageMessage(bool success, string message)
        {
            return PartialView("Component/_src_pg_message", new SRCPageMessage { IsSuccess = success, Message = message });
        }


        protected IActionResult PartialAOEBackMessage(SRCAoePageMessage page)
        {
            return PartialView("Component/_src_pg_message", page);
        }

        protected IActionResult PartialAOE_ADDFail(string message)
        {
            return PartialAOEBackMessage(false, message, ActionMode.ADD);
        }

        protected IActionResult PartialAOE_EditFail(string message)
        {
            return PartialAOEBackMessage(false, message, ActionMode.EDIT);
        }

        protected IActionResult PartialAOE_ModifyFail(string message)
        {
            return PartialAOEBackMessage(false, message, ActionMode.Modify);
        }

        protected IActionResult PartialAOE_UploadFail(string message)
        {
            return PartialAOEBackMessage(false, message, ActionMode.UPLOAD);
        }

        protected IActionResult PartialAOEBackMessage(bool success, string message, ActionMode mode)
        {
            SRCAoePageMessage page = new SRCAoePageMessage()
            {
                IsSuccess = success,
                Message = message,
                Mode = mode
            };

            return PartialView("Component/_src_pg_message", page);
        }

        protected IActionResult PartialSrcvalleyLayoutModal(SRCLayoutModal page)
        {
            return PartialView("Component/_src_ly_modal", page);
        }

        protected IActionResult PartialAOE(SRCLayoutAOE lyAoe)
        {
            return PartialView("Component/_src_ly_aoe", lyAoe);
        }

        protected IActionResult PartialConfirm(SRCLayoutConfrim lyConfirm)
        {
            return PartialView("Component/_src_ly_confirm", lyConfirm);
        }


        public string UserAccount
        {
            get
            {
                return ((ClaimsIdentity)User.Identity).Claims.Where(m => m.Type == "Account").First().Value;
            }
        }
    }
}
