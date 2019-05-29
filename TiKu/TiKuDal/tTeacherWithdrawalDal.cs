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
    public class tTeacherWithdrawalDal
    {
        public static int Modify(IList<tTeacherWithdrawalEntity> objList, string doType,
            string updateFiels, string unupdateFiels)
        {
            string xmlData = PubFun.EntityListToXml(objList, "tTeacherWithdrawal", "DataSet",
                unupdateFiels, updateFiels, null);

            DBParam[] lstParam = new DBParam[2];
            lstParam[0] = new DBParam("@DoType", doType);
            lstParam[1] = new DBParam("@XmlData", xmlData);
            DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "tTeacherWithdrawal_Modify",
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

                PubFun.WriteIDListToXml(xw, ids, "tTeacherWithdrawal", "tTeacherWithdrawalID");

                xw.WriteEndElement();
                xw.WriteEndDocument();
            }

            DBParam[] lstParam = new DBParam[2];
            lstParam[0] = new DBParam("@DoType", "delete");
            lstParam[1] = new DBParam("@XmlData", bufXml.ToString());
            DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "tTeacherWithdrawal_Modify",
                lstParam, DBHelper.ProcRstTypes.All);
            if (rst.Result < 0)
            {
                if (DBHelper.Log != null) DBHelper.Log.Error(rst.Message);
                throw new Exception(rst.Message);
            }
            return rst.Result;
        }

        public static tTeacherWithdrawalEntity GettTeacherWithdrawal(int id)
        {
            tTeacherWithdrawalEntity rst = null;
            string strSQL = "SELECT * FROM tTeacherWithdrawal WHERE fid=" + id.ToString();
            DataTable dt = DBHelper.QueryToTable("TiKu", strSQL);
            if (dt.Rows.Count > 0)
            {
                rst = new tTeacherWithdrawalEntity();
                Trip8H.Common.PubFun.DataRowToObject(dt.Rows[0], rst);
            }
            return rst;
        }

        public static List<tTeacherWithdrawalEntity> GettTeacherWithdrawalList(string strStatus,string strUserName)
        {
            StringBuilder bufSQL = new StringBuilder();
            List<DbParameter> lstParam = new List<DbParameter>();

            bufSQL.Append("SELECT * FROM tTeacherWithdrawal WHERE (1=1) ");

            if (!string.IsNullOrEmpty(strStatus))
            {
                bufSQL.Append(" AND fStatus=@status ");
                lstParam.Add(new DBParam("@status", strStatus));
            }
            if (!string.IsNullOrEmpty(strUserName))
            {
                bufSQL.Append(" AND fUserName=@username ");
                lstParam.Add(new DBParam("@username", strUserName));
            }



            DataTable dtRst = DBHelper.QueryToTable("TiKu", bufSQL.ToString(), lstParam);
            List<tTeacherWithdrawalEntity> lstRst = PubFun.DataTableToObjects<tTeacherWithdrawalEntity>(dtRst);
            return lstRst;
        }

        public static List<tTeacherWithdrawalEntity> GetWithdrawalList(string strBeginDate, string strEndDate, string strUserName)
        {
            StringBuilder bufSQL = new StringBuilder();
            List<DbParameter> lstParam = new List<DbParameter>();

            bufSQL.Append(@"SELECT a.*,fMobile FROM tTeacherWithdrawal a
                      LEFT JOIN tUser u on a.fUserName=u.fUserName
                     where (fEmail like '%'+@UserName+'%' or fMobile like '%'+@UserName+'%')
                        and (a.fCreateDate between @BeginDate and @EndDate or isnull(@BeginDate,'')='')");

            lstParam.Add(new DBParam("@UserName", strUserName));
            lstParam.Add(new DBParam("@BeginDate", strBeginDate));
            lstParam.Add(new DBParam("@EndDate", strEndDate));

            //防止返回数据过多
            if (lstParam.Count <= 0) throw new Exception("没有查询条件");
            DataTable dtRst = DBHelper.QueryToTable("TiKu", bufSQL.ToString(), lstParam);
            List<tTeacherWithdrawalEntity> lstRst = PubFun.DataTableToObjects<tTeacherWithdrawalEntity>(dtRst);
            return lstRst;
        }

        public static int CheckWithdrawal(int iAmount, string UserName,ref string strMsg)
        {
            List<DbParameter> lstParam = new List<DbParameter>();


            lstParam.Add(new DBParam("@UserName", UserName));
            lstParam.Add(new DBParam("@iAmount", iAmount));

            //防止返回数据过多
            if (lstParam.Count <= 0) throw new Exception("没有查询条件");
            DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "CheckWithdrawal",
        lstParam, DBHelper.ProcRstTypes.All);
            strMsg = rst.Message;
            return rst.Result;
        }


        public static int TeacherWithdrawalAgree(int iWithID, string strApplyNote, string strApplyOpr, DateTime applyDate, string strTransCerd, ref string strMsg)
        {
            List<DbParameter> lstParam = new List<DbParameter>();


            lstParam.Add(new DBParam("@WithID", iWithID));
            lstParam.Add(new DBParam("@ApplyNote", strApplyNote));
            lstParam.Add(new DBParam("@ApplyOpr", strApplyOpr));
            lstParam.Add(new DBParam("@ApplyDate", applyDate));
            lstParam.Add(new DBParam("@TransCerd", strTransCerd));

            //防止返回数据过多
            if (lstParam.Count <= 0) throw new Exception("没有查询条件");
            DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "TeacherWithdrawal_Agree",
        lstParam, DBHelper.ProcRstTypes.All);
            strMsg = rst.Message;
            return rst.Result;
        }
        public static int TeacherWithdrawalRefuse(int iWithID, string strApplyNote, string strApplyOpr, DateTime applyDate, ref string strMsg)
        {
            List<DbParameter> lstParam = new List<DbParameter>();


            lstParam.Add(new DBParam("@WithID", iWithID));
            lstParam.Add(new DBParam("@ApplyNote", strApplyNote));
            lstParam.Add(new DBParam("@ApplyOpr", strApplyOpr));
            lstParam.Add(new DBParam("@ApplyDate", applyDate));

            //防止返回数据过多
            if (lstParam.Count <= 0) throw new Exception("没有查询条件");
            DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "TeacherWithdrawal_Refuse",
        lstParam, DBHelper.ProcRstTypes.All);
            strMsg = rst.Message;
            return rst.Result;
        }
    }
}
