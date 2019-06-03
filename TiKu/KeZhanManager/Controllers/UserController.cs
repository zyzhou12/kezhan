using KeZhanManager.Filters;
using KeZhanManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiKu.Dal;
using TiKu.Entity;

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
            List<tUserEntity> userList = tUserDal.GetUserList(strBeginDate, strEndDate, strUserName);
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
            List<tTeachValidEntity> validList = tTeachValidDal.GettTeachValidList(strBeginDate, strEndDate, strMobile, strStatus);
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



    }
}
