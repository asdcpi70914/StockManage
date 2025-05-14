using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using SRC.Backend.Models.System;

namespace SRC.Backend.Models.Filters
{
    public class SRCMenusFilter : Attribute, IAuthorizationFilter
    {
        protected MenusAuth Menus { get; set; }
        public SRCMenusFilter(MenusAuth menus)
        {
            Menus = menus;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new RedirectToActionResult("Entry", "Login", null);
                return;
            }

            //if (!Menus.FirstLogin)
            //{
            //    context.Result = new RedirectToActionResult("NoAppValidation", "Error", null);
            //    return;
            //}

            //Guid UserId;
            //ClaimsIdentity identity = (ClaimsIdentity)User?.Identity;
            //Guid.TryParse(identity.GetClaim(UserClaims.ClaimsKey.USERID).Value, out UserId);
            //var GetData = _STVMSUserDB.GetUserInfo(UserId);

            //if (GetData.FirstLogin)
            //{
            //    return View();
            //}
            //else
            //{
            //    return View("NoAppValidation");
            //}

            ControllerActionDescriptor descriptor = (ControllerActionDescriptor)context.ActionDescriptor;

            object[] list = descriptor.MethodInfo.GetCustomAttributes(typeof(FuncAliasAttribute), true);

            string requestUrl = $"/{descriptor.ControllerName}/{descriptor.ActionName}".ToUpper();

            if (list.Count() > 0)
            {
                var aliasActionName = ((FuncAliasAttribute)list[0]).AliasName;

                if (aliasActionName.Contains(","))
                {
                    string[] aliasArray = aliasActionName.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    string[] urlAliasArray = aliasArray.Select(m => $"/{descriptor.ControllerName}/{m}".ToUpper()).ToArray();
                    if (Menus.Urls.Where(x => urlAliasArray.Contains(x)).Count() == 0)
                    {
                        bool isAjax = context.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
                        if (isAjax)
                        {
                            ContentResult result = new ContentResult();
                            result.Content = "<script>_show_noauth()</script>";
                            context.Result = result;
                            //context.Result = new JsonResult(new { auth=false });
                        }
                        else
                        {
                            context.Result = new RedirectToActionResult("NoAuth", "Error", null);
                        }

                        return;
                    }

                    return;
                }

                requestUrl = $"/{descriptor.ControllerName}/{aliasActionName}".ToUpper();
            }


            if (!Menus.Urls.Contains(requestUrl))
            {
                bool isAjax = context.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
                if (isAjax)
                {
                    ContentResult result = new ContentResult();
                    result.Content = "<script>_show_noauth()</script>";
                    context.Result = result;
                    //context.Result = new JsonResult(new { auth=false });
                }
                else
                {
                    context.Result = new RedirectToActionResult("NoAuth", "Error", null);
                }

                return;
            }

        }
    }
}
