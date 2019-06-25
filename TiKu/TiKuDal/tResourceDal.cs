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
    public class tResourceDal
    {
        public static int Modify(IList<tResourceEntity> objList, string doType,
            string updateFiels, string unupdateFiels)
        {
            string xmlData = PubFun.EntityListToXml(objList, "tResource", "DataSet",
                unupdateFiels, updateFiels, null);

            DBParam[] lstParam = new DBParam[2];
            lstParam[0] = new DBParam("@DoType", doType);
            lstParam[1] = new DBParam("@XmlData", xmlData);
            DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "tResource_Modify",
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

                PubFun.WriteIDListToXml(xw, ids, "tResource", "tResourceID");

                xw.WriteEndElement();
                xw.WriteEndDocument();
            }

            DBParam[] lstParam = new DBParam[2];
            lstParam[0] = new DBParam("@DoType", "delete");
            lstParam[1] = new DBParam("@XmlData", bufXml.ToString());
            DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "tResource_Modify",
                lstParam, DBHelper.ProcRstTypes.All);
            if (rst.Result < 0)
            {
                if (DBHelper.Log != null) DBHelper.Log.Error(rst.Message);
                throw new Exception(rst.Message);
            }
            return rst.Result;
        }

        public static tResourceEntity GettResource(string strResourceCode)
        {
            tResourceEntity rst = null;
            string strSQL = "SELECT * FROM tResource WHERE fResourceCode=@ResourceCode";

            List<DbParameter> lstParam = new List<DbParameter>();
            lstParam.Add(new DBParam("@ResourceCode", strResourceCode));
            DataTable dt = DBHelper.QueryToTable("TiKu", strSQL, lstParam);
            if (dt.Rows.Count > 0)
            {
                rst = new tResourceEntity();
                Trip8H.Common.PubFun.DataRowToObject(dt.Rows[0], rst);
            }
            return rst;
        }

        public static DataTable GetResourceInfo(string strUserName)
        {
            StringBuilder bufSQL = new StringBuilder();
            bufSQL.Append(@"select fUserName,sum(Convert(decimal(18,2),fDateLength)) fDateLength,sum(fSize) fSize,count(*) fCount from tResource 
                            where 1=1 and fStatus not in ('已删除','已销毁') and fType='ClassRoom' ");


            List<DbParameter> lstParam = new List<DbParameter>();
            bufSQL.Append(" and fUserName=@UserName");
            bufSQL.Append(" group by fUserName");
            lstParam.Add(new DBParam("@UserName", strUserName));
            DataTable dt = DBHelper.QueryToTable("TiKu", bufSQL.ToString(), lstParam);
            return dt;
        }

        public static DataTable GetResourceInfoByClassRoomCode(string strClassRoomCode)
        {
            StringBuilder bufSQL = new StringBuilder();
            bufSQL.Append(@"select sum(Convert(decimal(18,2),fDateLength)) fDateLength,sum(fSize) fSize,count(*) fCount from tResource where 1=1
                            and fResourceCode in (select fResourceUrl from tCourse where fClassRoomCode=@ClassRoomCode)");


            List<DbParameter> lstParam = new List<DbParameter>();
            lstParam.Add(new DBParam("@ClassRoomCode", strClassRoomCode));
            DataTable dt = DBHelper.QueryToTable("TiKu", bufSQL.ToString(), lstParam);
            return dt;
        }

        public static List<tResourceEntity> GettResourceList(string strUserName, string strType)
        {
            StringBuilder bufSQL = new StringBuilder();
            List<DbParameter> lstParam = new List<DbParameter>();

            bufSQL.Append("SELECT * FROM tResource WHERE (1=1) and fStatus not in ('已删除','已销毁') ");

            bufSQL.Append(" AND fUserName=@UserName ");
            lstParam.Add(new DBParam("@UserName", strUserName));
            if (!string.IsNullOrEmpty(strType))
            {
                bufSQL.Append(" AND fType=@Type ");
                lstParam.Add(new DBParam("@Type", strType));
            }

            //防止返回数据过多
            if (lstParam.Count <= 0) throw new Exception("没有查询条件");
            DataTable dtRst = DBHelper.QueryToTable("TiKu", bufSQL.ToString(), lstParam);
            List<tResourceEntity> lstRst = PubFun.DataTableToObjects<tResourceEntity>(dtRst);
            return lstRst;
        }

        public static List<tResourceEntity> GetResourceList(string strBeginDate, string strEndDate, string strName)
        {
            StringBuilder bufSQL = new StringBuilder();
            List<DbParameter> lstParam = new List<DbParameter>();

            bufSQL.Append(@"select fCourseTitle,r.* from  tResource r 
                        left join tCourse c on c.fResourceUrl=r.fResourceCode
                     where fCourseTitle like '%'+@Name+'%' 
                        and (c.fUploadDate between @BeginDate and @EndDate or isnull(@BeginDate,'')='')");

            lstParam.Add(new DBParam("@Name", strName));
            lstParam.Add(new DBParam("@BeginDate", strBeginDate));
            lstParam.Add(new DBParam("@EndDate", strEndDate));

            //防止返回数据过多
            if (lstParam.Count <= 0) throw new Exception("没有查询条件");
            DataTable dtRst = DBHelper.QueryToTable("TiKu", bufSQL.ToString(), lstParam);
            List<tResourceEntity> lstRst = PubFun.DataTableToObjects<tResourceEntity>(dtRst);
            return lstRst;
        }

        public static List<tResourceEntity> GetDelResourceList(string strUserName)
        {
            StringBuilder bufSQL = new StringBuilder();
            List<DbParameter> lstParam = new List<DbParameter>();

            bufSQL.Append("SELECT * FROM tResource WHERE (1=1) and fStatus='已删除' ");

            bufSQL.Append(" AND fUserName=@UserName ");
            lstParam.Add(new DBParam("@UserName", strUserName));


            //防止返回数据过多
            if (lstParam.Count <= 0) throw new Exception("没有查询条件");
            DataTable dtRst = DBHelper.QueryToTable("TiKu", bufSQL.ToString(), lstParam);
            List<tResourceEntity> lstRst = PubFun.DataTableToObjects<tResourceEntity>(dtRst);
            return lstRst;
        }

        public static int DeleteResource()
        {
            List<DbParameter> lstParam = new List<DbParameter>();

            DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "DeleteResource",
        lstParam, DBHelper.ProcRstTypes.All);
            return rst.Result;
        }
    }
}
