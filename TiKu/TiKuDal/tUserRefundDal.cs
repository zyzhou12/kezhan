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
    public class tUserRefundDal
    {
        public static int Modify(IList<tUserRefundEntity> objList, string doType,
            string updateFiels, string unupdateFiels)
        {
            string xmlData = PubFun.EntityListToXml(objList, "tUserRefund", "DataSet",
                unupdateFiels, updateFiels, null);

            DBParam[] lstParam = new DBParam[2];
            lstParam[0] = new DBParam("@DoType", doType);
            lstParam[1] = new DBParam("@XmlData", xmlData);
            DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "tUserRefund_Modify",
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

                PubFun.WriteIDListToXml(xw, ids, "tUserRefund", "tUserRefundID");

                xw.WriteEndElement();
                xw.WriteEndDocument();
            }

            DBParam[] lstParam = new DBParam[2];
            lstParam[0] = new DBParam("@DoType", "delete");
            lstParam[1] = new DBParam("@XmlData", bufXml.ToString());
            DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "tUserRefund_Modify",
                lstParam, DBHelper.ProcRstTypes.All);
            if (rst.Result < 0)
            {
                if (DBHelper.Log != null) DBHelper.Log.Error(rst.Message);
                throw new Exception(rst.Message);
            }
            return rst.Result;
        }

        public static tUserRefundEntity GettUserRefund(int id)
        {
            tUserRefundEntity rst = null;
            string strSQL = @"select fClassRoomTitle,r.* from tUserRefund r 
	left join tBooking b on b.fBookingNo=r.fBookingNO
	left join tClassRoom c on c.fClassRoomCode=b.fTypeCode and b.fType='ClassRoom' WHERE r.fID=@id";

            List<DbParameter> lstParam = new List<DbParameter>();
            lstParam.Add(new DBParam("@id", id));
            DataTable dt = DBHelper.QueryToTable("TiKu", strSQL,lstParam);
            if (dt.Rows.Count > 0)
            {
                rst = new tUserRefundEntity();
                Trip8H.Common.PubFun.DataRowToObject(dt.Rows[0], rst);
            }
            return rst;
        }

        public static tUserRefundEntity GettUserRefundByBookingNo(string strBooking)
        {
            tUserRefundEntity rst = null;
            string strSQL = @"select * from tUserRefund  WHERE fStatus=1 and fBookingNO=@bookingNo";
            List<DbParameter> lstParam = new List<DbParameter>();
            lstParam.Add(new DBParam("@bookingNo", strBooking));
            DataTable dt = DBHelper.QueryToTable("TiKu", strSQL, lstParam);
            if (dt.Rows.Count > 0)
            {
                rst = new tUserRefundEntity();
                Trip8H.Common.PubFun.DataRowToObject(dt.Rows[0], rst);
            }
            return rst;
        }

        public static List<tUserRefundEntity> GettUserRefundList(string strUserName,string strStatus,string strTeacher)
        {
            StringBuilder bufSQL = new StringBuilder();
            List<DbParameter> lstParam = new List<DbParameter>();

            bufSQL.Append(@"select fClassRoomTitle,r.* from tUserRefund r 
	left join tBooking b on b.fBookingNo=r.fBookingNO
	left join tClassRoom c on c.fClassRoomCode=b.fTypeCode and b.fType='ClassRoom' WHERE (1=1) ");

            if (!string.IsNullOrEmpty(strUserName))
            {
                bufSQL.Append(" AND r.fUserName=@UserName ");
                lstParam.Add(new DBParam("@UserName", strUserName));
            }

            if (!string.IsNullOrEmpty(strStatus))
            {
                bufSQL.Append(" AND r.fStatus=@Status ");
                lstParam.Add(new DBParam("@Status", strStatus));
            }
            if (!string.IsNullOrEmpty(strTeacher))
            {
                bufSQL.Append(" AND c.fCreateOpr=@Teacher ");
                lstParam.Add(new DBParam("@Teacher", strTeacher));
            }

            //防止返回数据过多
            if (lstParam.Count <= 0) throw new Exception("没有查询条件");
            DataTable dtRst = DBHelper.QueryToTable("TiKu", bufSQL.ToString(), lstParam);
            List<tUserRefundEntity> lstRst = PubFun.DataTableToObjects<tUserRefundEntity>(dtRst);
            return lstRst;
        }

        /// <summary>
        /// 获取最大可退款金额
        /// </summary>
        /// <param name="BookingNo"></param>
        /// <returns></returns>
        public static decimal GetBokingMaxReturnAmount(string BookingNo)
        {
            List<DbParameter> lstParam = new List<DbParameter>();


            lstParam.Add(new DBParam("@BookingNo", BookingNo));

            //防止返回数据过多
            if (lstParam.Count <= 0) throw new Exception("没有查询条件");
            DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "GetReturnAmount ", lstParam, DBHelper.ProcRstTypes.All);
            decimal dMaxReturnAmount=0;
            if(rst.DataSet.Tables.Count>0){
                dMaxReturnAmount = Convert.ToDecimal(rst.DataSet.Tables[0].Rows[0][0]);
            }
            return dMaxReturnAmount;
        }

        public static int CheckRefund(decimal iAmount, string strBookingNo, ref string strMsg)
        {
            List<DbParameter> lstParam = new List<DbParameter>();


            lstParam.Add(new DBParam("@BookingNo", strBookingNo));
            lstParam.Add(new DBParam("@iAmount", iAmount));

            //防止返回数据过多
            if (lstParam.Count <= 0) throw new Exception("没有查询条件");
            DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "CheckRefund",
        lstParam, DBHelper.ProcRstTypes.All);
            strMsg = rst.Message;
            return rst.Result;
        }

        public static int ConfirmUserRefund(int iRefundID, decimal refundAmount, string strApplyNote, string strApplyOpr, ref string strMsg)
        {
            List<DbParameter> lstParam = new List<DbParameter>();


            lstParam.Add(new DBParam("@iRefundID", iRefundID));
            lstParam.Add(new DBParam("@RefundAmount", refundAmount));
            lstParam.Add(new DBParam("@Note", strApplyNote));
            lstParam.Add(new DBParam("@OprUser", strApplyOpr));

            //防止返回数据过多
            if (lstParam.Count <= 0) throw new Exception("没有查询条件");
            DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "UserRefund_Confirm",
        lstParam, DBHelper.ProcRstTypes.All);
            strMsg = rst.Message;
            return rst.Result;
        }

        public static int UserAmountRefund(int iRefundID,string strBookingNo, decimal refundAmount, string strNote, string strApplyOpr, ref string strMsg)
        {
            List<DbParameter> lstParam = new List<DbParameter>();


            lstParam.Add(new DBParam("@iRefundID", iRefundID));
            lstParam.Add(new DBParam("@BookingNo", strBookingNo));
            lstParam.Add(new DBParam("@RefundAmount", refundAmount));
            lstParam.Add(new DBParam("@Note", strNote));
            lstParam.Add(new DBParam("@OprUser", strApplyOpr));

            //防止返回数据过多
            if (lstParam.Count <= 0) throw new Exception("没有查询条件");
            DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "UserAmount_Refund",
        lstParam, DBHelper.ProcRstTypes.All);
            strMsg = rst.Message;
            return rst.Result;
        }

    }
}
