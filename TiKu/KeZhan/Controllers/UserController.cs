using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeZhan.Models;
using TiKu.Bll;
using KeZhan.Filters;
using TiKuBll.Model;
using System.Web.Script.Serialization;
using TiKuBll;
using System.IO;
using System.Xml.Serialization;

namespace KeZhan.Controllers
{
    [UserActionFilter]
    public class UserController : Controller
    {
        //
        // GET: /User/
        public ActionResult UserInfoEdit()
        {


            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            if (UserBll.CheckUserInfo(userInfo.fUserName, userInfo.fRole))//已补全信息
            {
                return RedirectToAction("ClassRoomList", "Open");
            }
            else
            {

                UserBaseModel model = new UserBaseModel();
                model.userRole = userInfo.fRole;
                model.userInfo = userInfo;
                if (userInfo.fRole == "Teacher")
                {
                    model.teacherInfo = UserBll.GetUserTeacherBase(userInfo.fUserName);
                }
                else if (userInfo.fRole == "Student")
                {
                    model.studentInfo = UserBll.GetUserStudentBase(userInfo.fUserName);
                }
                else if (userInfo.fRole == "Parents")
                {
                    model.parentsInfo = UserBll.GetUserParentsBase(userInfo.fUserName);
                }
                return View(model);
            }

        }




        public ActionResult ValidInfo(int iValidID)
        {

            ValidDetailListModel model = UserBll.GetTeachValidDetailListByID(iValidID);
            return View(model);
        }

        public ActionResult UpdatePassWord(string redirect_uri = null)
        {
            FileModel model = new FileModel();
            if (string.IsNullOrEmpty(redirect_uri))
            {
                model.FileUrl = "";
            }
            else
            {
                model.FileUrl = redirect_uri;
            }
            return View(model);
        }

        public ActionResult UserInfo()
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);

            UserBaseModel model = new UserBaseModel();
            model.userRole = userInfo.fRole;
            model.userInfo = UserBll.GetUserInfo(userInfo.fUserName);
            if (userInfo.fRole == "Teacher")
            {
                model.teacherInfo = UserBll.GetUserTeacherBase(userInfo.fUserName);
                model.validInfo = UserBll.GettTeachValidList(userInfo.fUserName);
            }
            else if (userInfo.fRole == "Student")
            {
                model.studentInfo = UserBll.GetUserStudentBase(userInfo.fUserName);
            }
            else if (userInfo.fRole == "Parents")
            {
                model.parentsInfo = UserBll.GetUserParentsBase(userInfo.fUserName);
            }

            return View(model);
        }

      
        public ActionResult WeiXinLogin(string redirect_uri = null)
        {

            if (string.IsNullOrEmpty(redirect_uri))
            {
                redirect_uri = Request.Url.ToString();
            }
            string strAppID = "";
            string strState = "kezhanlogin";

            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);


            string oauthUrl = string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_base&state={2}#wechat_redirect", strAppID, "http://weixin.aizhusoft.com/AuthoRedirect.aspx?url=http://hotellife.aizhusoft.com/usercount/DoWeiXinLogin?strParams=" + strAppID, strState);


            return Redirect(oauthUrl);
        }




        //public ActionResult DoWeiXinLogin(string code, string strParams, string state)
        //{
        //  string redirecturi = "http://www.baidu.com?1=" + code + strParams + state;

        //  try
        //  {
        //    string strAppID = "";
        //    string strAppSecret = "";

        //    string sUrl = String.Format(APIRequest.GetUrl("getaccesstoken"), strAppID, strAppSecret, code);

        //    String strJson = APIRequest.GetData(sUrl);

        //    JavaScriptSerializer jss = new JavaScriptSerializer();

        //    PublicTokenModel token = jss.Deserialize<PublicTokenModel>(strJson);

        //    if (token.access_token != null)
        //    {
        //      //更新用户信息
        //      ResponseModel<UserModel> Response = UserBll.getUser(microPublic.fPublicID, token.openid);

        //      //先登录

        //      UserInfoModel userInfo = new UserInfoModel();
        //      userInfo.User = Response.ResultObj;


        //      if (!string.IsNullOrEmpty(token.access_token))
        //      {
        //        string strtoken = HotelLife.Bll.ConfigBll.GetToken(strAppID);

        //        //获取个人信息
        //        //sUrl = String.Format(APIRequest.GetUrl("getuserinfo"), strtoken, token.openid);
        //        //strJson = APIRequest.GetData(sUrl);
        //        //PublicUserModel user = jss.Deserialize<PublicUserModel>(strJson);
        //        //userInfo.User.fNickName = user.nickname;
        //        //userInfo.User.fHeadImg = user.headimgurl;
        //        //userInfo.User.fsubscribe = user.subscribe;
        //      }
        //      Code.Fun.SetSessionUserInfo(this, userInfo);



        //    }


        //    return Redirect("");
        //  }
        //  catch (Exception ex)
        //  {
        //    return Redirect(redirecturi + "&" + ex.Message);
        //  }
        //}




        public ActionResult UpdateRole()
        {
            return View();
        }




        public ActionResult UserAccount()
        {
            return View();
        }

        public ActionResult QueryUserAccount(string strTradingType, string strSystem, string strType, DateTime beginDate, DateTime endDate)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            UserAccountListModel model = UserBll.GetUserAccountData(userInfo.fUserName, strTradingType, strSystem, strType, beginDate, endDate);

            return PartialView("UserAccountListControl", model);
        }



        //
        // GET: /Booking/
        public ActionResult BookingList(string strStatus = null)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            BookingListModel model = BookingBll.GetBookingList(userInfo.fUserName, strStatus, DateTime.Now, DateTime.Now);
            model.fStatus = strStatus;
            return View(model);
        }




        public ActionResult UserBooking(string strBookingNo)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            BookingModel model = BookingBll.GetBookingByNo(strBookingNo);
            model.UserAccountAmount = UserBll.GetUserAccountAmount(userInfo.fUserName);
            model.UserName = userInfo.fUserName;


            return View(model);
        }


        public ActionResult ClassRoomBooking(string strClassRoomCode)
        {
            //校验是否已购买
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            BookingModel booking = BookingBll.GettBooking(userInfo.fUserName, "ClassRoom", strClassRoomCode, "已支付");
            if (booking != null)
            {
                return RedirectToAction("UserBooking", "Booking", new { strBookingNo = booking.fBookingNo });
            }
            else
            {
                // ClassRoomModel model = ClassRoomBll.GetClassRoomByCode(strClassRoomCode, null);
                ClassRoomModel model = ClassRoomBll.GetClassRoomDetail(strClassRoomCode, null);
                return View(model);
            }
        }

        /// <summary>
        /// 申请退款
        /// </summary>
        /// <param name="strBookingNo"></param>
        /// <returns></returns>
        public ActionResult BookingRefund(string strBookingNo)
        {
            BookingModel model = BookingBll.GetBookingByNo(strBookingNo);
            model.MaxReturnAmount = BookingBll.GetBokingMaxReturnAmount(strBookingNo);
            return View(model);
        }

      



    }
}
