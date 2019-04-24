using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using Trip8H.Common;
using TuYou.DBUtility;
using TiKu.Entity;

namespace TiKu.Dal
{
  public class tBookingDal
  {
    public static int Modify(IList<tBookingEntity> objList, string doType,
        string updateFiels, string unupdateFiels)
    {
      string xmlData = PubFun.EntityListToXml(objList, "tBooking", "DataSet",
          unupdateFiels, updateFiels, null);

      DBParam[] lstParam = new DBParam[2];
      lstParam[0] = new DBParam("@DoType", doType);
      lstParam[1] = new DBParam("@XmlData", xmlData);
      DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "tBooking_Modify",
          lstParam, DBHelper.ProcRstTypes.All);
      if (rst.Result < 0)
      {
        if (DBHelper.Log != null) DBHelper.Log.Error(rst.Message);
        throw new Exception(rst.Message);
      }
      return rst.Result;
    }

    public static int Delete(string ids)
    {
      StringBuilder bufXml = new StringBuilder(1024);
      using (XmlWriter xw = XmlWriter.Create(bufXml))
      {
        xw.WriteStartDocument();
        xw.WriteStartElement("DataSet");

        PubFun.WriteIDListToXml(xw, ids, "tBooking", "tBookingID");

        xw.WriteEndElement();
        xw.WriteEndDocument();
      }

      DBParam[] lstParam = new DBParam[2];
      lstParam[0] = new DBParam("@DoType", "delete");
      lstParam[1] = new DBParam("@XmlData", bufXml.ToString());
      DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "tBooking_Modify",
          lstParam, DBHelper.ProcRstTypes.All);
      if (rst.Result < 0)
      {
        if (DBHelper.Log != null) DBHelper.Log.Error(rst.Message);
        throw new Exception(rst.Message);
      }
      return rst.Result;
    }

    public static tBookingEntity GettBooking(string strBookingNo)
    {
      tBookingEntity rst = null;
      string strSQL = "SELECT * FROM tBooking WHERE fBookingNo=@BookingNo";
      List<DbParameter> lstParam = new List<DbParameter>();
      lstParam.Add(new DBParam("@BookingNo", strBookingNo));

      DataTable dt = DBHelper.QueryToTable("TiKu", strSQL,lstParam);
      if (dt.Rows.Count > 0)
      {
        rst = new tBookingEntity();
        Trip8H.Common.PubFun.DataRowToObject(dt.Rows[0], rst);
      }
      return rst;
    }

    public static tBookingEntity GettBooking(string strUserName,string strType,string strCode,string strStatus)
    {
        tBookingEntity rst = null;
        string strSQL = "SELECT * FROM tBooking WHERE (fStatus=@Status or isnull(@Status,'')='')  and fUserName=@UserName and fType=@Type and fTypeCode=@TypeCode order by fCreateDate desc";
        List<DbParameter> lstParam = new List<DbParameter>();
        lstParam.Add(new DBParam("@Status", strStatus));
        lstParam.Add(new DBParam("@UserName", strUserName));
        lstParam.Add(new DBParam("@Type", strType));
        lstParam.Add(new DBParam("@TypeCode", strCode));

        DataTable dt = DBHelper.QueryToTable("TiKu", strSQL, lstParam);
        if (dt.Rows.Count > 0)
        {
            rst = new tBookingEntity();
            Trip8H.Common.PubFun.DataRowToObject(dt.Rows[0], rst);
        }
        return rst;
    }

    public static string GetPayNoByBookingNo(string strBookingNo)
    {
        string strSQL = "select fOrderNo  from tuserpay where fType='BuyClass' and fPayNo is not null and fRemark=@BookingNo";
        List<DbParameter> lstParam = new List<DbParameter>();
        lstParam.Add(new DBParam("@BookingNo", strBookingNo));

        object dt = DBHelper.ExecuteScalar("TiKu", strSQL, lstParam);

        return dt.ToString();
    }

    public static List<tBookingEntity> GettBookingListByUserName(string strUserName)
    {
      StringBuilder bufSQL = new StringBuilder();
      List<DbParameter> lstParam = new List<DbParameter>();

      bufSQL.Append("SELECT * FROM tBooking WHERE (1=1) ");

      if (!string.IsNullOrEmpty(strUserName))
      {
        bufSQL.Append(" AND fUserName=@UserName ");
        lstParam.Add(new DBParam("@UserName", strUserName));
      }

      //防止返回数据过多
      if (lstParam.Count <= 0) throw new Exception("没有查询条件");
      DataTable dtRst = DBHelper.QueryToTable("TiKu", bufSQL.ToString(), lstParam);
      List<tBookingEntity> lstRst = PubFun.DataTableToObjects<tBookingEntity>(dtRst);
      return lstRst;
    }

    public static DataTable GetBookingList(string strUserName, string strStatus, DateTime beginDate, DateTime endDate)
    {
      List<DbParameter> lstParam = new List<DbParameter>();


      lstParam.Add(new DBParam("@UserName", strUserName));
      lstParam.Add(new DBParam("@BeginDate", beginDate.ToShortDateString()));
      lstParam.Add(new DBParam("@EndDate", endDate.ToShortDateString()));
      lstParam.Add(new DBParam("@Status", strStatus));

      //防止返回数据过多
      if (lstParam.Count <= 0) throw new Exception("没有查询条件");
      DataTable dtRst = DBHelper.ExecuteProc("TiKu", "User_QueryBooking", lstParam, DBHelper.ProcRstTypes.All).DataSet.Tables[0];

      return dtRst;
    }

    public static DataTable GetClassRoomBookingList(string strUserName,string strClassRoomCode,string strMobile,string strStatus,string beginDate,string endDate)
    {
      List<DbParameter> lstParam = new List<DbParameter>();
      if (!string.IsNullOrEmpty(strMobile))
      {
          strMobile = strMobile.Trim();
      }

      lstParam.Add(new DBParam("@UserName", strUserName));
      lstParam.Add(new DBParam("@ClassRoomCode", strClassRoomCode));
      lstParam.Add(new DBParam("@Mobile", strMobile));
      lstParam.Add(new DBParam("@Status", strStatus));
      lstParam.Add(new DBParam("@BeginDate", beginDate));
      lstParam.Add(new DBParam("@EndDate", endDate));

      //防止返回数据过多
      if (lstParam.Count <= 0) throw new Exception("没有查询条件");
      DataTable dtRst = DBHelper.ExecuteProc("TiKu", "ClassRoom_QueryBooking", lstParam, DBHelper.ProcRstTypes.All).DataSet.Tables[0];

      return dtRst;
    }


    public static int BookingPay(string strUserName,string strTradePass,string strBookingType,string strBookingNo,string strSystem,ref string strMsg)
    {
      List<DbParameter> lstParam = new List<DbParameter>();
      lstParam.Add(new DBParam("@UserName", strUserName));
      lstParam.Add(new DBParam("@TradePass", strTradePass));
      lstParam.Add(new DBParam("@BookingType", strBookingType));
      lstParam.Add(new DBParam("@BookingNo", strBookingNo));
      lstParam.Add(new DBParam("@System", strSystem));


      DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "User_PayBooking",
         lstParam, DBHelper.ProcRstTypes.All);
      strMsg = rst.Message;
      return rst.Result;
    }
  }
}
