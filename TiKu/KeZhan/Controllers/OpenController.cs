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
using System.Web.Script.Serialization;
using System.Configuration;

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
    public ActionResult ClassRoomList3(string strCity = null)
    {
        if (string.IsNullOrEmpty(strCity))
        {
            strCity = "上海";
        }

        ClassRoomListModel model = ClassRoomBll.GettClassRoomList(strCity, "9", null, null);

        return View(model);
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


    public ActionResult WeiXinLogin(string strParam, string strState)
    {

        string strAppID = "wx67d892b056103f43";

        string oauthUrl = string.Format(WXApiRequest.GetUrl("authorize"), strAppID, "http://weixin.aizhusoft.com/AuthoRedirect.aspx?url=http://test.2kezhan.com/open/DoWeiXinLogin?strParams=" + strParam, strState);

        return Redirect(oauthUrl);
        
    }




    public ActionResult DoWeiXinLogin(string code, string strParams, string state)
    {
        string redirecturi = "http://www.baidu.com?1=" + code + strParams + state;

        try
        {
            string strAppID = "wx67d892b056103f43";
            string strAppSecret = "293e98866a3ee91bf3c485a569b90319";

            string sUrl = String.Format(WXApiRequest.GetUrl("getaccesstoken"), strAppID, strAppSecret, code);

            String strJson = WXApiRequest.GetData(sUrl);

            JavaScriptSerializer jss = new JavaScriptSerializer();

            PublicTokenModel token = jss.Deserialize<PublicTokenModel>(strJson);

            if (token.access_token != null)
            {
                //更新用户信息
                //ResponseModel<UserModel> Response = UserBll.getUser(microPublic.fPublicID, token.openid);
                string strMsg = "";
                UserInfoModel userInfo = UserBll.UserWeiChatLogin(token.openid,state, ref strMsg);
               
               // userInfo.User = Response.ResultObj;


                //if (!string.IsNullOrEmpty(token.access_token))
                //{
                //    string strtoken = HotelLife.Bll.ConfigBll.GetToken(strAppID);

                //    //获取个人信息
                //    sUrl = String.Format(WXApiRequest.GetUrl("getuserinfo"), strtoken, token.openid);
                //    strJson = WXApiRequest.GetData(sUrl);
                //    PublicUserModel user = jss.Deserialize<PublicUserModel>(strJson);
                //    userInfo.User.fNickName = user.nickname;
                //    userInfo.User.fHeadImg = user.headimgurl;
                //    userInfo.User.fsubscribe = user.subscribe;
                //}
                Code.Fun.SetSessionUserInfo(this, userInfo);


            }


            string redirect_uri = ConfigurationManager.AppSettings["PayCallBack"].ToString()+"/user/userBooking?strBookingNo=" + strParams;


            return Redirect(redirect_uri);
        }
        catch (Exception ex)
        {
            return Redirect(redirecturi + "&" + ex.Message);
        }
    }


  }
}
