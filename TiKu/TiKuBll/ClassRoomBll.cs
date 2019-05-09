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
        public static ClassRoomListModel GetClassRoomByTeacher(string strTeacher, string strStatus, string strPayType, string strType, string strClassType)
        {
            List<tClassRoomEntity> list = tClassRoomDal.GettClassRoomListByTeacher(strTeacher, strStatus, strPayType, strType, strClassType);

            List<ClassRoomModel> modelList = new List<ClassRoomModel>();
            foreach (tClassRoomEntity entity in list)
            {
                ClassRoomModel model = new ClassRoomModel();
                model.fBasePrice = entity.fBasePrice;
                model.fClassRoomCode = entity.fClassRoomCode;
                model.fClassType = entity.fClassType;
                model.fClassRoomDate = entity.fClassRoomDate;
                model.fClassRoomTitle = entity.fClassRoomTitle;
                model.fCoverImg = entity.fCoverImg;
                model.fDeadLineDate = entity.fDeadLineDate;
                model.fDesc = entity.fDesc;
                model.fGrade = entity.fGrade;
                model.fInfo = entity.fInfo;
                model.fIsRecord = entity.fIsRecord;
                model.fIsReturn = entity.fIsReturn;
                model.fReturnType = entity.fReturnType;
                model.fReturnRule = entity.fReturnRule;
                model.fKnowLedge = entity.fKnowLedge;
                model.fMaxNumber = entity.fMaxNumber;
                model.fPayType = entity.fPayType;
                model.fPharse = entity.fPharse;
                model.fPrice = entity.fPrice;
                model.fStatus = entity.fStatus;
                model.fSubject = entity.fSubject;
                model.fTecharUserName = entity.fTecharUserName;
                model.TeacherName = entity.TeacherName;
                model.TeacherHead = entity.TeacherHead;
                modelList.Add(model);


            }
            ClassRoomListModel listModel = new ClassRoomListModel();
            listModel.classRoomList = modelList;
            return listModel;
        }

        public static ClassRoomListModel GetClassRoomByCreateOpr(string strCreate, string strStatus, string strPayType, string strType, string strClassType)
        {
            List<tClassRoomEntity> list = tClassRoomDal.GettClassRoomListByCreateOpr(strCreate, strStatus, strPayType, strType, strClassType);

            List<ClassRoomModel> modelList = new List<ClassRoomModel>();
            foreach (tClassRoomEntity entity in list)
            {
                ClassRoomModel model = new ClassRoomModel();
                model.fBasePrice = entity.fBasePrice;
                model.fClassRoomCode = entity.fClassRoomCode;
                model.fClassType = entity.fClassType;
                model.fClassRoomDate = entity.fClassRoomDate;
                model.fClassRoomTitle = entity.fClassRoomTitle;
                model.fCoverImg = entity.fCoverImg;
                model.fDeadLineDate = entity.fDeadLineDate;
                model.fDesc = entity.fDesc;
                model.fGrade = entity.fGrade;
                model.fInfo = entity.fInfo;
                model.fIsRecord = entity.fIsRecord;
                model.fIsReturn = entity.fIsReturn;
                model.fReturnType = entity.fReturnType;
                model.fReturnRule = entity.fReturnRule;
                model.fKnowLedge = entity.fKnowLedge;
                model.fMaxNumber = entity.fMaxNumber;
                model.fPayType = entity.fPayType;
                model.fPharse = entity.fPharse;
                model.fPrice = entity.fPrice;
                model.fStatus = entity.fStatus;
                model.fSubject = entity.fSubject;
                model.fTecharUserName = entity.fTecharUserName;

                model.TeacherName = entity.TeacherName;
                model.TeacherHead = entity.TeacherHead;
                modelList.Add(model);


            }
            ClassRoomListModel listModel = new ClassRoomListModel();
            listModel.classRoomList = modelList;
            return listModel;
        }

        public static ClassRoomModel GettClassRoomByOnLine(string strUserName)
        {
            ClassRoomModel model = new ClassRoomModel();
            tClassRoomEntity entity = tClassRoomDal.GettClassRoomByOnLine(strUserName);
            if (entity != null)
            {
                model.fID = entity.fID;
                model.fBasePrice = entity.fBasePrice;
                model.fClassRoomCode = entity.fClassRoomCode;
                model.fClassType = entity.fClassType;
                model.fClassRoomDate = entity.fClassRoomDate;
                model.fClassRoomTitle = entity.fClassRoomTitle;
                model.fCoverImg = entity.fCoverImg;
                model.fDeadLineDate = entity.fDeadLineDate;
                model.fDesc = entity.fDesc;
                model.fGrade = entity.fGrade;
                model.fInfo = entity.fInfo;
                model.fIsRecord = entity.fIsRecord;
                model.fIsReturn = entity.fIsReturn;
                model.fReturnType = entity.fReturnType;
                model.fReturnRule = entity.fReturnRule;
                model.fKnowLedge = entity.fKnowLedge;
                model.fMaxNumber = entity.fMaxNumber;
                model.fPayType = entity.fPayType;
                model.fPharse = entity.fPharse;
                model.fPrice = entity.fPrice;
                model.fStatus = entity.fStatus;
                model.fSubject = entity.fSubject;
                model.fTecharUserName = entity.fTecharUserName;
                model.fCreateOpr = entity.fCreateOpr;

                List<tCourseEntity> courseList = tCourseDal.GetCourseListByClassRoomCode(entity.fClassRoomCode);
                List<CourseModel> courseModelList = new List<CourseModel>();
                foreach (tCourseEntity ce in courseList)
                {
                    CourseModel cm = new CourseModel();
                    cm.fAuthor = ce.fAuthor;
                    cm.fClassDate = ce.fClassDate;
                    cm.fClassDateLength = ce.fClassDateLength;
                    cm.fPrice = ce.fPrice;
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
            }
            return model;
        }
        public static ClassRoomModel GetClassRoomByCode(string strClassRoomCode, string strUserName)
        {
            ClassRoomModel model = new ClassRoomModel();
            tClassRoomEntity entity = tClassRoomDal.GettClassRoomByCode(strClassRoomCode, strUserName);
            if (entity != null)
            {
                model.fID = entity.fID;
                model.fBasePrice = entity.fBasePrice;
                model.fClassRoomCode = entity.fClassRoomCode;
                model.fClassType = entity.fClassType;
                model.fClassRoomDate = entity.fClassRoomDate;
                model.fClassRoomTitle = entity.fClassRoomTitle;
                model.fCoverImg = entity.fCoverImg;
                model.fDeadLineDate = entity.fDeadLineDate;
                model.fDesc = entity.fDesc;
                model.fGrade = entity.fGrade;
                model.fInfo = entity.fInfo;
                model.fIsRecord = entity.fIsRecord;
                model.fIsReturn = entity.fIsReturn;
                model.fReturnType = entity.fReturnType;
                model.fReturnRule = entity.fReturnRule;
                model.fKnowLedge = entity.fKnowLedge;
                model.fMaxNumber = entity.fMaxNumber;
                model.fPayType = entity.fPayType;
                model.fPharse = entity.fPharse;
                model.fPrice = entity.fPrice;
                model.fStatus = entity.fStatus;
                model.fSubject = entity.fSubject;
                model.fTecharUserName = entity.fTecharUserName;
                model.fCreateOpr = entity.fCreateOpr;

                List<tCourseEntity> courseList = tCourseDal.GetCourseListByClassRoomCode(strClassRoomCode);
                List<CourseModel> courseModelList = new List<CourseModel>();
                foreach (tCourseEntity ce in courseList)
                {
                    CourseModel cm = new CourseModel();
                    cm.fAuthor = ce.fAuthor;
                    cm.fClassDate = ce.fClassDate;
                    cm.fClassRoomCode = ce.fClassRoomCode;
                    cm.fPrice = ce.fPrice;
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
            }
            return model;
        }

        public static ClassRoomModel GetClassRoomDetail(string strClassRoomCode, string strUserName)
        {
            DataSet dsRst = tClassRoomDal.GettClassRoomDetail(strClassRoomCode, strUserName);
            List<ClassRoomModel> modelList = PubFun.DataTableToObjects<ClassRoomModel>(dsRst.Tables[0]);
            List<DescModel> descList = PubFun.DataTableToObjects<DescModel>(dsRst.Tables[1]);
            List<CourseModel> courseList = PubFun.DataTableToObjects<CourseModel>(dsRst.Tables[2]);


            List<ClassRoomAccountModel> accountList = PubFun.DataTableToObjects<ClassRoomAccountModel>(dsRst.Tables[3]);


            List<TeacherValidDetailModel> validList = PubFun.DataTableToObjects<TeacherValidDetailModel>(dsRst.Tables[4]);

            ClassRoomModel model = new ClassRoomModel();
            if (modelList.Count > 0)
            {
                model = modelList[0];
            }
            if (accountList!=null && accountList.Count > 0)
            {
                model.account = accountList[0];
            }
            model.descList = descList;
            model.courseList = courseList;
            model.validList = validList;

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


        public static CourseModel GetCourseByID(int iCourseId)
        {
            tCourseEntity entity = tCourseDal.GettCourse(iCourseId);
            CourseModel model = new CourseModel();
            if (entity != null)
            {
                model.fAuthor = entity.fAuthor;
                model.fClassDate = entity.fClassDate;
                model.fClassDateLength = entity.fClassDateLength;
                model.fPrice = entity.fPrice;
                model.fClassRoomCode = entity.fClassRoomCode;
                model.fClassType = entity.fClassType;
                model.fClassType = entity.fClassType;
                model.fCourseTitle = entity.fCourseTitle;
                model.fDictTitle = entity.fDictTitle;
                model.fFileCoverUrl = entity.fFileCoverUrl;
                model.fFileSize = entity.fFileSize;
                model.fFileType = entity.fFileType;
                model.fID = entity.fID;
                model.fOrder = entity.fOrder;
                model.fResourceUrl = entity.fResourceUrl;
                model.fSource = entity.fSource;
                model.fStatus = entity.fStatus;
                model.fIsPay = entity.fIsPay;
                model.fUploadDate = entity.fUploadDate;
                model.fUploadOpr = entity.fUploadOpr;
            }
            return model;
        }

        public static CourseModel GetCourseById(int iCourdeId, string strUserName)
        {
            DataTable dsRst = tCourseDal.GetCourseById(iCourdeId, strUserName);
            List<CourseModel> modelList = PubFun.DataTableToObjects<CourseModel>(dsRst);


            CourseModel model = new CourseModel();
            if (modelList.Count > 0)
            {
                model = modelList[0];
            }

            return model;
        }

        public static decimal GetClassRoomFlow(int iCourseID)
        {
            decimal classRoomFlow = tClassRoomDal.GetClassRoomFlow(iCourseID);
            return classRoomFlow;
        }

        public static int ClassRoomCoursePay(int iCourseID, string strOprUser, string strSystem)
        {
            return tClassRoomDal.ClassRoomCoursePay(iCourseID, strOprUser,strSystem);
        }
        public static int OnLineCoursePay(int iCourseID, string strOprUser)
        {
            return tClassRoomDal.OnLineCoursePay(iCourseID, strOprUser);
        }

        public static int OnLineCourseOwePay(int iOweID, string strOprUser)
        {
            return tClassRoomDal.OnLineCourseOwePay(iOweID, strOprUser);
        }

        public static int ClassRoomSettlement(string strClassRoomCode, string strOprUser, ref string strMsg)
        {
            return tClassRoomDal.ClassRoomSettlement(strClassRoomCode, strOprUser,ref strMsg);
        }

        public static int SaveClassRoomInfo(string strClassRoomCode, string InfoType, string strValue,string strUserName)
        {
            tClassRoomEntity classRoom = tClassRoomDal.GettClassRoomByCode(strClassRoomCode, "");
            if (InfoType == "fQrCode")
            {
                classRoom.fQrCode = strValue;
            }
            else if (InfoType == "fTecharUserName")
            {
                classRoom.fTecharUserName = strValue;
            }
            classRoom.fModifyDate = DateTime.Now;
            classRoom.fModifyOpr = strUserName;

            List<tClassRoomEntity> list = new List<tClassRoomEntity>();
            list.Add(classRoom);
            int i = tClassRoomDal.Modify(list, "update", "fID,fModifyDate,fModifyOpr," + InfoType, null);
            return i;
        }

        public static int DoSaveClassRoom(ClassRoomModel model, string strUserName,ref string ClassRoomCode)
        {
            int i = 0;
            tClassRoomEntity entity = null;
            if (model.fID > 0)
            {
                
                entity = tClassRoomDal.GettClassRoomByID(model.fID);

                ClassRoomCode = entity.fClassRoomCode;
                entity.fClassType = model.fClassType;
                entity.fBasePrice = model.fBasePrice;
                entity.fClassRoomDate = model.fClassRoomDate;
                entity.fClassRoomTitle = model.fClassRoomTitle;
                entity.fCoverImg = model.fCoverImg;
                entity.fDeadLineDate = model.fDeadLineDate;
                entity.fDesc = model.fDesc;
                entity.fGrade = model.fGrade;
                entity.fInfo = model.fInfo;
                entity.fIsRecord = model.fIsRecord;
                entity.fIsReturn = model.fIsReturn;

                entity.fReturnType = model.fReturnType;
                entity.fReturnRule = model.fReturnRule;
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
                entity.fClassType = model.fClassType;
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
                entity.fReturnType = model.fReturnType;
                entity.fReturnRule = model.fReturnRule;
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

        public static int DoDelClassRoomCourse(int iCourseID)
        {

            int i = tCourseDal.Delete(iCourseID.ToString());


            return i;
        }

        public static int DoSaveClassRoomCourse(CourseModel model)
        {
            List<tCourseEntity> courseList = new List<tCourseEntity>();

            tCourseEntity cm = new tCourseEntity();
            cm.fAuthor = model.fAuthor;
            cm.fClassDate = model.fClassDate;
            cm.fClassDateLength = model.fClassDateLength;
            cm.fPrice = model.fPrice;
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

        public static int DoCourseDocumentUpload(CourseModel model)
        {
            List<tCourseEntity> courseList = new List<tCourseEntity>();

            tCourseEntity cm = new tCourseEntity();
            
            cm.fID = model.fID;
            cm.fDocoumentUrl = model.fDocoumentUrl;
            courseList.Add(cm);
            int i = tCourseDal.Modify(courseList, "update", "fID,fDocoumentUrl", null);


            return i;
        }

        public static int DoCourseDocumentDel(int iCourseId)
        {
            List<tCourseEntity> courseList = new List<tCourseEntity>();

            tCourseEntity cm = new tCourseEntity();

            cm.fID = iCourseId;
            cm.fDocoumentUrl = "";
            courseList.Add(cm);
            int i = tCourseDal.Modify(courseList, "update", "fID,fDocoumentUrl", null);
            
            return i;
        }

        public static int DoCourseResourceDel(int iCourseId)
        {
            List<tCourseEntity> courseList = new List<tCourseEntity>();

            tCourseEntity cm = new tCourseEntity();

            cm.fID = iCourseId;
            cm.fResourceUrl = "";
            cm.fFileCoverUrl = "";
            courseList.Add(cm);
            int i = tCourseDal.Modify(courseList, "update", "fID,fResourceUrl,fFileCoverUrl", null);

            return i;
        }

        public static ClassRoomListModel GetMyClassRoom(string strUserName, string strType)
        {
            List<tClassRoomEntity> list = tClassRoomDal.GetMyClassRoomList(strUserName,strType);

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
                model.fReturnType = entity.fReturnType;
                model.fReturnRule = entity.fReturnRule;
                model.fKnowLedge = entity.fKnowLedge;
                model.fMaxNumber = entity.fMaxNumber;
                model.fPayType = entity.fPayType;
                model.fPharse = entity.fPharse;
                model.fPrice = entity.fPrice;
                model.fStatus = entity.fStatus;
                model.fSubject = entity.fSubject;
                model.fTecharUserName = entity.fTecharUserName;
                model.TeacherName = entity.TeacherName;
                model.TeacherHead = entity.TeacherHead;
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
                model.fReturnType = entity.fReturnType;
                model.fReturnRule = entity.fReturnRule;
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

        public static ClassRoomListModel GettClassRoomListByTeacher(string strTeacherUser, string strStatus)
        {
            List<tClassRoomEntity> list = tClassRoomDal.GettClassRoomListByTeacher(strTeacherUser, strStatus);

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
                model.fReturnType = entity.fReturnType;
                model.fReturnRule = entity.fReturnRule;
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

        /// <summary>
        /// 课程发布/下架（修改状态）
        /// </summary>
        /// <returns></returns>
        public static int ClassRoomSubmitSend(string strClassRoomCode,string strStatus,string strNote,string strUserName)
        {
            tClassRoomApplyEntity entity = new tClassRoomApplyEntity();
            entity.fClassRoomCode = strClassRoomCode;
            entity.fStatus = strStatus;
            entity.fNote = strNote;
            entity.fSubmitDate = DateTime.Now;
            entity.fSubmitOpr = strUserName;
            List<tClassRoomApplyEntity> list=new List<tClassRoomApplyEntity>();
            list.Add(entity);
            int i = tClassRoomApplyDal.Modify(list, "insert", null, null);

            tClassRoomEntity classRoom = tClassRoomDal.GettClassRoomByCode(strClassRoomCode,null);
            classRoom.fStatus = strStatus + "中";
            List<tClassRoomEntity> classRoomList=new List<tClassRoomEntity>();
            classRoomList.Add(classRoom);
            i = tClassRoomDal.Modify(classRoomList, "update", "fID,fStatus", null);


            return i;
        }

      
    }
}
