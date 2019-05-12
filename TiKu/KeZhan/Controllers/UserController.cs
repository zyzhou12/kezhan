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
using System.Configuration;

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

     





        public ActionResult UserAccount()
        {
            return View();
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


        protected virtual bool IsWechatBrowser
        { get { return (Request.UserAgent.ToLower().Contains("micromessenger")); } }

        public ActionResult UserBooking(string strBookingNo)
        {

            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            BookingModel model = BookingBll.GetBookingByNo(strBookingNo);
            model.UserAccountAmount = UserBll.GetUserAccountAmount(userInfo.fUserName);
            model.UserName = userInfo.fUserName;
            model.OpenID = userInfo.fOpenID;
            model.BookingRefund = BookingBll.GetRefundByBookingNo(strBookingNo);
            if(IsWechatBrowser)
            {
                if (string.IsNullOrEmpty(userInfo.fOpenID))
                {

                    return Redirect(ConfigurationManager.AppSettings["PayCallBack"].ToString() + "/Open/WeiXinBooking?strParam=" + strBookingNo + "&strState=" + userInfo.fUserName);
                }
            }

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

      
        public ActionResult MyFocusTeacher()
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            UserListModel model = UserBll.GetFocusTeacherList(userInfo.fUserName);
            return View(model);
        }


    }
}
