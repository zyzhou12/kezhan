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
    public static string SubmitBooking(string strUserName,string strType,string strCode,decimal dAmount,bool strIsReturn,string strSystem)
    {
      tBookingEntity booking = new tBookingEntity();
      booking.fUserName = strUserName;
      Random rd = new Random();
      if (strType == "ClassRoom")
      {
        booking.fBookingNo = "C" + DateTime.Now.ToString("yyyyMMddHHmmssssss") + rd.Next(10, 100).ToString();
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
      booking.fIsReturn = strIsReturn.ToString();
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


    public static BookingListModel GetBookingList(string strTeacherUserName,string strClassRoomCode)
    {
      BookingListModel model = new BookingListModel();
      DataTable dt = tBookingDal.GetClassRoomBookingList(strTeacherUserName, strClassRoomCode);
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

      return model;
    }

    public static int BookingPay(string strUserName, string strTradePass, string strBookingNo,string strSystem, ref string strMsg)
    {
      int i = tBookingDal.BookingPay(strUserName, strTradePass, strBookingNo, strSystem, ref strMsg);
      return i;
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
  }
}
