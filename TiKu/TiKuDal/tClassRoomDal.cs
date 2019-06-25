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
    public class tClassRoomDal
    {
        public static int Modify(IList<tClassRoomEntity> objList, string doType,
            string updateFiels, string unupdateFiels)
        {
            string xmlData = PubFun.EntityListToXml(objList, "tClassRoom", "DataSet",
                unupdateFiels, updateFiels, null);

            DBParam[] lstParam = new DBParam[2];
            lstParam[0] = new DBParam("@DoType", doType);
            lstParam[1] = new DBParam("@XmlData", xmlData);
            DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "tClassRoom_Modify",
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

                PubFun.WriteIDListToXml(xw, ids, "tClassRoom", "tClassRoomID");

                xw.WriteEndElement();
                xw.WriteEndDocument();
            }

            DBParam[] lstParam = new DBParam[2];
            lstParam[0] = new DBParam("@DoType", "delete");
            lstParam[1] = new DBParam("@XmlData", bufXml.ToString());
            DBHelper.ProcRstInfo rst = DBHelper.ExecuteProc("TiKu", "tClassRoom_Modify",
                lstParam, DBHelper.ProcRstTypes.All);
            if (rst.Result < 0)
            {
                if (DBHelper.Log != null) DBHelper.Log.Error(rst.Message);
                throw new Exception(rst.Message);
            }
            return rst.Result;
        }


        public static tClassRoomEntity GettClassRoomByID(int fID)
        {
            tClassRoomEntity rst = null;
            string strSQL = "SELECT * FROM tClassRoom WHERE fID=@ID";
            List<DbParameter> lstParam = new List<DbParameter>();
            lstParam.Add(new DBParam("@ID", fID));
            DataTable dt = DBHelper.QueryToTable("TiKu", strSQL, lstParam);
            if (dt.Rows.Count > 0)
            {
                rst = new tClassRoomEntity();
                Trip8H.Common.PubFun.DataRowToObject(dt.Rows[0], rst);
            }
            return rst;
        }

        public static tClassRoomEntity GettClassRoomByCode(string strClassRoomCode, string strUserName)
        {
            tClassRoomEntity rst = null;
            string strSQL = "SELECT * FROM tClassRoom WHERE fClassRoomCode=@ClassRoomCode";
            List<DbParameter> lstParam = new List<DbParameter>();
            lstParam.Add(new DBParam("@ClassRoomCode", strClassRoomCode));
            if (!string.IsNullOrEmpty(strUserName))
            {
                strSQL += " and fTecharUserName=@UserName";
                lstParam.Add(new DBParam("@UserName", strUserName));
            }
            DataTable dt = DBHelper.QueryToTable("TiKu", strSQL, lstParam);
            if (dt.Rows.Count > 0)
            {
                rst = new tClassRoomEntity();
                Trip8H.Common.PubFun.DataRowToObject(dt.Rows[0], rst);
            }
            return rst;
        }

        public static tClassRoomEntity GettClassRoomByOnLine(string strUserName)
        {
            tClassRoomEntity rst = null;
            string strSQL = "SELECT * FROM tClassRoom WHERE fClassType='OnLine'";
            List<DbParameter> lstParam = new List<DbParameter>();

            if (!string.IsNullOrEmpty(strUserName))
            {
                strSQL += " and fTecharUserName=@UserName";
                lstParam.Add(new DBParam("@UserName", strUserName));
            }
            DataTable dt = DBHelper.QueryToTable("TiKu", strSQL, lstParam);
            if (dt.Rows.Count > 0)
            {
                rst = new tClassRoomEntity();
                Trip8H.Common.PubFun.DataRowToObject(dt.Rows[0], rst);
            }
            return rst;
        }

        public static List<tClassRoomEntity> GettClassRoomListByTeacher(string strTeacher, string strStatus, string strPayType, string strType, string strClassType)
        {
            StringBuilder bufSQL = new StringBuilder();
            List<DbParameter> lstParam = new List<DbParameter>();

            bufSQL.Append(@"select cr.*,fNickName TeacherName,fHeadImg TeacherHead from tClassRoom cr
    left join tUser u on u.fUserName=cr.fTecharUserName WHERE cr.fClassType<>'OnLine' and cr.fType='Live' and cr.fTecharUserName=@UserName ");
            lstParam.Add(new DBParam("@UserName", strTeacher));


            if (!string.IsNullOrEmpty(strPayType))
            {
                bufSQL.Append(" AND cr.fPayType=@PayType ");
                lstParam.Add(new DBParam("@PayType", strPayType));
            }
            if (!string.IsNullOrEmpty(strClassType))
            {
                bufSQL.Append(" AND cr.fClassType=@Type ");
                lstParam.Add(new DBParam("@Type", strClassType));
            }

            if (strType == "未开始")
            {
                bufSQL.Append(" AND ((cr.fClassRoomDate>getdate() AND cr.fStatus='发布')  or cr.fStatus='保存' or cr.fStatus='发布中')");
            }
            else if (strType == "正在上")
            {
                bufSQL.Append(" AND cr.fClassRoomDate<getdate() AND cr.fStatus='发布' ");
            }
            else if (strType == "已结束")
            {
                bufSQL.Append(" AND (cr.fStatus='结算' or cr.fStatus='下线中' or cr.fStatus='下线') ");
            }
            else
            {
                if (!string.IsNullOrEmpty(strStatus))
                {
                    bufSQL.Append(" AND cr.fStatus=@Status ");
                    lstParam.Add(new DBParam("@Status", strStatus));
                }
            }

            //防止返回数据过多
            if (lstParam.Count <= 0) throw new Exception("没有查询条件");
            DataTable dtRst = DBHelper.QueryToTable("TiKu", bufSQL.ToString(), lstParam);
            List<tClassRoomEntity> lstRst = PubFun.DataTableToObjects<tClassRoomEntity>(dtRst);
            return lstRst;
        }


        public static List<tClassRoomEntity> GettClassRoomListByCreateOpr(string strCreate, string strStatus, string strPayType, string strClassType)
        {
            StringBuilder bufSQL = new StringBuilder();
            List<DbParameter> lstParam = new List<DbParameter>();

            bufSQL.Append(@"select cr.*,fNickName TeacherName,fHeadImg TeacherHead from tClassRoom cr
    left join tUser u on u.fUserName=cr.fTecharUserName WHERE cr.fclasstype<>'online' and cr.fCreateOpr=@UserName ");
            lstParam.Add(new DBParam("@UserName", strCreate));


            if (!string.IsNullOrEmpty(strPayType))
            {
                bufSQL.Append(" AND cr.fPayType=@PayType ");
                lstParam.Add(new DBParam("@PayType", strPayType));
            }

            if (!string.IsNullOrEmpty(strClassType))
            {
                bufSQL.Append(" AND cr.fClassType=@Type ");
                lstParam.Add(new DBParam("@Type", strClassType));
            }

          
                if (!string.IsNullOrEmpty(strStatus))
                {
                    bufSQL.Append(" AND cr.fStatus=@Status ");
                    lstParam.Add(new DBParam("@Status", strStatus));
                }
            

            bufSQL.Append(" order by cr.fCreateDate desc ");

            //防止返回数据过多
            if (lstParam.Count <= 0) throw new Exception("没有查询条件");
            DataTable dtRst = DBHelper.QueryToTable("TiKu", bufSQL.ToString(), lstParam);
            List<tClassRoomEntity> lstRst = PubFun.DataTableToObjects<tClassRoomEntity>(dtRst);
            return lstRst;
        }

        public static List<tClassRoomEntity> GetMyClassRoomList(string strUserName,string strClassType, string strType)
        {
            StringBuilder bufSQL = new StringBuilder();
            List<DbParameter> lstParam = new List<DbParameter>();

            bufSQL.Append(@"SELECT cr.*,fNickName TeacherName,fHeadImg TeacherHead FROM tClassRoom cr 
left join tUser u on u.fUserName=cr.fTecharUserName
                      LEFT JOIN tBooking b on cr.fClassRoomCode=b.fTypeCode and b.fType='ClassRoom' and b.fStatus in ('已支付','已驳回') 
WHERE 1=1  ");


            if (!string.IsNullOrEmpty(strUserName))
            {
                bufSQL.Append(" AND b.fUserName=@UserName ");
                lstParam.Add(new DBParam("@UserName", strUserName));
            }
            if (!string.IsNullOrEmpty(strClassType))
            {
                bufSQL.Append(" AND cr.fType=@Type ");
                lstParam.Add(new DBParam("@Type", strClassType));
            }
            if (strType == "未开始")
            {
                bufSQL.Append(" AND ((cr.fClassRoomDate>getdate() AND cr.fStatus='发布')  or cr.fStatus='保存' or cr.fStatus='发布中')");
            }
            else if (strType == "正在上")
            {
                bufSQL.Append(" AND cr.fClassRoomDate<getdate() AND cr.fStatus='发布' ");
            }
            else if (strType == "已结束")
            {
                bufSQL.Append(" AND (cr.fStatus='结算' or cr.fStatus='下线中' or cr.fStatus='下线') ");
            }
            bufSQL.Append(" order by cr.fCreateDate desc ");
            //防止返回数据过多
            if (lstParam.Count <= 0) throw new Exception("没有查询条件");
            DataTable dtRst = DBHelper.QueryToTable("TiKu", bufSQL.ToString(), lstParam);
            List<tClassRoomEntity> lstRst = PubFun.DataTableToObjects<tClassRoomEntity>(dtRst);
            return lstRst;
        }

        public static List<tClassRoomEntity> GettClassRoomListByCity(string strCity, string strStatus)
        {
            StringBuilder bufSQL = new StringBuilder();
            List<DbParameter> lstParam = new List<DbParameter>();

            bufSQL.Append(@"SELECT * FROM tClassRoom cr 
                      LEFT JOIN tUser u on cr.fTecharUserName=u.fUserName WHERE fCity=@City ");
            lstParam.Add(new DBParam("@City", strCity));

            if (!string.IsNullOrEmpty(strStatus))
            {
                bufSQL.Append(" AND cr.fStatus=@Status ");
                lstParam.Add(new DBParam("@Status", strStatus));
            }
            bufSQL.Append(" order by cr.fCreateDate desc ");
            //防止返回数据过多
            if (lstParam.Count <= 0) throw new Exception("没有查询条件");
            DataTable dtRst = DBHelper.QueryToTable("TiKu", bufSQL.ToString(), lstParam);
            List<tClassRoomEntity> lstRst = PubFun.DataTableToObjects<tClassRoomEntity>(dtRst);
            return lstRst;
        }


        public static List<tClassRoomEntity> ClassRoomListQuery(string strBeginDate,string strEndDate,string strClassRoomTitle)
        {
            StringBuilder bufSQL = new StringBuilder();
            List<DbParameter> lstParam = new List<DbParameter>();

            bufSQL.Append(@"SELECT cr.*,u.fName TeacherName FROM tClassRoom cr 
                      LEFT JOIN tUser u on cr.fTecharUserName=u.fUserName 
                     where (fClassRoomTitle like '%'+@ClassRoomTitle+'%' or u.fMobile like '%'+@ClassRoomTitle+'%' or u.fName like '%'+@ClassRoomTitle+'%')
                        and (cr.fCreateDate between @BeginDate and @EndDate or isnull(@BeginDate,'')='') ");

            lstParam.Add(new DBParam("@ClassRoomTitle", strClassRoomTitle));
            lstParam.Add(new DBParam("@BeginDate", strBeginDate));
            lstParam.Add(new DBParam("@EndDate", strEndDate));

            //防止返回数据过多
            if (lstParam.Count <= 0) throw new Exception("没有查询条件");
            DataTable dtRst = DBHelper.QueryToTable("TiKu", bufSQL.ToString(), lstParam);
            List<tClassRoomEntity> lstRst = PubFun.DataTableToObjects<tClassRoomEntity>(dtRst);
            return lstRst;
        }



        public static List<tClassRoomEntity> GettClassRoomListByTeacher(string strTeacherUser, string strStatus)
        {
            StringBuilder bufSQL = new StringBuilder();
            List<DbParameter> lstParam = new List<DbParameter>();

            bufSQL.Append(@"SELECT * FROM tClassRoom cr 
                      LEFT JOIN tUser u on cr.fTecharUserName=u.fUserName WHERE fClassType<>'OnLine' and  fTecharUserName=@TeacherUser ");
            lstParam.Add(new DBParam("@TeacherUser", strTeacherUser));

            if (!string.IsNullOrEmpty(strStatus))
            {
                bufSQL.Append(" AND cr.fStatus=@Status ");
                lstParam.Add(new DBParam("@Status", strStatus));
            }
            bufSQL.Append(" order by cr.fCreateDate desc ");
            //防止返回数据过多
            if (lstParam.Count <= 0) throw new Exception("没有查询条件");
            DataTable dtRst = DBHelper.QueryToTable("TiKu", bufSQL.ToString(), lstParam);
            List<tClassRoomEntity> lstRst = PubFun.DataTableToObjects<tClassRoomEntity>(dtRst);
            return lstRst;
        }
        public static List<tClassRoomEntity> QueryClassRoomList(string strLike)
        {
            StringBuilder bufSQL = new StringBuilder();
            List<DbParameter> lstParam = new List<DbParameter>();

            bufSQL.Append(@"select r.*,fName TeacherName from tclassRoom r
                            left join tuser u on u.fUserName=r.fTecharUserName
                            where fClassType='Public' and fDeadLineDate>getdate() and r.fStatus='发布' and (r.fClassRoomTitle like '%'+@like+'%' or fMobile like '%'+@like+'%' or fNickName like '%'+@like+'%' or fName like '%'+@like+'%')
                            ");
            lstParam.Add(new DBParam("@like", strLike));

           
            //防止返回数据过多
            if (lstParam.Count <= 0) throw new Exception("没有查询条件");
            DataTable dtRst = DBHelper.QueryToTable("TiKu", bufSQL.ToString(), lstParam);
            List<tClassRoomEntity> lstRst = PubFun.DataTableToObjects<tClassRoomEntity>(dtRst);
            return lstRst;
        }


        public static DataTable GettClassRoomList(string strCity, string strPharse, string strGrade, string strSubject)
        {
            List<DbParameter> lstParam = new List<DbParameter>();


            lstParam.Add(new DBParam("@City", strCity));
            lstParam.Add(new DBParam("@Pharse", strPharse));
            lstParam.Add(new DBParam("@Grade", strGrade));
            lstParam.Add(new DBParam("@Subject", strSubject));

            //防止返回数据过多
            if (lstParam.Count <= 0) throw new Exception("没有查询条件");
            DataTable dtRst = DBHelper.ExecuteProc("TiKu", "ClassRoom_Query", lstParam, DBHelper.ProcRstTypes.All).DataSet.Tables[0];

            return dtRst;
        }

        public static DataSet GettClassRoomDetail(string strClassRoomCode, string strUserName)
        {
            List<DbParameter> lstParam = new List<DbParameter>();


            lstParam.Add(new DBParam("@ClassRoomCode", strClassRoomCode));
            lstParam.Add(new DBParam("@UserName", strUserName));

            //防止返回数据过多
            if (lstParam.Count <= 0) throw new Exception("没有查询条件");
            DataSet dtRst = DBHelper.ExecuteProc("TiKu", "ClassRoom_QueryDetail", lstParam, DBHelper.ProcRstTypes.All).DataSet;

            return dtRst;
        }


        public static DataTable GetClassRoomByCourseId(int iCourseID, string strUserName)
        {
            StringBuilder bufSQL = new StringBuilder();
            List<DbParameter> lstParam = new List<DbParameter>();

            bufSQL.Append(@"select isnull(b.fID,0) IsBuy,cr.* from tCourse c
                        left join tClassRoom cr on cr.fClassRoomCode=c.fClassRoomCode
                        left join tbooking b on b.fType='ClassRoom' and b.fStatus in ('已支付','已驳回') and b.fTypeCode=c.fClassRoomCode and fUserName=@UserName
                        where c.fId=@CourseID ");
            lstParam.Add(new DBParam("@CourseID", iCourseID));
            lstParam.Add(new DBParam("@UserName", strUserName));

            //防止返回数据过多
            if (lstParam.Count <= 0) throw new Exception("没有查询条件");
            DataTable dtRst = DBHelper.QueryToTable("TiKu", bufSQL.ToString(), lstParam);
            return dtRst;
        }

        public static List<tMediaEntity> GetCourseMediaList(string strResourceCode)
        {
            StringBuilder bufSQL = new StringBuilder();
            List<DbParameter> lstParam = new List<DbParameter>();

            bufSQL.Append(@"select m.* from tMedia m
left join tResource r on r.fMediaId=m.fMediaId 
where fResourceCode=@ResourceCode ");
            lstParam.Add(new DBParam("@ResourceCode", strResourceCode));

            //防止返回数据过多
            if (lstParam.Count <= 0) throw new Exception("没有查询条件");
            DataTable dtRst = DBHelper.QueryToTable("TiKu", bufSQL.ToString(), lstParam);
            List<tMediaEntity> lstRst = PubFun.DataTableToObjects<tMediaEntity>(dtRst);
            return lstRst;
        }

        public static decimal GetClassRoomFlow(int iCourseID)
        {
            List<DbParameter> lstParam = new List<DbParameter>();

            lstParam.Add(new DBParam("@iCourseID", iCourseID));

            //防止返回数据过多
            if (lstParam.Count <= 0) throw new Exception("没有查询条件");
            object dtRst = DBHelper.ExecuteProc("TiKu", "ClassRoomPrice_Query", lstParam, DBHelper.ProcRstTypes.All).Value;

            return Convert.ToDecimal(dtRst);
        }

        public static int ClassRoomCoursePay(int iCourseID, string strOprUser, string strSystem)
        {
            List<DbParameter> lstParam = new List<DbParameter>();

            lstParam.Add(new DBParam("@iCourseID", iCourseID));
            lstParam.Add(new DBParam("@OprUser", strOprUser));
            lstParam.Add(new DBParam("@System", strSystem));

            //防止返回数据过多
            if (lstParam.Count <= 0) throw new Exception("没有查询条件");
            int dtRst = DBHelper.ExecuteProc("TiKu", "ClassRoomCourse_Pay", lstParam, DBHelper.ProcRstTypes.All).Result;

            return dtRst;
        }

        public static int OnLineCoursePay(int iCourseID, string strOprUser)
        {
            List<DbParameter> lstParam = new List<DbParameter>();

            lstParam.Add(new DBParam("@iCourseID", iCourseID));
            lstParam.Add(new DBParam("@OprUser", strOprUser));

            //防止返回数据过多
            if (lstParam.Count <= 0) throw new Exception("没有查询条件");
            int dtRst = DBHelper.ExecuteProc("TiKu", "OnLineCourse_Pay", lstParam, DBHelper.ProcRstTypes.All).Result;

            return dtRst;
        }

        public static int OnLineCourseOwePay(int iOweID, string strOprUser)
        {
            List<DbParameter> lstParam = new List<DbParameter>();

            lstParam.Add(new DBParam("@OweID", iOweID));
            lstParam.Add(new DBParam("@OprUser", strOprUser));

            //防止返回数据过多
            if (lstParam.Count <= 0) throw new Exception("没有查询条件");
            int dtRst = DBHelper.ExecuteProc("TiKu", "OnLineCourse_LeftOwePay", lstParam, DBHelper.ProcRstTypes.All).Result;

            return dtRst;
        }

        public static int ClassRoomSettlement(string strClassRoomCode, string strOprUser, ref string strMsg)
        {
            List<DbParameter> lstParam = new List<DbParameter>();

            lstParam.Add(new DBParam("@ClassRoomCode", strClassRoomCode));
            lstParam.Add(new DBParam("@Opr", strOprUser));

            //防止返回数据过多
            if (lstParam.Count <= 0) throw new Exception("没有查询条件");
            DBHelper.ProcRstInfo ds = DBHelper.ExecuteProc("TiKu", "ClassRoom_Settlement", lstParam, DBHelper.ProcRstTypes.All);
            strMsg = ds.Message;
            return ds.Result;
        }

        public static int ClassRoomRecordedSettlement(string strClassRoomCode, string strOprUser, ref string strMsg)
        {
            List<DbParameter> lstParam = new List<DbParameter>();

            lstParam.Add(new DBParam("@ClassRoomCode", strClassRoomCode));
            lstParam.Add(new DBParam("@Opr", strOprUser));

            //防止返回数据过多
            if (lstParam.Count <= 0) throw new Exception("没有查询条件");
            DBHelper.ProcRstInfo ds = DBHelper.ExecuteProc("TiKu", "ClassRoom_RecordedSettlement", lstParam, DBHelper.ProcRstTypes.All);
            strMsg = ds.Message;
            return ds.Result;
        }


        //    public static decimal GetClassRoomFlootPrice(string strClassRoomCode)
        //    {
        //        string strSql = @"select sum(fClassDateLength)*max(fClassFee)*sum(cr.fMaxNumber) from tCourse c
        //                        left join tClassRoom cr on cr.fClassRoomCode=c.fClassRoomCode
        //                        left join tuser u on u.fUserName=cr.fTecharUserName
        //                        left join tSystemConfig sc on sc.fCity=u.fCity
        //                        where c.fClassRoomCode=@ClassRoomCode";
        //        List<DbParameter> lstParam = new List<DbParameter>();
        //        lstParam.Add(new DBParam("@ClassRoomCode", strClassRoomCode));

        //        object dtRst = DBHelper.ExecuteScalar("TiKu", strSql, lstParam);

        //        return Convert.ToDecimal(dtRst);
        //    }
    }
}
