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
  public class tClassRoomDal
  {
    public static int Modify(IList<tClassRoomEntity> objList, string doType,
        string updateFiels, string unupdateFiels)
    {
      string xmlData = PubFun.EntityListToXml(objList, "tClassRoom", "DataSet",
          unupdateFiels, updateFiels, null);

      DBParam[] lstParam = new DBParam[2];
      lstParam[0] = new DBParam("@DoType", doType);
      lstParam[1] = new DBParam("@XmlData", xmlData);
      DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "tClassRoom_Modify",
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

        PubFun.WriteIDListToXml(xw, ids, "tClassRoom", "tClassRoomID");

        xw.WriteEndElement();
        xw.WriteEndDocument();
      }

      DBParam[] lstParam = new DBParam[2];
      lstParam[0] = new DBParam("@DoType", "delete");
      lstParam[1] = new DBParam("@XmlData", bufXml.ToString());
      DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "tClassRoom_Modify",
          lstParam, DBHelper.ProcRstTypes.All);
      if (rst.Result < 0)
      {
        if (DBHelper.Log != null) DBHelper.Log.Error(rst.Message);
        throw new Exception(rst.Message);
      }
      return rst.Result;
    }


    public static tClassRoomEntity GettClassRoomByID(int fID)
    {
      tClassRoomEntity rst = null;
      string strSQL = "SELECT * FROM tClassRoom WHERE fID=@ID";
      List<DbParameter> lstParam = new List<DbParameter>();
      lstParam.Add(new DBParam("@ID", fID));
      DataTable dt = DBHelper.QueryToTable("TiKu", strSQL, lstParam);
      if (dt.Rows.Count > 0)
      {
        rst = new tClassRoomEntity();
        Trip8H.Common.PubFun.DataRowToObject(dt.Rows[0], rst);
      }
      return rst;
    }

    public static tClassRoomEntity GettClassRoomByCode(string strClassRoomCode,string strUserName)
    {
      tClassRoomEntity rst = null;
      string strSQL = "SELECT * FROM tClassRoom WHERE fClassRoomCode=@ClassRoomCode";
      List<DbParameter> lstParam = new List<DbParameter>();
      lstParam.Add(new DBParam("@ClassRoomCode", strClassRoomCode));
      if (!string.IsNullOrEmpty(strUserName))
      {
          strSQL += " and fTecharUserName=@UserName";
        lstParam.Add(new DBParam("@UserName", strUserName));
      }
      DataTable dt = DBHelper.QueryToTable("TiKu", strSQL, lstParam);
      if (dt.Rows.Count > 0)
      {
        rst = new tClassRoomEntity();
        Trip8H.Common.PubFun.DataRowToObject(dt.Rows[0], rst);
      }
      return rst;
    }

    public static List<tClassRoomEntity> GettClassRoomListByTeacher(string strTeacher,string strStatus)
    {
      StringBuilder bufSQL = new StringBuilder();
      List<DbParameter> lstParam = new List<DbParameter>();

      bufSQL.Append("SELECT * FROM tClassRoom WHERE fTecharUserName=@UserName ");
      lstParam.Add(new DBParam("@UserName", strTeacher));

      if (!string.IsNullOrEmpty(strStatus))
      {
        bufSQL.Append(" AND fStatus=@Status ");
        lstParam.Add(new DBParam("@Status", strStatus));
      }

      //防止返回数据过多
      if (lstParam.Count <= 0) throw new Exception("没有查询条件");
      DataTable dtRst = DBHelper.QueryToTable("TiKu", bufSQL.ToString(), lstParam);
      List<tClassRoomEntity> lstRst = PubFun.DataTableToObjects<tClassRoomEntity>(dtRst);
      return lstRst;
    }

    public static List<tClassRoomEntity> GetMyClassRoomList(string strUserName)
    {
        StringBuilder bufSQL = new StringBuilder();
        List<DbParameter> lstParam = new List<DbParameter>();

        bufSQL.Append(@"SELECT * FROM tClassRoom cr 
                      LEFT JOIN tBooking b on cr.fClassRoomCode=b.fTypeCode and b.fType='ClassRoom' and b.fStatus='已支付' WHERE 1=1 ");


        if (!string.IsNullOrEmpty(strUserName))
        {
            bufSQL.Append(" AND b.fUserName=@UserName ");
            lstParam.Add(new DBParam("@UserName", strUserName));
        }

        //防止返回数据过多
        if (lstParam.Count <= 0) throw new Exception("没有查询条件");
        DataTable dtRst = DBHelper.QueryToTable("TiKu", bufSQL.ToString(), lstParam);
        List<tClassRoomEntity> lstRst = PubFun.DataTableToObjects<tClassRoomEntity>(dtRst);
        return lstRst;
    }

    public static List<tClassRoomEntity> GettClassRoomListByCity(string strCity, string strStatus)
    {
      StringBuilder bufSQL = new StringBuilder();
      List<DbParameter> lstParam = new List<DbParameter>();

      bufSQL.Append(@"SELECT * FROM tClassRoom cr 
                      LEFT JOIN tUser u on cr.fTecharUserName=u.fUserName WHERE fCity=@City ");
      lstParam.Add(new DBParam("@City", strCity));

      if (!string.IsNullOrEmpty(strStatus))
      {
        bufSQL.Append(" AND cr.fStatus=@Status ");
        lstParam.Add(new DBParam("@Status", strStatus));
      }

      //防止返回数据过多
      if (lstParam.Count <= 0) throw new Exception("没有查询条件");
      DataTable dtRst = DBHelper.QueryToTable("TiKu", bufSQL.ToString(), lstParam);
      List<tClassRoomEntity> lstRst = PubFun.DataTableToObjects<tClassRoomEntity>(dtRst);
      return lstRst;
    }

    public static DataTable GettClassRoomList(string strCity, string strPharse, string strGrade, string strSubject)
    {
      List<DbParameter> lstParam = new List<DbParameter>();


      lstParam.Add(new DBParam("@City", strCity));
      lstParam.Add(new DBParam("@Pharse", strPharse));
      lstParam.Add(new DBParam("@Grade", strGrade));
      lstParam.Add(new DBParam("@Subject", strSubject));

      //防止返回数据过多
      if (lstParam.Count <= 0) throw new Exception("没有查询条件");
      DataTable dtRst = DBHelper.ExecuteProc("TiKu", "ClassRoom_Query", lstParam,DBHelper.ProcRstTypes.All).DataSet.Tables[0];
      
      return dtRst;
    }

    public static DataSet GettClassRoomDetail(string strClassRoomCode, string strUserName)
    {
      List<DbParameter> lstParam = new List<DbParameter>();


      lstParam.Add(new DBParam("@ClassRoomCode", strClassRoomCode));
      lstParam.Add(new DBParam("@UserName", strUserName));

      //防止返回数据过多
      if (lstParam.Count <= 0) throw new Exception("没有查询条件");
      DataSet dtRst = DBHelper.ExecuteProc("TiKu", "ClassRoom_QueryDetail", lstParam, DBHelper.ProcRstTypes.All).DataSet;

      return dtRst;
    }


    public static DataTable GetClassRoomByCourseId(int iCourseID, string strUserName)
    {
        StringBuilder bufSQL = new StringBuilder();
        List<DbParameter> lstParam = new List<DbParameter>();

        bufSQL.Append(@"select isnull(b.fID,0) IsBuy,cr.* from tCourse c
                        left join tClassRoom cr on cr.fClassRoomCode=c.fClassRoomCode
                        left join tbooking b on b.fType='ClassRoom' and b.fStatus='已支付' and b.fTypeCode=c.fClassRoomCode and fUserName=@UserName
                        where c.fId=@CourseID ");
        lstParam.Add(new DBParam("@CourseID", iCourseID));
        lstParam.Add(new DBParam("@UserName", strUserName));

        //防止返回数据过多
        if (lstParam.Count <= 0) throw new Exception("没有查询条件");
        DataTable dtRst = DBHelper.QueryToTable("TiKu", bufSQL.ToString(), lstParam);
        return dtRst;
    }

    public static List<tMediaEntity> GetCourseMediaList(int iCourseID)
    {
      StringBuilder bufSQL = new StringBuilder();
      List<DbParameter> lstParam = new List<DbParameter>();

      bufSQL.Append(@"select * from tMedia where fCourseID=@CourseID ");
      lstParam.Add(new DBParam("@CourseID", iCourseID));

      //防止返回数据过多
      if (lstParam.Count <= 0) throw new Exception("没有查询条件");
      DataTable dtRst = DBHelper.QueryToTable("TiKu", bufSQL.ToString(), lstParam);
      List<tMediaEntity> lstRst = PubFun.DataTableToObjects<tMediaEntity>(dtRst);
      return lstRst;
    }

    public static decimal GetClassRoomPrice(int iCourseID)
    {
        List<DbParameter> lstParam = new List<DbParameter>();


        lstParam.Add(new DBParam("@iCourseID", iCourseID));

        //防止返回数据过多
        if (lstParam.Count <= 0) throw new Exception("没有查询条件");
        object dtRst = DBHelper.ExecuteScalar("TiKu", "ClassRoomPrice_Query", lstParam);

        return Convert.ToDecimal(dtRst);
    }
  }
}
