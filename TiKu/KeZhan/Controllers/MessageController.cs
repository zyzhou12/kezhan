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

      
        public ActionResult MessageList()
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            MessageListModel model = UserBll.GetMessageList(userInfo.fUserName, "");
            return View(model);
        }

      
    }
}
