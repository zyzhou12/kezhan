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
  public class tUserDal
  {
    public static int Modify(IList<tUserEntity> objList, string doType,
        string updateFiels, string unupdateFiels)
    {
      string xmlData = PubFun.EntityListToXml(objList, "tUser", "DataSet",
          unupdateFiels, updateFiels, null);

      DBParam[] lstParam = new DBParam[2];
      lstParam[0] = new DBParam("@DoType", doType);
      lstParam[1] = new DBParam("@XmlData", xmlData);
      DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "tUser_Modify",
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

        PubFun.WriteIDListToXml(xw, ids, "tUser", "tUserID");

        xw.WriteEndElement();
        xw.WriteEndDocument();
      }

      DBParam[] lstParam = new DBParam[2];
      lstParam[0] = new DBParam("@DoType", "delete");
      lstParam[1] = new DBParam("@XmlData", bufXml.ToString());
      DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "tUser_Modify",
          lstParam, DBHelper.ProcRstTypes.All);
      if (rst.Result < 0)
      {
        if (DBHelper.Log != null) DBHelper.Log.Error(rst.Message);
        throw new Exception(rst.Message);
      }
      return rst.Result;
    }

    public static tUserEntity GettUser(string strUserName)
    {
      tUserEntity rst = null;
      List<DbParameter> lstParam = new List<DbParameter>();
      string strSQL = "SELECT * FROM tUser WHERE fUserName=@UserName";
      lstParam.Add(new DBParam("@UserName", strUserName));
      DataTable dt = DBHelper.QueryToTable("TiKu", strSQL, lstParam);
      if (dt.Rows.Count > 0)
      {
        rst = new tUserEntity();
        Trip8H.Common.PubFun.DataRowToObject(dt.Rows[0], rst);
      }
      return rst;
    }


    public static tUserEntity GettUserByMobile(string strMobile)
    {
        tUserEntity rst = null;
        List<DbParameter> lstParam = new List<DbParameter>();
        string strSQL = "SELECT * FROM tUser WHERE fMobile=@Mobile";
        lstParam.Add(new DBParam("@Mobile", strMobile));
        DataTable dt = DBHelper.QueryToTable("TiKu", strSQL, lstParam);
        if (dt.Rows.Count > 0)
        {
            rst = new tUserEntity();
            Trip8H.Common.PubFun.DataRowToObject(dt.Rows[0], rst);
        }
        return rst;
    }

    public static tUserEntity GettUserByOpenID(string strOpenID)
    {
        tUserEntity rst = null;
        List<DbParameter> lstParam = new List<DbParameter>();
        string strSQL = "SELECT * FROM tUser WHERE fOpenID=@OpenID";
        lstParam.Add(new DBParam("@OpenID", strOpenID));
        DataTable dt = DBHelper.QueryToTable("TiKu", strSQL, lstParam);
        if (dt.Rows.Count > 0)
        {
            rst = new tUserEntity();
            Trip8H.Common.PubFun.DataRowToObject(dt.Rows[0], rst);
        }
        return rst;
    }

    public static List<tUserEntity> GetFocusTeacherList(string strUserName)
    {
      StringBuilder bufSQL = new StringBuilder();
      List<DbParameter> lstParam = new List<DbParameter>();

      bufSQL.Append(@"select u.*,fFocusDate from tuserfocus f
                        left join tUser u on u.fUserName=f.fTeacherUser
                        where f.fUserName=@UserName ");

      lstParam.Add(new DBParam("@UserName", strUserName));
      

      //防止返回数据过多
      if (lstParam.Count <= 0) throw new Exception("没有查询条件");
      DataTable dtRst = DBHelper.QueryToTable("TiKu", bufSQL.ToString(), lstParam);
      List<tUserEntity> lstRst = PubFun.DataTableToObjects<tUserEntity>(dtRst);
      return lstRst;
    }
    public static List<tUserEntity> GetUserList(string strBeginDate,string strEndDate,string strUserName)
    {
        StringBuilder bufSQL = new StringBuilder();
        List<DbParameter> lstParam = new List<DbParameter>();

        bufSQL.Append(@" select * from tUser 
                        where (fUserName like '%'+@UserName+'%' or fMobile like '%'+@UserName+'%' or fEmail like '%'+@UserName+'%')
                        and (fCreateDate between @BeginDate and @EndDate or isnull(@BeginDate,'')='') ");

        lstParam.Add(new DBParam("@UserName", strUserName));
        lstParam.Add(new DBParam("@BeginDate", strBeginDate));
        lstParam.Add(new DBParam("@EndDate", strEndDate));


        //防止返回数据过多
        if (lstParam.Count <= 0) throw new Exception("没有查询条件");
        DataTable dtRst = DBHelper.QueryToTable("TiKu", bufSQL.ToString(), lstParam);
        List<tUserEntity> lstRst = PubFun.DataTableToObjects<tUserEntity>(dtRst);
        return lstRst;
    }


    public static int UserSendCode(string strMobile,string strCode,ref string strMsg)
    {
      List<DbParameter> lstParam = new List<DbParameter>();
        lstParam.Add(new DBParam("@Mobile", strMobile));
        lstParam.Add(new DBParam("@Code", strCode));

        DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "User_SendCode", lstParam, DBHelper.ProcRstTypes.AllRst);
       strMsg = rst.Message;
       return rst.Result;
    }

    public static int UserSendEmailCode(string strUserName,string strEmail, string strCode, ref string strMsg)
    {
        List<DbParameter> lstParam = new List<DbParameter>();
        lstParam.Add(new DBParam("@UserName", strUserName));
        lstParam.Add(new DBParam("@Email", strEmail));
        lstParam.Add(new DBParam("@Code", strCode));

        DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "User_SendEmailCode", lstParam, DBHelper.ProcRstTypes.AllRst);
        strMsg = rst.Message;
        return rst.Result;
    }

    public static tUserEntity UserLogin(string strMobile, string strCode,string strRole,string strSystem, ref string strMsg)
    {
      List<DbParameter> lstParam = new List<DbParameter>();
      lstParam.Add(new DBParam("@Mobile", strMobile));
      lstParam.Add(new DBParam("@Code", strCode));
      lstParam.Add(new DBParam("@Role", strRole));
      lstParam.Add(new DBParam("@System", strSystem));

      DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "User_Login", lstParam, DBHelper.ProcRstTypes.All);
      strMsg = rst.Message;

      tUserEntity user = null;
      if (rst.DataSet.Tables.Count > 0)
      {
        List<tUserEntity> lstRst = PubFun.DataTableToObjects<tUserEntity>(rst.DataSet.Tables[0]);
        user = lstRst[0];
      }

      return user;
    }

    public static tUserEntity UserWeiChatLogin(string strOpenID, string strUserName, ref string strMsg)
    {
        List<DbParameter> lstParam = new List<DbParameter>();
        lstParam.Add(new DBParam("@OpenID", strOpenID));
        lstParam.Add(new DBParam("@UserName", strUserName));

        DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "User_WeiChatLogin", lstParam, DBHelper.ProcRstTypes.All);
        strMsg = rst.Message;

        tUserEntity user = null;
        if (rst.DataSet.Tables.Count > 0)
        {
            List<tUserEntity> lstRst = PubFun.DataTableToObjects<tUserEntity>(rst.DataSet.Tables[0]);
            user = lstRst[0];
        }

        return user;
    }

    public static tUserEntity UserPassLogin(string strMobile, string strPass, string strSystem, ref string strMsg)
    {
      List<DbParameter> lstParam = new List<DbParameter>();
      lstParam.Add(new DBParam("@Mobile", strMobile));
      lstParam.Add(new DBParam("@Pass", strPass));
      lstParam.Add(new DBParam("@System", strSystem));

      DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "User_PassLogin", lstParam, DBHelper.ProcRstTypes.All);
      strMsg = rst.Message;

      tUserEntity user = null;
      if (rst.DataSet.Tables.Count > 0)
      {
        List<tUserEntity> lstRst = PubFun.DataTableToObjects<tUserEntity>(rst.DataSet.Tables[0]);
        user = lstRst[0];
      }

      return user;
    }

    public static DataTable GetUserInfoByRole(string strUserName, string strRole, string strInfoType)
    {
      List<DbParameter> lstParam = new List<DbParameter>();
      lstParam.Add(new DBParam("@UserName", strUserName));
      lstParam.Add(new DBParam("@Role", strRole));
      lstParam.Add(new DBParam("@InfoType", strInfoType));

      DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "User_QueryInfo", lstParam, DBHelper.ProcRstTypes.All);

      return rst.DataSet.Tables[0];
    }
  }
}
