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
    public class tGroupUserInfoDal
    {
        public static int Modify(IList<tGroupUserInfoEntity> objList, string doType,
            string updateFiels, string unupdateFiels)
        {
            string xmlData = PubFun.EntityListToXml(objList, "tGroupUserInfo", "DataSet",
                unupdateFiels, updateFiels, null);

            DBParam[] lstParam = new DBParam[2];
            lstParam[0] = new DBParam("@DoType", doType);
            lstParam[1] = new DBParam("@XmlData", xmlData);
            DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "tGroupUserInfo_Modify",
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

                PubFun.WriteIDListToXml(xw, ids, "tGroupUserInfo", "fID");

                xw.WriteEndElement();
                xw.WriteEndDocument();
            }

            DBParam[] lstParam = new DBParam[2];
            lstParam[0] = new DBParam("@DoType", "delete");
            lstParam[1] = new DBParam("@XmlData", bufXml.ToString());
            DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "tGroupUserInfo_Modify",
                lstParam, DBHelper.ProcRstTypes.All);
            if (rst.Result < 0)
            {
                if (DBHelper.Log != null) DBHelper.Log.Error(rst.Message);
                throw new Exception(rst.Message);
            }
            return rst.Result;
        }

        public static tGroupUserInfoEntity GettGroupUserInfo(string strUserId,string strGroupId)
        {
            tGroupUserInfoEntity rst = null;
            List<DbParameter> lstParam = new List<DbParameter>();
            string strSQL = @"SELECT isnull(fNickName,gu.fUserName) fNickName,fHeadImg,gu.* FROM tGroupUserInfo gu
                                left join tuser u on u.fUserName=gu.fUserId WHERE fUserId=@UserId and fGroupID=@GroupId ";

            lstParam.Add(new DBParam("@UserId", strUserId));
            lstParam.Add(new DBParam("@GroupId", strGroupId));
            DataTable dt = DBHelper.QueryToTable("TiKu", strSQL,lstParam);
            if (dt.Rows.Count > 0)
            {
                rst = new tGroupUserInfoEntity();
                Trip8H.Common.PubFun.DataRowToObject(dt.Rows[0], rst);
            }
            return rst;
        }


        public static List<tGroupUserInfoEntity> GetGroupUserList(string strGroupId)
        {
            List<DbParameter> lstParam = new List<DbParameter>();
            string strSQL = @"SELECT fNickName,fHeadImg,gu.* FROM tGroupUserInfo gu
                                left join tuser u on u.fUserName=gu.fUserId
                                 WHERE fGroupID=@GroupId 
                                order by frole desc,fisonline desc,fispush desc";

            lstParam.Add(new DBParam("@GroupId", strGroupId));
            DataTable dt = DBHelper.QueryToTable("TiKu", strSQL, lstParam);
            List<tGroupUserInfoEntity> lstRst = PubFun.DataTableToObjects<tGroupUserInfoEntity>(dt);
            return lstRst;
        }

        public static DataTable GetGroupUserListInfo(string strGroupId)
        {
            List<DbParameter> lstParam = new List<DbParameter>();
            string strSQL = @"SELECT fNickName,fHeadImg,gu.* FROM tGroupUserInfo gu
                                left join tuser u on u.fUserName=gu.fUserId
                                 WHERE fGroupID=@GroupId 
                                order by frole desc,fisonline desc,fispush desc";

            lstParam.Add(new DBParam("@GroupId", strGroupId));
            DataTable dt = DBHelper.QueryToTable("TiKu", strSQL,lstParam);
           
            return dt;
        }

        public static List<tGroupUserInfoEntity> GettGroupUserInfoList(int id)
        {
            StringBuilder bufSQL = new StringBuilder();
            List<DbParameter> lstParam = new List<DbParameter>();

            bufSQL.Append("SELECT * FROM tGroupUserInfo WHERE (1=1) ");

            if (id > 0)
            {
                bufSQL.Append(" AND tGroupUserInfoID=@id ");
                lstParam.Add(new DBParam("@id", id));
            }

            //防止返回数据过多
            if (lstParam.Count <= 0) throw new Exception("没有查询条件");
            DataTable dtRst = DBHelper.QueryToTable("TiKu", bufSQL.ToString(), lstParam);
            List<tGroupUserInfoEntity> lstRst = PubFun.DataTableToObjects<tGroupUserInfoEntity>(dtRst);
            return lstRst;
        }

    }
}
