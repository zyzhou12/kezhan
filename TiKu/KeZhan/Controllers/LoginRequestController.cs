﻿using Aliyun.OSS;
using KeZhan.Code;
using KeZhan.Filters;
using KeZhan.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using TiKu.Bll;
using TiKuBll;
using TiKuBll.Model;

namespace KeZhan.Controllers
{
    [LoginActionFilter]
    public class LoginRequestController : Controller
    {



        #region ClassRoom
        public JsonResult GetOpenUrl(string strOpenType, string iClassID)
        {
            string strUrl = "";
            string strWebUrl = "https://tedu.qcloudtrtc.com/#/class/{0}/{1}/{2}/{3}/{4}";
            string strPCUrl = "tc-videochat://tclass/{0}/{1}?user_id={2}&user_token={3}&user_sig={4}";

            string configurl = "{\"title\": \"2kezhan\",\"logo\": \"https://test.2kezhan.com/Content/images/header_logo.jpg\"}";
            configurl = "http://www.2kezhan.com/logo.json";

            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            string usersig = UserSig.GetSig(userInfo.fUserName);
            string sdkappid = "189277";
            ResponseBaseModel response = new ResponseBaseModel();
            if (strOpenType == "WEB")
            {
                strUrl = string.Format(strWebUrl, sdkappid, iClassID, userInfo.fUserName, usersig, userInfo.fUserToken);

                response.iResult = 1;
                response.strMsg = strUrl;
            }
            else if (strOpenType == "PC")
            {
                strUrl = string.Format(strPCUrl, sdkappid, iClassID, userInfo.fUserName, userInfo.fUserToken, usersig, configurl);
                response.iResult = 1;
                response.strMsg = strUrl;
            }
            else
            {
                response.iResult = -1;
                response.strMsg = "进入课堂失败";
            }


            JsonResult jr = new JsonResult();
            jr.Data = response;
            return jr;
        }

        [HttpPost]
        public JsonResult DoSaveClassRoomInfo(string strClassRoomCode, string strType, string strInfo)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            ResponseBaseModel response = new ResponseBaseModel();
            int i = ClassRoomBll.SaveClassRoomInfo(strClassRoomCode, strType, strInfo, userInfo.fUserName);

            JsonResult jr = new JsonResult();
            jr.Data = response;
            return jr;
        }

        /// <summary>
        /// 预约课堂
        /// </summary>
        /// <param name="iCourseID"></param>
        /// <returns></returns>
        public JsonResult DoCreateClass(int iCourseID)
        {
            ResponseBaseModel response = new ResponseBaseModel();
            response.strMsg = TICRequest.CreateClass(iCourseID);
            response.iResult = 0;
            JsonResult jr = new JsonResult();
            jr.Data = response;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }


        public JsonResult GetTeacherValidList(string strUserName)
        {
            ValidDetailListModel model = UserBll.GetTeachValidDetailList(strUserName, "已审核");

            return Json(model.detailList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTeacherList(string strPharse, string strSubject)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            UserMemberListModel model = UserBll.GetTeacherList(userInfo.fUserName, strPharse, strSubject);

            //UserMemberModel my = new UserMemberModel();
            //my.fMemberUserName = userInfo.fUserName;
            //my.MemberName = userInfo.fNickName+"-"+userInfo.fMobile;
            //model.userMemberList.Add(my);
            return Json(model.userMemberList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DoSaveClassRoom(ClassRoomModel model)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            ResponseBaseModel response = new ResponseBaseModel();
            if (string.IsNullOrEmpty(model.fClassRoomTitle))
            {
                response.iResult = -1;
                response.strMsg = "请输入课程名称";
            }
            else if (string.IsNullOrEmpty(model.fCoverImg))
            {
                response.iResult = -1;
                response.strMsg = "请上传封面图片";
            }
            else if (string.IsNullOrEmpty(model.fInfo))
            {
                response.iResult = -1;
                response.strMsg = "请填写简介";
            }
            else if (string.IsNullOrEmpty(model.fTecharUserName))
            {
                response.iResult = -1;
                response.strMsg = "请选择授课老师";
            }
            //else if (string.IsNullOrEmpty(model.fDesc))
            //{
            //  response.iResult = -1;
            //  response.strMsg = "课程详情不能为空";
            //}
            else if (string.IsNullOrEmpty(model.fPrice.ToString()) || model.fPrice <= 0)
            {
                response.iResult = -1;
                response.strMsg = "价格必须大于0";
            }
            //else if (model.fClassRoomDate < DateTime.Now)
            //{
            //    response.iResult = -1;
            //    response.strMsg = "开课时间必须大于当天";
            //}
            //else if (model.fDeadLineDate < DateTime.Now)
            //{
            //    response.iResult = -1;
            //    response.strMsg = "截止报名时间必须大于当天";
            //}
            else if (string.IsNullOrEmpty(model.fClassType))
            {
                response.iResult = -1;
                response.strMsg = "请选择公开类型";
            }
            else if (string.IsNullOrEmpty(model.fPharse))
            {
                response.iResult = -1;
                response.strMsg = "请选择课程二级分类";
            }
            else if (string.IsNullOrEmpty(model.fSubject))
            {
                response.iResult = -1;
                response.strMsg = "请选择课程三级分类";
            }
            else if (string.IsNullOrEmpty(model.fPayType))
            {
                response.iResult = -1;
                response.strMsg = "请选择支付类型";
            }
            else if (model.fType == "Recorded" && model.fDeadLineDate < DateTime.Now.AddDays(7))
            {
                response.iResult = -1;
                response.strMsg = "销售截止时间必须大于7天";
            }
            else if (model.fType == "Recorded" && model.fEffectDay < 0)
            {
                response.iResult = -1;
                response.strMsg = "请输入有效天数";
            }
            else if (model.fType == "Recorded" && model.fFeeLength < 0)
            {
                response.iResult = -1;
                response.strMsg = "请输入免费时长";
            }
            else
            {
                // model.fGrade = "";
                //model.fKnowLedge = "";

                //model.fTecharUserName = userInfo.fUserName;
                // model.fStatus = "保存";

                string ClassRoomCode = "";
                response.iResult = ClassRoomBll.DoSaveClassRoom(model, userInfo.fUserName, ref ClassRoomCode);

                response.strMsg = ClassRoomCode;

            }


            JsonResult jr = new JsonResult();
            jr.Data = response;
            return jr;
        }


        [HttpPost]
        public JsonResult DoSaveClassRoomDesc(DescModel model)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            ResponseBaseModel response = new ResponseBaseModel();
            if (string.IsNullOrEmpty(model.fContent))
            {
                response.iResult = -1;
                response.strMsg = "内容不能为空";
            }
            else
            {
                response.iResult = ClassRoomBll.DoSaveClassRoomDesc(model);
            }

            JsonResult jr = new JsonResult();
            jr.Data = response;
            return jr;
        }

        [HttpPost]
        public JsonResult DoDelClassRoomDesc(int iDescID)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            ResponseBaseModel response = new ResponseBaseModel();

            response.iResult = ClassRoomBll.DoDelClassRoomDesc(iDescID);


            JsonResult jr = new JsonResult();
            jr.Data = response;
            return jr;
        }
        [HttpPost]
        public JsonResult DoSaveClassRoomCourse(CourseModel model)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            ResponseBaseModel response = new ResponseBaseModel();
            if (string.IsNullOrEmpty(model.fDictTitle))
            {
                response.iResult = -1;
                response.strMsg = "请输入课时标题";
            }
            else if (model.fClassDate < DateTime.Now)
            {
                response.iResult = -1;
                response.strMsg = "上课时间必须大于当前时间";
            }
            else if (model.fClassDateLength <= 0)
            {
                response.iResult = -1;
                response.strMsg = "上课时长必须大于0";
            }
            else
            {
                response.iResult = ClassRoomBll.DoSaveClassRoomCourse(model);
            }

            JsonResult jr = new JsonResult();
            jr.Data = response;
            return jr;
        }

        [HttpPost]
        public JsonResult DoSaveOnLineCourse(CourseModel model)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);


            ResponseBaseModel response = new ResponseBaseModel();
            if (string.IsNullOrEmpty(model.fCourseTitle))
            {
                response.iResult = -1;
                response.strMsg = "请输入课时标题";
            }
            else if (model.fClassDateLength <= 0)
            {
                response.iResult = -1;
                response.strMsg = "上课时长必须大于0";
            }
            else
            {
                model.fClassDate = DateTime.Now;
                int iCourseID = ClassRoomBll.DoSaveClassRoomCourse(model);
                ClassRoomModel cr = ClassRoomBll.GetClassRoomByCode(model.fClassRoomCode, userInfo.fUserName);
                response.iResult = iCourseID;
                //response.strMsg = (10000 + cr.fID).ToString() + iCourseID.ToString();

                response.strMsg = TICRequest.CreateClass(iCourseID);
            }

            JsonResult jr = new JsonResult();
            jr.Data = response;
            return jr;
        }


        /// <summary>
        /// 修改状态（发布/下架）
        /// </summary>
        /// <param name="strClassRoomCode"></param>
        /// <param name="strStatus"></param>
        /// <returns></returns>
        public JsonResult DoUpdateStatus(string strClassRoomCode, string strStatus, string strNote)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            ResponseBaseModel response = new ResponseBaseModel();
            if (string.IsNullOrEmpty(strStatus))
            {
                response.iResult = -1;
                response.strMsg = "状态不能为空";
            }
            else
            {
                ClassRoomModel model = ClassRoomBll.GetClassRoomDetail(strClassRoomCode, userInfo.fUserName);
                BookingListModel bookingList = BookingBll.GetBookingList(userInfo.fUserName, strClassRoomCode, null, "退款中", null, null);
                if (strStatus == "发布" && model.courseList.Count == 0)
                {
                    response.iResult = -1;
                    response.strMsg = "至少要一节课时才能发布";
                }
                else if (strStatus == "发布" && model.courseList.Count > 0)
                {
                    if (model.fType == "Live")
                    {
                        DateTime classDate = model.courseList[0].fClassDate.AddMinutes(model.courseList[0].fClassDateLength);
                        int iCourseID = model.courseList[0].fID;
                        foreach (CourseModel course in model.courseList)
                        {
                            if (iCourseID != course.fID && classDate > course.fClassDate)
                            {
                                response.iResult = -1;
                                response.strMsg = "上课时间不能重叠，请检查每个课时上课时间";
                            }
                        }
                    }
                    else
                    {
                        ConfigModel config = ManagerBll.GetSystemConfig("上海");
                        decimal iDateLength = ResourceBll.GetResourceInfoByClassRoomCode(strClassRoomCode).fDateLength / 60;
                        if (model.fPrice < iDateLength * config.fSourceFee)
                        {
                            response.iResult = -1;

                            response.strMsg = "根据本课程所有课时的视频时长，平台收费是（￥" + Math.Round(iDateLength * config.fSourceFee, 2).ToString() + "），因您当前设置的录播销售价小于平台收费，不能发布本课程，请修改销售价格后再发布。";
                        }
                    }
                }
                else if (strStatus == "下线" && bookingList.list.Count > 0)
                {
                    response.iResult = -1;
                    response.strMsg = "退款处理完成才能下线";
                }

                if (response.iResult >= 0)
                {
                    response.iResult = ClassRoomBll.ClassRoomSubmitSend(strClassRoomCode, strStatus, strNote, userInfo.fUserName);
                }
            }

            JsonResult jr = new JsonResult();
            jr.Data = response;
            return jr;
        }

        [HttpPost]
        public JsonResult DoUpdateBookingStatus(string strBookingNo, string strStatus, string strRemark)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            ResponseBaseModel response = new ResponseBaseModel();

            response.iResult = BookingBll.BookingUpdateStatus(strBookingNo, strStatus, strRemark, userInfo.fUserName);

            JsonResult jr = new JsonResult();
            jr.Data = response;
            return jr;
        }


        /// <summary>
        /// 提交退款申请
        /// </summary>
        /// <param name="strBookingNo"></param>
        /// <param name="dAmount"></param>
        /// <returns></returns>
        public JsonResult DoSubmitBookingRefund(string strOrderNo, string strUserName, decimal dAmount, string strRemark)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            ResponseBaseModel response = new ResponseBaseModel();
            string strMsg = "";
            int i = BookingBll.CheckRefund(dAmount, strOrderNo, ref strMsg);
            if (i >= 0)
            {
                response.iResult = BookingBll.SubmitBookingRefund(strOrderNo, strUserName, userInfo.fUserName, dAmount, strRemark);
            }
            else
            {
                response.iResult = -1;
                response.strMsg = strMsg;
            }
            JsonResult jr = new JsonResult();
            jr.Data = response;
            return jr;
        }



        [HttpPost]
        public JsonResult DoCheckBookingClassRoom(string strClassRoomCode)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            ResponseBaseModel response = new ResponseBaseModel();
            BookingModel booking = BookingBll.GettBooking(userInfo.fUserName, "ClassRoom", strClassRoomCode, "");
            if (booking == null)
            {

                ClassRoomModel classRoom = ClassRoomBll.GetClassRoomByCode(strClassRoomCode, null);
                BookingListModel list = BookingBll.GetBookingList(classRoom.fTecharUserName, strClassRoomCode, null, null, null, null);
                if (classRoom.fMaxNumber > list.list.Where(m => m.fStatus == "已支付" || m.fStatus == "已驳回").ToList().Count)
                {
                    response.iResult = 0;
                }
                else
                {
                    response.strMsg = "课程已达到最大报名人数，请选择其他课程";
                    response.iResult = -1;
                }
            }
            else if (booking.fStatus == "提交")
            {
                response.strMsg = "存在提交未支付订单，请不要重复提交订单";
                response.iResult = -1;
            }
            else if (booking.fStatus == "已支付")
            {
                response.strMsg = "不能重复购买";
                response.iResult = -1;
            }
            else
            {
                response.iResult = 0;
            }

            JsonResult jr = new JsonResult();
            jr.Data = response;
            return jr;
        }
        [HttpPost]
        public JsonResult DoSubmitBooking(string strClassRoomCode)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            ResponseBaseModel response = new ResponseBaseModel();
            BookingModel booking = BookingBll.GettBooking(userInfo.fUserName, "ClassRoom", strClassRoomCode, "");
            if (booking == null)
            {

                ClassRoomModel classRoom = ClassRoomBll.GetClassRoomByCode(strClassRoomCode, null);
                BookingListModel list = BookingBll.GetBookingList(classRoom.fTecharUserName, strClassRoomCode, null, null, null, null);
                if (classRoom.fMaxNumber > list.list.Where(m => m.fStatus == "已支付" || m.fStatus == "已驳回").ToList().Count)
                {
                    response.strMsg = BookingBll.SubmitBooking(userInfo.fUserName, "ClassRoom", strClassRoomCode, classRoom.fPrice, classRoom.fIsReturn, "Web");
                }
                else
                {
                    response.strMsg = "课程已达到最大报名人数，请选择其他课程";
                    response.iResult = -1;
                }
            }
            else if (booking.fStatus == "提交")
            {
                response.strMsg = booking.fBookingNo;
                response.iResult = 0;
            }
            else if (booking.fStatus == "已支付" || booking.fStatus == "退款中" || booking.fStatus == "已驳回")
            {
                response.strMsg = "不能重复购买";
                response.iResult = -1;
            }
            else
            {
                ClassRoomModel classRoom = ClassRoomBll.GetClassRoomByCode(strClassRoomCode, null);
                BookingListModel list = BookingBll.GetBookingList(classRoom.fTecharUserName, strClassRoomCode, null, null, null, null);
                if (classRoom.fMaxNumber > list.list.Where(m => m.fStatus == "已支付" || m.fStatus == "已驳回").ToList().Count)
                {
                    response.iResult = 0;
                    response.strMsg = BookingBll.SubmitBooking(userInfo.fUserName, "ClassRoom", strClassRoomCode, classRoom.fPrice, classRoom.fIsReturn, "Web");
                }
                else
                {
                    response.strMsg = "课程已达到最大报名人数，请选择其他课程";
                    response.iResult = -1;
                }
            }

            JsonResult jr = new JsonResult();
            jr.Data = response;
            return jr;
        }

        [HttpPost]
        public JsonResult DoSubmitCourseBooking(int iCourseID)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            ResponseBaseModel response = new ResponseBaseModel();
            BookingModel booking = BookingBll.GettBooking(userInfo.fUserName, "OnLineClass", iCourseID.ToString(), "");
            if (booking == null)
            {
                CourseModel course = ClassRoomBll.GetCourseByID(iCourseID);

                response.strMsg = BookingBll.SubmitBooking(userInfo.fUserName, "OnLineClass", iCourseID.ToString(), course.fPrice, false, "Web");

            }
            else if (booking.fStatus == "提交")
            {
                response.strMsg = booking.fBookingNo;
                response.iResult = 0;
            }
            else if (booking.fStatus == "已支付" || booking.fStatus == "退款中" || booking.fStatus == "已驳回")
            {
                response.strMsg = "不能重复购买";
                response.iResult = -1;
            }
            else
            {
                CourseModel course = ClassRoomBll.GetCourseByID(iCourseID);

                response.strMsg = BookingBll.SubmitBooking(userInfo.fUserName, "OnLineClass", iCourseID.ToString(), course.fPrice, false, "Web");

            }

            JsonResult jr = new JsonResult();
            jr.Data = response;
            return jr;
        }


        [HttpPost]
        public JsonResult DoSaveUploadFile(CourseModel model)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            ResponseBaseModel response = new ResponseBaseModel();
            model.fUploadDate = DateTime.Now;
            model.fUploadOpr = userInfo.fUserName;
            model.fSource = "老师上传";
            model.fStatus = "已上传";


            JsonResult jr = new JsonResult();
            response.iResult = ClassRoomBll.DoClassRoomCourseUpload(model);
            jr.Data = response;
            return jr;
        }

        [HttpPost]
        public JsonResult DoSaveCourseDocument(CourseModel model)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            ResponseBaseModel response = new ResponseBaseModel();


            JsonResult jr = new JsonResult();
            response.iResult = ClassRoomBll.DoCourseDocumentUpload(model);
            jr.Data = response;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }

        [HttpPost]
        public JsonResult DoDelCourseDocument(int iCourseId)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            ResponseBaseModel response = new ResponseBaseModel();


            JsonResult jr = new JsonResult();
            response.iResult = ClassRoomBll.DoCourseDocumentDel(iCourseId);
            jr.Data = response;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }

        [HttpPost]
        public JsonResult DoDelCourseResource(int iCourseId)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            ResponseBaseModel response = new ResponseBaseModel();


            JsonResult jr = new JsonResult();
            response.iResult = ClassRoomBll.DoCourseResourceDel(iCourseId);
            jr.Data = response;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }

        public JsonResult DoClassRoomSettlement(string strClassRooomCode)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            ResponseBaseModel response = new ResponseBaseModel();

            string strMsg = "";
            JsonResult jr = new JsonResult();
            try
            {
                response.iResult = ClassRoomBll.ClassRoomSettlement(strClassRooomCode, userInfo.fUserName, ref strMsg);
                response.strMsg = strMsg;
            }
            catch (Exception ex)
            {
                response.iResult = -1;
                response.strMsg = "结算失败";
            }

            jr.Data = response;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }

        #endregion

        #region Message

        public JsonResult MessageLook(int iMessageID)
        {
            ResponseBaseModel response = new ResponseBaseModel();
            response.iResult = UserBll.MessageUpdate(iMessageID);
            JsonResult jr = new JsonResult();
            jr.Data = response;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }
        #endregion


        #region User



        public JsonResult DoSaveTeacherInfo(TeacherBaseModel model)
        {
            ResponseBaseModel response = new ResponseBaseModel();

            if (string.IsNullOrEmpty(model.fDesc))
            {
                response.iResult = -1;
                response.strMsg = "请输入介绍";
            }
            //else if (model.fIsLive && model.fVideoFee <= 0)
            //{
            //    response.iResult = -1;
            //    response.strMsg = "开通一对一必须填写费用";
            //}
            //else if (model.fIsProblem && model.fProblemFee <= 0)
            //{
            //    response.iResult = -1;
            //    response.strMsg = "开通答疑必须填写费用";
            //}
            else
            {
                UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
                response.iResult = UserBll.SaveTeacherInfo(userInfo.fUserName, model);


            }


            JsonResult jr = new JsonResult();
            jr.Data = response;
            return jr;
        }


        [HttpPost]
        public JsonResult DoSaveStudentInfo(StudentBaseModel model)
        {
            ResponseBaseModel response = new ResponseBaseModel();

            if (string.IsNullOrEmpty(model.Pharse))
            {
                response.iResult = -1;
                response.strMsg = "请选择学段";
            }
            else if (string.IsNullOrEmpty(model.Grade))
            {
                response.iResult = -1;
                response.strMsg = "请选择年级";
            }
            else if (string.IsNullOrEmpty(model.School))
            {
                response.iResult = -1;
                response.strMsg = "请填写您的学校名称";
            }
            else
            {
                UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
                response.iResult = UserBll.SaveStudentInfo(userInfo.fUserName, model);


            }
            JsonResult jr = new JsonResult();
            jr.Data = response;
            return jr;
        }




        public JsonResult SaveTeacherValidFile(string jsonData)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            ResponseBaseModel response = new ResponseBaseModel();
            try
            {
                TeacherValidModel model = new JavaScriptSerializer().Deserialize<TeacherValidModel>(jsonData);
                TeacherBaseModel teacher = UserBll.GetUserTeacherBase(userInfo.fUserName);
                if (teacher.fStatus == "未认证" && string.IsNullOrEmpty(model.fIDCard1))
                {
                    response.iResult = -1;
                    response.strMsg = "请上传证件照";
                }
                else if (string.IsNullOrEmpty(model.fName))
                {
                    response.iResult = -1;
                    response.strMsg = "请输入真实姓名";
                }
                else if (string.IsNullOrEmpty(model.fIDType))
                {
                    response.iResult = -1;
                    response.strMsg = "请选择证件类型";
                }
                else if (string.IsNullOrEmpty(model.fUID))
                {
                    response.iResult = -1;
                    response.strMsg = "请输入证件号码";
                }
                else if (model.fIDType == "1" && model.fUID.Length != 18)
                {
                    response.iResult = -1;
                    response.strMsg = "证件号码错误";
                }
                else if (model.detailList == null || model.detailList.Count <= 0)
                {
                    response.iResult = -1;
                    response.strMsg = "请上传证书文件";
                }
                else
                {

                    response.iResult = UserBll.SaveTeacherValidInfo(userInfo.fUserName, model);

                }
            }
            catch (Exception ex)
            {
                response.iResult = -1;
                response.strMsg = ex.Message;
            }
            JsonResult jr = new JsonResult();
            jr.Data = response;
            return jr;
        }
        [HttpPost]
        public JsonResult DoSaveInfo(string strType, string strInfo)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            ResponseBaseModel response = new ResponseBaseModel();
            int i = UserBll.SaveUserInfo(userInfo.fUserName, strType, strInfo);

            //  TICRequest.CreateUser(userInfo);

            TICRequest.UpdateUserInfo(userInfo.fUserName, strType, strInfo);
            //更新账号票据
            TICRequest.UpdateUserToken(userInfo);


            userInfo = UserBll.GetUserInfo(userInfo.fUserName);
            Code.Fun.SetSessionUserInfo(this, userInfo);


            JsonResult jr = new JsonResult();
            jr.Data = response;
            return jr;
        }

        [HttpPost]
        public JsonResult DoUpdateMobile(string strMobile, string strCode)
        {
            ResponseBaseModel response = new ResponseBaseModel();
            if (string.IsNullOrEmpty(strMobile))
            {
                response.iResult = -1;
                response.strMsg = "请输入手机号";
            }
            else if (string.IsNullOrEmpty(strCode))
            {
                response.iResult = -1;
                response.strMsg = "请输入验证码";
            }
            else
            {
                UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);

                //校验验证码
                UserInfoModel newUser = UserBll.GetUserInfo("1" + strMobile);
                if (newUser.fCode == strCode)
                {
                    UserBll.SaveUserInfo(userInfo.fUserName, "fMobile", strMobile);

                    response.iResult = 0;
                }
                else
                {
                    response.strMsg = "验证码不对";
                    response.iResult = -1;
                }



            }

            JsonResult jr = new JsonResult();
            jr.Data = response;
            return jr;
        }


        [HttpPost]
        public JsonResult DoSendEmailCode(string strEmail)
        {
            string strMsg = "";
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            ResponseBaseModel response = new ResponseBaseModel();
            if (string.IsNullOrEmpty(strEmail))
            {
                response.iResult = -1;
                response.strMsg = "请输入邮箱";
            }
            else
            {
                response.iResult = UserBll.UserSendEmailCode(userInfo.fUserName, strEmail, ref strMsg);
                response.strMsg = strMsg;
            }

            JsonResult jr = new JsonResult();
            jr.Data = response;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }

        [HttpPost]
        public JsonResult DoUpdateEmail(string strEmail, string strCode)
        {
            ResponseBaseModel response = new ResponseBaseModel();
            if (string.IsNullOrEmpty(strEmail))
            {
                response.iResult = -1;
                response.strMsg = "请输入邮箱";
            }
            else if (string.IsNullOrEmpty(strCode))
            {
                response.iResult = -1;
                response.strMsg = "请输入验证码";
            }
            else
            {
                UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);

                //校验验证码
                UserInfoModel newUser = UserBll.GetUserInfo(userInfo.fUserName);
                if (newUser.fEmailCode == strCode && newUser.fEmailCodeEffectDate > DateTime.Now)
                {
                    UserBll.SaveUserInfo(userInfo.fUserName, "fEmail", strEmail);

                    response.iResult = 0;
                }
                else
                {
                    response.strMsg = "验证码不对";
                    response.iResult = -1;
                }



            }

            JsonResult jr = new JsonResult();
            jr.Data = response;
            return jr;
        }

        /// <summary>
        /// 关注老师
        /// </summary>
        /// <param name="strTeacherUser"></param>
        /// <param name="IsFocus"></param>
        /// <returns></returns>
        public JsonResult DoUserFocus(string strTeacherUser, bool IsFocus)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            ResponseBaseModel response = new ResponseBaseModel();
            response.iResult = UserBll.TeacherFocus(userInfo.fUserName, strTeacherUser, IsFocus);
            JsonResult jr = new JsonResult();
            jr.Data = response;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }
        #endregion

        #region Teacher
        public JsonResult DoDeleteFile(string strResourceCode)
        {
            ResponseBaseModel response = new ResponseBaseModel();
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);

            response.iResult = ResourceBll.DeleteFile(strResourceCode);

            JsonResult jr = new JsonResult();
            jr.Data = response;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }
        public JsonResult DoRestoreFile(string strResourceCode)
        {
            ResponseBaseModel response = new ResponseBaseModel();
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            ResourseModel resource = ResourceBll.GetResource(strResourceCode);
            ResourceInfoModel info = ResourceBll.GetResourceInfo(userInfo.fUserName);
            if ((resource.fSize + info.fSize) > 20971520)
            {
                response.iResult = -1;
                response.strMsg = "您已超过允许的容量，请删除不再使用的文件后再恢复";
            }
            else
            {
                response.iResult = ResourceBll.RestoreFile(strResourceCode);
            }

            JsonResult jr = new JsonResult();
            jr.Data = response;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }
        public JsonResult DoChangeFileType(string strResourceCode, string strType)
        {
            ResponseBaseModel response = new ResponseBaseModel();
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);

            response.iResult = ResourceBll.ChangeFileType(strResourceCode, strType);

            JsonResult jr = new JsonResult();
            jr.Data = response;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }


        public JsonResult DoSaveUserBank(UserBankAccountModel model)
        {
            ResponseBaseModel response = new ResponseBaseModel();
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            if (model.fID <= 0)
            {
                model.fUserName = userInfo.fUserName;
                model.fCreateDate = DateTime.Now;
                model.fCreateOpr = userInfo.fUserName;
            }
            int i = UserBll.SaveUserBankAccount(model);

            JsonResult jr = new JsonResult();
            jr.Data = response;
            return jr;

        }


        public JsonResult SubmitBuyFlowOrder(int iNum, decimal dPrice)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            JsonResult jr = new JsonResult();
            jr.Data = UserBll.SubmitBuyFlowOrder(userInfo.fUserName, iNum, dPrice, "购买", DateTime.Now.AddDays(365), userInfo.fUserName);
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }

        public JsonResult CheckUser(string strMobile)
        {
            ResponseBaseModel response = new ResponseBaseModel();

            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            UserInfoModel model = UserBll.GetUserInfoByMobile(strMobile);
            if (model == null)
            {
                response.iResult = -1;
                response.strMsg = "该手机号还未注册，请确认手机号是否正确";
            }
            else if (model.fRole != "Teacher")
            {
                response.iResult = -1;
                response.strMsg = "该用户不是老师，不能邀请";
            }
            else
            {

                UserMemberModel um = UserBll.GetUserMember(userInfo.fUserName, strMobile);
                if (um != null && um.fStatus.Trim() == "2")
                {
                    response.iResult = -1;
                    response.strMsg = "该用户已是你的授课老师，不用重复邀请";
                }
                else
                {
                    response.iResult = 0;
                }
            }

            JsonResult jr = new JsonResult();
            jr.Data = response;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }

        /// <summary>
        /// 邀请授课老师
        /// </summary>
        /// <param name="strMobile"></param>
        /// <param name="strNote"></param>
        /// <returns></returns>
        public JsonResult InviteMember(string strMobile, string strNote)
        {
            ResponseBaseModel response = new ResponseBaseModel();
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            string strMsg = "";
            strNote = userInfo.fNickName + "邀请您成为他的授课老师，是否同意" + strNote;
            response.iResult = UserBll.UserInviteMember(userInfo.fUserName, strMobile, strNote, ref strMsg);
            response.strMsg = strMsg;

            JsonResult jr = new JsonResult();
            jr.Data = response;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }

        public JsonResult InviteAgree(int id)
        {
            ResponseBaseModel response = new ResponseBaseModel();
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);

            response.iResult = UserBll.UserInviteAgree(userInfo.fUserName, id);

            JsonResult jr = new JsonResult();
            jr.Data = response;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }
        public JsonResult InviteRefused(int id)
        {
            ResponseBaseModel response = new ResponseBaseModel();
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);

            response.iResult = UserBll.UserInviteRefused(userInfo.fUserName, id);

            JsonResult jr = new JsonResult();
            jr.Data = response;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }


        public JsonResult InviteCancel(int id)
        {
            ResponseBaseModel response = new ResponseBaseModel();
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);

            response.iResult = UserBll.UserInviteCancel(userInfo.fUserName, id);

            JsonResult jr = new JsonResult();
            jr.Data = response;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }


        public JsonResult InviteRemove(int id)
        {
            ResponseBaseModel response = new ResponseBaseModel();
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);

            response.iResult = UserBll.UserInviteRemove(userInfo.fUserName, id);

            JsonResult jr = new JsonResult();
            jr.Data = response;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }
        [HttpPost]
        public JsonResult DoConfrimRefund(int refundID, string strPayType, decimal refundPrice, string applyStatus, string applyNote)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            ResponseBaseModel response = new ResponseBaseModel();

            string strMessage = "";
            try
            {
                UserRefundModel refund = BookingBll.GetBookingRefund(refundID);

                //业务订单号转换成支付订单号
                //select @BookingNo=fOrderNo  from tuserpay where fType='BuyClass' and fPayNo is not null and fRemark=@BookingNo
                string strPayNo = BookingBll.GetPayNoByBookingNo(refund.fBookingNo);
                BookingModel booking = BookingBll.GetBookingByNo(refund.fBookingNo);

                if (applyStatus == "1")
                {
                    bool issuccess = false;

                    if (refund.fPayType == "weixinpay")
                    {
                        WeiXinPayController pay = new WeiXinPayController();
                        string RefundResult = pay.RefundAmount(strPayNo, refund.fOrderNo, Convert.ToInt32(booking.fAmount * 100), Convert.ToInt32(refund.fApplyAmount * 100), Request.UserHostAddress);


                        xml result = Deserialize(typeof(xml), RefundResult) as xml;
                        if (result.result_code == "SUCCESS")
                        {
                            issuccess = true;
                        }
                        else
                        {
                            if (result.return_code == "SUCCESS")
                            {
                                strMessage = "退款失败，" + result.err_code_des;
                            }
                            else
                            {
                                strMessage = "退款失败，" + result.return_msg;
                            }

                            response.iResult = -1;
                        }
                    }
                    else if (refund.fPayType == "alipay")
                    {
                        AliPayController pay = new AliPayController();
                        string RefundResult = pay.RefundAmount(strPayNo, refund.fOrderNo, refund.fApplyAmount);

                        if (RefundResult == "SUCCESS")
                        {
                            issuccess = true;
                        }
                        else
                        {
                            strMessage = RefundResult;

                            response.iResult = -1;
                        }
                    }
                    else if (refund.fPayType == "aizhupay")
                    {
                        AiZhuPayController pay = new AiZhuPayController();
                        string RefundResult = pay.RefundAmount(refund.fID, refund.fBookingNo, "applyNote", Convert.ToInt32(refund.fRefundAmount), userInfo.fUserName);

                        if (RefundResult == "SUCCESS")
                        {
                            issuccess = true;
                        }
                        else
                        {
                            strMessage = RefundResult;
                            response.iResult = -1;
                        }
                    }


                    if (issuccess)
                    {
                        response.iResult = BookingBll.ConfirmUserRefund(true, refundID, refundPrice, applyNote, userInfo.fUserName, ref strMessage);

                        strMessage = "退款成功";
                    }
                }
                else
                {
                    //response.iResult = BookingBll.RefusedUserRefund(refundID, applyNote, userInfo.fUserName);
                    response.iResult = BookingBll.ConfirmUserRefund(false, refundID, 0, applyNote, userInfo.fUserName, ref strMessage);
                    strMessage = "退款已驳回";


                }

            }
            catch (Exception ex)
            {
                response.iResult = -1;
                strMessage = ex.Message;
            }
            response.strMsg = strMessage;
            JsonResult jr = new JsonResult();
            jr.Data = response;
            return jr;
        }


        public JsonResult DoSaveTeacherWithDrawal(TeacherWithdrawalModel model)
        {
            ResponseBaseModel response = new ResponseBaseModel();
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            string strMsg = "";
            int checkResult = UserBll.CheckWithdrawal(Convert.ToInt32(model.fAmount), userInfo.fUserName, ref strMsg);
            if (checkResult == 0)
            {

                model.fUserName = userInfo.fUserName;
                model.fSubmitDate = DateTime.Now;
                model.fStatus = "0";
                model.fCreateDate = DateTime.Now;
                model.fCreateOpr = userInfo.fUserName;

                int i = UserBll.SaveTeacherWithdrawal(model);
                response.iResult = 1;
            }
            else
            {
                response.iResult = -1;
                response.strMsg = strMsg;
            }

            JsonResult jr = new JsonResult();
            jr.Data = response;
            return jr;
        }



        #endregion

        #region Home

        public JsonResult GetBrowserVersions()
        {
            string browserVersions = string.Empty;
            HttpBrowserCapabilitiesBase hbc = HttpContext.Request.Browser;
            string browserType = hbc.Browser.ToString();     //获取浏览器类型
            string browserVersion = hbc.Version.ToString();    //获取版本号

            ResponseBaseModel response = new ResponseBaseModel();
            if (browserType == "Chrome" || browserType == "Saifer" || browserType == "QQ")
            {
                response.iResult = 0;
            }
            else
            {
                response.iResult = -1;
                response.strMsg = "您当前使用的浏览器版本为：" + browserType + browserVersion + ",在线课堂可能会出现音视频问题，推荐使用Chrome、Saifer、QQ浏览器，确认要继续吗？";
            }

            JsonResult jr = new JsonResult();
            jr.Data = response;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }

        /// <summary>
        /// 开始上课 进入课堂
        /// </summary>
        /// <param name="iCourseID"></param>
        /// <returns></returns>

        public JsonResult OpenClassRoom(int iCourseID)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            ClassRoomModel cr = ClassRoomBll.GetClassRoomByCourseId(iCourseID, userInfo.fUserName);
            ResponseBaseModel response = new ResponseBaseModel();
            GroupModel group = GroupUserBll.GetGroup((10000 + cr.fID).ToString() + iCourseID.ToString());
            if (group == null || (group != null && group.fIsValid))//判断是否已销毁课堂
            {
                if (cr.IsBuy > 0 || cr.fTecharUserName == userInfo.fUserName)
                {

                    CourseModel course = ClassRoomBll.GetCourseByID(iCourseID);
                    if (course != null && course.fClassDate.AddMinutes(course.fClassDateLength) > DateTime.Now)
                    {
                        if (course.fIsPay)
                        {
                            response.iResult = 0;
                        }
                        else
                        {
                            decimal userAccount = UserBll.GetUserAccountAmount(cr.fCreateOpr);
                            decimal classRoomFlow = ClassRoomBll.GetClassRoomFlow(iCourseID);

                            if (classRoomFlow > userAccount)
                            {
                                if (cr.fTecharUserName == cr.fCreateOpr)
                                {
                                    response.iResult = -1;
                                    response.strMsg = "该课时需要" + classRoomFlow.ToString() + "流量，账户剩余" + userAccount.ToString() + "流量。账户余额不足用，请先去购买流量";
                                }
                                else
                                {

                                    response.iResult = -2;
                                    response.strMsg = "该课时需要" + classRoomFlow.ToString() + "流量，账户剩余" + userAccount.ToString() + "流量。账户余额不足用，请课程发布者先去购买流量";
                                }
                            }
                            else
                            {
                                //支付流量
                                int i = ClassRoomBll.ClassRoomCoursePay(iCourseID, userInfo.fUserName, "Web");
                                if (i == 0)
                                {
                                    if (string.IsNullOrEmpty(course.fClassId))
                                    {
                                        //预约课堂
                                        response.iResult = 999999;
                                        response.strMsg=TICRequest.CreateClass(iCourseID);
                                    }
                                    else
                                    {
                                        response.iResult = 0;
                                    }

                                }
                                else
                                {
                                    response.iResult = -1;
                                    response.strMsg = "流量扣除失败，请重新再试！";
                                }
                            }
                        }
                    }
                    else
                    {
                        response.iResult = -1;
                        response.strMsg = "课程已结束！";
                    }

                }
                else
                {
                    response.iResult = -1;
                    response.strMsg = "未购买课程，请先购买再进入课堂";
                }
            }
            else
            {
                response.iResult = -1;
                response.strMsg = "课程已结束！";
            }
            JsonResult jr = new JsonResult();
            jr.Data = response;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }


        #endregion

        #region Start


        public JsonResult CheckOnlineClass(string strCourseNo, string strRole = "Student")
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            ResponseCourseModel response = new ResponseCourseModel();
            //if (strCourseNo.Length > 7)
            //{
            //    string strCourseID = strCourseNo.Substring(5, strCourseNo.Length - 5);
            string strCourseID = ClassRoomBll.GetCourseByClassID(strCourseNo).fID.ToString();
            ClassRoomModel cr = ClassRoomBll.GetClassRoomByCourseId(Convert.ToInt32(strCourseID), userInfo.fUserName);
            if (cr.fClassType == "OnLine")
            {
                GroupUserInfoListModel ListModel = GroupUserBll.GetGroupUserInfiList(strCourseNo);
                if (ListModel.infoList != null && ListModel.infoList.Count > cr.fMaxNumber)
                {
                    response.iResult = -1;
                    response.strMsg = "课堂已达到最大人数，不能进入！";
                }
                else
                {
                    decimal userAccount = UserBll.GetUserAccountAmount(userInfo.fUserName);
                    decimal classRoomFlow = ClassRoomBll.GetClassRoomFlow(Convert.ToInt32(strCourseID));
                    decimal leftFlow = UserBll.GetUserLeftFlow(userInfo.fUserName);

                    if (userAccount < classRoomFlow && strRole == "Teacher")
                    {
                        response.iResult = -1;
                        if (leftFlow > 0)
                        {
                            response.strMsg = "该课时最少需要" + classRoomFlow.ToString() + "分钟流量，您的账户流量欠费" + leftFlow.ToString() + "分钟。请先去购买流量";
                        }
                        else
                        {
                            response.strMsg = "该课时最少需要" + classRoomFlow.ToString() + "分钟流量，您的账户流量剩余" + userAccount.ToString() + "分钟。请先去购买流量";
                        }
                    }
                    else
                    {


                        CourseModel course = ClassRoomBll.GetCourseById(Convert.ToInt32(strCourseID), userInfo.fUserName);

                        if (course != null && course.fClassDate.AddMinutes(course.fClassDateLength) > DateTime.Now)
                        {
                            if (course != null && strRole == "Student" && course.fPrice > 0 && course.IsBuy == 0)
                            {
                                response.iResult = 0;
                                response.strMsg = "未购买";
                                response.CourseID = course.fID;
                                response.CourseNo = strCourseNo;
                                response.ClassTitle = course.fCourseTitle;
                                response.ClassDate = course.fClassDate.ToString();
                                response.ClassLength = course.fClassDateLength.ToString();
                                response.Price = course.fPrice.ToString();
                            }
                            else
                            {
                                response.iResult = Convert.ToInt32(strCourseID);
                                response.strMsg = "";
                            }
                        }
                        else
                        {
                            response.iResult = -1;
                            response.strMsg = "课程已结束！";
                        }
                    }
                }
            }
            else
            {
                response.iResult = -1;
                response.strMsg = "课堂号错误！";
            }
            //}
            //else
            //{
            //    response.iResult = -1;
            //    response.strMsg = "课堂号错误！";
            //}
            JsonResult jr = new JsonResult();
            jr.Data = response;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }


        #endregion

        #region File



        public JsonResult UploadFileSuccess(string strkey, string strFileType, string strFileName)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            ResponseCourseModel response = new ResponseCourseModel();

            var endpoint = "oss-cn-hangzhou.aliyuncs.com";
            var accessKeyId = "LTAI0W5pqyqDXHhs"; //LTAIlLsb3W0Idk6a
            var accessKeySecret = "c2sUv3Lf3hNr1DSsQdb3KqYcMQiGlD "; //EGCWeEQlwLqLaIGZRYrfEmcpPInQCV 
            var bucketName = "aizhu-ducation";

            OssClient ossClient = new OssClient(endpoint, accessKeyId, accessKeySecret);
            // 获取文件元信息。
            var oldMeta = ossClient.GetObjectMetadata(bucketName, strkey);
            ResourseModel model = new ResourseModel();
            model.fCreateDate = DateTime.Now;
            model.fCreateOpr = userInfo.fUserName;
            model.fFileType = oldMeta.ContentType;
            model.fIsDownLoad = false;
            model.fIsTrySee = false;
            model.fPayIsDown = false;
            model.fResourceCode = Guid.NewGuid().ToString();
            model.fResourceTitle = strFileName;
            model.fSize = Convert.ToInt32(oldMeta.ContentLength);
            if (oldMeta.ContentType.Split('/')[0] == "image")
            {
                model.fCoverImg = string.Format("http://{0}.oss.aliyuncs.com/{1}?x-oss-process=style/fang", bucketName, strkey);
            }
            else if (oldMeta.ContentType.Split('/')[0] == "video")
            {
                model.fCoverImg = "https://aizhu-ducation.oss-cn-hangzhou.aliyuncs.com/WebImage/%E5%BE%AE%E4%BF%A1%E5%9B%BE%E7%89%87_20181221120133.png?x-oss-process=style/fang";
            }
            else if (oldMeta.ContentType.Split('/')[0] == "application")
            {
                model.fCoverImg = "https://aizhu-ducation.oss-cn-hangzhou.aliyuncs.com/WebImage/%E5%BE%AE%E4%BF%A1%E5%9B%BE%E7%89%87_20181221120133.png?x-oss-process=style/fang";
            }

            model.fStatus = "已上传";
            model.fType = strFileType;
            model.fUrl = strkey;
            model.fUserName = userInfo.fUserName;
            ResourceBll.InsertResource(model);

            JsonResult jr = new JsonResult();
            jr.Data = response;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }

        #endregion

        public static object Deserialize(Type type, string xml)
        {
            try
            {
                using (StringReader sr = new StringReader(xml))
                {
                    XmlSerializer xmldes = new XmlSerializer(type);
                    return xmldes.Deserialize(sr);
                }
            }
            catch (Exception e)
            {

                return null;
            }
        }
    }
}
