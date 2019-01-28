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

        public ActionResult TeacherValid(int iValidFid)
        {
          TeacherValidModel model = UserBll.GetTeacherValid(iValidFid);
          return PartialView("TeacherValid", model);
        }


        [HttpPost]
        public JsonResult DoTeacherValid(int iValidFid, bool ValidResult, string strName, string strUID, string strCertNo, string strEffect, string ValidMessage)
        {
          UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
          ResponseBaseModel response = new ResponseBaseModel();
          response.iResult = ManagerBll.TeacherValid(userInfo.fUserName, iValidFid, ValidResult,strName,strUID,strCertNo,strEffect, ValidMessage);

          JsonResult jr = new JsonResult();
          jr.Data = response;
          return jr;
        }

        public ActionResult ClassRoomApplyManager()
        {
            TeacherValidModel model = ClassRoomBll.getApplyList();
            return PartialView("TeacherValid", model);
        }


        [HttpPost]
        public JsonResult DoTeacherValid(int iValidFid, bool ValidResult, string strName, string strUID, string strCertNo, string strEffect, string ValidMessage)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            ResponseBaseModel response = new ResponseBaseModel();
            response.iResult = ManagerBll.TeacherValid(userInfo.fUserName, iValidFid, ValidResult, strName, strUID, strCertNo, strEffect, ValidMessage);

            JsonResult jr = new JsonResult();
            jr.Data = response;
            return jr;
        }
    }
}
