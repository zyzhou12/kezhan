using KeZhan.Filters;
using KeZhan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiKu.Bll;
using TiKuBll.Model;

namespace KeZhan.Controllers
{
    [UserActionFilter]
    public class MessageController : Controller
    {
        //
        // GET: /Message/

        public JsonResult GetMessageCount()
        {
            ResponseBaseModel response = new ResponseBaseModel();
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            MessageListModel model = UserBll.GetMessageList(userInfo.fUserName, "0");
            response.iResult = model.messageList.Count;

            JsonResult jr = new JsonResult();
            jr.Data = response;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }

        public ActionResult MessageList()
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            MessageListModel model = UserBll.GetMessageList(userInfo.fUserName, "");
            return View(model);
        }

        public JsonResult MessageLook(int iMessageID)
        {
            ResponseBaseModel response = new ResponseBaseModel();
            response.iResult= UserBll.MessageUpdate(iMessageID);
            JsonResult jr = new JsonResult();
            jr.Data = response;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }
    }
}
