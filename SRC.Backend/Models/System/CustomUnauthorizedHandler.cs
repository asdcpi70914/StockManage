using Microsoft.AspNetCore.Authorization;
using System.Net;
using System;
using Microsoft.AspNetCore.Diagnostics;
using DocumentFormat.OpenXml.InkML;

namespace SRC.Backend.Models.System
{
    public class CustomUnauthorizedHandler
    {
        private readonly RequestDelegate _next;
        private readonly string _redirectPath;
        public CustomUnauthorizedHandler(RequestDelegate next, string redirectPath)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _redirectPath = redirectPath ?? throw new ArgumentNullException(nameof(redirectPath));
        }

        public async Task Invoke(HttpContext context)
        {
            await _next(context);

            IStatusCodeReExecuteFeature iStatusCodeReExecuteFeature = context.Features.Get<IStatusCodeReExecuteFeature>();

            if (context.Request.Cookies["ClickAdLoginBtn"] == null)
            {
                context.Response.Cookies.Append("ClickAdLoginBtn", "false");
            }



            if (context.Response.StatusCode == 401 && !Convert.ToBoolean(context.Request.Cookies["ClickAdLoginBtn"]) && context.Request.Path.HasValue && context.Request.Path.Value.Contains("LoginAd"))
            {
                context.Response.Cookies.Delete("ClickAdLoginBtn");
                context.Response.Cookies.Append("ClickAdLoginBtn", "true");
            }
            else if (context.Response.StatusCode == 401 && Convert.ToBoolean(context.Request.Cookies["ClickAdLoginBtn"]) && context.Request.Path.HasValue && context.Request.Path.Value.Contains("LoginAd"))
            {
                if (context.Request.Cookies["ClickLoginOrCancelBtn"] == null)
                {
                    context.Response.Cookies.Append("ClickLoginOrCancelBtn", "true");
                }

                context.Response.Cookies.Delete("ClickAdLoginBtn");
                context.Response.Cookies.Append("ClickAdLoginBtn", "false");
                //context.Response.StatusCode = (int)HttpStatusCode.Found;
                context.Response.Redirect("/Entry/LoginAD");
            }
        }
    }

}
