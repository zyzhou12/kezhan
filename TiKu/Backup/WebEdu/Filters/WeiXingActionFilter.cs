using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.IO;
using WebEdu.Models;
using TiKuBll.Model;
using TiKu.Bll;

namespace WebEdu.Filters
{
  public class WeiXingActionFilter : ActionFilterAttribute
  {
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
      //MicroPublicModel microInfo = filterContext.HttpContext.Session["microInfo"] as MicroPublicModel;
      UserInfoModel userInfo = filterContext.HttpContext.Session["userInfo"] as UserInfoModel;
      string msg = string.Empty;




      string redurl = filterContext.HttpContext.Request.Url.ToString();
      if (userInfo == null)
      {
        filterContext.Result = new RedirectToRouteResult("Default", new RouteValueDictionary(new { controller = "Open", action = "RegsiterLogin", redirect_uri = redurl }));
      }
      else
      {
        filterContext.Controller.ViewData["userInfo"] = userInfo;
      }


      //判断是否补齐用户信息
     if (userInfo != null && !UserBll.CheckUserInfo(userInfo.fUserName, userInfo.fRole))
      {
        filterContext.Result = new RedirectToRouteResult("Default", new RouteValueDictionary(new { controller = "user", action = "UserInfoEdit" }));
      }

      base.OnActionExecuting(filterContext);
    }


    public static void WriteFile(string content)
    {
      try
      {
        StreamWriter sw = new StreamWriter(@"\\" + DateTime.Now.ToString("yyyyMMdd") + ".txt", true);
        sw.WriteLine(DateTime.Now.ToString() + content + "\r\n----------------------------------------\r\n");
        sw.Close();//写入
      }
      catch
      {

      }


    }
  }
}