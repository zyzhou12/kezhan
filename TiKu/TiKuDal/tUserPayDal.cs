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
  public class tUserPayDal
  {
    public static int Modify(IList<tUserPayEntity> objList, string doType,
        string updateFiels, string unupdateFiels)
    {
      string xmlData = PubFun.EntityListToXml(objList, "tUserPay", "DataSet",
          unupdateFiels, updateFiels, null);

      DBParam[] lstParam = new DBParam[2];
      lstParam[0] = new DBParam("@DoType", doType);
      lstParam[1] = new DBParam("@XmlData", xmlData);
      DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "tUserPay_Modify",
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

        PubFun.WriteIDListToXml(xw, ids, "tUserPay", "tUserPayID");

        xw.WriteEndElement();
        xw.WriteEndDocument();
      }

      DBParam[] lstParam = new DBParam[2];
      lstParam[0] = new DBParam("@DoType", "delete");
      lstParam[1] = new DBParam("@XmlData", bufXml.ToString());
      DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "tUserPay_Modify",
          lstParam, DBHelper.ProcRstTypes.All);
      if (rst.Result < 0)
      {
        if (DBHelper.Log != null) DBHelper.Log.Error(rst.Message);
        throw new Exception(rst.Message);
      }
      return rst.Result;
    }

    public static tUserPayEntity GettUserPayByBookingNo(string strBooking)
    {
      tUserPayEntity rst = null;
      string strSQL = "SELECT * FROM tUserPay WHERE fRemark=@BookingNo";
      List<DbParameter> lstParam = new List<DbParameter>();
      lstParam.Add(new DBParam("@BookingNo", strBooking));

      DataTable dt = DBHelper.QueryToTable("TiKu", strSQL,lstParam);
      if (dt.Rows.Count > 0)
      {
        rst = new tUserPayEntity();
        Trip8H.Common.PubFun.DataRowToObject(dt.Rows[0], rst);
      }
      return rst;
    }


    public static List<tUserPayEntity> GettUserPayList(int id)
    {
      StringBuilder bufSQL = new StringBuilder();
      List<DbParameter> lstParam = new List<DbParameter>();

      bufSQL.Append("SELECT * FROM tUserPay WHERE (1=1) ");

      if (id > 0)
      {
        bufSQL.Append(" AND tUserPayID=@id ");
        lstParam.Add(new DBParam("@id", id));
      }

      //防止返回数据过多
      if (lstParam.Count <= 0) throw new Exception("没有查询条件");
      DataTable dtRst = DBHelper.QueryToTable("TiKu", bufSQL.ToString(), lstParam);
      List<tUserPayEntity> lstRst = PubFun.DataTableToObjects<tUserPayEntity>(dtRst);
      return lstRst;
    }

    public static int UserPaySuccess(string strPayType, string strPayNo, string strTradeNo)
    {
      List<DbParameter> lstParam = new List<DbParameter>();


      lstParam.Add(new DBParam("@PayType", strPayType));
      lstParam.Add(new DBParam("@PayNo", strPayNo));
      lstParam.Add(new DBParam("@TradeNo", strTradeNo));

      //防止返回数据过多
      if (lstParam.Count <= 0) throw new Exception("没有查询条件");
      DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "UserPay_Success", lstParam, DBHelper.ProcRstTypes.All);

      return Convert.ToInt32(rst.Result);
    }


      public static decimal GetUserAccountAmount(string strUserName)
    {

        StringBuilder bufSQL = new StringBuilder();
        List<DbParameter> lstParam = new List<DbParameter>();

        bufSQL.Append("select isnull(sum(fAmount),0) from tuserAmount WHERE (1=1) ");

      
            bufSQL.Append(" AND fUserName=@username ");
            lstParam.Add(new DBParam("@username", strUserName));
        

        //防止返回数据过多
        if (lstParam.Count <= 0) throw new Exception("没有查询条件");
        object dtRst = DBHelper.ExecuteScalar("TiKu", bufSQL.ToString(), lstParam);
        return Convert.ToDecimal(dtRst);
    }

  }
}
