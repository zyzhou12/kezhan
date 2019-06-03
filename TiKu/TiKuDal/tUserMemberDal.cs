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
    public class tUserMemberDal
    {
        public static int Modify(IList<tUserMemberEntity> objList, string doType,
            string updateFiels, string unupdateFiels)
        {
            string xmlData = PubFun.EntityListToXml(objList, "tUserMember", "DataSet",
                unupdateFiels, updateFiels, null);

            DBParam[] lstParam = new DBParam[2];
            lstParam[0] = new DBParam("@DoType", doType);
            lstParam[1] = new DBParam("@XmlData", xmlData);
            DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "tUserMember_Modify",
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

                PubFun.WriteIDListToXml(xw, ids, "tUserMember", "tUserMemberID");

                xw.WriteEndElement();
                xw.WriteEndDocument();
            }

            DBParam[] lstParam = new DBParam[2];
            lstParam[0] = new DBParam("@DoType", "delete");
            lstParam[1] = new DBParam("@XmlData", bufXml.ToString());
            DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "tUserMember_Modify",
                lstParam, DBHelper.ProcRstTypes.All);
            if (rst.Result < 0)
            {
                if (DBHelper.Log != null) DBHelper.Log.Error(rst.Message);
                throw new Exception(rst.Message);
            }
            return rst.Result;
        }

        public static tUserMemberEntity GettUserMember(int id)
        {
            tUserMemberEntity rst = null;
            string strSQL = "SELECT * FROM tUserMember WHERE tUserMemberID=" + id.ToString();
            DataTable dt = DBHelper.QueryToTable("TiKu", strSQL);
            if (dt.Rows.Count > 0)
            {
                rst = new tUserMemberEntity();
                Trip8H.Common.PubFun.DataRowToObject(dt.Rows[0], rst);
            }
            return rst;
        }

        public static tUserMemberEntity GetUserMember(string strUserName,string strMobile)
        {
            tUserMemberEntity rst = null;
            string strSQL = @"SELECT * FROM tUserMember um
                             left join tUser u on u.fUserName=um.fMemberUserName WHERE um.fUserName=@UserName and fMobile=@Mobile";
            List<DbParameter> lstParam = new List<DbParameter>();
            lstParam.Add(new DBParam("@UserName", strUserName));
            lstParam.Add(new DBParam("@Mobile", strMobile));
            DataTable dt = DBHelper.QueryToTable("TiKu", strSQL, lstParam);
            if (dt.Rows.Count > 0)
            {
                rst = new tUserMemberEntity();
                Trip8H.Common.PubFun.DataRowToObject(dt.Rows[0], rst);
            }
            return rst;
        }

        //public static List<tUserMemberEntity> GettUserMemberList(string strUserName)
        //{
        //    StringBuilder bufSQL = new StringBuilder();
        //    List<DbParameter> lstParam = new List<DbParameter>();

        //    bufSQL.Append("SELECT * FROM tUserMember WHERE (1=1) ");

        //    if (!string.IsNullOrEmpty(strUserName))
        //    {
        //        bufSQL.Append(" AND fUserName=@UserName ");
        //        lstParam.Add(new DBParam("@UserName", strUserName));
        //    }

        //    //防止返回数据过多
        //    if (lstParam.Count <= 0) throw new Exception("没有查询条件");
        //    DataTable dtRst = DBHelper.QueryToTable("TiKu", bufSQL.ToString(), lstParam);
        //    List<tUserMemberEntity> lstRst = PubFun.DataTableToObjects<tUserMemberEntity>(dtRst);
        //    return lstRst;
        //}

        public static DataTable GettUserMemberList(string strUserName,string strStatus,string strPharse,string strSubject)
        {
            StringBuilder bufSQL = new StringBuilder();
            List<DbParameter> lstParam = new List<DbParameter>();

            bufSQL.Append(@"select distinct u.fNickName+'-'+u.fMobile as MemberName,u.fHeadImg as MemberHead,um.* from tUserMember um
                            left join tUser u on u.fUserName=um.fMemberUserName
                            left join tTeachValid v on v.fUserName=u.fUserName
                            left join tTeacherValidDetail vd on v.fid=vd.fTeacherValidID WHERE (1=1) and vd.fStatus='已审核' and um.fStatus<>5 ");

            if (!string.IsNullOrEmpty(strUserName))
            {
                bufSQL.Append(" AND um.fUserName=@UserName ");
                lstParam.Add(new DBParam("@UserName", strUserName));
            }
            if (!string.IsNullOrEmpty(strStatus))
            {
                bufSQL.Append(" AND um.fStatus=@Status ");
                lstParam.Add(new DBParam("@Status", strStatus));
            }
            if (!string.IsNullOrEmpty(strPharse))
            {
                bufSQL.Append(" AND vd.fPharse<=@Pharse ");
                lstParam.Add(new DBParam("@Pharse", strPharse));
            }
            if (!string.IsNullOrEmpty(strSubject))
            {
                bufSQL.Append(" AND vd.fSubject=@Subject ");
                lstParam.Add(new DBParam("@Subject", strSubject));
            }

            //防止返回数据过多
            if (lstParam.Count <= 0) throw new Exception("没有查询条件");
            DataTable dtRst = DBHelper.QueryToTable("TiKu", bufSQL.ToString(), lstParam);
            return dtRst;
        }


        public static DataTable GetTeacherList(string strUserName, string strPharse, string strSubject)
        {
            StringBuilder bufSQL = new StringBuilder();
            List<DbParameter> lstParam = new List<DbParameter>();

            bufSQL.Append(@"select distinct * from (select u.fNickName+'-'+u.fMobile as MemberName,u.fHeadImg as MemberHead,fMemberUserName fUserName from tUserMember um
                            left join tUser u on u.fUserName=um.fMemberUserName
                            left join tTeachValid v on v.fUserName=u.fUserName
                            inner join tTeacherValidDetail vd on v.fid=vd.fTeacherValidID WHERE (1=1)  
                            AND um.fUserName=@UserName  AND um.fStatus=2 and vd.fStatus='已审核'  AND (vd.fPharse>=@Pharse or @Pharse='')  AND (vd.fSubject=@Subject or @Subject='' or vd.fPharse=4)
                            union all
                            select u.fNickName+'-'+u.fMobile as MemberName,u.fHeadImg as MemberHead,u.fUserName from tUser u 
                            left join tTeachValid v on v.fUserName=u.fUserName
                            inner join tTeacherValidDetail vd on v.fid=vd.fTeacherValidID WHERE (1=1)  
                            AND u.fUserName=@UserName   and vd.fStatus='已审核'  AND (vd.fPharse>=@Pharse or @Pharse='')  AND (vd.fSubject=@Subject or @Subject='' or vd.fPharse=4)  
                            )A");

            lstParam.Add(new DBParam("@UserName", strUserName));
            lstParam.Add(new DBParam("@Pharse", strPharse));
            lstParam.Add(new DBParam("@Subject", strSubject));
           
         

            //防止返回数据过多
            if (lstParam.Count <= 0) throw new Exception("没有查询条件");
            DataTable dtRst = DBHelper.QueryToTable("TiKu", bufSQL.ToString(), lstParam);
            return dtRst;
        }


        public static DataTable GetMemberList(string strUserName)
        {
            StringBuilder bufSQL = new StringBuilder();
            List<DbParameter> lstParam = new List<DbParameter>();

            bufSQL.Append(@"select u.fNickName+'-'+u.fMobile as MemberName,u.fHeadImg as MemberHead,um.* from tUserMember um
                            left join tUser u on u.fUserName=um.fUserName WHERE (1=1) and um.fStatus<>5 ");

            if (!string.IsNullOrEmpty(strUserName))
            {
                bufSQL.Append(" AND um.fMemberUserName=@UserName ");
                lstParam.Add(new DBParam("@UserName", strUserName));
            }

            //防止返回数据过多
            if (lstParam.Count <= 0) throw new Exception("没有查询条件");
            DataTable dtRst = DBHelper.QueryToTable("TiKu", bufSQL.ToString(), lstParam);
            return dtRst;
        }

        public static int UserInviteMember(string strUserName,string strMobile,string strNote,ref string strMsg)
        {
            List<DbParameter> lstParam = new List<DbParameter>();
            lstParam.Add(new DBParam("@UserName", strUserName));
            lstParam.Add(new DBParam("@Mobile", strMobile));
            lstParam.Add(new DBParam("@Note", strNote));

            //防止返回数据过多
            if (lstParam.Count <= 0) throw new Exception("没有查询条件");
            DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "UserInviteMember",
        lstParam, DBHelper.ProcRstTypes.All);
            strMsg = rst.Message;
            return rst.Result;
        }

        public static int InviteAgree(string strUser,int InviteID)
        {
            List<DbParameter> lstParam = new List<DbParameter>();
            lstParam.Add(new DBParam("@UserName", strUser));
            lstParam.Add(new DBParam("@UserMemberID", InviteID));

            //防止返回数据过多
            if (lstParam.Count <= 0) throw new Exception("没有查询条件");
            DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "UserInviteAgree",
        lstParam, DBHelper.ProcRstTypes.All);
            return rst.Result;
        }

        public static int InviteRefused(string strUser, int InviteID)
        {
            List<DbParameter> lstParam = new List<DbParameter>();
            lstParam.Add(new DBParam("@UserName", strUser));
            lstParam.Add(new DBParam("@UserMemberID", InviteID));

            //防止返回数据过多
            if (lstParam.Count <= 0) throw new Exception("没有查询条件");
            DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "UserInviteRefused",
        lstParam, DBHelper.ProcRstTypes.All);
            return rst.Result;
        }


        public static int InviteCancel(string strUser, int InviteID)
        {
            List<DbParameter> lstParam = new List<DbParameter>();
            lstParam.Add(new DBParam("@UserName", strUser));
            lstParam.Add(new DBParam("@UserMemberID", InviteID));

            //防止返回数据过多
            if (lstParam.Count <= 0) throw new Exception("没有查询条件");
            DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "UserInviteCancel",
        lstParam, DBHelper.ProcRstTypes.All);
            return rst.Result;
        }


        public static int InviteRemove(string strUser, int InviteID)
        {
            List<DbParameter> lstParam = new List<DbParameter>();
            lstParam.Add(new DBParam("@UserName", strUser));
            lstParam.Add(new DBParam("@UserMemberID", InviteID));

            //防止返回数据过多
            if (lstParam.Count <= 0) throw new Exception("没有查询条件");
            DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "UserInviteRemove",
        lstParam, DBHelper.ProcRstTypes.All);
            return rst.Result;
        }
    }
}
