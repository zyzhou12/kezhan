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


        public ActionResult MyClassRoomList(string strRoleType, string strStatus = null, string strPayType = null, string strType = null,string strClassType=null)
        {

            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);

            ClassRoomListModel model = null;
            if (strRoleType == "buy")
            {
                model = ClassRoomBll.GetMyClassRoom(userInfo.fUserName, strType);
            }
            else if (strRoleType == "create")
            {
                model = ClassRoomBll.GetClassRoomByCreateOpr(userInfo.fUserName, strStatus, strPayType, strType, strClassType);
            }
            else if (strRoleType == "teacher")
            {
                model = ClassRoomBll.GetClassRoomByTeacher(userInfo.fUserName, strStatus, strPayType, strType, strClassType);
            }
            model.listType = strRoleType;
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
            if (strClassType == null)
            {
                model.strClassType = "";
            }
            else
            {
                model.strClassType = strClassType;
            }
            return View(model);
        }

        public ActionResult CreateOnLineClass()
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            ClassRoomModel model= ClassRoomBll.GettClassRoomByOnLine(userInfo.fUserName);
            return View(model);
        }

        public ActionResult OnLineClass()
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);

            return View(userInfo);
        }
    }
}
