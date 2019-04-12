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
    public class tTeacherValidDetailDal
    {
        public static int Modify(IList<tTeacherValidDetailEntity> objList, string doType,
            string updateFiels, string unupdateFiels)
        {
            string xmlData = PubFun.EntityListToXml(objList, "tTeacherValidDetail", "DataSet",
                unupdateFiels, updateFiels, null);

            DBParam[] lstParam = new DBParam[2];
            lstParam[0] = new DBParam("@DoType", doType);
            lstParam[1] = new DBParam("@XmlData", xmlData);
            DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "tTeacherValidDetail_Modify",
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

                PubFun.WriteIDListToXml(xw, ids, "tTeacherValidDetail", "tTeacherValidDetailID");

                xw.WriteEndElement();
                xw.WriteEndDocument();
            }

            DBParam[] lstParam = new DBParam[2];
            lstParam[0] = new DBParam("@DoType", "delete");
            lstParam[1] = new DBParam("@XmlData", bufXml.ToString());
            DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "tTeacherValidDetail_Modify",
                lstParam, DBHelper.ProcRstTypes.All);
            if (rst.Result < 0)
            {
                if (DBHelper.Log != null) DBHelper.Log.Error(rst.Message);
                throw new Exception(rst.Message);
            }
            return rst.Result;
        }

        public static tTeacherValidDetailEntity GettTeacherValidDetail(int id)
        {
            tTeacherValidDetailEntity rst = null;
            string strSQL = "SELECT * FROM tTeacherValidDetail WHERE fID=" + id.ToString();
            DataTable dt = DBHelper.QueryToTable("TiKu", strSQL);
            if (dt.Rows.Count > 0)
            {
                rst = new tTeacherValidDetailEntity();
                Trip8H.Common.PubFun.DataRowToObject(dt.Rows[0], rst);
            }
            return rst;
        }

        public static List<tTeacherValidDetailEntity> GettTeacherValidDetailList(string strUserName,string strStatus)
        {
            StringBuilder bufSQL = new StringBuilder();
            List<DbParameter> lstParam = new List<DbParameter>();

            bufSQL.Append(@"select vd.* from tTeacherValidDetail vd
                            left join tTeachValid v on v.fID=vd.fTeacherValidID
                            where (1=1) ");

            if (!string.IsNullOrEmpty(strUserName))
            {
                bufSQL.Append(" AND v.fUserName=@UserName ");
                lstParam.Add(new DBParam("@UserName", strUserName));
            }
            if (!string.IsNullOrEmpty(strStatus))
            {
                bufSQL.Append(" AND vd.fStatus=@Status ");
                lstParam.Add(new DBParam("@Status", strStatus));
            }

            //防止返回数据过多
            if (lstParam.Count <= 0) throw new Exception("没有查询条件");
            DataTable dtRst = DBHelper.QueryToTable("TiKu", bufSQL.ToString(), lstParam);
            List<tTeacherValidDetailEntity> lstRst = PubFun.DataTableToObjects<tTeacherValidDetailEntity>(dtRst);
            return lstRst;
        }

        public static List<tTeacherValidDetailEntity> GettTeacherValidDetailList(int iValidID)
        {
            StringBuilder bufSQL = new StringBuilder();
            List<DbParameter> lstParam = new List<DbParameter>();

            bufSQL.Append(@"select * from tTeacherValidDetail 
                            where (1=1) ");

          
                bufSQL.Append(" AND fTeacherValidID=@ValidID ");
                lstParam.Add(new DBParam("@ValidID", iValidID));
           

            //防止返回数据过多
            if (lstParam.Count <= 0) throw new Exception("没有查询条件");
            DataTable dtRst = DBHelper.QueryToTable("TiKu", bufSQL.ToString(), lstParam);
            List<tTeacherValidDetailEntity> lstRst = PubFun.DataTableToObjects<tTeacherValidDetailEntity>(dtRst);
            return lstRst;
        }
    }
}
