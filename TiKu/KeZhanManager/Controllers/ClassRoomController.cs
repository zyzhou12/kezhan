using KeZhanManager.Filters;
using KeZhanManager.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TiKu.Dal;
using TiKu.Entity;
using Trip8H.Common;

namespace KeZhanManager.Controllers
{
    [LoginActionFilter]
    public class ClassRoomController : Controller
    {
        //
        // GET: /ClassRoom/

        public ActionResult ClassRoomQuery()
        {
            return View();
        }

        public ActionResult GetClassRoomList(string strBeginDate, string strEndDate, string strClassRoomTitle)
        {
            List<tClassRoomEntity> classRoomList = tClassRoomDal.ClassRoomListQuery(strBeginDate,strEndDate,strClassRoomTitle);
            ClassRoomListModel model = new ClassRoomListModel();
            if (classRoomList != null)
            {
                model.iCount = classRoomList.Count;
                model.classRoomList = classRoomList;
            }
            return PartialView("ClassRoomList", model);
        }

        public ActionResult ClassRoomSendQuery()
        {
            return View();
        }

        public ActionResult GetClassRoomApplyList(string strBeginDate, string strEndDate, string strClassRoomTitle,string strStatus)
        {

            List<tClassRoomApplyEntity> classRoomApplyList = tClassRoomApplyDal.GetClassRoomApplyList(strBeginDate, strEndDate, strClassRoomTitle, strStatus);
            ClassRoomApplyListModel model = new ClassRoomApplyListModel();
            if (classRoomApplyList != null)
            {
                model.iCount = classRoomApplyList.Count;
                model.classRoomApplyList = classRoomApplyList;
            }
            return PartialView("ClassRoomApplyList", model);
        }

        public ActionResult ClassRoomOffLineQuery()
        {
            return View();
        }

        public ActionResult ClassRoomGroupQuery()
        {
            return View();
        }
        public ActionResult GetClassRoomGroupList(string strBeginDate, string strEndDate, string strUserName)
        {

            DataTable dt = tGroupDal.GettGroupList(strBeginDate, strEndDate, strUserName);
            List<GroupModel> groupList = PubFun.DataTableToObjects<GroupModel>(dt);
            GroupListModel model = new GroupListModel();
            if (groupList != null)
            {
                model.iCount = groupList.Count;
                model.groupList = groupList;
            }
            return PartialView("GroupList", model);
        }

        public ActionResult ClassRoomApply(int iApplyID)
        {
            tClassRoomApplyEntity model= tClassRoomApplyDal.GettClassRoomApply(iApplyID);
            return View(model);
        }

        public JsonResult DoApplyAgree(int iApplyID, string strApplyNote)
        {
            tUserEntity userInfo = Code.Fun.GetSessionUserInfo(this);
            ResponseBaseModel response = new ResponseBaseModel();
            string strMsg = null;
            response.iResult = tClassRoomApplyDal.ClassRoomApplyAgree(iApplyID, strApplyNote, userInfo.fUserName, DateTime.Now, ref strMsg);

            JsonResult jr = new JsonResult();
            jr.Data = response;
            return jr;
        }

        public JsonResult DoApplyRefuse(int iApplyID, string strApplyNote)
        {
            tUserEntity userInfo = Code.Fun.GetSessionUserInfo(this);
            ResponseBaseModel response = new ResponseBaseModel();
            string strMsg = null;
            response.iResult = tClassRoomApplyDal.ClassRoomApplyRefuse(iApplyID, strApplyNote, userInfo.fUserName, DateTime.Now, ref strMsg);

            JsonResult jr = new JsonResult();
            jr.Data = response;
            return jr;
        }

        public JsonResult DoDestroyClass(string strGroupId)
        {
            ResponseBaseModel response = new ResponseBaseModel();
            
            Destroy_group(strGroupId);

            JsonResult jr = new JsonResult();
            jr.Data = response;
            return jr;
        }

        /// <summary>
        /// 销毁课堂
        /// </summary>
        /// <param name="strGroupId"></param>
        public void Destroy_group(string strGroupId)
        {
            Random rand = new Random();
            StringBuilder sb = new StringBuilder();
            for (int i = 1; i <= 32; i++)
            {
                int randNum = rand.Next(9) + 1;
                String num = randNum + "";
                sb = sb.Append(num);
            }
            String random = sb.ToString();

            string strUrl = string.Format("https://console.tim.qq.com/v4/group_open_http_svc/destroy_group?usersig={0}&identifier={1}&sdkappid={2}&random={3}&contenttype=json", "eJxlj81Og0AYRfc8BWFtdH6YlJp0gbYKpiQaalPZkJEZ8MOUgZmhrW18dyPWSOLdnpN7c0*O67reaple8qJQfWNz*9FKz712PeRd-MG2BZFzm1Mt-kF5aEHLnJdW6gFixhhBaOyAkI2FEs4GF1towFjNrdIjzYj3fNj66fERwpOABdOxAtUAk8XzbRy9vFVwtA*PEcN9TFc89LskocfX4t4v*TwV6-0d3WB-sxMhLELDg7kiKTGRikxfP10texTd1EqldWYO-jreV12msrqboNlo0sJW-h6jmAVkOr62k9qAagaBIMwwoeg7nvPpfAFRuGD7", "administrator", 1400178589, random);
            string strBody = "{\"GroupId\": \"" + strGroupId + "\"}";

            byte[] postData = Encoding.UTF8.GetBytes(strBody);
            WebClient MyWebClient = new WebClient();

            MyWebClient.Credentials = CredentialCache.DefaultCredentials;
            Byte[] pageData = MyWebClient.UploadData(strUrl, "post", postData);

            String strJson = Encoding.UTF8.GetString(pageData) ?? "";

            JavaScriptSerializer jss = new JavaScriptSerializer();

            GroupDestoryResponseModel response = jss.Deserialize<GroupDestoryResponseModel>(strJson);
            if (response.ActionStatus == "OK")
            {
                tGroupEntity entity = tGroupDal.GettGroup(strGroupId);
                string strUpdateFiels = "fIsValid,fDestoryDate";
                entity.fIsValid = false;
                entity.fDestoryDate = DateTime.Now;

                List<tGroupUserInfoEntity> userList = tGroupUserInfoDal.GetGroupUserList(strGroupId);

                string strUserUpdateFiels = "";
                foreach (tGroupUserInfoEntity user in userList)
                {
                    if (user.fIsOnLine)
                    {
                        strUserUpdateFiels = "fIsOnLine,fIsPush,fIsVideo,fIsAudio,fIsBorad,fLastQuitTime";
                        user.fIsOnLine = false;
                        user.fIsPush = false;
                        user.fIsVideo = false;
                        user.fIsAudio = false;
                        user.fIsBorad = false;
                        user.fLastQuitTime = DateTime.Now;

                        tGroupUserJoinHistoryEntity histoty = tGroupUserJoinHistoryDal.GettGroupUserJoinHistory(user.fUserId, strGroupId);
                        histoty.fQuitTime = DateTime.Now;
                        List<tGroupUserJoinHistoryEntity> hisList = new List<tGroupUserJoinHistoryEntity>();
                        hisList.Add(histoty);
                        tGroupUserJoinHistoryDal.Modify(hisList, "update", "fID,fQuitTime", null);
                    }
                    else
                    {
                        strUserUpdateFiels = "fIsPush,fIsVideo,fIsAudio,fIsBorad";
                        user.fIsPush = false;
                        user.fIsVideo = false;
                        user.fIsAudio = false;
                        user.fIsBorad = false;
                    }
                    tGroupUserInfoDal.Modify(userList, "update", "fID," + strUserUpdateFiels, "fUserId,fGroupId");
                }
                List<tGroupEntity> list = new List<tGroupEntity>();
                list.Add(entity);
                int i = tGroupDal.Modify(list, "update", "fID," + strUpdateFiels, "fGroupId");
            }

        }

    }
}
