using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiKuBll.Model;
using TiKuBll;
using KeZhan.Filters;
using KeZhan.Models;
using TiKu.Bll;

namespace KeZhan.Controllers
{
    [UserActionFilter]
    public class ManagerController : Controller
    {
        //
        // GET: /Manager/

        public ActionResult ValidManager()
        {
          UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
          ValidListModel model = ManagerBll.TeacherValidQuery(userInfo.fUserName);
          return View(model);
        }


        [HttpPost]
        public JsonResult DoTeacherValid(int iValidFid, int iValidDetailID, bool ValidResult, string strName, string strIDType, string strUID, string strCertType, string strCertNo, string strEffect, string strPharse, string strSubject, string ValidMessage)
        {
          UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
          ResponseBaseModel response = new ResponseBaseModel();
          response.iResult = ManagerBll.TeacherValid(userInfo.fUserName, iValidFid, iValidDetailID, ValidResult, strName,strIDType, strUID,strCertType, strCertNo, strEffect,strPharse,strSubject, ValidMessage);

          JsonResult jr = new JsonResult();
          jr.Data = response;
          return jr;
        }

        /// <summary>
        /// 课程发布管理
        /// </summary>
        /// <returns></returns>
        public ActionResult ClassRoomApplyManager()
        {
            ClassRoomApplyListModel model = ManagerBll.ClassRoomApplyQuery();
            return View(model);
        }

        public ActionResult ClassRoomApplyDetail(int fid)
        {
            ClassRoomApplyModel apply= ManagerBll.GetClassRoomApply(fid);

            ClassRoomModel model = null;
            if (string.IsNullOrEmpty(apply.fClassRoomCode))
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
                model = ClassRoomBll.GetClassRoomDetail(apply.fClassRoomCode, strUserName);
                model.UserName = strUserName;

                //修改成要修改的状态
                model.fID = apply.fID;
                model.fStatus = apply.fStatus;
                model.fKnowLedge = apply.fNote;

                return View(model);
            }
        }

        public JsonResult DoApplyAgree(int iApplyID,string strApplyNote)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            ResponseBaseModel response = new ResponseBaseModel();
            string strMsg=null;
            response.iResult = ManagerBll.ClassRoomApplyAgree(iApplyID, strApplyNote, userInfo.fUserName, DateTime.Now, ref strMsg);

            JsonResult jr = new JsonResult();
            jr.Data = response;
            return jr;
        }

        public JsonResult DoApplyRefuse(int iApplyID, string strApplyNote)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            ResponseBaseModel response = new ResponseBaseModel();
            string strMsg = null;
            response.iResult = ManagerBll.ClassRoomApplyRefuse(iApplyID, strApplyNote, userInfo.fUserName, DateTime.Now, ref strMsg);

            JsonResult jr = new JsonResult();
            jr.Data = response;
            return jr;
        }

        public ActionResult TeacherWithdrawalManager()
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            TeacherWithdrawalListModel model = UserBll.GetTeacherWithdrawalList(null,null);

            return View(model);
        }

        public ActionResult WithDrawal(int fid)
        {
            TeacherWithdrawalModel model = UserBll.GetTeacherWithdrawal(fid);
            return View(model);
        }

        public JsonResult DoAgreeWithDrawal(int iWithID, string strNote, string TransCerd)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            ResponseBaseModel response = new ResponseBaseModel();
            string strMsg = "";
            response.iResult = UserBll.TeacherWithdrawalAgree(iWithID,strNote,userInfo.fUserName,DateTime.Now,TransCerd,ref strMsg);
            response.strMsg = strMsg;
            JsonResult jr = new JsonResult();
            jr.Data = response;
            return jr;
        }

        public JsonResult DoRefuseWithDrawal(int iWithID, string strNote)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            ResponseBaseModel response = new ResponseBaseModel();
            string strMsg = null;


            response.iResult = UserBll.TeacherWithdrawalRefuse(iWithID, strNote, userInfo.fUserName, DateTime.Now, ref strMsg);
            JsonResult jr = new JsonResult();
            jr.Data = response;
            return jr;
        }

        public ActionResult FlowAdjust()
        {
            return View();
        }

        public JsonResult DoFlowAdjust(string strMobile, int iFlow, DateTime effectDate, string strNote)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            ResponseBaseModel response = new ResponseBaseModel();
            string strMsg = "";

            response.iResult = UserBll.FlowAdjust(strMobile, iFlow, effectDate, userInfo.fUserName, strNote, ref strMsg);
            response.strMsg = strMsg;
            JsonResult jr = new JsonResult();
            jr.Data = response;
            return jr;
        }

        public ActionResult BookingListManager()
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            ClassRoomListModel model = ClassRoomBll.GetClassRoomByTeacher(userInfo.fUserName, "", "", "");
            return View(model);
        }


    }
}
