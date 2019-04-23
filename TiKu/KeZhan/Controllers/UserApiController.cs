using KeZhan.Code;
using KeZhan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TiKuBll;
using TiKuBll.Model;

namespace KeZhan.Controllers
{
    public class UserApiController : Controller
    {
        //
        // GET: /UserApi/


        public UserInfoModel GetLoginUser()
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            return userInfo;
        }

        public JsonResult CheckLoginUser()
        {
            UserInfoModel user = GetLoginUser();
            ResponseBaseModel response = new ResponseBaseModel();
            if(user==null)
            {
                response.iResult = -1;
                response.strMsg = "请重新登录";
            }
            else
            {
                response.iResult = 0;
            }

            JsonResult jr = new JsonResult();
            jr.Data = response;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }

        /// <summary>
        /// 进入课堂
        /// </summary>
        /// <param name="strUserId"></param>
        /// <param name="strGroupId"></param>
        /// <returns></returns>
        public JsonResult JoinClass(string strUserId, string strGroupId, string strRole)
        {
            GroupUserInfoModel model = GroupUserBll.GetGroupUserInfo(strUserId, strGroupId);
            if (model == null)
            {
                int i = GroupUserBll.InsertGroupUserInfo(strUserId, strGroupId, strRole);
            }
            else
            {
                int i = GroupUserBll.UpdateGroupUserInfo(strUserId, strGroupId, "online");
            }

            JsonResult jr = new JsonResult();
            jr.Data = "success";
            return jr;
        }

        /// <summary>
        /// 退出课堂
        /// </summary>
        /// <param name="strUserId"></param>
        /// <param name="strGroupId"></param>
        /// <returns></returns>
        public JsonResult QuitClass(string strUserId, string strGroupId)
        {
            int i = GroupUserBll.UpdateGroupUserInfo(strUserId, strGroupId, "offline");

            JsonResult jr = new JsonResult();
            jr.Data = "success";
            return jr;
        }

        /// <summary>
        /// 销毁课堂
        /// </summary>
        /// <param name="strGroupId"></param>
        /// <returns></returns>
        public JsonResult DestroyClass(string strGroupId)
        {
            int i = GroupUserBll.UpdateGroup(strGroupId, "closegroup");
            JsonResult jr = new JsonResult();
            jr.Data = "success";
            return jr;
        }

        public JsonResult SetGroupPer(string strGroupId, string strType)
        {
            int i = GroupUserBll.UpdateGroup(strGroupId, strType);
            JsonResult jr = new JsonResult();
            jr.Data = "success";
            return jr;
        }


        public JsonResult SetUserPer(string strUserId, string strGroupId, string strType)
        {            

           int i=GroupUserBll.UpdateGroupUserInfo(strUserId, strGroupId, strType);
            JsonResult jr = new JsonResult();
            jr.Data = "success";
            return jr;
        }

        public JsonResult GetGroup( string strGroupId)
        {
            GroupModel model = GroupUserBll.GetGroup(strGroupId);

            JsonResult jr = new JsonResult();
            jr.Data = model;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }

        /// <summary>
        /// 获取群成员
        /// </summary>
        /// <param name="strUserId"></param>
        /// <param name="strGroupId"></param>
        /// <returns></returns>
        public JsonResult GetGroupUserList(string strGroupId)
        {
            //Random rand = new Random();
            //StringBuilder sb = new StringBuilder();
            //for (int i = 1; i <= 32; i++)
            //{
            //    int randNum = rand.Next(9) + 1;
            //    String num = randNum + "";
            //    sb = sb.Append(num);
            //}
            //String random = sb.ToString();

            //string strUrl = string.Format("https://console.tim.qq.com/v4/openim/querystate?usersig={0}&identifier={1}&sdkappid={2}&random={3}&contenttype=json", UserSig.GetSig("administrator"), "administrator", 1400178589, random);
            //StringBuilder strUserIds = new StringBuilder();
            //strUserIds.Append("{\"To_Account\": [");
            //GroupUserInfoListModel model = GroupUserBll.GetGroupUserInfiList(strGroupId);
            //foreach (GroupUserInfoModel info in model.infoList)
            //{
            //    strUserIds.Append("\"" + info.fUserId + "\",");
            //}
            //strUserIds.Append("\"\"]}");

            //byte[] postData = Encoding.UTF8.GetBytes(strUserIds.ToString());
            //WebClient MyWebClient = new WebClient();

            //MyWebClient.Credentials = CredentialCache.DefaultCredentials;
            //Byte[] pageData = MyWebClient.UploadData(strUrl, "post", postData);

            //String strJson = Encoding.UTF8.GetString(pageData) ?? "";

            //JavaScriptSerializer jss = new JavaScriptSerializer();

            //GroupUserStatusModel groupUserStatusModel = jss.Deserialize<GroupUserStatusModel>(strJson);
            //if (groupUserStatusModel.ActionStatus == "OK")
            //{
            //    foreach (QueryResultModel result in groupUserStatusModel.QueryResult)
            //    {
            //        if (!string.IsNullOrEmpty(result.To_Account))
            //        {
            //            if (result.State == "Offline")
            //            {
            //                GroupUserBll.UpdateGroupUserInfo(result.To_Account, strGroupId, "offline");
            //            }
            //        }
            //    }
            //}



            GroupUserInfoListModel ListModel = GroupUserBll.GetGroupUserInfiList(strGroupId);

            JsonResult jr = new JsonResult();
            jr.Data = ListModel;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }
        /// <summary>
        /// 获取用户当前状态
        /// </summary>
        /// <param name="strUserId"></param>
        /// <param name="strGroupId"></param>
        /// <returns></returns>
        public JsonResult GetGroupUser(string strUserId, string strGroupId)
        {
            GroupUserInfoModel model = GroupUserBll.GetGroupUserInfo(strUserId, strGroupId);

            JsonResult jr = new JsonResult();
            jr.Data = model;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }
        /// <summary>
        /// 获取课时状态
        /// </summary>
        /// <param name="iCourseID"></param>
        /// <returns></returns>
        public JsonResult GerCourseStatus(int iCourseID)
        {
            CourseModel course = ClassRoomBll.GetCourseByID(iCourseID);
            ResponseBaseModel response = new ResponseBaseModel();
            if (course.fClassDate.AddMinutes(course.fClassDateLength) < DateTime.Now)
            {
                response.iResult = -1;
                response.strMsg = "课时已结束";
            }
            else
            {
                response.iResult = 0;
            }
            JsonResult jr = new JsonResult();
            jr.Data = response;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }

    }
}
