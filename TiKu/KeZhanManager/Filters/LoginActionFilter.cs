using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Web.Mvc;
using WebMatrix.WebData;
using KeZhanManager.Models;
using System.Web.Routing;
using TiKu.Entity;

namespace KeZhanManager.Filters
{
    public sealed class LoginActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            tUserEntity userInfo = filterContext.HttpContext.Session["KeZhanUserInfo"] as tUserEntity;
            string msg = string.Empty;
            string redurl = filterContext.HttpContext.Request.Url.ToString();
            if (userInfo == null)
            {
                filterContext.Result = new RedirectToRouteResult("Default", new RouteValueDictionary(new { controller = "Home", action = "Login" }));

            }
            else
            {
                filterContext.Controller.ViewData["KeZhanUserInfo"] = userInfo;
            }

            base.OnActionExecuting(filterContext);
        }

    }
}
