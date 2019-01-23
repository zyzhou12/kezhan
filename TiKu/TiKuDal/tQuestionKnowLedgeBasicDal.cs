﻿using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using Trip8H.Common;
using TiKu.Entity;
using TuYou.DBUtility;

namespace TiKu.Dal
{
  public class tQuestionKnowLedgeBasicDal
  {
    public static int Modify(IList<tQuestionKnowLedgeBasicEntity> objList, string doType,
        string updateFiels, string unupdateFiels)
    {
      string xmlData = PubFun.EntityListToXml(objList, "tQuestionKnowLedgeBasic", "DataSet",
          unupdateFiels, updateFiels, null);

      DBParam[] lstParam = new DBParam[2];
      lstParam[0] = new DBParam("@DoType", doType);
      lstParam[1] = new DBParam("@XmlData", xmlData);
      DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "tQuestionKnowLedgeBasic_Modify",
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

        PubFun.WriteIDListToXml(xw, ids, "tQuestionKnowLedgeBasic", "tQuestionKnowLedgeBasicID");

        xw.WriteEndElement();
        xw.WriteEndDocument();
      }

      DBParam[] lstParam = new DBParam[2];
      lstParam[0] = new DBParam("@DoType", "delete");
      lstParam[1] = new DBParam("@XmlData", bufXml.ToString());
      DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "tQuestionKnowLedgeBasic_Modify",
          lstParam, DBHelper.ProcRstTypes.All);
      if (rst.Result < 0)
      {
        if (DBHelper.Log != null) DBHelper.Log.Error(rst.Message);
        throw new Exception(rst.Message);
      }
      return rst.Result;
    }

    public static tQuestionKnowLedgeBasicEntity GettQuestionKnowLedgeBasic(int id)
    {
      tQuestionKnowLedgeBasicEntity rst = null;
      string strSQL = "SELECT * FROM tQuestionKnowLedgeBasic WHERE tQuestionKnowLedgeBasicID=" + id.ToString();
      DataTable dt = DBHelper.QueryToTable("TiKu", strSQL);
      if (dt.Rows.Count > 0)
      {
        rst = new tQuestionKnowLedgeBasicEntity();
        Trip8H.Common.PubFun.DataRowToObject(dt.Rows[0], rst);
      }
      return rst;
    }

    public static List<tQuestionKnowLedgeBasicEntity> GettQuestionKnowLedgeBasicList(int id)
    {
      StringBuilder bufSQL = new StringBuilder();
      List<DbParameter> lstParam = new List<DbParameter>();

      bufSQL.Append("SELECT * FROM tQuestionKnowLedgeBasic WHERE (1=1) ");

      if (id > 0)
      {
        bufSQL.Append(" AND tQuestionKnowLedgeBasicID=@id ");
        lstParam.Add(new DBParam("@id", id));
      }

      //防止返回数据过多
      if (lstParam.Count <= 0) throw new Exception("没有查询条件");
      DataTable dtRst = DBHelper.QueryToTable("TiKu", bufSQL.ToString(), lstParam);
      List<tQuestionKnowLedgeBasicEntity> lstRst = PubFun.DataTableToObjects<tQuestionKnowLedgeBasicEntity>(dtRst);
      return lstRst;
    }

  }
}
