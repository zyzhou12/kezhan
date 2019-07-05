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
    public class tGroupDal
    {
        public static int Modify(IList<tGroupEntity> objList, string doType,
            string updateFiels, string unupdateFiels)
        {
            string xmlData = PubFun.EntityListToXml(objList, "tGroup", "DataSet",
                unupdateFiels, updateFiels, null);

            DBParam[] lstParam = new DBParam[2];
            lstParam[0] = new DBParam("@DoType", doType);
            lstParam[1] = new DBParam("@XmlData", xmlData);
            DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "tGroup_Modify",
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

                PubFun.WriteIDListToXml(xw, ids, "tGroup", "tGroupID");

                xw.WriteEndElement();
                xw.WriteEndDocument();
            }

            DBParam[] lstParam = new DBParam[2];
            lstParam[0] = new DBParam("@DoType", "delete");
            lstParam[1] = new DBParam("@XmlData", bufXml.ToString());
            DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "tGroup_Modify",
                lstParam, DBHelper.ProcRstTypes.All);
            if (rst.Result < 0)
            {
                if (DBHelper.Log != null) DBHelper.Log.Error(rst.Message);
                throw new Exception(rst.Message);
            }
            return rst.Result;
        }

        public static tGroupEntity GettGroup(string id)
        {
            tGroupEntity rst = null;
            string strSQL = "SELECT * FROM tGroup WHERE fGroupID='" + id+"'";
            DataTable dt = DBHelper.QueryToTable("TiKu", strSQL);
            if (dt.Rows.Count > 0)
            {
                rst = new tGroupEntity();
                Trip8H.Common.PubFun.DataRowToObject(dt.Rows[0], rst);
            }
            return rst;
        }

        public static DataTable GettGroupList(string strBeginDate, string strEndDate,string strUserName)
        {
            StringBuilder bufSQL = new StringBuilder();
            List<DbParameter> lstParam = new List<DbParameter>();

            bufSQL.Append(@" select g.*,
(select count(*) from tgroupuserinfo where fgroupid=g.fgroupid) fUserCount,
( select sum(datediff(ss,fJoinTime,fQuitTime)) from tGroupUserJoinHistory where fgroupid=g.fgroupid) fSumLength
 from tGroup g
                            left join tuser u on u.fUserName=g.fTeacherId
                        where (fTeacherId like '%'+@UserName+'%' or fGroupName like '%'+@UserName+'%' or fMobile like '%'+@UserName+'%' or fEmail like '%'+@UserName+'%')
                        and (g.fCreateDate between @BeginDate and @EndDate or isnull(@BeginDate,'')='') ");

            lstParam.Add(new DBParam("@UserName", strUserName));
            lstParam.Add(new DBParam("@BeginDate", strBeginDate));
            lstParam.Add(new DBParam("@EndDate", strEndDate));


            //防止返回数据过多
            if (lstParam.Count <= 0) throw new Exception("没有查询条件");
            DataTable dtRst = DBHelper.QueryToTable("TiKu", bufSQL.ToString(), lstParam);
            
            return dtRst;
        }

    }
}
