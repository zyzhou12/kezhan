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
  public class tStudentInfoDal
  {
    public static int Modify(IList<tStudentInfoEntity> objList, string doType,
        string updateFiels, string unupdateFiels)
    {
      string xmlData = PubFun.EntityListToXml(objList, "tStudentInfo", "DataSet",
          unupdateFiels, updateFiels, null);

      DBParam[] lstParam = new DBParam[2];
      lstParam[0] = new DBParam("@DoType", doType);
      lstParam[1] = new DBParam("@XmlData", xmlData);
      DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "tStudentInfo_Modify",
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

        PubFun.WriteIDListToXml(xw, ids, "tStudentInfo", "tStudentInfoID");

        xw.WriteEndElement();
        xw.WriteEndDocument();
      }

      DBParam[] lstParam = new DBParam[2];
      lstParam[0] = new DBParam("@DoType", "delete");
      lstParam[1] = new DBParam("@XmlData", bufXml.ToString());
      DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "tStudentInfo_Modify",
          lstParam, DBHelper.ProcRstTypes.All);
      if (rst.Result < 0)
      {
        if (DBHelper.Log != null) DBHelper.Log.Error(rst.Message);
        throw new Exception(rst.Message);
      }
      return rst.Result;
    }

    public static tStudentInfoEntity GettStudentInfoByUserName(string strUserName)
    {
      tStudentInfoEntity rst = null;
      string strSQL = "SELECT * FROM tStudentInfo WHERE fUserName=@UserName";
      List<DbParameter> lstParam = new List<DbParameter>();
      lstParam.Add(new DBParam("@UserName", strUserName));
      DataTable dt = DBHelper.QueryToTable("TiKu", strSQL, lstParam);
      if (dt.Rows.Count > 0)
      {
        rst = new tStudentInfoEntity();
        Trip8H.Common.PubFun.DataRowToObject(dt.Rows[0], rst);
      }
      return rst;
    }

    public static List<tStudentInfoEntity> GettStudentInfoList(int id)
    {
      StringBuilder bufSQL = new StringBuilder();
      List<DbParameter> lstParam = new List<DbParameter>();

      bufSQL.Append("SELECT * FROM tStudentInfo WHERE (1=1) ");

      if (id > 0)
      {
        bufSQL.Append(" AND tStudentInfoID=@id ");
        lstParam.Add(new DBParam("@id", id));
      }

      //防止返回数据过多
      if (lstParam.Count <= 0) throw new Exception("没有查询条件");
      DataTable dtRst = DBHelper.QueryToTable("TiKu", bufSQL.ToString(), lstParam);
      List<tStudentInfoEntity> lstRst = PubFun.DataTableToObjects<tStudentInfoEntity>(dtRst);
      return lstRst;
    }

  }
}
