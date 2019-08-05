using KeZhan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using TiKu.Bll;
using TiKuBll;
using TiKuBll.Model;

namespace KeZhan.Code
{
    public class TICRequest
    {
        /// <summary>
        /// 获取接口地址
        /// </summary>
        /// <param name="strType"></param>
        /// <returns></returns>
        public static string GetUrl(string strType)
        {
            string strUrl = "";
            string sdkappid = "1400178589";
            string tickey = "mr2vZXozWioy6kE8I3XdwVqcCuErq38o";
            string random = new Random().Next(10000, 99999).ToString();
            string expireTime = (GetTimeStamp(DateTime.UtcNow) + 120).ToString(); ;
            string sign = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.Default.GetBytes(tickey + expireTime))).Replace("-", "").ToLower();

            if (strType == "classcreate")//预约课堂
            {
                strUrl = "https://iclass.api.qcloud.com/paas/v1/class/create?sdkappid={0}&random={1}&expire_time={2}&sign={3}";
            }
            else if (strType == "classdelete")
            {
                strUrl = "https://iclass.api.qcloud.com/paas/v1/class/delete?sdkappid={0}&random={1}&expire_time={2}&sign={3}";
            }
            else if (strType == "userregister")//注册账号
            {
                strUrl = "https://iclass.api.qcloud.com/paas/v1/user/register?sdkappid={0}&random={1}&expire_time={2}&sign={3}";
            }
            else if (strType == "usermodify")//修改账号信息
            {
                strUrl = "https://iclass.api.qcloud.com/paas/v1/user/profile/modify?sdkappid={0}&random={1}&expire_time={2}&sign={3}";
            }
            else if (strType == "usertokenupdate")//更新账号票据
            {
                strUrl = "https://iclass.api.qcloud.com/paas/v1/user/token/update?sdkappid={0}&random={1}&expire_time={2}&sign={3}";
            }
            else if (strType == "documentadd")//添加课件
            {
                strUrl = "https://iclass.api.qcloud.com/paas/v1/document/add?sdkappid={0}&random={1}&expire_time={2}&sign={3}";
            }
            else if (strType == "documentdelete")//删除课件
            {
                strUrl = "https://iclass.api.qcloud.com/paas/v1/document/delete?sdkappid={0}&random={1}&expire_time={2}&sign={3}";
            }
            strUrl = string.Format(strUrl, sdkappid, random, expireTime, sign);
            return strUrl;
        }


        public static Int64 GetTimeStamp(DateTime time)
        {

            //TimeSpan ts = time - new DateTime(1970, 1, 1, 0, 0, 0, 0,DateTimeKind.Utc);
            return Convert.ToInt64((time.ToUniversalTime().Ticks - 621355968000000000) / 10000000);
        }

        public static String GetData(string strUrl)
        {
            WebClient MyWebClient = new WebClient();


            MyWebClient.Credentials = CredentialCache.DefaultCredentials;
            Byte[] pageData = MyWebClient.DownloadData(strUrl);

            String strJson = Encoding.UTF8.GetString(pageData) ?? "";
            return strJson;

        }


        public static String PostData(string strUrl, string strBody)
        {
            WebClient MyWebClient = new WebClient();


            MyWebClient.Encoding = Encoding.UTF8;

            byte[] post = Encoding.UTF8.GetBytes(strBody);
            byte[] pageData = MyWebClient.UploadData(strUrl, "post", post);

            String strJson = Encoding.UTF8.GetString(pageData) ?? "";
            return strJson;

        }

        public static string CreateUser(UserInfoModel user)
        {
            string strUrl = GetUrl("userregister");
            string strBody = "{\"list\":[{\"user_id\":\"" + user.fUserName + "\",\"role\":\"" + user.fRole.ToLower() + "\",\"nickname\":\"https://aizhu-ducation.oss-cn-hangzhou.aliyuncs.com/" + user.fNickName + "\",\"gender\":\"male\",\"avatar\":\"" + user.fHeadImg + "\", \"phone_no\":\"" + user.fMobile + "\",\"e_mail\":\"" + user.fEmail + "\"}]}";

            string strResult = PostData(strUrl, strBody);
            JavaScriptSerializer jss = new JavaScriptSerializer();

            TICUserResponseModel response = jss.Deserialize<TICUserResponseModel>(strResult);

            if (response.error_code == 0)
            {
                foreach (TICUser ticUser in response.user_list)
                {
                    UserBll.UpdateUserToken(ticUser.user_id, ticUser.user_token);
                }
            }
            return "";
        }

        public static string UpdateUserInfo(string strUserName, string strType, string strValue)
        {
            string strUrl = GetUrl("usermodify");
            if (strType == "fMobile")
            {
                strType = "phone_no";
            }
            else if (strType == "fEmail")
            {
                strType = "e_mail";
            }
            else if (strType == "fNickName")
            {
                strType = "nickname";
            }
            else if (strType == "fHeadImg")
            {
                strType = "avatar";
            }

            string strBody = "{\"user_id\":\"" + strUserName + "\",\"" + strType + "\":\"" + strValue + "\"}";

            string strResult = PostData(strUrl, strBody);
            JavaScriptSerializer jss = new JavaScriptSerializer();

            TICBaseModel response = jss.Deserialize<TICUserResponseModel>(strResult);

            if (response.error_code == 0)
            {

            }
            return "";
        }

        public static string UpdateUserToken(UserInfoModel user)
        {
            string strUrl = GetUrl("usertokenupdate");
            string strBody = "{\"user_id\":\"" + user.fUserName + "\"}";

            string strResult = PostData(strUrl, strBody);
            JavaScriptSerializer jss = new JavaScriptSerializer();

            TICUserToken response = jss.Deserialize<TICUserToken>(strResult);

            if (response.error_code == 0)
            {

                UserBll.UpdateUserToken(user.fUserName, response.user_token);

            }
            return "";
        }

        public static string CreateClass(int iCourseID)
        {
            CourseModel course = ClassRoomBll.GetCourseByID(iCourseID);
            ClassRoomModel classroom = ClassRoomBll.GetClassRoomByCourseId(iCourseID, "");


            string strUrl = GetUrl("classcreate");
            string strBody = "{\"teacher_id\":\"" + classroom.fTecharUserName + "\",\"class_topic\": \"" + course.fDictTitle + "\",\"class_type\":\"public\",\"start_time\": " + GetTimeStamp(course.fClassDate) + ", \"stop_time\": " + GetTimeStamp(course.fClassDate.AddMinutes(course.fClassDateLength)) + ",\"admin_id\":\"administrator\",\"admin_sig\":\"eJxNjV1PgzAUhv8Ltxil42Pg3cbMQN20bsxhTJoCZTs4Si3dBI3-XSQs2bl8nvd9z4*2flxd0zStjlwR1Qqm3WqGdtVjyBhXkAOTHaRZCRxqJamq5BCgQkBGqCKmzC56dfZBetUxZBkGGru26w2SNQIkIzRX-Syyx45r-N-gT0zWUPFOjQxko5F5KRWUrC85JrItB6HzR9h1eHEX*yH2vThZvMlT0fDX*33Ii0lh4adoE6PgwZlZOjt4JV5XsvVLHO4ny3aG02Q5P77fJLDNi-Ql0sVhug2*vp-taRNF88-AS1a0xfpO*-0DDrxcrQ__\"";


            UserListModel userList = UserBll.GetClassUser(classroom.fClassRoomCode);

            if (course.fClassType.Trim() != "OnLine")
            {
                strBody += ",\"members\": [";
                if (userList.userList != null && userList.userList.Count > 0)
                    foreach (UserInfoModel user in userList.userList)
                    {
                        strBody += " {\"role\": \"student\",\"user_id\": \"" + user.fUserName + "\"},";
                    }

                strBody += "{\"role\": \"teacher\",\"user_id\": \"" + classroom.fTecharUserName + "\"}]";
            }
            strBody += "}";


            string strResult = PostData(strUrl, strBody);
            //            {
            //  "error_code":0,
            //  "error_msg":"",
            //  "class_id":100012345,
            //  "teacher_url":"https://tedu.qcloudtrtc.com/1400127140/100012345/0",
            //  "student_url":"https://tedu.qcloudtrtc.com/1400127140/100012345/1"
            //}
            JavaScriptSerializer jss = new JavaScriptSerializer();

            TICClassResponseModel response = jss.Deserialize<TICClassResponseModel>(strResult);

            if (response.error_code == 0)
            {
                ClassRoomBll.UpdateClassID(iCourseID, response.class_id);
            }

            return response.class_id;
        }

        public static string AddDocument(int iCourseID)
        {


            string strUrl = GetUrl("documentadd");
            string strBody = "{\"doc_url\": \"课件地址\",\"doc_name\":\"课件名字\",\"doc_ext\": \"ppt\",\"doc_size\":1024,\"doc_md5\": \"c4ca4238a0b923820dcc509a6f75849b\",\"permission\":\"private\",\"owner\":\"xxx\",\"is_transcode\":false}";

            string strResult = PostData(strUrl, strBody);


            JavaScriptSerializer jss = new JavaScriptSerializer();

            TICClassResponseModel response = jss.Deserialize<TICClassResponseModel>(strResult);

            if (response.error_code == 0)
            {

            }

            return response.class_id;
        }
    }
}