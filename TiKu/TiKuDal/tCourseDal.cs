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
    public class tCourseDal
    {
        public static int Modify(IList<tCourseEntity> objList, string doType,
            string updateFiels, string unupdateFiels)
        {
            string xmlData = PubFun.EntityListToXml(objList, "tCourse", "DataSet",
                unupdateFiels, updateFiels, null);

            DBParam[] lstParam = new DBParam[2];
            lstParam[0] = new DBParam("@DoType", doType);
            lstParam[1] = new DBParam("@XmlData", xmlData);
            DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "tCourse_Modify",
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

                PubFun.WriteIDListToXml(xw, ids, "tCourse", "fID");

                xw.WriteEndElement();
                xw.WriteEndDocument();
            }

            DBParam[] lstParam = new DBParam[2];
            lstParam[0] = new DBParam("@DoType", "delete");
            lstParam[1] = new DBParam("@XmlData", bufXml.ToString());
            DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "tCourse_Modify",
                lstParam, DBHelper.ProcRstTypes.All);
            if (rst.Result < 0)
            {
                if (DBHelper.Log != null) DBHelper.Log.Error(rst.Message);
                throw new Exception(rst.Message);
            }
            return rst.Result;
        }
        
        public static tCourseEntity GettCourse(int id)
        {
            tCourseEntity rst = null;
            string strSQL = "SELECT * FROM tCourse WHERE fID=" + id.ToString();
            DataTable dt = DBHelper.QueryToTable("TiKu", strSQL);
            if (dt.Rows.Count > 0)
            {
                rst = new tCourseEntity();
                Trip8H.Common.PubFun.DataRowToObject(dt.Rows[0], rst);
            }
            return rst;
        }

        public static tCourseEntity GetCourseByClassID(string strClassID)
        {
            tCourseEntity rst = null;
            string strSQL = "SELECT * FROM tCourse WHERE fClassID=@ClassID";
            List<DbParameter> lstParam = new List<DbParameter>();
            lstParam.Add(new DBParam("@ClassID", strClassID));
            DataTable dt = DBHelper.QueryToTable("TiKu", strSQL, lstParam);
            if (dt.Rows.Count > 0)
            {
                rst = new tCourseEntity();
                Trip8H.Common.PubFun.DataRowToObject(dt.Rows[0], rst);
            }
            return rst;
        }

        public static List<tCourseEntity> GetCourseListByClassRoomCode(string strClassRoomCode)
        {
            StringBuilder bufSQL = new StringBuilder();
            List<DbParameter> lstParam = new List<DbParameter>();

            bufSQL.Append("SELECT * FROM tCourse WHERE (1=1) ");

            if (!string.IsNullOrEmpty(strClassRoomCode))
            {
                bufSQL.Append(" AND fClassRoomCode=@ClassRoomCode ");
                lstParam.Add(new DBParam("@ClassRoomCode", strClassRoomCode));
            }

            //防止返回数据过多
            if (lstParam.Count <= 0) throw new Exception("没有查询条件");
            DataTable dtRst = DBHelper.QueryToTable("TiKu", bufSQL.ToString(), lstParam);
            List<tCourseEntity> lstRst = PubFun.DataTableToObjects<tCourseEntity>(dtRst);
            return lstRst;
        }

        public static List<tCourseEntity> GetMyCourseList(string strUserName)
        {
            StringBuilder bufSQL = new StringBuilder();
            List<DbParameter> lstParam = new List<DbParameter>();

            bufSQL.Append(@"SELECT c.* FROM tCourse c
                                left join tClassRoom cr on cr.fClassRoomCode=c.fClassRoomCode
                                left join tUser u on u.fUserName=cr.fTecharUserName
                                LEFT JOIN tBooking b on cr.fClassRoomCode=b.fTypeCode and b.fType='ClassRoom' 
                                WHERE 1=1  
                                AND b.fUserName=@UserName
                                AND cr.fType='Live'
                                 and b.fStatus in ('已支付','已驳回') 
                                 AND cr.fStatus='发布' ");

          
                lstParam.Add(new DBParam("@UserName", strUserName));
            

            //防止返回数据过多
            if (lstParam.Count <= 0) throw new Exception("没有查询条件");
            DataTable dtRst = DBHelper.QueryToTable("TiKu", bufSQL.ToString(), lstParam);
            List<tCourseEntity> lstRst = PubFun.DataTableToObjects<tCourseEntity>(dtRst);
            return lstRst;
        }


        public static DataTable GetCourseById(int iCourseID, string strUserName)
        {
            StringBuilder bufSQL = new StringBuilder();
            List<DbParameter> lstParam = new List<DbParameter>();

            bufSQL.Append(@"select isnull(b.fID,0) IsBuy,c.* from tCourse c
                        left join tbooking b on b.fType='OnLineClass' and b.fStatus in ('已支付','已驳回') and b.fTypeCode=c.fid and fUserName=@UserName
                        where c.fId=@CourseID ");
            lstParam.Add(new DBParam("@CourseID", iCourseID));
            lstParam.Add(new DBParam("@UserName", strUserName));

            //防止返回数据过多
            if (lstParam.Count <= 0) throw new Exception("没有查询条件");
            DataTable dtRst = DBHelper.QueryToTable("TiKu", bufSQL.ToString(), lstParam);
            return dtRst;
        }
        public static int UpdateClassID(int iCourseID, string strClassID)
        {
            string strSql = "update tCourse set fClassID=@ClassID where fID=@CourseID";
            List<DbParameter> lstParam = new List<DbParameter>();
            lstParam.Add(new DBParam("@CourseID", iCourseID));
            lstParam.Add(new DBParam("@ClassID", strClassID));
            object obj = DBHelper.ExecuteScalar("TiKu", strSql, lstParam);
            return Convert.ToInt32(obj);
        }


        public static int UpdateConver(string strResourceCode, string strCoverUrl)
        {
            StringBuilder bufSQL = new StringBuilder();
            List<DbParameter> lstParam = new List<DbParameter>();

            bufSQL.Append(@"update tCourse
set fFileCoverUrl=@CoverUrl
where fResourceUrl=@ResourceCode");
            lstParam.Add(new DBParam("@ResourceCode", strResourceCode));
            lstParam.Add(new DBParam("@CoverUrl", strCoverUrl));

            //防止返回数据过多
            if (lstParam.Count <= 0) throw new Exception("没有查询条件");
            Object dtRst = DBHelper.ExecuteScalar("TiKu", bufSQL.ToString(), lstParam);
            return Convert.ToInt32(dtRst);
        }

    }
}
