using KeZhanManager.Filters;
using KeZhanManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TiKu.Dal;
using TiKu.Entity;

namespace KeZhanManager.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DoValidPass(LoginModel model)
        {

            if (ModelState.IsValid)
            {
                string strMsg = ""; 
                tUserEntity userInfo = tUserDal.UserPassLogin(model.UserName, model.UserPass, "Manager", ref strMsg);
                if (userInfo == null)
                {
                    ModelState.AddModelError("UserPass", "用户名或密码错误");
                    return View("Login");
                }
                else
                {
                    Code.Fun.SetSessionUserInfo(this, userInfo);
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return View("Login");
            }
        }

      


        public ActionResult IndexAction(string strAction)
        {
            switch (strAction)
            {
                case "signout":
                    FormsAuthentication.SignOut();
                    Session.Clear();
                    Session.Abandon();
                    break;
                case "timeout":
                    ModelState.AddModelError("", "登录状态已失效，请重新登录");
                    FormsAuthentication.SignOut();
                    Session.Clear();
                    Session.Abandon();
                    break;
            }
            return RedirectToAction("Login", "Home");
        }



        public JsonResult CheckLoginUser(string redirect_uri)
        {
            tUserEntity user = Code.Fun.GetSessionUserInfo(this);
            ResponseBaseModel response = new ResponseBaseModel();
            if (user == null)
            {
                response.iResult = -9999;
                response.strMsg = redirect_uri;
            }
            else
            {
                response.iResult = 0;
            }

            JsonResult jr = new JsonResult();
            jr.Data = response;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }

        [LoginActionFilter]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Welcome()
        {
            return View();
        }
    }
}
