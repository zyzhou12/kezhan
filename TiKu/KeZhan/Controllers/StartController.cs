using KeZhan.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiKuBll;
using TiKuBll.Model;

namespace KeZhan.Controllers
{
    [WeiXingActionFilter]
    public class StartController : Controller
    {
        //
        // GET: /Start/


        public ActionResult MyClassRoomList(string strClassType, string strStatus = null, string strPayType = null, string strType = null)
        {

            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);

            ClassRoomListModel model = null;
            if (strClassType == "buy")
            {
                model = ClassRoomBll.GetMyClassRoom(userInfo.fUserName, strType);
            }
            else if (strClassType == "create")
            {
                model = ClassRoomBll.GetClassRoomByCreateOpr(userInfo.fUserName, strStatus, strPayType, strType);
            }
            else if (strClassType == "teacher")
            {
                model = ClassRoomBll.GetClassRoomByTeacher(userInfo.fUserName, strStatus, strPayType, strType);
            }
            model.listType = strClassType;
            if (strStatus == null)
            {
                model.strStatus = "";
            }
            else
            {
                model.strStatus = strStatus;
            }
            if (strPayType == null)
            {
                model.strPayType = "";
            }
            else
            {
                model.strPayType = strPayType;
            }

            if (strType == null)
            {
                model.strType = "";
            }
            else
            {
                model.strType = strType;
            }
            return View(model);
        }


    }
}
