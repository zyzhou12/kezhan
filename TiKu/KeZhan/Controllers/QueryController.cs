using KeZhan.Filters;
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
    public class QueryController : Controller
    {


        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="strCity"></param>
        /// <param name="strPharse"></param>
        /// <param name="strGrade"></param>
        /// <param name="strSubjet"></param>
        /// <returns></returns>
        public ActionResult QueryClassRoom(string strPharse, string strGrade, string strSubjet, string strCity = null)
        {
            if (string.IsNullOrEmpty(strCity))
            {
                strCity = "上海";
            }
            ClassRoomListModel model = ClassRoomBll.GettClassRoomList(strCity, strPharse, strGrade, strSubjet);
            return PartialView("ClassRoomControl", model);
        }

        public ActionResult QueryTeacherClassRoom(string strTeacherUser)
        {
            ClassRoomListModel model = ClassRoomBll.GettClassRoomListByTeacher(strTeacherUser, "发布");
            return PartialView("ClassRoomControl", model);
        }

        public ActionResult QueryLikeClassRoom(string strlike, string strCity = null)
        {
            if (string.IsNullOrEmpty(strCity))
            {
                strCity = "上海";
            }
            ClassRoomListModel model = ClassRoomBll.QueryClassRoomList(strlike);
            return PartialView("ClassRoomControl", model);
        }

    }
}
