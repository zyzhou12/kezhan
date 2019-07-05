using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiKu.Api.Code;
using TiKu.Api.Models;
using TiKu.Bll;
using TiKuBll;
using TiKuBll.Model;

namespace TiKu.Api.Controllers
{
    public class ClassRoomController : Controller
    {
        //
        // GET: /ClassRoom/

        public JsonResult GetClassRoomByUser(string strOpenid)
        {
            UserInfoModel user = UserBll.GettUserByOpenID(strOpenid);
            CourseListModel model = ClassRoomBll.GetMyCourseList(user.fUserName);

            JsonResult jr = new JsonResult();
            jr.Data = model;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }

        public JsonResult JoinClass(int iCourseID, string strUserName)
        {
            ResponseOnLineClassModel response = new ResponseOnLineClassModel();
            ClassRoomModel cr = ClassRoomBll.GetClassRoomByCourseId(iCourseID, strUserName);
            try
            {
                CourseModel course = ClassRoomBll.GetCourseByID(iCourseID);
                string strRole = "0";
                if (course != null && course.fClassDate.AddMinutes(course.fClassDateLength) > DateTime.Now)
                {
                    bool isJoin = false;
                    if (cr.IsBuy > 0 || cr.fTecharUserName == strUserName)//老师本人不用管是否支付
                    {
                        isJoin = true;
                    }
                    if (cr.fTecharUserName == strUserName)
                    {
                        strRole = "1";
                    }
                    if (cr.fClassType == "OnLine")
                    {
                        isJoin = true;
                    }

                    if (isJoin)
                    {
                        response.resultCode = iCourseID;
                        response.resultMessage = "课堂创建成功";
                        response.strRole = strRole;
                        //response.strRoomID = strCourseNo;
                        response.strRoomID = (10000 + cr.fID).ToString() + iCourseID.ToString();
                        response.strSig = WebRTCSig.GetSig(strUserName);
                        response.strUserName = strUserName;


                        //添加课堂记录
                        GroupModel group = GroupUserBll.GetGroup(response.strRoomID);
                        if (group == null)
                        {
                            int i = GroupUserBll.InsertGroup(response.strRoomID, course.fDictTitle, iCourseID, cr.fTecharUserName);
                        }


                    }
                    else
                    {
                        response.resultCode = -1;
                        response.resultMessage = "购买支付后才能进入课堂";
                    }

                }
                else
                {
                    response.resultCode = -1;
                    response.resultMessage = "已到下课时间，不能再进入课堂";
                }
            }
            catch (Exception ex)
            {
                response.resultCode = -1;
                response.resultMessage = "出错啦，请联系老师";
            }

            JsonResult jr = new JsonResult();
            jr.Data = response;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }

        /// <summary>
        /// 创建课堂
        /// </summary>
        /// <param name="strClassName"></param>
        /// <param name="iDateLength"></param>
        /// <param name="strUserName"></param>
        /// <returns></returns>
        public JsonResult DoCreateOnLineCourse(string strClassName, int iDateLength, string strUserName)
        {
            CourseModel model = new CourseModel();
            model.fID = 0;
            model.fClassType = "OnLine";
            model.fClassRoomCode = ClassRoomBll.GettClassRoomByOnLine(strUserName).fClassRoomCode;
            model.fDictTitle = strClassName;
            model.fCourseTitle = strClassName;
            model.fClassDate = DateTime.Now;
            model.fClassDateLength = iDateLength;
            model.fPrice = 0;
            model.fOrder = 0;

            ResponseOnLineClassModel response = new ResponseOnLineClassModel();
            if (string.IsNullOrEmpty(model.fCourseTitle))
            {
                response.resultCode = -1;
                response.resultMessage = "请输入课时标题";
            }
            else if (model.fClassDateLength <= 0)
            {
                response.resultCode = -1;
                response.resultMessage = "上课时长必须大于0";
            }
            else
            {

                model.fClassDate = DateTime.Now;
                int iCourseID = ClassRoomBll.DoSaveClassRoomCourse(model);

                string strCourseNo = model.fClassRoomCode + iCourseID;
                ClassRoomModel cr = ClassRoomBll.GetClassRoomByCourseId(iCourseID, strUserName);
                if (cr.fClassType == "OnLine")
                {
                    GroupUserInfoListModel ListModel = GroupUserBll.GetGroupUserInfiList(strCourseNo);
                    if (ListModel.infoList != null && ListModel.infoList.Count > cr.fMaxNumber)
                    {
                        response.resultCode = -1;
                        response.resultMessage = "课堂已达到最大人数，不能进入！";
                    }
                    else
                    {
                        decimal userAccount = UserBll.GetUserAccountAmount(strUserName);
                        decimal classRoomFlow = ClassRoomBll.GetClassRoomFlow(iCourseID);
                        decimal leftFlow = UserBll.GetUserLeftFlow(strUserName);

                        if (userAccount < classRoomFlow)
                        {
                            response.resultCode = -1;
                            if (leftFlow > 0)
                            {
                                response.resultMessage = "该课时最少需要" + classRoomFlow.ToString() + "分钟流量，您的账户流量欠费" + leftFlow.ToString() + "分钟。请先去购买流量";
                            }
                            else
                            {
                                response.resultMessage = "该课时最少需要" + classRoomFlow.ToString() + "分钟流量，您的账户流量剩余" + userAccount.ToString() + "分钟。请先去购买流量";
                            }
                        }
                        else
                        {


                            CourseModel course = ClassRoomBll.GetCourseById(iCourseID, strUserName);

                            if (course != null && course.fClassDate.AddMinutes(course.fClassDateLength) > DateTime.Now)
                            {

                                response.resultCode = iCourseID;
                                response.resultMessage = "课堂创建成功";
                                response.strRole = "1";
                                //response.strRoomID = strCourseNo;
                                response.strRoomID = (10000 + cr.fID).ToString() + iCourseID.ToString();
                                response.strSig = WebRTCSig.GetSig(strUserName);
                                response.strUserName = strUserName;

                                //添加课堂记录
                                GroupModel group = GroupUserBll.GetGroup(response.strRoomID);
                                if (group == null)
                                {
                                    int i = GroupUserBll.InsertGroup(response.strRoomID, strClassName, iCourseID, strUserName);
                                }
                            }
                            else
                            {
                                response.resultCode = -1;
                                response.resultMessage = "课程已结束！";
                            }
                        }
                    }
                }
                else
                {
                    response.resultCode = -1;
                    response.resultMessage = "课堂号错误！";
                }
            }

            JsonResult jr = new JsonResult();
            jr.Data = response;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }

        public JsonResult CheckOnlineClass(string strUserName, string strCourseNo, string strRole = "Student")
        {
            ResponseOnLineClassModel response = new ResponseOnLineClassModel();
            if (strCourseNo.Length > 7)
            {
                string strCourseID = strCourseNo.Substring(5, strCourseNo.Length - 5);
                ClassRoomModel cr = ClassRoomBll.GetClassRoomByCourseId(Convert.ToInt32(strCourseID), strUserName);
                if (cr.fClassType == "OnLine")
                {
                    GroupUserInfoListModel ListModel = GroupUserBll.GetGroupUserInfiList(strCourseNo);
                    if (ListModel.infoList != null && ListModel.infoList.Count > cr.fMaxNumber)
                    {
                        response.resultCode = -1;
                        response.resultMessage = "课堂已达到最大人数，不能进入！";
                    }
                    else
                    {
                        decimal userAccount = UserBll.GetUserAccountAmount(strUserName);
                        decimal classRoomFlow = ClassRoomBll.GetClassRoomFlow(Convert.ToInt32(strCourseID));
                        decimal leftFlow = UserBll.GetUserLeftFlow(strUserName);

                        if (userAccount < classRoomFlow && strRole == "Teacher")
                        {
                            response.resultCode = -1;
                            if (leftFlow > 0)
                            {
                                response.resultMessage = "该课时最少需要" + classRoomFlow.ToString() + "分钟流量，您的账户流量欠费" + leftFlow.ToString() + "分钟。请先去购买流量";
                            }
                            else
                            {
                                response.resultMessage = "该课时最少需要" + classRoomFlow.ToString() + "分钟流量，您的账户流量剩余" + userAccount.ToString() + "分钟。请先去购买流量";
                            }
                        }
                        else
                        {


                            CourseModel course = ClassRoomBll.GetCourseById(Convert.ToInt32(strCourseID), strUserName);

                            if (course != null && course.fClassDate.AddMinutes(course.fClassDateLength) > DateTime.Now)
                            {
                                if (course != null && strRole == "Student" && course.fPrice > 0 && course.IsBuy == 0)
                                {
                                    response.resultCode = 0;
                                    response.resultMessage = "未购买";
                                    //response.CourseID = course.fID;
                                    //response.CourseNo = strCourseNo;
                                    //response.ClassTitle = course.fCourseTitle;
                                    //response.ClassDate = course.fClassDate.ToString();
                                    //response.ClassLength = course.fClassDateLength.ToString();
                                    //response.Price = course.fPrice.ToString();
                                }
                                else
                                {
                                    response.resultCode = Convert.ToInt32(strCourseID);
                                    response.resultMessage = "进入成功";
                                    response.strRole = "0";
                                    response.strRoomID = strCourseNo;
                                    response.strSig = WebRTCSig.GetSig(strUserName);
                                    response.strUserName = strUserName;
                                }
                            }
                            else
                            {
                                response.resultCode = -1;
                                response.resultMessage = "课程已结束！";
                            }
                        }
                    }
                }
                else
                {
                    response.resultCode = -1;
                    response.resultMessage = "课堂号错误！";
                }
            }
            else
            {
                response.resultCode = -1;
                response.resultMessage = "课堂号错误！";
            }
            JsonResult jr = new JsonResult();
            jr.Data = response;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }




    }
}
