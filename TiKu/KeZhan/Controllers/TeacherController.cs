using KeZhan.Filters;
using KeZhan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiKu.Bll;
using TiKuBll;
using TiKuBll.Model;

namespace KeZhan.Controllers
{
    [WeiXingActionFilter]
    public class TeacherController : Controller
    {
        //
        // GET: /Teacher/

        public ActionResult TeacherWithdrawalApply()
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            UserBankAccountListModel model = UserBll.GetUserBankAccountList(userInfo.fUserName);

            return View(model);
        }

     
        public ActionResult UserPay()
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            ConfigModel config = ManagerBll.GetSystemConfig("上海");
            UserPayModel model = new UserPayModel();
            model.UserName = userInfo.fUserName;
            model.ClassFee = config.fClassFee;
            return View(model);
        }



        public ActionResult UserRefundList(string strUserName = "", string strStatus = "0")
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            UserRefundListModel model = BookingBll.GetBookingRefundList(strUserName, strStatus, userInfo.fUserName);

            return View(model);
        }


        public ActionResult UserRefund(int iRefundID)
        {
            UserRefundModel model = BookingBll.GetBookingRefund(iRefundID);
            return View(model);
        }

        /// <summary>
        /// 银行账户管理
        /// </summary>
        /// <returns></returns>
        public ActionResult UserBankManager()
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            UserBankAccountListModel model = UserBll.GetUserBankAccountList(userInfo.fUserName);

            return View(model);
        }

        public ActionResult UserBankEdit(int iBankId)
        {
            UserBankAccountModel model = new UserBankAccountModel();
            if (iBankId > 0)
            {
                model = UserBll.GetUserBankAccount(iBankId);
            }
            return View(model);
        }



        public ActionResult UserMemberList()
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            UserMemberListModel model = UserBll.GetUserMemberList(userInfo.fUserName, "", null, null);
            return View(model);
        }


        public ActionResult ValidUploadFile()
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            TeacherValidModel model = new TeacherValidModel();
            model.fUserName = userInfo.fUserName;
            TeacherBaseModel teacher = UserBll.GetUserTeacherBase(userInfo.fUserName);
            model.fStatus = teacher.fStatus;
            model.userInfo = userInfo;

            return View(model);
        }
    }
}
