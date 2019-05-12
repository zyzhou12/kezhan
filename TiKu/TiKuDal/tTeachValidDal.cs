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
  public class tTeachValidDal
  {
    public static int Modify(IList<tTeachValidEntity> objList, string doType,
        string updateFiels, string unupdateFiels)
    {
      string xmlData = PubFun.EntityListToXml(objList, "tTeachValid", "DataSet",
          unupdateFiels, updateFiels, null);

      DBParam[] lstParam = new DBParam[2];
      lstParam[0] = new DBParam("@DoType", doType);
      lstParam[1] = new DBParam("@XmlData", xmlData);
      DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "tTeachValid_Modify",
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

        PubFun.WriteIDListToXml(xw, ids, "tTeachValid", "tTeachValidID");

        xw.WriteEndElement();
        xw.WriteEndDocument();
      }

      DBParam[] lstParam = new DBParam[2];
      lstParam[0] = new DBParam("@DoType", "delete");
      lstParam[1] = new DBParam("@XmlData", bufXml.ToString());
      DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "tTeachValid_Modify",
          lstParam, DBHelper.ProcRstTypes.All);
      if (rst.Result < 0)
      {
        if (DBHelper.Log != null) DBHelper.Log.Error(rst.Message);
        throw new Exception(rst.Message);
      }
      return rst.Result;
    }



    public static int TeachValidSubmit(IList<tTeachValidEntity> objList, IList<tTeacherValidDetailEntity> detailList)
    {
        string xmlData = PubFun.EntityListToXml(objList, "tTeachValid", "DataSet",
            "", "", null);
        string detailxmlData = PubFun.EntityListToXml(detailList, "tTeacherValidDetail", "DataSet",
            "", "", null);

        DBParam[] lstParam = new DBParam[2];
        lstParam[0] = new DBParam("@XmlData", xmlData);
        lstParam[1] = new DBParam("@DetailXmlData", detailxmlData);
        DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "TeachValid_Submit",
            lstParam, DBHelper.ProcRstTypes.All);
        if (rst.Result < 0)
        {
            if (DBHelper.Log != null) DBHelper.Log.Error(rst.Message);
            throw new Exception(rst.Message);
        }
        return rst.Result;
    }


    public static tTeachValidEntity GettTeachValid(int iFID)
    {
      tTeachValidEntity rst = null;
      string strSQL = "SELECT * FROM tTeachValid WHERE fID=@ID order by 1 desc";
      List<DbParameter> lstParam = new List<DbParameter>();
      lstParam.Add(new DBParam("@ID", iFID));
      DataTable dt = DBHelper.QueryToTable("TiKu", strSQL,lstParam);
      if (dt.Rows.Count > 0)
      {
        rst = new tTeachValidEntity();
        Trip8H.Common.PubFun.DataRowToObject(dt.Rows[0], rst);
      }
      return rst;
    }

    public static List<tTeachValidEntity> GettTeachValidList(string strUserName,string strStatus=null)
    {
      StringBuilder strSQL = new StringBuilder();
      strSQL.Append("SELECT * FROM tTeachValid WHERE fUserName=@UserName ");
      List<DbParameter> lstParam = new List<DbParameter>();
      lstParam.Add(new DBParam("@UserName", strUserName));
      if (!string.IsNullOrEmpty(strStatus))
      {
        strSQL.Append(" and fStatus=@Status ");
        lstParam.Add(new DBParam("@Status", strStatus));
      }
      DataTable dtRst = DBHelper.QueryToTable("TiKu", strSQL.ToString(), lstParam);

      List<tTeachValidEntity> lstRst = PubFun.DataTableToObjects<tTeachValidEntity>(dtRst);
      return lstRst;
    }

    public static List<tTeachValidEntity> GettTeachValidList(int id)
    {
      StringBuilder bufSQL = new StringBuilder();
      List<DbParameter> lstParam = new List<DbParameter>();

      bufSQL.Append("SELECT * FROM tTeachValid WHERE (1=1) ");

      if (id > 0)
      {
        bufSQL.Append(" AND tTeachValidID=@id ");
        lstParam.Add(new DBParam("@id", id));
      }

      //防止返回数据过多
      if (lstParam.Count <= 0) throw new Exception("没有查询条件");
      DataTable dtRst = DBHelper.QueryToTable("TiKu", bufSQL.ToString(), lstParam);
      List<tTeachValidEntity> lstRst = PubFun.DataTableToObjects<tTeachValidEntity>(dtRst);
      return lstRst;
    }

    public static DataTable TeacherValidQuery(string strUserName)
    {
      List<DbParameter> lstParam = new List<DbParameter>();
      lstParam.Add(new DBParam("@UserName", strUserName));

      DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "Teacher_ValidQuery", lstParam, DBHelper.ProcRstTypes.All);

      return rst.DataSet.Tables[0];
    }

    public static int TeacherValid(string strUserName, int iValidFid, int iValidDetailFid, bool ValidResutl, string strName, string strIDType, string strUID, string strCertType, string strCertNo, string strEffect, string strPharse,string strSubject, string strValidMessage)
    {
      List<DbParameter> lstParam = new List<DbParameter>();
      lstParam.Add(new DBParam("@UserName", strUserName));
      lstParam.Add(new DBParam("@ValidFID", iValidFid));
      lstParam.Add(new DBParam("@ValidDetailFID", iValidDetailFid));
      lstParam.Add(new DBParam("@ValidResult", ValidResutl));
      lstParam.Add(new DBParam("@Name", strName));
      lstParam.Add(new DBParam("@IDType", strIDType));
      lstParam.Add(new DBParam("@UId", strUID));
      lstParam.Add(new DBParam("@CertType", strCertType));
      lstParam.Add(new DBParam("@CertNo", strCertNo));
      lstParam.Add(new DBParam("@CertEffect", strEffect));
      lstParam.Add(new DBParam("@Pharse", strPharse));
      lstParam.Add(new DBParam("@Subject", strSubject));
      lstParam.Add(new DBParam("@ValidMessage", strValidMessage));
     
      DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "Teacher_Valid", lstParam, DBHelper.ProcRstTypes.All);

      return rst.Result;
    }

  }
}
