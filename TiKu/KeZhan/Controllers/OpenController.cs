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


  }
}
