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
    public class tFlowStoredDal
    {
        public static int Modify(IList<tFlowStoredEntity> objList, string doType,
            string updateFiels, string unupdateFiels)
        {
            string xmlData = PubFun.EntityListToXml(objList, "tFlowStored", "DataSet",
                unupdateFiels, updateFiels, null);

            DBParam[] lstParam = new DBParam[2];
            lstParam[0] = new DBParam("@DoType", doType);
            lstParam[1] = new DBParam("@XmlData", xmlData);
            DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "tFlowStored_Modify",
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

                PubFun.WriteIDListToXml(xw, ids, "tFlowStored", "tFlowStoredID");

                xw.WriteEndElement();
                xw.WriteEndDocument();
            }

            DBParam[] lstParam = new DBParam[2];
            lstParam[0] = new DBParam("@DoType", "delete");
            lstParam[1] = new DBParam("@XmlData", bufXml.ToString());
            DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "tFlowStored_Modify",
                lstParam, DBHelper.ProcRstTypes.All);
            if (rst.Result < 0)
            {
                if (DBHelper.Log != null) DBHelper.Log.Error(rst.Message);
                throw new Exception(rst.Message);
            }
            return rst.Result;
        }


        public static Decimal GetUserFlowAccount(string strUserName)
        {
            string strSql = @"select isnull(sum(fLeftNum),0) from tFlowStored 
                        where fUserName=@UserName and fStatus=1 and fEffectDate>dateadd(day,-1,getdate()) ";
            List<DbParameter> lstParam = new List<DbParameter>();
            lstParam.Add(new DBParam("@UserName", strUserName));

            object obj = DBHelper.ExecuteScalar("TiKu", strSql, lstParam);
            return Convert.ToDecimal(obj);
        }

        public static Decimal GetUserFlowOwe(string strUserName)
        {
            string strSql = @"select isnull(sum(fLeftOwe),0) from tuserowe where fUserName=@UserName";
            List<DbParameter> lstParam = new List<DbParameter>();
            lstParam.Add(new DBParam("@UserName", strUserName));

            object obj = DBHelper.ExecuteScalar("TiKu", strSql, lstParam);
            return Convert.ToDecimal(obj);
        }

        public static tFlowStoredEntity GettFlowStored(string strStoredNo)
        {
            tFlowStoredEntity rst = null;
            string strSQL = "SELECT * FROM tFlowStored WHERE fStoredNo=@StoredNo";
            List<DbParameter> lstParam = new List<DbParameter>();
            lstParam.Add(new DBParam("@StoredNo", strStoredNo));
            DataTable dt = DBHelper.QueryToTable("TiKu", strSQL, lstParam);
            if (dt.Rows.Count > 0)
            {
                rst = new tFlowStoredEntity();
                Trip8H.Common.PubFun.DataRowToObject(dt.Rows[0], rst);
            }
            return rst;
        }

        public static List<tFlowStoredEntity> GettFlowStoredList(int id)
        {
            StringBuilder bufSQL = new StringBuilder();
            List<DbParameter> lstParam = new List<DbParameter>();

            bufSQL.Append("SELECT * FROM tFlowStored WHERE (1=1) ");

            if (id > 0)
            {
                bufSQL.Append(" AND tFlowStoredID=@id ");
                lstParam.Add(new DBParam("@id", id));
            }

            //防止返回数据过多
            if (lstParam.Count <= 0) throw new Exception("没有查询条件");
            DataTable dtRst = DBHelper.QueryToTable("TiKu", bufSQL.ToString(), lstParam);
            List<tFlowStoredEntity> lstRst = PubFun.DataTableToObjects<tFlowStoredEntity>(dtRst);
            return lstRst;
        }

        public static List<tFlowStoredEntity> GettFlowStoredList(string strBeginDate, string strEndDate, string strUserName)
        {
            StringBuilder bufSQL = new StringBuilder();
            List<DbParameter> lstParam = new List<DbParameter>();

            bufSQL.Append(@"SELECT a.*,fMobile FROM tFlowStored a
                      LEFT JOIN tUser u on a.fUserName=u.fUserName
                     where (fEmail like '%'+@UserName+'%' or fMobile like '%'+@UserName+'%')
                        and (a.fCreateDate between @BeginDate and @EndDate or isnull(@BeginDate,'')='')");

            lstParam.Add(new DBParam("@UserName", strUserName));
            lstParam.Add(new DBParam("@BeginDate", strBeginDate));
            lstParam.Add(new DBParam("@EndDate", strEndDate));

            //防止返回数据过多
            if (lstParam.Count <= 0) throw new Exception("没有查询条件");
            DataTable dtRst = DBHelper.QueryToTable("TiKu", bufSQL.ToString(), lstParam);
            List<tFlowStoredEntity> lstRst = PubFun.DataTableToObjects<tFlowStoredEntity>(dtRst);
            return lstRst;
        }

        public static int FlowAdjust(string strMobile, int iFlow, DateTime effectDate, string strUserName, string strNote, ref string strMsg)
        {
            List<DbParameter> lstParam = new List<DbParameter>();
            lstParam.Add(new DBParam("@Mobile", strMobile));
            lstParam.Add(new DBParam("@iFlow", iFlow));
            lstParam.Add(new DBParam("@EffectDate", effectDate));
            lstParam.Add(new DBParam("@Note", strNote));
            lstParam.Add(new DBParam("@OprUser", strUserName));

            //防止返回数据过多
            if (lstParam.Count <= 0) throw new Exception("没有查询条件");
            DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "UserFlowAdjust",
        lstParam, DBHelper.ProcRstTypes.All);
            strMsg = rst.Message;
            return rst.Result;
        }

        public static int CheckFlowEffect()
        {
            List<DbParameter> lstParam = new List<DbParameter>();
          
            DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "CheckFlowEffect",
        lstParam, DBHelper.ProcRstTypes.All);
            return rst.Result;
        }

    }
}
