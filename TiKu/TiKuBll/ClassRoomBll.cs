using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiKuBll.Model;
using TiKu.Entity;
using TiKu.Dal;
using System.Data;
using Trip8H.Common;

namespace TiKuBll
{
    public class ClassRoomBll
    {
        public static ClassRoomListModel GetClassRoomByTeacher(string strTeacher, string strStatus)
        {
            List<tClassRoomEntity> list = tClassRoomDal.GettClassRoomListByTeacher(strTeacher, strStatus);

            List<ClassRoomModel> modelList = new List<ClassRoomModel>();
            foreach (tClassRoomEntity entity in list)
            {
                ClassRoomModel model = new ClassRoomModel();
                model.fBasePrice = entity.fBasePrice;
                model.fClassRoomCode = entity.fClassRoomCode;
                model.fClassRoomDate = entity.fClassRoomDate;
                model.fClassRoomTitle = entity.fClassRoomTitle;
                model.fCoverImg = entity.fCoverImg;
                model.fDeadLineDate = entity.fDeadLineDate;
                model.fDesc = entity.fDesc;
                model.fGrade = entity.fGrade;
                model.fInfo = entity.fInfo;
                model.fIsRecord = entity.fIsRecord;
                model.fIsReturn = entity.fIsReturn;
                model.fKnowLedge = entity.fKnowLedge;
                model.fMaxNumber = entity.fMaxNumber;
                model.fPayType = entity.fPayType;
                model.fPharse = entity.fPharse;
                model.fPrice = entity.fPrice;
                model.fStatus = entity.fStatus;
                model.fSubject = entity.fSubject;
                model.fTecharUserName = entity.fTecharUserName;
                modelList.Add(model);


            }
            ClassRoomListModel listModel = new ClassRoomListModel();
            listModel.classRoomList = modelList;
            return listModel;
        }


        public static ClassRoomModel GetClassRoomByCode(string strClassRoomCode, string strUserName)
        {
            tClassRoomEntity entity = tClassRoomDal.GettClassRoomByCode(strClassRoomCode, strUserName);
            ClassRoomModel model = new ClassRoomModel();
            model.fID = entity.fID;
            model.fBasePrice = entity.fBasePrice;
            model.fClassRoomCode = entity.fClassRoomCode;
            model.fClassRoomDate = entity.fClassRoomDate;
            model.fClassRoomTitle = entity.fClassRoomTitle;
            model.fCoverImg = entity.fCoverImg;
            model.fDeadLineDate = entity.fDeadLineDate;
            model.fDesc = entity.fDesc;
            model.fGrade = entity.fGrade;
            model.fInfo = entity.fInfo;
            model.fIsRecord = entity.fIsRecord;
            model.fIsReturn = entity.fIsReturn;
            model.fKnowLedge = entity.fKnowLedge;
            model.fMaxNumber = entity.fMaxNumber;
            model.fPayType = entity.fPayType;
            model.fPharse = entity.fPharse;
            model.fPrice = entity.fPrice;
            model.fStatus = entity.fStatus;
            model.fSubject = entity.fSubject;
            model.fTecharUserName = entity.fTecharUserName;

            List<tCourseEntity> courseList = tCourseDal.GetCourseListByClassRoomCode(strClassRoomCode);
            List<CourseModel> courseModelList = new List<CourseModel>();
            foreach (tCourseEntity ce in courseList)
            {
                CourseModel cm = new CourseModel();
                cm.fAuthor = ce.fAuthor;
                cm.fClassDate = ce.fClassDate;
                cm.fClassRoomCode = ce.fClassRoomCode;
                cm.fClassType = ce.fClassType;
                cm.fCourseTitle = ce.fCourseTitle;
                cm.fDictTitle = ce.fDictTitle;
                cm.fFileSize = ce.fFileSize;
                cm.fFileType = ce.fFileType;
                cm.fID = ce.fID;
                cm.fOrder = ce.fOrder;
                cm.fResourceUrl = ce.fResourceUrl;
                cm.fSource = ce.fSource;
                cm.fUploadDate = ce.fUploadDate;
                cm.fUploadOpr = ce.fUploadOpr;
                courseModelList.Add(cm);
            }
            model.courseList = courseModelList;

            return model;
        }

        public static ClassRoomModel GetClassRoomDetail(string strClassRoomCode, string strUserName)
        {
            DataSet dsRst = tClassRoomDal.GettClassRoomDetail(strClassRoomCode, strUserName);
            List<ClassRoomModel> modelList = PubFun.DataTableToObjects<ClassRoomModel>(dsRst.Tables[0]);
            List<DescModel> descList = PubFun.DataTableToObjects<DescModel>(dsRst.Tables[1]);
            List<CourseModel> courseList = PubFun.DataTableToObjects<CourseModel>(dsRst.Tables[2]);


            ClassRoomModel model = new ClassRoomModel();
            if (modelList.Count > 0)
            {
                model = modelList[0];
            }
            model.descList = descList;
            model.courseList = courseList;

            return model;
        }

        public static ClassRoomModel GetClassRoomByCourseId(int iCourdeId,string strUserName)
        {
            DataTable dsRst = tClassRoomDal.GetClassRoomByCourseId(iCourdeId,strUserName);
            List<ClassRoomModel> modelList = PubFun.DataTableToObjects<ClassRoomModel>(dsRst);


            ClassRoomModel model = new ClassRoomModel();
            if (modelList.Count > 0)
            {
                model = modelList[0];
            }

            return model;
        }

        public static decimal GetClassRoomPrice(int iCourseID)
        {
            decimal classRoomPrice = tClassRoomDal.GetClassRoomPrice(iCourseID);
            return classRoomPrice;
        }


        public static int DoSaveClassRoom(ClassRoomModel model, string strUserName)
        {
            int i = 0;
            tClassRoomEntity entity = null;
            if (model.fID > 0)
            {
                entity = tClassRoomDal.GettClassRoomByID(model.fID);
                entity.fBasePrice = model.fBasePrice;
                entity.fClassRoomCode = model.fClassRoomCode;
                entity.fClassRoomDate = model.fClassRoomDate;
                entity.fClassRoomTitle = model.fClassRoomTitle;
                entity.fCoverImg = model.fCoverImg;
                entity.fDeadLineDate = model.fDeadLineDate;
                entity.fDesc = model.fDesc;
                entity.fGrade = model.fGrade;
                entity.fInfo = model.fInfo;
                entity.fIsRecord = model.fIsRecord;
                entity.fIsReturn = model.fIsReturn;
                entity.fKnowLedge = model.fKnowLedge;
                entity.fMaxNumber = model.fMaxNumber;
                entity.fPayType = model.fPayType;
                entity.fTeacherValidID = model.fTeacherValidID;
                entity.fPharse = model.fPharse;
                entity.fPrice = model.fPrice;
                entity.fStatus = model.fStatus;
                entity.fSubject = model.fSubject;
                entity.fQrCode = model.fQrCode;
                entity.fTecharUserName = model.fTecharUserName;
                entity.fModifyDate = DateTime.Now;
                entity.fModifyOpr = strUserName;
                List<tClassRoomEntity> list = new List<tClassRoomEntity>();
                list.Add(entity);
                i = tClassRoomDal.Modify(list, "update", null, null);
            }
            else
            {
                string ClassRoomCode = "";
                Random rd = new Random();
                if (model.fPayType == "在线支付")
                {
                    ClassRoomCode = "1" + rd.Next(100000, 999999).ToString();
                }
                else
                {
                    ClassRoomCode = "2" + rd.Next(100000, 999999).ToString();
                }

                entity = new tClassRoomEntity();
                entity.fBasePrice = model.fBasePrice;
                entity.fClassRoomCode = ClassRoomCode;
                entity.fClassRoomDate = model.fClassRoomDate;
                entity.fClassRoomTitle = model.fClassRoomTitle;
                entity.fCoverImg = model.fCoverImg;
                entity.fDeadLineDate = model.fDeadLineDate;
                entity.fDesc = model.fDesc;
                entity.fGrade = model.fGrade;
                entity.fInfo = model.fInfo;
                entity.fIsRecord = model.fIsRecord;
                entity.fIsReturn = model.fIsReturn;
                entity.fKnowLedge = model.fKnowLedge;
                entity.fMaxNumber = model.fMaxNumber;
                entity.fPayType = model.fPayType;
                entity.fPharse = model.fPharse;
                entity.fPrice = model.fPrice;
                entity.fStatus = model.fStatus;
                entity.fSubject = model.fSubject;
                entity.fQrCode = model.fQrCode;
                entity.fTecharUserName = model.fTecharUserName;
                entity.fCreateDate = DateTime.Now;
                entity.fCreateOpr = strUserName;
                List<tClassRoomEntity> list = new List<tClassRoomEntity>();
                list.Add(entity);
                i = tClassRoomDal.Modify(list, "insert", null, null);
            }


            return i;
        }

        public static int DoSaveClassRoomDesc(DescModel model)
        {
            List<tClassRoomDetailEntity> descList = new List<tClassRoomDetailEntity>();

            tClassRoomDetailEntity cm = new tClassRoomDetailEntity();
            cm.fClassRoomCode = model.fClassRoomCode;
            cm.fContent = model.fContent;
            cm.fOrder = model.fOrder;
            cm.fType = model.fType;
            int i = 0;
            if (model.fID > 0)
            {
                cm.fID = model.fID;
                cm.fModifyDate = DateTime.Now;
                cm.fModifyOpr = "";
                descList.Add(cm);

                i = tClassRoomDetailDal.Modify(descList, "update", null, null);
            }
            else
            {
                cm.fCreateDate = DateTime.Now;
                cm.fCreateOpr = "";
                descList.Add(cm);

                i = tClassRoomDetailDal.Modify(descList, "insert", null, null);
            }

            return i;
        }

        public static int DoDelClassRoomDesc(int iDescID)
        {

            int i = tClassRoomDetailDal.Delete(iDescID.ToString());


            return i;
        }


        public static int DoSaveClassRoomCourse(CourseModel model)
        {
            List<tCourseEntity> courseList = new List<tCourseEntity>();

            tCourseEntity cm = new tCourseEntity();
            cm.fAuthor = model.fAuthor;
            cm.fClassDate = model.fClassDate;
            cm.fClassDateLength = model.fClassDateLength;
            cm.fClassRoomCode = model.fClassRoomCode;
            cm.fClassType = model.fClassType;
            cm.fCourseTitle = model.fCourseTitle;
            cm.fDictTitle = model.fDictTitle;
            cm.fOrder = model.fOrder;
            int i = 0;
            if (model.fID > 0)
            {
                cm.fID = model.fID;
                courseList.Add(cm);

                i = tCourseDal.Modify(courseList, "update", null, null);
            }
            else
            {
                courseList.Add(cm);

                i = tCourseDal.Modify(courseList, "insert", null, null);
            }

            return i;
        }

        public static int DoClassRoomCourseUpload(CourseModel model)
        {
            List<tCourseEntity> courseList = new List<tCourseEntity>();

            tCourseEntity cm = new tCourseEntity();
            cm.fAuthor = model.fAuthor;
            cm.fID = model.fID;
            cm.fUploadDate = model.fUploadDate;
            cm.fUploadOpr = model.fUploadOpr;
            cm.fResourceUrl = model.fResourceUrl;
            cm.fSource = model.fSource;
            cm.fStatus = model.fStatus;
            courseList.Add(cm);
            int i = tCourseDal.Modify(courseList, "update", "fID,fUploadDate,fUploadOpr,fAuthor,fResourceUrl,fSource,fStatus", null);


            return i;
        }
        public static ClassRoomListModel GetMyClassRoom(string strUserName)
        {
            List<tClassRoomEntity> list = tClassRoomDal.GetMyClassRoomList(strUserName);

            List<ClassRoomModel> modelList = new List<ClassRoomModel>();
            foreach (tClassRoomEntity entity in list)
            {
                ClassRoomModel model = new ClassRoomModel();
                model.fBasePrice = entity.fBasePrice;
                model.fClassRoomCode = entity.fClassRoomCode;
                model.fClassRoomDate = entity.fClassRoomDate;
                model.fClassRoomTitle = entity.fClassRoomTitle;
                model.fCoverImg = entity.fCoverImg;
                model.fDeadLineDate = entity.fDeadLineDate;
                model.fDesc = entity.fDesc;
                model.fGrade = entity.fGrade;
                model.fInfo = entity.fInfo;
                model.fIsRecord = entity.fIsRecord;
                model.fIsReturn = entity.fIsReturn;
                model.fKnowLedge = entity.fKnowLedge;
                model.fMaxNumber = entity.fMaxNumber;
                model.fPayType = entity.fPayType;
                model.fPharse = entity.fPharse;
                model.fPrice = entity.fPrice;
                model.fStatus = entity.fStatus;
                model.fSubject = entity.fSubject;
                model.fTecharUserName = entity.fTecharUserName;
                modelList.Add(model);
            }
            ClassRoomListModel listModel = new ClassRoomListModel();
            listModel.classRoomList = modelList;
            return listModel;
        }


        public static ClassRoomListModel GetClassRoomByCity(string strCity, string strStatus)
        {
            List<tClassRoomEntity> list = tClassRoomDal.GettClassRoomListByCity(strCity, strStatus);

            List<ClassRoomModel> modelList = new List<ClassRoomModel>();
            foreach (tClassRoomEntity entity in list)
            {
                ClassRoomModel model = new ClassRoomModel();
                model.fBasePrice = entity.fBasePrice;
                model.fClassRoomCode = entity.fClassRoomCode;
                model.fClassRoomDate = entity.fClassRoomDate;
                model.fClassRoomTitle = entity.fClassRoomTitle;
                model.fCoverImg = entity.fCoverImg;
                model.fDeadLineDate = entity.fDeadLineDate;
                model.fDesc = entity.fDesc;
                model.fGrade = entity.fGrade;
                model.fInfo = entity.fInfo;
                model.fIsRecord = entity.fIsRecord;
                model.fIsReturn = entity.fIsReturn;
                model.fKnowLedge = entity.fKnowLedge;
                model.fMaxNumber = entity.fMaxNumber;
                model.fPayType = entity.fPayType;
                model.fPharse = entity.fPharse;
                model.fPrice = entity.fPrice;
                model.fStatus = entity.fStatus;
                model.fSubject = entity.fSubject;
                model.fTecharUserName = entity.fTecharUserName;
                modelList.Add(model);
            }
            ClassRoomListModel listModel = new ClassRoomListModel();
            listModel.classRoomList = modelList;
            return listModel;
        }

        public static ClassRoomListModel GettClassRoomList(string strCity, string strPharse, string strGrade, string strSubject)
        {
            DataTable dt = tClassRoomDal.GettClassRoomList(strCity, strPharse, strGrade, strSubject);

            List<ClassRoomModel> modelList = PubFun.DataTableToObjects<ClassRoomModel>(dt);


            ClassRoomListModel listModel = new ClassRoomListModel();
            listModel.classRoomList = modelList;
            return listModel;
        }

        public static int DoBookingClassRoom(BookingModel model)
        {
            int i = 0;
            tBookingEntity booking = new tBookingEntity();
            booking.fAmount = model.fAmount;
            booking.fBookingNo = model.fBookingNo;
            booking.fBuyDate = model.fBuyDate;
            booking.fBuySystem = model.fBuySystem;
            booking.fCreateDate = model.fCreateDate;
            booking.fCreateOpr = model.fCreateOpr;
            booking.fID = model.fID;
            booking.fIsPay = model.fIsPay;
            booking.fIsReturn = model.fIsReturn;
            booking.fModifyDate = model.fModifyDate;
            booking.fModifyOpr = model.fModifyOpr;
            booking.fStatus = model.fStatus;
            booking.fType = model.fType;
            booking.fTypeCode = model.fTypeCode;
            booking.fUserName = model.fUserName;

            List<tBookingEntity> list = new List<tBookingEntity>();
            list.Add(booking);
            i = tBookingDal.Modify(list, "insert", null, null);
            return i;
        }

        public static MediaListModel GetCourseMediaList(int iCourseID)
        {
            List<tMediaEntity> list = tClassRoomDal.GetCourseMediaList(iCourseID);

            List<MediaModel> modelList = new List<MediaModel>();
            foreach (tMediaEntity entity in list)
            {
                MediaModel model = new MediaModel();
                model.fActivityName = entity.fActivityName;
                model.fCourseId = entity.fCourseId;
                model.fHeight = entity.fHeight;
                model.fID = entity.fID;
                model.fMediaID = entity.fMediaID;
                model.fSize = entity.fSize;
                model.fUrl = entity.fUrl;
                model.fWidth = entity.fWidth;

                modelList.Add(model);
            }
            MediaListModel listModel = new MediaListModel();
            listModel.MediaList = modelList;
            return listModel;
        }
    }
}
