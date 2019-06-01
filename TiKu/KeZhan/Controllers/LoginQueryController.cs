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
    [LoginActionFilter]
    public class LoginQueryController : Controller
    {
        //
        // GET: /Query/

        public ActionResult DeleteCourse(int iCourseID, string strClassRoomCode, bool isEdit)
        {
            ClassRoomBll.DoDelClassRoomCourse(iCourseID);

            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            ClassRoomModel model = ClassRoomBll.GetClassRoomDetail(strClassRoomCode, userInfo.fUserName); ;

            CourseListModel courseModel = new CourseListModel();
            courseModel.courseList = model.courseList;
            courseModel.isEdit = isEdit;
            courseModel.TeacherUserName = model.fTecharUserName;
            if (model.fType == "Live")
            {
                return PartialView("CourseListControl", courseModel);
            }
            else
            {
                return PartialView("RecordedCourseListControl", courseModel);
            }
        }
        /// <summary>
        /// 查询介绍信息
        /// </summary>
        /// <param name="strClassRoomCode"></param>
        /// <returns></returns>
        public ActionResult QueryDescList(string strClassRoomCode, bool isEdit)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            ClassRoomModel model = ClassRoomBll.GetClassRoomDetail(strClassRoomCode, userInfo.fUserName); ;

            DescListModel descModel = new DescListModel();
            descModel.descList = model.descList;
            descModel.isEdit = isEdit;
            descModel.TeacherUserName = model.fTecharUserName;

            return PartialView("ClassRoomDescControl", descModel);
        }
        /// <summary>
        /// 查询章节信息
        /// </summary>
        /// <param name="strClassRoomCode"></param>
        /// <returns></returns>
        public ActionResult QueryCourseList(string strClassRoomCode, bool isEdit)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            ClassRoomModel model = ClassRoomBll.GetClassRoomDetail(strClassRoomCode, userInfo.fUserName); ;

            CourseListModel courseModel = new CourseListModel();
            courseModel.courseList = model.courseList;
            courseModel.isEdit = isEdit;
            courseModel.TeacherUserName = model.fTecharUserName;
            if (model.fType == "Live")
            {
                return PartialView("CourseListControl", courseModel);
            }
            else
            {
                return PartialView("RecordedCourseListControl", courseModel);
            }
        }




        public ActionResult QueryBookingList(string strClassRoomCode, string strMobile = null, string strStatus = null, string beginDate = null, string endDate = null)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            BookingListModel model = BookingBll.GetBookingList(userInfo.fUserName, strClassRoomCode, strMobile, strStatus, beginDate, endDate);
            ClassRoomModel classRoom = ClassRoomBll.GetClassRoomByCode(strClassRoomCode, "");
            model.fClassRoomCode = strClassRoomCode;
            model.fClassType = classRoom.fClassType;
            model.fPayType = classRoom.fPayType;
            model.fStatus = classRoom.fStatus;
            model.IsManager = classRoom.fCreateOpr == userInfo.fUserName;
            // model.list = model.list.Where(m => m.fIsPay == true).ToList();
            return PartialView("BookingListControl", model);
        }

        public ActionResult TeacherValid(int iValidFid, int iDetailID)
        {
            ValidModel model = UserBll.GetTeacherValid(iValidFid, iDetailID);
            return PartialView("TeacherValid", model);
        }



        public ActionResult QueryMemberList()
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            UserMemberListModel model = UserBll.GetMemberList(userInfo.fUserName, "");

            return PartialView("UserMemberListControl", model);
        }


        public ActionResult QueryUserAccount(string strTradingType, string strSystem, string strType, DateTime beginDate, DateTime endDate)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            UserAccountListModel model = UserBll.GetUserAccountData(userInfo.fUserName, strTradingType, strSystem, strType, beginDate, endDate);

            return PartialView("UserAccountListControl", model);
        }

        public ActionResult QueryFileList(string strType)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            ResourseListModel model = ResourceBll.GetResourseList(userInfo.fUserName, strType);

            return PartialView("FileListControl", model);
        }

        public ActionResult QueryDelFileList()
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            ResourseListModel model = ResourceBll.GetDelResourseList(userInfo.fUserName);

            return PartialView("FileListControl", model);
        }

        public ActionResult QueryUploadFileList(string strType)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            ResourseListModel model = ResourceBll.GetResourseList(userInfo.fUserName, strType);

            return PartialView("FileUploadListControl", model);
        }
    }
}
