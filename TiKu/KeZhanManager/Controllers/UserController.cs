using KeZhanManager.Filters;
using KeZhanManager.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiKu.Dal;
using TiKu.Entity;
using Trip8H.Common;

namespace KeZhanManager.Controllers
{
    [LoginActionFilter]
    public class UserController : Controller
    {
        //
        // GET: /User/

        public ActionResult UserQuery()
        {
            return View();
        }

        public ActionResult DoGetUserList(string strBeginDate, string strEndDate, string strUserName)
        {
            DataTable dt = tUserDal.GetUserList(strBeginDate, strEndDate, strUserName);
            List<UserModel> userList = PubFun.DataTableToObjects<UserModel>(dt);
            UserListModel model = new UserListModel();
            if (userList != null)
            {
                model.iCount = userList.Count;
                model.userList = userList;
            }
            return PartialView("UserList", model);
        }

        public ActionResult UserDetail(string strUserName)
        {
            tUserEntity user = tUserDal.GettUser(strUserName);
            return View(user);
        }

        public ActionResult UserValidQuery()
        {
            return View();
        }
        public ActionResult GetUserValidList(string strBeginDate, string strEndDate, string strMobile, string strStatus)
        {
            DataTable dt = tTeachValidDal.GettTeachValidList(strBeginDate, strEndDate, strMobile, strStatus);
            List<TeachValidModel> validList = PubFun.DataTableToObjects<TeachValidModel>(dt);
            UserValidListModel model = new UserValidListModel();
            if (validList != null)
            {
                model.iCount = validList.Count;
                model.validList = validList;
            }
            return PartialView("UserValidList", model);
        }

        public ActionResult UserValidDetail(int iValidID)
        {
            UserValidModel model = new UserValidModel();
            model.validDetail = tTeacherValidDetailDal.GettTeacherValidDetailList(iValidID, null);
            model.valid = tTeachValidDal.GettTeachValid(iValidID);
            return View(model);
        }

        public ActionResult UserValid(int iValidDetailID)
        {
            tTeacherValidDetailEntity detail = tTeacherValidDetailDal.GettTeacherValidDetail(iValidDetailID);
            tTeachValidEntity valid = tTeachValidDal.GettTeachValid(detail.fTeacherValidID);
            ValidModel model = new ValidModel();
            if (valid != null)
            {
                model.fIDCard1 = valid.fIDCard1;
                model.fIDCard2 = valid.fIDCard2;
                model.fName = valid.fName;
                model.fIDType = valid.fIDType;
                model.fUID = valid.fUID;
                model.fUserName = valid.fUserName;
                model.fTeacherValidID = valid.fID;

                model.fID = detail.fID;
                model.fCertNo = detail.fCertNo;
                model.fCertType = detail.fCertType;
                model.fNote = detail.fNote;
                model.fPharse = detail.fPharse;
                model.fSubject = detail.fSubject;
                model.fTeachCert1 = detail.fTeachCert1;
                model.fTeachCert2 = detail.fTeachCert2;
                model.fUploadDate = detail.fUploadDate;
                model.fValidDate = detail.fValidDate;
                model.fValidUser = detail.fValidUser;

                model.fStatus = valid.fStatus;
            }

            return View(model);
        }
        [HttpPost]
        public JsonResult DoTeacherValid(int iValidFid, int iValidDetailID, bool ValidResult, string strName, string strIDType, string strUID, string strCertType, string strCertNo, string strEffect, string strPharse, string strSubject, string ValidMessage)
        {
            tUserEntity userInfo = Code.Fun.GetSessionUserInfo(this);
            ResponseBaseModel response = new ResponseBaseModel();
            response.iResult = tTeachValidDal.TeacherValid(userInfo.fUserName, iValidFid, iValidDetailID, ValidResult, strName, strIDType, strUID, strCertType, strCertNo, strEffect, strPharse, strSubject, ValidMessage);

            JsonResult jr = new JsonResult();
            jr.Data = response;
            return jr;
        }
    }
}
