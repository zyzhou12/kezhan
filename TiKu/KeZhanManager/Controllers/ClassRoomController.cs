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
    public class ClassRoomController : Controller
    {
        //
        // GET: /ClassRoom/

        public ActionResult ClassRoomQuery()
        {
            return View();
        }

        public ActionResult GetClassRoomList(string strBeginDate, string strEndDate, string strClassRoomTitle)
        {
            List<tClassRoomEntity> classRoomList = tClassRoomDal.ClassRoomListQuery(strBeginDate,strEndDate,strClassRoomTitle);
            ClassRoomListModel model = new ClassRoomListModel();
            if (classRoomList != null)
            {
                model.iCount = classRoomList.Count;
                model.classRoomList = classRoomList;
            }
            return PartialView("ClassRoomList", model);
        }

        public ActionResult ClassRoomSendQuery()
        {
            return View();
        }

        public ActionResult GetClassRoomApplyList(string strBeginDate, string strEndDate, string strClassRoomTitle,string strStatus)
        {

            List<tClassRoomApplyEntity> classRoomApplyList = tClassRoomApplyDal.GetClassRoomApplyList(strBeginDate, strEndDate, strClassRoomTitle, strStatus);
            ClassRoomApplyListModel model = new ClassRoomApplyListModel();
            if (classRoomApplyList != null)
            {
                model.iCount = classRoomApplyList.Count;
                model.classRoomApplyList = classRoomApplyList;
            }
            return PartialView("ClassRoomApplyList", model);
        }

        public ActionResult ClassRoomOffLineQuery()
        {
            return View();
        }

        public ActionResult ClassRoomGroupQuery()
        {
            return View();
        }
        public ActionResult GetClassRoomGroupList(string strBeginDate, string strEndDate, string strUserName)
        {

            List<tGroupEntity> groupList = tGroupDal.GettGroupList(strBeginDate, strEndDate, strUserName);
            GroupListModel model = new GroupListModel();
            if (groupList != null)
            {
                model.iCount = groupList.Count;
                model.groupList = groupList;
            }
            return PartialView("GroupList", model);
        }
        
    }
}
