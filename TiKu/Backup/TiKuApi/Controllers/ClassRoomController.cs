using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiKu.Bll;

namespace TiKu.Api.Controllers
{
    public class ClassRoomController : Controller
    {
        //
        // GET: /ClassRoom/

        public ActionResult GetClassRoomByUser(string strOpenid)
        {
          UserBll.GetUserInfo(strOpenid);

            return View();
        }

    }
}
