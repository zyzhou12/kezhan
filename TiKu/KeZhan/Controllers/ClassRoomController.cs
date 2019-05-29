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

        public ActionResult ClassRoomManager(string strStatus = null, string strPayType = null, string strType = null)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            TeacherClassRoomListModel model = new TeacherClassRoomListModel();
            ClassRoomListModel list = ClassRoomBll.GetClassRoomByCreateOpr(userInfo.fUserName, strStatus, strPayType,  strType);
            model.strUserName = userInfo.fUserName;
            model.strStatus = strStatus;
            model.strPayType = strPayType;
            model.strType = strType;
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


        public ActionResult ClassRoomCourse(string strClassRoomCode)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);

            ClassRoomModel model = ClassRoomBll.GetClassRoomDetail(strClassRoomCode, userInfo.fUserName); ;

            return View(model);
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





        public ActionResult OpenClassRoom(int iCourseID)
        {

            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            ClassRoomModel classRoom = ClassRoomBll.GetClassRoomByCourseId(iCourseID, userInfo.fUserName);
            int isBuy = BookingBll.GetCourseIsBuy(iCourseID, userInfo.fUserName);
            if (isBuy > 0 || classRoom.fTecharUserName == userInfo.fUserName)
            {
                CourseModel course= ClassRoomBll.GetCourseByID(iCourseID);
                MediaListModel model = ClassRoomBll.GetCourseMediaList(course.fResourceUrl);
                if (model.MediaList == null || model.MediaList.Count <= 0)
                {
                    return RedirectToAction("Message", "Home", new { strMessage = "视频加载失败" });
                }
                else
                {
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
            }
            else
            {
                return RedirectToAction("Message", "Home", new { strMessage = "观看时效到期或未购买" });
            }
        }

        public ActionResult OpenFreeClassRoom(int iCourseID)
        {

            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            ClassRoomModel classRoom = ClassRoomBll.GetClassRoomByCourseId(iCourseID, userInfo.fUserName);
            
            CourseModel course = ClassRoomBll.GetCourseByID(iCourseID);
            MediaListModel model = ClassRoomBll.GetCourseMediaList(course.fResourceUrl);
            FileModel file = new FileModel();
            file.FreeDateLength = classRoom.fFeeLength;
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


    }
}
