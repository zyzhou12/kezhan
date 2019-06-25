using System;
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
  public class tMediaDal
  {
    public static int Modify(IList<tMediaEntity> objList, string doType,
        string updateFiels, string unupdateFiels)
    {
      string xmlData = PubFun.EntityListToXml(objList, "tMedia", "DataSet",
          unupdateFiels, updateFiels, null);

      DBParam[] lstParam = new DBParam[2];
      lstParam[0] = new DBParam("@DoType", doType);
      lstParam[1] = new DBParam("@XmlData", xmlData);
      DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "tMedia_Modify",
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

        PubFun.WriteIDListToXml(xw, ids, "tMedia", "tMediaID");

        xw.WriteEndElement();
        xw.WriteEndDocument();
      }

      DBParam[] lstParam = new DBParam[2];
      lstParam[0] = new DBParam("@DoType", "delete");
      lstParam[1] = new DBParam("@XmlData", bufXml.ToString());
      DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "tMedia_Modify",
          lstParam, DBHelper.ProcRstTypes.All);
      if (rst.Result < 0)
      {
        if (DBHelper.Log != null) DBHelper.Log.Error(rst.Message);
        throw new Exception(rst.Message);
      }
      return rst.Result;
    }

    public static tMediaEntity GettMedia(int id)
    {
      tMediaEntity rst = null;
      string strSQL = "SELECT * FROM tMedia WHERE tMediaID=" + id.ToString();
      DataTable dt = DBHelper.QueryToTable("TiKu", strSQL);
      if (dt.Rows.Count > 0)
      {
        rst = new tMediaEntity();
        Trip8H.Common.PubFun.DataRowToObject(dt.Rows[0], rst);
      }
      return rst;
    }

    public static List<tMediaEntity> GettMediaList(string strBeginDate, string strEndDate, string strName)
    {
        StringBuilder bufSQL = new StringBuilder();
        List<DbParameter> lstParam = new List<DbParameter>();

        bufSQL.Append(@"select fCourseTitle,m.* from tmedia m
left join tResource r on r.fMediaID=m.fMediaID
                        left join tCourse c on c.fResourceUrl=r.fResourceCode
                     where fCourseTitle like '%'+@Name+'%' 
                        and (c.fUploadDate between @BeginDate and @EndDate or isnull(@BeginDate,'')='')");

        lstParam.Add(new DBParam("@Name", strName));
        lstParam.Add(new DBParam("@BeginDate", strBeginDate));
        lstParam.Add(new DBParam("@EndDate", strEndDate));

        //防止返回数据过多
        if (lstParam.Count <= 0) throw new Exception("没有查询条件");
        DataTable dtRst = DBHelper.QueryToTable("TiKu", bufSQL.ToString(), lstParam);
        List<tMediaEntity> lstRst = PubFun.DataTableToObjects<tMediaEntity>(dtRst);
        return lstRst;
    }

  }
}
