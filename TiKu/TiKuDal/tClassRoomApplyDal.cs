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
    public class tClassRoomApplyDal
    {
        public static int Modify(IList<tClassRoomApplyEntity> objList, string doType,
            string updateFiels, string unupdateFiels)
        {
            string xmlData = PubFun.EntityListToXml(objList, "tClassRoomApply", "DataSet",
                unupdateFiels, updateFiels, null);

            DBParam[] lstParam = new DBParam[2];
            lstParam[0] = new DBParam("@DoType", doType);
            lstParam[1] = new DBParam("@XmlData", xmlData);
            DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "tClassRoomApply_Modify",
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

                PubFun.WriteIDListToXml(xw, ids, "tClassRoomApply", "tClassRoomApplyID");

                xw.WriteEndElement();
                xw.WriteEndDocument();
            }

            DBParam[] lstParam = new DBParam[2];
            lstParam[0] = new DBParam("@DoType", "delete");
            lstParam[1] = new DBParam("@XmlData", bufXml.ToString());
            DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "tClassRoomApply_Modify",
                lstParam, DBHelper.ProcRstTypes.All);
            if (rst.Result < 0)
            {
                if (DBHelper.Log != null) DBHelper.Log.Error(rst.Message);
                throw new Exception(rst.Message);
            }
            return rst.Result;
        }

        public static tClassRoomApplyEntity GettClassRoomApply(int id)
        {
            tClassRoomApplyEntity rst = null;
            string strSQL = "SELECT * FROM tClassRoomApply WHERE fid=" + id.ToString();
            DataTable dt = DBHelper.QueryToTable("TiKu", strSQL);
            if (dt.Rows.Count > 0)
            {
                rst = new tClassRoomApplyEntity();
                Trip8H.Common.PubFun.DataRowToObject(dt.Rows[0], rst);
            }
            return rst;
        }

        public static DataTable GettClassRoomApplyList()
        {
            StringBuilder bufSQL = new StringBuilder();
            List<DbParameter> lstParam = new List<DbParameter>();

            bufSQL.Append(@"select cra.*,fClassRoomTitle from tclassRoomapply  cra
                            left join tClassRoom cr on cr.fClassRoomCode=cra.fClassRoomCode
                            where fApplyDate is null");

            //if (!string.IsNullOrEmpty(strClassRoomCode))
            //{
            //    bufSQL.Append(" AND fClassRoomCode=@ClassRoomCode ");
            //    lstParam.Add(new DBParam("@ClassRoomCode", strClassRoomCode));
            //}

            ////防止返回数据过多
            //if (lstParam.Count <= 0) throw new Exception("没有查询条件");
            DataTable dtRst = DBHelper.QueryToTable("TiKu", bufSQL.ToString(), lstParam);
            return dtRst;
        }

        /// <summary>
        /// 同意申请
        /// </summary>
        /// <param name="iApplyID"></param>
        /// <param name="strApplyNote"></param>
        /// <param name="strApplyOpr"></param>
        /// <param name="applyDate"></param>
        /// <param name="strMsg"></param>
        /// <returns></returns>
        public static int  ClassRoomApplyAgree(int iApplyID,string strApplyNote,string strApplyOpr,DateTime applyDate,ref string strMsg)
        {
            List<DbParameter> lstParam = new List<DbParameter>();


            lstParam.Add(new DBParam("@ApplyID", iApplyID));
            lstParam.Add(new DBParam("@ApplyNote", strApplyNote));
            lstParam.Add(new DBParam("@ApplyOpr", strApplyOpr));
            lstParam.Add(new DBParam("@ApplyDate", applyDate));

            //防止返回数据过多
            if (lstParam.Count <= 0) throw new Exception("没有查询条件");
            DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "ClassRoomApply_Agree",
        lstParam, DBHelper.ProcRstTypes.All);
            strMsg = rst.Message;
            return rst.Result;
        }

        /// <summary>
        /// 拒绝申请
        /// </summary>
        /// <param name="iApplyID"></param>
        /// <param name="strApplyNote"></param>
        /// <param name="strApplyOpr"></param>
        /// <param name="applyDate"></param>
        /// <param name="strMsg"></param>
        /// <returns></returns>
        public static int ClassRoomApplyRefuse(int iApplyID, string strApplyNote, string strApplyOpr, DateTime applyDate, ref string strMsg)
        {
            List<DbParameter> lstParam = new List<DbParameter>();


            lstParam.Add(new DBParam("@ApplyID", iApplyID));
            lstParam.Add(new DBParam("@ApplyNote", strApplyNote));
            lstParam.Add(new DBParam("@ApplyOpr", strApplyOpr));
            lstParam.Add(new DBParam("@ApplyDate", applyDate));

            //防止返回数据过多
            if (lstParam.Count <= 0) throw new Exception("没有查询条件");
            DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "ClassRoomApply_Refuse",
        lstParam, DBHelper.ProcRstTypes.All);
            strMsg = rst.Message;
            return rst.Result;
        }
    }
}
