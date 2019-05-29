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
    public class tClassRoomAccountDal
    {
        public static int Modify(IList<tClassRoomAccountEntity> objList, string doType,
            string updateFiels, string unupdateFiels)
        {
            string xmlData = PubFun.EntityListToXml(objList, "tClassRoomAccount", "DataSet",
                unupdateFiels, updateFiels, null);

            DBParam[] lstParam = new DBParam[2];
            lstParam[0] = new DBParam("@DoType", doType);
            lstParam[1] = new DBParam("@XmlData", xmlData);
            DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "tClassRoomAccount_Modify",
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

                PubFun.WriteIDListToXml(xw, ids, "tClassRoomAccount", "tClassRoomAccountID");

                xw.WriteEndElement();
                xw.WriteEndDocument();
            }

            DBParam[] lstParam = new DBParam[2];
            lstParam[0] = new DBParam("@DoType", "delete");
            lstParam[1] = new DBParam("@XmlData", bufXml.ToString());
            DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "tClassRoomAccount_Modify",
                lstParam, DBHelper.ProcRstTypes.All);
            if (rst.Result < 0)
            {
                if (DBHelper.Log != null) DBHelper.Log.Error(rst.Message);
                throw new Exception(rst.Message);
            }
            return rst.Result;
        }

        public static tClassRoomAccountEntity GettClassRoomAccount(int id)
        {
            tClassRoomAccountEntity rst = null;
            string strSQL = "SELECT * FROM tClassRoomAccount WHERE tClassRoomAccountID=" + id.ToString();
            DataTable dt = DBHelper.QueryToTable("TiKu", strSQL);
            if (dt.Rows.Count > 0)
            {
                rst = new tClassRoomAccountEntity();
                Trip8H.Common.PubFun.DataRowToObject(dt.Rows[0], rst);
            }
            return rst;
        }

        public static List<tClassRoomAccountEntity> GettClassRoomAccountList(int id)
        {
            StringBuilder bufSQL = new StringBuilder();
            List<DbParameter> lstParam = new List<DbParameter>();

            bufSQL.Append("SELECT * FROM tClassRoomAccount WHERE (1=1) ");

            if (id > 0)
            {
                bufSQL.Append(" AND tClassRoomAccountID=@id ");
                lstParam.Add(new DBParam("@id", id));
            }

            //防止返回数据过多
            if (lstParam.Count <= 0) throw new Exception("没有查询条件");
            DataTable dtRst = DBHelper.QueryToTable("TiKu", bufSQL.ToString(), lstParam);
            List<tClassRoomAccountEntity> lstRst = PubFun.DataTableToObjects<tClassRoomAccountEntity>(dtRst);
            return lstRst;
        }

        public static List<tClassRoomAccountEntity> GettClassRoomAccountList(string strBeginDate, string strEndDate, string strUserName)
        {
            StringBuilder bufSQL = new StringBuilder();
            List<DbParameter> lstParam = new List<DbParameter>();

            bufSQL.Append(@"SELECT a.*,cr.fClassRoomTitle FROM tClassRoomAccount a
                      LEFT JOIN tClassRoom cr on cr.fClassRoomCode=a.fClassRoomCode
                      LEFT JOIN tUser u on cr.fTecharUserName=u.fUserName
                     where (fClassRoomTitle like '%'+@UserName+'%' or fMobile like '%'+@UserName+'%')
                        and (a.fCreateDate between @BeginDate and @EndDate or isnull(@BeginDate,'')='')");

            lstParam.Add(new DBParam("@UserName", strUserName));
            lstParam.Add(new DBParam("@BeginDate", strBeginDate));
            lstParam.Add(new DBParam("@EndDate", strEndDate));

            //防止返回数据过多
            if (lstParam.Count <= 0) throw new Exception("没有查询条件");
            DataTable dtRst = DBHelper.QueryToTable("TiKu", bufSQL.ToString(), lstParam);
            List<tClassRoomAccountEntity> lstRst = PubFun.DataTableToObjects<tClassRoomAccountEntity>(dtRst);
            return lstRst;
        }

    }
}
