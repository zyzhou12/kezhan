using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeZhan.Models;
using TiKuBll.Model;
using TiKuBll;
using TiKu.Bll;
using KeZhan.Filters;
using System.Text;
using System.Security.Cryptography;
using KeZhan.Code;
using System.Web.Security;

namespace KeZhan.Controllers
{
  public class OpenController : Controller
  {
      public ActionResult WebInfo()
      {
          return View();
      }
      public ActionResult QAInfo()
      {
          return View();
      }

    public ActionResult RegsiterLogin(string redirect_uri = null)
    {

      if (string.IsNullOrEmpty(redirect_uri))
      {
        redirect_uri = Request.Url.ToString();
      }

      LoginModel model = new LoginModel();
      model.redirect_uri = redirect_uri;

      return View(model);
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
      return RedirectToAction("ClassRoomList", "Open");
    }

    [HttpPost]
    public JsonResult DoSendCode(string strMobile)
    {
      string strMsg = "";
      ResponseBaseModel response = new ResponseBaseModel();
      if (string.IsNullOrEmpty(strMobile))
      {
        response.iResult = -1;
        response.strMsg = "请输入手机号";
      }
      else
      {
        response.iResult = UserBll.UserSendCode(strMobile, ref strMsg);
        response.strMsg = strMsg;
      }

      JsonResult jr = new JsonResult();
      jr.Data = response;
      jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
      return jr;
    }


    [HttpPost]
    public JsonResult DoValidCode(string strMobile, string strCode, string strRole)
    {
      string strMsg = "";
      ResponseBaseModel response = new ResponseBaseModel();
      if (string.IsNullOrEmpty(strMobile))
      {
        response.iResult = -1;
        response.strMsg = "请输入手机号";
      }
      else if (string.IsNullOrEmpty(strCode))
      {
        response.iResult = -1;
        response.strMsg = "请输入验证码";
      }
      else
      {
        UserInfoModel userInfo = UserBll.UserLogin(strMobile, strCode, strRole, "Web", ref strMsg);
        response.strMsg = strMsg;

        if (userInfo != null)
        {

          Code.Fun.SetSessionUserInfo(this, userInfo);
          if (string.IsNullOrEmpty(userInfo.IsPassWord))
          {
            response.iResult = 2;
          }
          else if (UserBll.CheckUserInfo(userInfo.fUserName, userInfo.fRole))
          {
            response.iResult = 0;
          }
          else
          {
            response.iResult = 1;
          }
        }
        else
        {
          response.iResult = -1;
        }
      }

      JsonResult jr = new JsonResult();
      jr.Data = response;
      return jr;
    }



    public ActionResult TeacherRegsiter(string redirect_uri = null)
    {

      if (string.IsNullOrEmpty(redirect_uri))
      {
        redirect_uri = Request.Url.ToString();
      }

      LoginModel model = new LoginModel();
      model.redirect_uri = redirect_uri;

      return View(model);
    }

    [HttpPost]
    public JsonResult DoTeacherValidCode(string strMobile, string strCode, string strPass, string strPass2)
    {
      string strMsg = "";
      ResponseBaseModel response = new ResponseBaseModel();
      if (string.IsNullOrEmpty(strMobile))
      {
        response.iResult = -1;
        response.strMsg = "请输入手机号";
      }
      else if (string.IsNullOrEmpty(strCode))
      {
        response.iResult = -1;
        response.strMsg = "请输入验证码";
      }
      else if (string.IsNullOrEmpty(strPass))
      {
        response.iResult = -1;
        response.strMsg = "请输入密码";
      }
      else if (string.IsNullOrEmpty(strPass2))
      {
        response.iResult = -1;
        response.strMsg = "请再次输入密码";
      }
      else
      {
        UserInfoModel userInfo = UserBll.UserLogin(strMobile, strCode, "Teacher", "Web", ref strMsg);
        response.strMsg = strMsg;

        if (userInfo != null)
        {
          //设置密码
          UserBll.SaveUserInfo(userInfo.fUserName,"fPassWord",strPass);

          Code.Fun.SetSessionUserInfo(this, userInfo);
          if (UserBll.CheckUserInfo(userInfo.fUserName, userInfo.fRole))
          {
            response.iResult = 0;
          }
          else
          {
            response.iResult = 1;
          }
        }
        else
        {
          response.iResult = -1;
        }
      }

      JsonResult jr = new JsonResult();
      jr.Data = response;
      return jr;
    }

    public ActionResult StudentRegsiter(string redirect_uri = null)
    {

      if (string.IsNullOrEmpty(redirect_uri))
      {
        redirect_uri = Request.Url.ToString();
      }

      LoginModel model = new LoginModel();
      model.redirect_uri = redirect_uri;

      return View(model);
    }

    [HttpPost]
    public JsonResult DoStudentValidCode(string strMobile, string strCode,string strNickName, string strPass, string strPass2)
    {
      string strMsg = "";
      ResponseBaseModel response = new ResponseBaseModel();
      if (string.IsNullOrEmpty(strMobile))
      {
        response.iResult = -1;
        response.strMsg = "请输入手机号";
      }
      else if (string.IsNullOrEmpty(strCode))
      {
        response.iResult = -1;
        response.strMsg = "请输入验证码";
      }
      else if (string.IsNullOrEmpty(strNickName))
      {
          response.iResult = -1;
          response.strMsg = "请输入昵称";
      }
      else if (string.IsNullOrEmpty(strPass))
      {
        response.iResult = -1;
        response.strMsg = "请输入密码";
      }
      else if (string.IsNullOrEmpty(strPass2))
      {
        response.iResult = -1;
        response.strMsg = "请再次输入密码";
      }
      else
      {
        UserInfoModel userInfo = UserBll.UserLogin(strMobile, strCode, "Student", "Web", ref strMsg);
        response.strMsg = strMsg;

        if (userInfo != null)
        {
            UserBll.SaveUserInfo(userInfo.fUserName, "fNickName", strNickName);
          //设置密码
          UserBll.SaveUserInfo(userInfo.fUserName, "fPassWord", strPass);

          Code.Fun.SetSessionUserInfo(this, userInfo);
          if (UserBll.CheckUserInfo(userInfo.fUserName, userInfo.fRole))
          {
            response.iResult = 0;
          }
          else
          {
            response.iResult = 1;
          }
        }
        else
        {
          response.iResult = -1;
        }
      }

      JsonResult jr = new JsonResult();
      jr.Data = response;
      return jr;
    }


    [HttpPost]
    public JsonResult DoValidPass(string strMobile, string strPass)
    {
      string strMsg = "";
      ResponseBaseModel response = new ResponseBaseModel();
      if (string.IsNullOrEmpty(strMobile))
      {
        response.iResult = -1;
        response.strMsg = "请输入用户名或手机号";
      }
      else if (string.IsNullOrEmpty(strPass))
      {
        response.iResult = -1;
        response.strMsg = "请输入密码";
      }
      else
      {
        UserInfoModel userInfo = UserBll.UserPassLogin(strMobile, strPass, "Web", ref strMsg);
        response.strMsg = strMsg;

        

        if (userInfo != null)
        {

          Code.Fun.SetSessionUserInfo(this, userInfo);
          if (UserBll.CheckUserInfo(userInfo.fUserName, userInfo.fRole))
          {

            response.iResult = 0;
          }
          else
          {
            response.iResult = 1;
          }
        }
        else
        {
          response.iResult = -1;
        }
      }

      JsonResult jr = new JsonResult();
      jr.Data = response;
      return jr;
    }



    public ActionResult ClassRoomList(string strCity = null)
    {
      if (string.IsNullOrEmpty(strCity))
      {
        strCity = "上海";
      }
      ClassRoomListModel model = ClassRoomBll.GettClassRoomList(strCity, "", null, null);
        //K12
      model.classRoomList = model.classRoomList.Where(m => "1;2;3;4".Contains(m.fPharse)).ToList();
      return View(model);
    }

    public ActionResult ClassRoomList2(string strCity = null)
    {
        if (string.IsNullOrEmpty(strCity))
        {
            strCity = "上海";
        }

        ClassRoomListModel model = ClassRoomBll.GettClassRoomList(strCity, "8", null, null);

        return View(model);
    }
    public ActionResult ClassRoomList3(string strPharse, string strCity = null)
    {
        if (string.IsNullOrEmpty(strCity))
        {
            strCity = "上海";
        }

        ClassRoomListModel model = ClassRoomBll.GettClassRoomList(strCity, "9", null, null);

        return View(model);
    }

    /// <summary>
    /// 条件查询
    /// </summary>
    /// <param name="strCity"></param>
    /// <param name="strPharse"></param>
    /// <param name="strGrade"></param>
    /// <param name="strSubjet"></param>
    /// <returns></returns>
    public ActionResult QueryClassRoom(string strPharse, string strGrade, string strSubjet,string strCity=null)
    {
        if (string.IsNullOrEmpty(strCity))
        {
            strCity = "上海";
        }
      ClassRoomListModel model = ClassRoomBll.GettClassRoomList(strCity, strPharse, strGrade, strSubjet);



      return PartialView("ClassRoomControl", model);
    }

    public ActionResult ClassRoomDetail(string strClassRoomCode)
    {
      ClassRoomModel model = null;
      if (string.IsNullOrEmpty(strClassRoomCode))
      {
        return View(model);
      }
      else
      {
        UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
        string strUserName = "";
        if (userInfo != null)//已登录
        {
          strUserName = userInfo.fUserName;
        }
        model = ClassRoomBll.GetClassRoomDetail(strClassRoomCode, strUserName);
        model.UserName = strUserName;



        return View(model);
      }
    }

    public ActionResult About()
    {
      return View();
    }

    public ActionResult TeacherInfo()
    {
      return View();
    }
    public ActionResult KeZhanDesc()
    {
      return View();
    }
    public ActionResult KeZhanTeachDesc()
    {
      return View();
    }
  }
}
