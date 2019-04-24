using KeZhan.Filters;
using KeZhan.Models;
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
            if (string.IsNullOrEmpty(model.fCourseTitle))
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

                ClassRoomModel model = ClassRoomBll.GetClassRoomDetail(strClassRoomCode, userInfo.fUserName); ;
                if (strStatus == "发布" && model.courseList.Count == 0)
                {
                    response.iResult = -1;
                    response.strMsg = "至少要一节课时才能发布";
                }
                else
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
        public JsonResult DoSubmitBookingRefund(string strOrderNo, decimal dAmount, string strRemark)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            ResponseBaseModel response = new ResponseBaseModel();
            string strMsg = "";
            int i = BookingBll.CheckRefund(dAmount, strOrderNo, ref strMsg);
            if (i >= 0)
            {
                response.iResult = BookingBll.SubmitBookingRefund(strOrderNo, userInfo.fUserName, dAmount, strRemark);
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
            else if (booking.fStatus == "已支付")
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
            response.iResult = ClassRoomBll.ClassRoomSettlement(strClassRooomCode, userInfo.fUserName, ref strMsg);
            response.strMsg = strMsg;
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
            else if (model.fIsLive && model.fVideoFee <= 0)
            {
                response.iResult = -1;
                response.strMsg = "开通一对一必须填写费用";
            }
            else if (model.fIsProblem && model.fProblemFee <= 0)
            {
                response.iResult = -1;
                response.strMsg = "开通答疑必须填写费用";
            }
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
        #endregion

        #region Teacher
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
            jr.Data = UserBll.SubmitBuyFlowOrder(userInfo.fUserName, iNum, dPrice, "购买", Convert.ToDateTime("2099-12-31"), userInfo.fUserName);
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
                        response.iResult = BookingBll.ConfirmUserRefund(refundID, refundPrice, applyNote, userInfo.fUserName, ref strMessage);

                        strMessage = "退款成功";

                    }
                }
                else
                {

                    strMessage = "退款已驳回";
                    response.iResult = BookingBll.RefusedUserRefund(refundID, applyNote, userInfo.fUserName);


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

        public JsonResult OpenClassRoom(int iCourseID)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            ClassRoomModel cr = ClassRoomBll.GetClassRoomByCourseId(iCourseID, userInfo.fUserName);
            ResponseBaseModel response = new ResponseBaseModel();
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
                                response.iResult = 0;
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
            JsonResult jr = new JsonResult();
            jr.Data = response;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }


        #endregion

        #region Start
       

        public JsonResult CheckOnlineClass(string strCourseNo)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            string strCourseID = strCourseNo.Substring(6, strCourseNo.Length - 6);
            ResponseBaseModel response = new ResponseBaseModel();
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

                    CourseModel course = ClassRoomBll.GetCourseByID(Convert.ToInt32(strCourseID));
                    if (course != null && course.fClassDate.AddMinutes(course.fClassDateLength) > DateTime.Now)
                    {
                        response.iResult = Convert.ToInt32(strCourseID);
                        response.strMsg = "";
                    }
                    else
                    {
                        response.iResult = -1;
                        response.strMsg = "课程已结束！";
                    }
                }
            }
            else
            {
                response.iResult = -1;
                response.strMsg = "课堂号错误！";
            }
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
