using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiKu.Entity;
using TiKu.Dal;
using TiKuBll.Model;
using System.Data;
using Trip8H.Common;

namespace TiKuBll
{
    public class BookingBll
    {
        public static string SubmitBooking(string strUserName, string strType, string strCode, decimal dAmount, bool strIsReturn, string strSystem)
        {
            tBookingEntity booking = new tBookingEntity();
            booking.fUserName = strUserName;
            Random rd = new Random();
            if (strType == "ClassRoom")
            {
                booking.fBookingNo = "C" + DateTime.Now.ToString("yyyyMMddHHmmssssss") + rd.Next(10, 100).ToString();
            }
            else if (strType == "OnLineClass")
            {
                booking.fBookingNo = "O" + DateTime.Now.ToString("yyyyMMddHHmmssssss") + rd.Next(10, 100).ToString(); ;
            }
            else if (strType == "Resource")
            {
                booking.fBookingNo = "R" + DateTime.Now.ToString("yyyyMMddHHmmssssss") + rd.Next(10, 100).ToString(); ;
            }
            booking.fType = strType;
            booking.fTypeCode = strCode;
            booking.fAmount = dAmount;
            booking.fBuyDate = DateTime.Now;
            booking.fBuySystem = strSystem;
            booking.fIsPay = false;
            booking.fIsReturn = strIsReturn;
            booking.fStatus = "提交";

            booking.fCreateDate = DateTime.Now;
            booking.fCreateOpr = strUserName;

            List<tBookingEntity> list = new List<tBookingEntity>();
            list.Add(booking);
            int i = tBookingDal.Modify(list, "insert", null, null);
            if (i > 0)
            {
                return booking.fBookingNo;
            }
            else
            {
                return "";

            }
        }

        public static int GetCourseIsBuy(int iCourseID, string strUserName)
        {
            return tBookingDal.GetCourseIsBuy(iCourseID, strUserName);
        }
        public static BookingModel GettBooking(string strUserName, string strType, string strCode, string strStatus)
        {
            BookingModel model = null;
            tBookingEntity booking = tBookingDal.GettBooking(strUserName, strType, strCode, strStatus);
            if (booking != null)
            {
                model = new BookingModel();
                model.fAmount = booking.fAmount;
                model.fBookingNo = booking.fBookingNo;
                model.fBuyDate = booking.fBuyDate;
                model.fBuySystem = booking.fBuySystem;
                model.fCreateDate = booking.fCreateDate;
                model.fCreateOpr = booking.fCreateOpr;
                model.fID = booking.fID;
                model.fIsPay = booking.fIsPay;
                model.fIsReturn = booking.fIsReturn;
                model.fModifyDate = booking.fModifyDate;
                model.fModifyOpr = booking.fModifyOpr;
                model.fStatus = booking.fStatus;
                model.fType = booking.fType;
                model.fTypeCode = booking.fTypeCode;
                model.fUserName = booking.fUserName;

                //课程信息
                DataSet dsRst = tClassRoomDal.GettClassRoomDetail(booking.fTypeCode, booking.fUserName);
                List<ClassRoomModel> modelList = PubFun.DataTableToObjects<ClassRoomModel>(dsRst.Tables[0]);
                // List<DescModel> descList = PubFun.DataTableToObjects<DescModel>(dsRst.Tables[1]);
                List<CourseModel> courseList = PubFun.DataTableToObjects<CourseModel>(dsRst.Tables[2]);
                ClassRoomModel classRoom = new ClassRoomModel();
                if (modelList.Count > 0)
                {
                    classRoom = modelList[0];
                }
                //classRoom.descList = descList;
                classRoom.courseList = courseList;
                model.ClassRoom = classRoom;
            }
            return model;
        }


        public static BookingListModel GetBookingList(string strUsername, string strClassRoomCode, string strMobile, string strStatus, string beginDate, string endDate)
        {
            BookingListModel model = new BookingListModel();
            DataTable dt = tBookingDal.GetClassRoomBookingList(strUsername, strClassRoomCode,strMobile,strStatus,beginDate,endDate);
            List<BookingModel> modelList = PubFun.DataTableToObjects<BookingModel>(dt);
            model.list = modelList;
            return model;
        }


        public static BookingListModel GetBookingList(string strUserName, string strStatus, DateTime Begindate, DateTime EndDate)
        {
            BookingListModel model = new BookingListModel();
            DataTable dt = tBookingDal.GetBookingList(strUserName, strStatus, Begindate, EndDate);
            List<BookingModel> modelList = PubFun.DataTableToObjects<BookingModel>(dt);
            model.list = modelList;
            return model;

            //List<tBookingEntity> list = tBookingDal.GettBookingListByUserName(strUserName);
            //BookingListModel model = new BookingListModel();
            //List<BookingModel> bookingList = new List<BookingModel>();
            //foreach (tBookingEntity entity in list)
            //{
            //  BookingModel booking = new BookingModel();
            //  booking.fAmount = entity.fAmount;
            //  booking.fBookingNo = entity.fBookingNo;
            //  booking.fBuyDate = entity.fBuyDate;
            //  booking.fBuySystem = entity.fBuySystem;
            //  booking.fCreateDate = entity.fCreateDate;
            //  booking.fCreateOpr = entity.fCreateOpr;
            //  booking.fID = entity.fID;
            //  booking.fIsPay = entity.fIsPay;
            //  booking.fIsReturn = entity.fIsReturn;
            //  booking.fModifyDate = entity.fModifyDate;
            //  booking.fModifyOpr = entity.fModifyOpr;
            //  booking.fStatus = entity.fStatus;
            //  booking.fType = entity.fType;
            //  booking.fTypeCode = entity.fTypeCode;
            //  booking.fUserName = entity.fUserName;
            //  bookingList.Add(booking);
            //}
            //model.list=bookingList;
            return model;
        }


        public static BookingModel GetBookingByNo(string strBookingNo)
        {
            BookingModel model = new BookingModel();
            tBookingEntity booking = tBookingDal.GettBooking(strBookingNo);

            model.fAmount = booking.fAmount;
            model.fBookingNo = booking.fBookingNo;
            model.fBuyDate = booking.fBuyDate;
            model.fBuySystem = booking.fBuySystem;
            model.fCreateDate = booking.fCreateDate;
            model.fCreateOpr = booking.fCreateOpr;
            model.fID = booking.fID;
            model.fIsPay = booking.fIsPay;
            model.fIsReturn = booking.fIsReturn;
            model.fModifyDate = booking.fModifyDate;
            model.fModifyOpr = booking.fModifyOpr;
            model.fStatus = booking.fStatus;
            model.fType = booking.fType;
            model.fTypeCode = booking.fTypeCode;
            model.fUserName = booking.fUserName;

            //课程信息
            string strClassRoomCode = "";
            if (booking.fType == "ClassRoom")
            {
                strClassRoomCode = booking.fTypeCode;
            }
            else if (booking.fType == "OnLineClass")
            {
                tCourseEntity course = tCourseDal.GettCourse(Convert.ToInt32(booking.fTypeCode));
                strClassRoomCode = course.fClassRoomCode;
            }
            DataSet dsRst = tClassRoomDal.GettClassRoomDetail(strClassRoomCode, booking.fUserName);
                List<ClassRoomModel> modelList = PubFun.DataTableToObjects<ClassRoomModel>(dsRst.Tables[0]);
                // List<DescModel> descList = PubFun.DataTableToObjects<DescModel>(dsRst.Tables[1]);
                List<CourseModel> courseList = PubFun.DataTableToObjects<CourseModel>(dsRst.Tables[2]);
                ClassRoomModel classRoom = new ClassRoomModel();
                if (modelList.Count > 0)
                {
                    classRoom = modelList[0];
                }
                //classRoom.descList = descList;
                classRoom.courseList = courseList;
                model.ClassRoom = classRoom;
            

            tUserPayEntity pay = tUserPayDal.GettUserPayByBookingNo(booking.fBookingNo);
            if (pay != null)
            {
                model.PayOrderNo = pay.fOrderNo;
            }

            return model;
        }

        public static int BookingPay(string strUserName, string strTradePass,string strBookingType, string strBookingNo, string strSystem, ref string strMsg)
        {
            int i = tBookingDal.BookingPay(strUserName, strTradePass, strBookingType, strBookingNo, strSystem, ref strMsg);
            return i;
        }

        public static string GetPayNoByBookingNo(string strBookingNo)
        {
            return tBookingDal.GetPayNoByBookingNo(strBookingNo);
        }

        public static int BookingUpdateStatus(string strBookingNo, string strStatus, string strRemark, string strUserName)
        {
            tBookingEntity booking = tBookingDal.GettBooking(strBookingNo);
            booking.fStatus = strStatus;
            
            booking.fModifyDate = DateTime.Now;
            booking.fModifyOpr = strUserName;

            List<tBookingEntity> list = new List<tBookingEntity>();
            list.Add(booking);
            int i = tBookingDal.Modify(list, "update", "fID,fStatus,fModifyDate,fModifyOpr", null);
            return i;
        }

        public static int CheckRefund(decimal iAmount, string strBookingNo, ref string strMsg)
        {
            return tUserRefundDal.CheckRefund(iAmount, strBookingNo, ref strMsg);
        }

        public static int SubmitBookingRefund(string strBookingNo, string strUserName,string strOpr, decimal dAmount, string strRemark)
        {
            tUserRefundEntity entity = new tUserRefundEntity();
            entity.fApplyAmount = dAmount;
            entity.fApplyDate = DateTime.Now;
            entity.fApplyRemark = strRemark;
            entity.fCreateDate = DateTime.Now;
            entity.fCreateOpr = strOpr;
            entity.fOrderNo = "T"+DateTime.Now.ToString("yyyyMMddHHmmssssss");
            entity.fBookingNo = strBookingNo;
            entity.fStatus = 0;
            entity.fUserName = strUserName;
            List<tUserRefundEntity> list = new List<tUserRefundEntity>();
            list.Add(entity);
            int i = tUserRefundDal.Modify(list, "insert", null, null);

            BookingUpdateStatus(strBookingNo, "退款中", strRemark, strUserName);
            return i;
        }

        public static UserRefundListModel GetBookingRefundList(string strUserName, string strStatus, string strTeacher)
        {
            UserRefundListModel model = new UserRefundListModel();
            List<tUserRefundEntity> list = tUserRefundDal.GettUserRefundList(strUserName, strStatus, strTeacher);
            List<UserRefundModel> refundList = new List<UserRefundModel>();
            foreach (tUserRefundEntity entity in list)
            {
                UserRefundModel refund = new UserRefundModel();
                refund.fApplyAmount = entity.fApplyAmount;
                refund.fApplyDate = entity.fApplyDate;
                refund.fApplyRemark = entity.fApplyRemark;
                refund.fCreateDate = entity.fCreateDate;
                refund.fCreateOpr = entity.fCreateOpr;
                refund.fID = entity.fID;
                refund.fModifyDate = entity.fModifyDate;
                refund.fModifyOpr = entity.fModifyOpr;
                refund.fOrderNo = entity.fOrderNo;
                refund.fBookingNo = entity.fBookingNo;
                refund.fRefundAmount = entity.fRefundAmount;
                refund.fRefundDate = entity.fRefundDate;
                refund.fRefundNote = entity.fRefundNote;
                refund.fRefundUserName = entity.fRefundUserName;
                refund.fStatus = entity.fStatus;
                refund.fUserName = entity.fUserName;
                refund.fClassRoomTitle = entity.fClassRoomTitle;
                refund.fNickName = entity.fNickName;
               if(entity.fUserName!=entity.fCreateOpr)
               {
                   refund.fApplyRemark += "(老师提交)";
               }
                refundList.Add(refund);
            }
            model.refundList = refundList;
            return model;
        }

        public static UserRefundModel GetBookingRefund(int iRefundID)
        {
            tUserRefundEntity entity = tUserRefundDal.GettUserRefund(iRefundID);
            UserRefundModel refund = new UserRefundModel();
            refund.fApplyAmount = entity.fApplyAmount;
            refund.fApplyDate = entity.fApplyDate;
            refund.fApplyRemark = entity.fApplyRemark;
            refund.fCreateDate = entity.fCreateDate;
            refund.fCreateOpr = entity.fCreateOpr;
            refund.fID = entity.fID;
            refund.fModifyDate = entity.fModifyDate;
            refund.fModifyOpr = entity.fModifyOpr;
            refund.fOrderNo = entity.fOrderNo;
            refund.fBookingNo = entity.fBookingNo;
            refund.fRefundAmount = entity.fRefundAmount;
            refund.fRefundDate = entity.fRefundDate;
            refund.fRefundNote = entity.fRefundNote;
            refund.fRefundUserName = entity.fRefundUserName;
            refund.fStatus = entity.fStatus;
            refund.fUserName = entity.fUserName;
            refund.fClassRoomTitle = entity.fClassRoomTitle;
            refund.fPayType = tUserPayDal.GettUserPayByBookingNo(refund.fBookingNo).fPayType;
            return refund;
        }

        public static UserRefundModel GetRefundByBookingNo(string strBookingNo)
        {
            tUserRefundEntity entity = tUserRefundDal.GettUserRefundByBookingNo(strBookingNo);
            UserRefundModel refund =null;
            if (entity != null)
            {
                refund = new UserRefundModel();
                refund.fApplyAmount = entity.fApplyAmount;
                refund.fApplyDate = entity.fApplyDate;
                refund.fApplyRemark = entity.fApplyRemark;
                refund.fCreateDate = entity.fCreateDate;
                refund.fCreateOpr = entity.fCreateOpr;
                refund.fID = entity.fID;
                refund.fModifyDate = entity.fModifyDate;
                refund.fModifyOpr = entity.fModifyOpr;
                refund.fOrderNo = entity.fOrderNo;
                refund.fBookingNo = entity.fBookingNo;
                refund.fRefundAmount = entity.fRefundAmount;
                refund.fRefundDate = entity.fRefundDate;
                refund.fRefundNote = entity.fRefundNote;
                refund.fRefundUserName = entity.fRefundUserName;
                refund.fStatus = entity.fStatus;
                refund.fUserName = entity.fUserName;

                refund.fPayType = tUserPayDal.GettUserPayByBookingNo(refund.fBookingNo).fPayType;
            }
            return refund;
        }

        public static decimal GetBokingMaxReturnAmount(string strBookingNo)
        {
            return tUserRefundDal.GetBokingMaxReturnAmount(strBookingNo);
        }


        public static int ConfirmUserRefund(bool bResult,int iRefundID, decimal refundAmount, string strApplyNote, string strApplyOpr, ref string strMsg)
        {
            return tUserRefundDal.ConfirmUserRefund(bResult,iRefundID, refundAmount, strApplyNote, strApplyOpr, ref strMsg);
        }

        public static int RefusedUserRefund(int iRefundID, string strApplyNote, string strApplyOpr)
        {
            tUserRefundEntity refund = tUserRefundDal.GettUserRefund(iRefundID);
            refund.fStatus = 2;
            refund.fRefundNote = "驳回退款";
            refund.fRefundDate = DateTime.Now;
            refund.fRefundUserName = strApplyOpr;
            refund.fModifyDate = DateTime.Now;
            refund.fModifyOpr = strApplyOpr;
            List<tUserRefundEntity> list = new List<tUserRefundEntity>();
            list.Add(refund);

            BookingUpdateStatus(refund.fBookingNo, "已驳回", strApplyNote, strApplyOpr);

            return tUserRefundDal.Modify(list, "update", "fID,fStatus,fRefundNote,fRefundDate,fRefundUserName,fModifyDate,fModifyOpr", null);
        }
    }
}
