using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TiKu.Api.Controllers
{
    public class UserController : Controller
    {

      //public JsonResult DoLogin(string code)
      //{
      //  string strAppID = "wxac8ba84fbd65faf7";
      //  string strSecret = "4853b7cdcd3804c7b143b9f2fa76dc56";


      //  string sUrl = String.Format("https://api.weixin.qq.com/sns/jscode2session?appid={0}&secret={1}&js_code={2}&grant_type=authorization_code", strAppID, strSecret, code);

      //  WebClient MyWebClient = new WebClient();

      //  MyWebClient.Credentials = CredentialCache.DefaultCredentials;
      //  Byte[] pageData = MyWebClient.DownloadData(sUrl);

      //  String strJson = Encoding.UTF8.GetString(pageData) ?? "";

      //  JavaScriptSerializer jss = new JavaScriptSerializer();

      //  UserLoginModel loginInfo = jss.Deserialize<UserLoginModel>(strJson);

      //  tUserEntity user = tUserDal.GetUserByOpenID(loginInfo.openid);



      //  if (user != null)
      //  {
      //    if (user.fUserName != null)
      //    {
      //      tUserRoleEntity role = new tUserRoleEntity();
      //      role.fID = user.fRoleID;

      //      role.fSessionKey = loginInfo.session_key;
      //      role.fUnionID = loginInfo.unionid;

      //      UserBll.UpdateUserRoleInfo(role, "fSessionKey,fUnionID");


      //    }
      //  }

      //  UserResponseModel response = new UserResponseModel();
      //  if (user != null)
      //  {
      //    response.fMobile = user.fMobile;
      //    response.fUserName = user.fUserName;
      //    response.fNickName = user.fNickName;
      //    response.fHotelCode = user.fHotelCode;
      //  }
      //  response.fSessionKey = loginInfo.session_key;
      //  response.fUnionID = loginInfo.unionid;
      //  response.fOpenID = loginInfo.openid;

      //  response.resultCode = 1;
      //  response.resultMessage = "";

      //  JsonResult jsResult = new JsonResult();
      //  jsResult.Data = response;
      //  jsResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
      //  return jsResult;
      //}


        //
        // GET: /User/
      /// <summary>
      /// 导入用户
      /// </summary>
      /// <returns></returns>
        public ActionResult ImportUser()
        {
          string strUrl = "https://console.tim.qq.com/v4/im_open_login_svc/account_import?usersig=eJw9zl1PgzAUgOH/wq1GWqBsmOwCURZlOmGo84p0tLATt9JAGTDjf7cS5u375Hx8G+lqc0PzvGqFytQguXFr2O7ctYzrkYBxoaAAXmugcN63lB1BTEqlBJZRldk1046m3LCvbCTdsIMQnhFEyIS8l1DzjBZq3Gmjy9SJ1w1UQjcLYYItLegfFRz/XsPEIXPPIY53OQWlzs8PcfDoV90HSlanu6reffbLThS7bWlGkLjnbepGNAwGdvXK7mO098GPgxdTPnkofF/KQ++tBW6SMN3wMH+L2m4oLRMXB99Ds3W5WBg/vx7/WVg=&identifier=aizhuadmin&sdkappid=1400175055&random=99999999&contenttype=json";
          string strBody="{\"Identifier\":\"test\",\"Nick\":\"test\",\"FaceUrl\":\"http://www.qq.com\"}";


            return View();
        }


      
    }
}
