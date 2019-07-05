using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TiKu.Api.Models;
using TiKu.Dal;
using TiKu.Entity;

namespace TiKu.Api.Controllers
{
    public class UserController : Controller
    {

        public JsonResult DoLogin(string code)
        {
            string strAppID = "wx3988838a7c3fd439";
            string strSecret = "6250ac143711378cabe945d811c8c6cd";


            string sUrl = String.Format("https://api.weixin.qq.com/sns/jscode2session?appid={0}&secret={1}&js_code={2}&grant_type=authorization_code", strAppID, strSecret, code);

            WebClient MyWebClient = new WebClient();

            MyWebClient.Credentials = CredentialCache.DefaultCredentials;
            Byte[] pageData = MyWebClient.DownloadData(sUrl);

            String strJson = Encoding.UTF8.GetString(pageData) ?? "";

            JavaScriptSerializer jss = new JavaScriptSerializer();

            UserLoginModel loginInfo = jss.Deserialize<UserLoginModel>(strJson);

            tUserEntity user = tUserDal.GettUserByOpenID(loginInfo.unionid);




            UserResponseModel response = new UserResponseModel();
            response.Role = "Student";
            if (user != null)
            {
                response.Mobile = user.fMobile;
                response.UserName = user.fUserName;
                response.NickName = user.fNickName;
                response.Role = user.fRole;
            }
            response.SessionKey = loginInfo.session_key;
            response.UnionID = loginInfo.unionid;
            response.OpenID = loginInfo.openid;

            response.resultCode = 1;
            response.resultMessage = "";

            JsonResult jsResult = new JsonResult();
            jsResult.Data = response;
            jsResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsResult;
        }


        public JsonResult DoUpdateMobile(string strOpenId, string strMobile)
        {
            ResponseBase response = new ResponseBase();
            List<tUserEntity> list = new List<tUserEntity>();
            tUserEntity user = tUserDal.GettUserByMobile(strMobile);
            if (user != null)
            {
                user.fOpenID = strOpenId;
                list.Add(user);
                response.resultCode = tUserDal.Modify(list, "update", null, null);
            }
            else
            {
                user = new tUserEntity();
                user.fMobile = strMobile;
                user.fOpenID = strOpenId;
                user.fUserName = "2" + strMobile;
                user.fRole = "Student";
                list.Add(user);
                response.resultCode = tUserDal.Modify(list, "insert", null, null);
            }
            JsonResult jsResult = new JsonResult();
            jsResult.Data = response;
            jsResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsResult;

        }

        //
        // GET: /User/
        /// <summary>
        /// 导入用户
        /// </summary>
        /// <returns></returns>
        public ActionResult ImportUser()
        {
            string strUrl = "https://console.tim.qq.com/v4/im_open_login_svc/account_import?usersig=eJw9zl1PgzAUgOH/wq1GWqBsmOwCURZlOmGo84p0tLATt9JAGTDjf7cS5u375Hx8G+lqc0PzvGqFytQguXFr2O7ctYzrkYBxoaAAXmugcN63lB1BTEqlBJZRldk1046m3LCvbCTdsIMQnhFEyIS8l1DzjBZq3Gmjy9SJ1w1UQjcLYYItLegfFRz/XsPEIXPPIY53OQWlzs8PcfDoV90HSlanu6reffbLThS7bWlGkLjnbepGNAwGdvXK7mO098GPgxdTPnkofF/KQ++tBW6SMN3wMH+L2m4oLRMXB99Ds3W5WBg/vx7/WVg=&identifier=aizhuadmin&sdkappid=1400175055&random=99999999&contenttype=json";
            string strBody = "{\"Identifier\":\"test\",\"Nick\":\"test\",\"FaceUrl\":\"http://www.qq.com\"}";


            return View();
        }



    }
}
