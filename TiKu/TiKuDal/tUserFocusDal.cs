using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using Trip8H.Common;
using TuYou.DBUtility;

namespace TiKu.Dal
{
    public class tUserFocusDal
    {
        public static int Modify(IList<tUserFocusEntity> objList, string doType,
            string updateFiels, string unupdateFiels)
        {
            string xmlData = PubFun.EntityListToXml(objList, "tUserFocus", "DataSet",
                unupdateFiels, updateFiels, null);

            DBParam[] lstParam = new DBParam[2];
            lstParam[0] = new DBParam("@DoType", doType);
            lstParam[1] = new DBParam("@XmlData", xmlData);
            DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "tUserFocus_Modify",
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

                PubFun.WriteIDListToXml(xw, ids, "tUserFocus", "tUserFocusID");

                xw.WriteEndElement();
                xw.WriteEndDocument();
            }

            DBParam[] lstParam = new DBParam[2];
            lstParam[0] = new DBParam("@DoType", "delete");
            lstParam[1] = new DBParam("@XmlData", bufXml.ToString());
            DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "tUserFocus_Modify",
                lstParam, DBHelper.ProcRstTypes.All);
            if (rst.Result < 0)
            {
                if (DBHelper.Log != null) DBHelper.Log.Error(rst.Message);
                throw new Exception(rst.Message);
            }
            return rst.Result;
        }

        public static tUserFocusEntity GettUserFocus(int id)
        {
            tUserFocusEntity rst = null;
            string strSQL = "SELECT * FROM tUserFocus WHERE tUserFocusID=" + id.ToString();
            DataTable dt = DBHelper.QueryToTable("TiKu", strSQL);
            if (dt.Rows.Count > 0)
            {
                rst = new tUserFocusEntity();
                Trip8H.Common.PubFun.DataRowToObject(dt.Rows[0], rst);
            }
            return rst;
        }

        public static List<tUserFocusEntity> GettUserFocusList(int id)
        {
            StringBuilder bufSQL = new StringBuilder();
            List<DbParameter> lstParam = new List<DbParameter>();

            bufSQL.Append("SELECT * FROM tUserFocus WHERE (1=1) ");

            if (id > 0)
            {
                bufSQL.Append(" AND tUserFocusID=@id ");
                lstParam.Add(new DBParam("@id", id));
            }

            //防止返回数据过多
            if (lstParam.Count <= 0) throw new Exception("没有查询条件");
            DataTable dtRst = DBHelper.QueryToTable("TiKu", bufSQL.ToString(), lstParam);
            List<tUserFocusEntity> lstRst = PubFun.DataTableToObjects<tUserFocusEntity>(dtRst);
            return lstRst;
        }

        public static int TeacherFocus(string strUserName, string strTeacherUser, bool IsFocus)
        {
            List<DbParameter> lstParam = new List<DbParameter>();


            lstParam.Add(new DBParam("@UserName", strUserName));
            lstParam.Add(new DBParam("@TeacherUser", strTeacherUser));
            lstParam.Add(new DBParam("@IsFocus", IsFocus));

            //防止返回数据过多
            if (lstParam.Count <= 0) throw new Exception("没有查询条件");
            DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "User_Focus",
        lstParam, DBHelper.ProcRstTypes.All);
            return rst.Result;
        }

    }
}
