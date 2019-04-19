using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiKuBll.Model;
using TiKuBll;
using KeZhan.Filters;
using KeZhan.Models;
using TiKu.Bll;
using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Aliyun.OSS;
using Aliyun.OSS.Common;

namespace KeZhan.Controllers
{
    [WeiXingActionFilter]
    public class ClassRoomController : Controller
    {
        //
        // GET: /ClassRoom/

        public ActionResult ClassRoomManager(string strStatus = null, string strPayType = null)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            TeacherClassRoomListModel model = new TeacherClassRoomListModel();
            ClassRoomListModel list = ClassRoomBll.GetClassRoomByCreateOpr(userInfo.fUserName, strStatus, strPayType);
            model.strUserName = userInfo.fUserName;
            model.strStatus = strStatus;
            model.strPayType = strPayType;
            model.list = list;
            return View(model);
        }

        public ActionResult ClassRoomAdd()
        {
            ClassRoomModel model = null;
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);


            model = new ClassRoomModel();
            model.UserName = userInfo.fUserName;
            model.fMaxNumber = 100;
            model.fStatus = "保存";
            model.fCreateOpr = userInfo.fUserName;
            model.courseList = new List<CourseModel>();

            return View(model);
        }



        public ActionResult ClassRoomEdit(string strClassRoomCode, string strType = null)
        {
            ClassRoomModel model = null;
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);

            if (string.IsNullOrEmpty(strClassRoomCode))//新增
            {
                model = new ClassRoomModel();
                model.UserName = userInfo.fUserName;
                model.fMaxNumber = 1;
                model.fStatus = "保存";
                model.fCreateOpr = userInfo.fUserName;
                model.courseList = new List<CourseModel>();
            }
            else
            {
                model = ClassRoomBll.GetClassRoomDetail(strClassRoomCode, userInfo.fUserName); ;
                model.UserName = userInfo.fUserName;
                if (model.courseList == null)
                {
                    model.courseList = new List<CourseModel>();
                }
                model.showType = strType;
                //if (strType == "copy")
                //{
                //    model.fTecharUserName = "";
                //    model.fStatus = "保存";
                //    model.fID = 0;
                //    model.fClassRoomCode = "";
                //}
            }
            return View(model);
        }


        public ActionResult ClassRoomTeacher(string strClassRoomCode)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);

            ClassRoomModel model = ClassRoomBll.GetClassRoomDetail(strClassRoomCode, userInfo.fUserName); ;
            model.UserName = userInfo.fUserName;
            if (model.courseList == null)
            {
                model.courseList = new List<CourseModel>();
            }

            return View(model);
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

        public ActionResult ClassRoomCourse(string strClassRoomCode)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);

            ClassRoomModel model = ClassRoomBll.GetClassRoomDetail(strClassRoomCode, userInfo.fUserName); ;

            return View(model);
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

        public ActionResult ClassRoomShow(string strClassRoomCode)
        {
            ClassRoomModel model = null;
            if (string.IsNullOrEmpty(strClassRoomCode))
            {
                return View(model);
            }
            else
            {
                UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
                string strUserName = "";
                if (userInfo != null)//已登录
                {
                    strUserName = userInfo.fUserName;
                }
                model = ClassRoomBll.GetClassRoomDetail(strClassRoomCode, strUserName);
                model.UserName = strUserName;



                return View(model);
            }
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
            else if (model.fDeadLineDate < DateTime.Now)
            {
                response.iResult = -1;
                response.strMsg = "截止报名时间必须大于当天";
            }
            else if (string.IsNullOrEmpty(model.fClassType))
            {
                response.iResult = -1;
                response.strMsg = "请选择公开类型";
            }
            else if (string.IsNullOrEmpty(model.fPharse))
            {
                response.iResult = -1;
                response.strMsg = "请选择适应人群";
            }
            else if (string.IsNullOrEmpty(model.fSubject))
            {
                response.iResult = -1;
                response.strMsg = "请选择学科";
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


        public ActionResult DeleteCourse(int iCourseID, string strClassRoomCode, bool isEdit)
        {
            ClassRoomBll.DoDelClassRoomCourse(iCourseID);

            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            ClassRoomModel model = ClassRoomBll.GetClassRoomDetail(strClassRoomCode, userInfo.fUserName); ;

            CourseListModel courseModel = new CourseListModel();
            courseModel.courseList = model.courseList;
            courseModel.isEdit = isEdit;
            courseModel.TeacherUserName = model.fTecharUserName;

            return PartialView("CourseListControl", courseModel);
        }

        [HttpPost]
        public JsonResult DoSaveClassRoomCourse(CourseModel model)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            ResponseBaseModel response = new ResponseBaseModel();
            if (string.IsNullOrEmpty(model.fCourseTitle))
            {
                response.iResult = -1;
                response.strMsg = "请输入章节标题";
            }
            else if (model.fClassDate < DateTime.Now)
            {
                response.iResult = -1;
                response.strMsg = "上课时间必须大于当天";
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
        /// <summary>
        /// 查询介绍信息
        /// </summary>
        /// <param name="strClassRoomCode"></param>
        /// <returns></returns>
        public ActionResult QueryDescList(string strClassRoomCode, bool isEdit)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            ClassRoomModel model = ClassRoomBll.GetClassRoomDetail(strClassRoomCode, userInfo.fUserName); ;

            DescListModel descModel = new DescListModel();
            descModel.descList = model.descList;
            descModel.isEdit = isEdit;
            descModel.TeacherUserName = model.fTecharUserName;

            return PartialView("ClassRoomDescControl", descModel);
        }

        /// <summary>
        /// 查询章节信息
        /// </summary>
        /// <param name="strClassRoomCode"></param>
        /// <returns></returns>
        public ActionResult QueryCourseList(string strClassRoomCode, bool isEdit)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            ClassRoomModel model = ClassRoomBll.GetClassRoomDetail(strClassRoomCode, userInfo.fUserName); ;

            CourseListModel courseModel = new CourseListModel();
            courseModel.courseList = model.courseList;
            courseModel.isEdit = isEdit;
            courseModel.TeacherUserName = model.fTecharUserName;

            return PartialView("CourseListControl", courseModel);
        }


        public ActionResult QueryBookingList(string strClassRoomCode, string strMobile = null, string strStatus = null, string beginDate = null, string endDate = null)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            BookingListModel model = BookingBll.GetBookingList(userInfo.fUserName, strClassRoomCode, strMobile, strStatus, beginDate, endDate);
            ClassRoomModel classRoom = ClassRoomBll.GetClassRoomByCode(strClassRoomCode, "");
            model.fClassRoomCode = strClassRoomCode;
            model.fClassType = classRoom.fClassType;
            model.fStatus = classRoom.fStatus;
            model.IsManager = classRoom.fCreateOpr == userInfo.fUserName;
            // model.list = model.list.Where(m => m.fIsPay == true).ToList();
            return PartialView("BookingListControl", model);
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
        /// 申请退款
        /// </summary>
        /// <param name="strBookingNo"></param>
        /// <returns></returns>
        public ActionResult BookingRefund(string strBookingNo)
        {
            BookingModel model = BookingBll.GetBookingByNo(strBookingNo);
            model.MaxReturnAmount = BookingBll.GetBokingMaxReturnAmount(strBookingNo);
            return View(model);
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

        public ActionResult BookingListManager()
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            ClassRoomListModel model = ClassRoomBll.GetClassRoomByTeacher(userInfo.fUserName, "", "");
            return View(model);
        }


        public ActionResult ClassRoomBooking(string strClassRoomCode)
        {
            //校验是否已购买
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            BookingModel booking = BookingBll.GettBooking(userInfo.fUserName, "ClassRoom", strClassRoomCode, "已支付");
            if (booking != null)
            {
                return RedirectToAction("UserBooking", "Booking", new { strBookingNo = booking.fBookingNo });
            }
            else
            {
                ClassRoomModel model = ClassRoomBll.GetClassRoomByCode(strClassRoomCode, null);

                return View(model);
            }
        }

        [HttpPost]
        public JsonResult DoBookingClassRoom(string strClassRoomCode)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            ResponseBaseModel response = new ResponseBaseModel();
            BookingModel booking = BookingBll.GettBooking(userInfo.fUserName, "ClassRoom", strClassRoomCode, "提交");
            if (booking == null)
            {

                ClassRoomModel classRoom = ClassRoomBll.GetClassRoomByCode(strClassRoomCode, null);
                BookingListModel list = BookingBll.GetBookingList(classRoom.fTecharUserName, strClassRoomCode, null, null, null, null);
                if (classRoom.fMaxNumber > list.list.Where(m => m.fIsPay == true).ToList().Count)
                {
                    response.strMsg = BookingBll.SubmitBooking(userInfo.fUserName, "ClassRoom", strClassRoomCode, classRoom.fPrice, classRoom.fIsReturn, "Web");
                }
                else
                {
                    response.strMsg = "课程已达到最大报名人数，请选择其他课程";
                    response.iResult = -1;
                }
            }
            else
            {
                response.strMsg = "存在提交未支付订单，请不要重复提交订单";
                response.iResult = -1;
            }

            JsonResult jr = new JsonResult();
            jr.Data = response;
            return jr;
        }

        public ActionResult ValidUploadFile()
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            TeacherValidModel model = new TeacherValidModel();
            model.fUserName = userInfo.fUserName;
            TeacherBaseModel teacher = UserBll.GetUserTeacherBase(userInfo.fUserName);
            model.fStatus = teacher.fStatus;
            model.userInfo = userInfo;

            return View(model);
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

        public ActionResult OpenClassRoom(int iCourseID)
        {
            MediaListModel model = ClassRoomBll.GetCourseMediaList(iCourseID);
            FileModel file = new FileModel();
            string u = Request.ServerVariables["HTTP_USER_AGENT"];
            Regex b = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Regex v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            if ((b.IsMatch(u) || v.IsMatch(u.Substring(0, 4))))
            {
                //手机访问 Response.Redirect("http://detectmobilebrowser.com/mobile"); 
                foreach (var m in model.MediaList)
                {
                    if (m.fActivityName == "Act-ss-mp4-ld")
                    {
                        file.FileType = m.fActivityName;
                        file.FileUrl = getUrl(m.fUrl.Remove(0, 43));
                    }
                }
            }
            else
            {
                //电脑访问 
                foreach (var m in model.MediaList)
                {
                    if (m.fActivityName == "Act-ss-mp4-hd")
                    {
                        file.FileType = m.fActivityName;
                        file.FileUrl = getUrl(m.fUrl.Remove(0, 43));
                    }
                }
            }



            return View(file);
        }

        public static String getUrl(String key)
        {
            var endpoint = "oss-cn-hangzhou.aliyuncs.com";
            var accessKeyId = "LTAI0W5pqyqDXHhs"; //LTAIlLsb3W0Idk6a
            var accessKeySecret = "c2sUv3Lf3hNr1DSsQdb3KqYcMQiGlD "; //EGCWeEQlwLqLaIGZRYrfEmcpPInQCV 
            var bucketName = "kezhan";
            var signedUrl = "";
            // 创建OSSClient实例。
            var client = new OssClient(endpoint, accessKeyId, accessKeySecret);
            try
            {
                key = HttpUtility.UrlDecode(key);

                // 生成上传签名URL。
                var generatePresignedUriRequest = new GeneratePresignedUriRequest(bucketName, key);
                generatePresignedUriRequest.Expiration = DateTime.Now.AddHours(1);


                signedUrl = client.GeneratePresignedUri(generatePresignedUriRequest).ToString();


            }
            catch (OssException ex)
            {
                Console.WriteLine("Failed with error code: {0}; Error info: {1}. \nRequestID:{2}\tHostID:{3}",
                    ex.ErrorCode, ex.Message, ex.RequestId, ex.HostId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed with error info: {0}", ex.Message);
            }


            return signedUrl;
        }


        public ActionResult MyClassRoomList(string strClassType, string strStatus = null, string strPayType = null)
        {

            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);

            ClassRoomListModel model = null;
            if (strClassType == "buy")
            {
                model = ClassRoomBll.GetMyClassRoom(userInfo.fUserName);
            }
            else if (strClassType == "create")
            {
                model = ClassRoomBll.GetClassRoomByCreateOpr(userInfo.fUserName, strStatus, strPayType);
            }
            else if (strClassType == "teacher")
            {
                model = ClassRoomBll.GetClassRoomByTeacher(userInfo.fUserName, strStatus, strPayType);
            }
            model.listType = strClassType;
            if (strStatus == null)
            {
                model.strStatus = "";
            }
            else
            {
                model.strStatus = strStatus;
            }
            if (strPayType == null)
            {
                model.strPayType = "";
            }
            else
            {
                model.strPayType = strPayType;
            }
            return View(model);
        }

    }
}
