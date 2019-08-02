using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TiKu.Dal;
using TiKu.Entity;
using TiKuBll.Model;
using Trip8H.Common;

namespace TiKuBll
{
    public class GroupUserBll
    {
        public static GroupModel GetGroup(string strGroupId)
        {
            tGroupEntity entity = tGroupDal.GettGroup(strGroupId);
            GroupModel model = null;
            if (entity != null)
            {
                model = new GroupModel();
                model.fGroupID = entity.fGroupID;
                model.fID = entity.fID;
                model.fCreateDate = entity.fCreateDate;
                model.fGroupName = entity.fGroupName;
                model.fIsOpenHand = entity.fIsOpenHand;
                model.fIsValid = entity.fIsValid;
                model.fTeacherID = entity.fTeacherID;
            }
            return model;
        }

        public static GroupUserInfoModel GetGroupUserInfo(string strUserId, string strGroupId)
        {
            tGroupUserInfoEntity entity = tGroupUserInfoDal.GettGroupUserInfo(strUserId, strGroupId);
            GroupUserInfoModel model = null;
            if (entity != null)
            {
                model = new GroupUserInfoModel();
                model.fGroupID = entity.fGroupID;
                model.fID = entity.fID;
                model.fIsAudio = entity.fIsAudio;
                model.fIsBorad = entity.fIsBorad;
                model.fIsOnLine = entity.fIsOnLine;
                model.fIsVideo = entity.fIsVideo;
                model.fIsPush = entity.fIsPush;
                model.fLastJoinTime = entity.fLastJoinTime;
                model.fRole = entity.fRole;
                model.fUserId = entity.fUserId;
                model.fUserName = entity.fUserName;

                model.fNickName = entity.fNickName;
            }
            return model;
        }

        public static GroupUserInfoListModel GetGroupUserInfiList(string strGroupID)
        {
            GroupUserInfoListModel model = new GroupUserInfoListModel();
            List<GroupUserInfoModel> rst = new List<GroupUserInfoModel>();
            DataTable dt = tGroupUserInfoDal.GetGroupUserListInfo(strGroupID);
            if (dt.Rows.Count > 0)
            {
                rst = PubFun.DataTableToObjects<GroupUserInfoModel>(dt);
            }
            model.infoList = rst;
            return model;
        }

        public static int InsertGroup(string strGroupId,string strGroupName,int iCourseId,string strUserName)
        {
            tGroupEntity group = new tGroupEntity();
            group.fGroupID = strGroupId;
            group.fGroupName = strGroupName;
            group.fCourseId=iCourseId;
            group.fIsOpenHand = false;
            group.fIsValid = true;
            group.fTeacherID = strUserName;
            group.fCreateDate = DateTime.Now;
            List<tGroupEntity> groupList = new List<tGroupEntity>();
            groupList.Add(group);
            int i=tGroupDal.Modify(groupList, "insert", null, null);
            return i;
        }

        public static int InsertGroupUserInfo(string strUserId, string strGroupId, string strRole)
        {
            tGroupUserInfoEntity entity = new tGroupUserInfoEntity();
            entity.fUserId = strUserId;
            entity.fUserName = strUserId;
            entity.fGroupID = strGroupId;
            entity.fRole = strRole;
            entity.fLastJoinTime = DateTime.Now;
            entity.fIsOnLine = true;
            List<tGroupUserInfoEntity> list = new List<tGroupUserInfoEntity>();
            list.Add(entity);
            int i = tGroupUserInfoDal.Modify(list, "insert", null, null);


            tGroupUserJoinHistoryEntity history = new tGroupUserJoinHistoryEntity();
            history.fUserId = strUserId;
            history.fUserName = strUserId;
            history.fGroupID = strGroupId;
            history.fJoinTime = DateTime.Now;
            List<tGroupUserJoinHistoryEntity> historyList = new List<tGroupUserJoinHistoryEntity>();
            historyList.Add(history);
            int j = tGroupUserJoinHistoryDal.Modify(historyList, "insert", null, null);
            return i;
        }

        public static int DeleteGroupUserInfo(string strUserId, string strGroupId)
        {
            tGroupUserInfoEntity entity = tGroupUserInfoDal.GettGroupUserInfo(strUserId, strGroupId);
            int i = tGroupUserInfoDal.Delete(entity.fID.ToString());
            return i;
        }


        public static int UpdateGroup(string strGroupId, string strType)
        {

            tGroupEntity entity = tGroupDal.GettGroup(strGroupId);
            string strUpdateFiels = "";
            if (strType == "openhand")
            {
                strUpdateFiels = "fIsOpenHand";
                entity.fIsOpenHand = true;
            }
            else if (strType == "closehand")
            {
                strUpdateFiels = "fIsOpenHand";
                entity.fIsOpenHand = false;
            }
            else if (strType == "opengroup")
            {
                strUpdateFiels = "fIsValid";
                entity.fIsValid = true;
            }
            else if (strType == "closegroup")//销毁课堂
            {
                strUpdateFiels = "fIsValid,fDestoryDate";
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



            }

            List<tGroupEntity> list = new List<tGroupEntity>();
            list.Add(entity);
            int i = tGroupDal.Modify(list, "update", "fID," + strUpdateFiels, "fGroupId");
            return i;
        }

        public static int UpdateGroupUserInfo(string strUserId, string strGroupId, string strType)
        {
            tGroupEntity group = tGroupDal.GettGroup(strGroupId);
            if (group == null)
            {
                group = new tGroupEntity();
                group.fGroupID = strGroupId;
                group.fGroupName = strGroupId;
                group.fIsOpenHand = false;
                group.fIsValid = true;
                group.fTeacherID = strUserId;
                group.fCreateDate = DateTime.Now;
                List<tGroupEntity> groupList = new List<tGroupEntity>();
                groupList.Add(group);
                tGroupDal.Modify(groupList, "insert", null, null);
            }


            tGroupUserInfoEntity entity = tGroupUserInfoDal.GettGroupUserInfo(strUserId, strGroupId);
            string strUpdateFiels = "";
            if (strType == "online")
            {
                strUpdateFiels = "fIsOnLine,fLastJoinTime";
                entity.fIsOnLine = true;
                entity.fLastJoinTime = DateTime.Now;

                List<tGroupUserJoinHistoryEntity> hisList = new List<tGroupUserJoinHistoryEntity>();
                tGroupUserJoinHistoryEntity his = tGroupUserJoinHistoryDal.GettGroupUserJoinHistory(strUserId, strGroupId);
                if (his.fQuitTime == DateTime.MinValue)
                {
                    his.fQuitTime = DateTime.Now;
                    hisList.Add(his);
                    tGroupUserJoinHistoryDal.Modify(hisList, "update", "fID,fQuitTime", null);
                }

                tGroupUserJoinHistoryEntity histoty = new tGroupUserJoinHistoryEntity();
                histoty.fGroupID = strGroupId;
                histoty.fJoinTime = DateTime.Now;
                histoty.fUserId = strUserId;
                histoty.fUserName = strUserId;
                hisList = new List<tGroupUserJoinHistoryEntity>();
                hisList.Add(histoty);
                tGroupUserJoinHistoryDal.Modify(hisList, "insert", null, null);
            }
            else if (strType == "offline")
            {
                if (entity.fIsOnLine)
                {
                    strUpdateFiels = "fIsOnLine,fIsPush,fIsVideo,fIsAudio,fIsBorad,fLastQuitTime";
                    entity.fIsOnLine = false;
                    entity.fIsPush = false;
                    entity.fIsVideo = false;
                    entity.fIsAudio = false;
                    entity.fIsBorad = false;
                    entity.fLastQuitTime = DateTime.Now;

                    tGroupUserJoinHistoryEntity histoty = tGroupUserJoinHistoryDal.GettGroupUserJoinHistory(strUserId, strGroupId);
                    histoty.fQuitTime = DateTime.Now;
                    List<tGroupUserJoinHistoryEntity> hisList = new List<tGroupUserJoinHistoryEntity>();
                    hisList.Add(histoty);
                    tGroupUserJoinHistoryDal.Modify(hisList, "update", "fID,fQuitTime", null);
                }
                else
                {
                    strUpdateFiels = "fIsPush,fIsVideo,fIsAudio,fIsBorad";
                    entity.fIsPush = false;
                    entity.fIsVideo = false;
                    entity.fIsAudio = false;
                    entity.fIsBorad = false;
                }
            }
            else if (strType == "OpenPush")
            {
                strUpdateFiels = "fIsPush,fIsVideo,fIsAudio";
                entity.fIsPush = true;
                entity.fIsVideo = true;
                entity.fIsAudio = true;
            }
            else if (strType == "ClosePush")
            {
                strUpdateFiels = "fIsPush,fIsVideo,fIsAudio,fIsBorad";
                entity.fIsPush = false;
                entity.fIsVideo = false;
                entity.fIsAudio = false;
                entity.fIsBorad = false;
            }
            else if (strType == "OpenVideo")
            {
                strUpdateFiels = "fIsVideo";
                entity.fIsVideo = true;
            }
            else if (strType == "CloseVideo")
            {
                strUpdateFiels = "fIsVideo";
                entity.fIsVideo = false;
            }
            else if (strType == "OpenAudio")
            {
                strUpdateFiels = "fIsAudio";
                entity.fIsAudio = true;
            }
            else if (strType == "CloseAudio")
            {
                strUpdateFiels = "fIsAudio";
                entity.fIsAudio = false;
            }
            else if (strType == "OpenBoard")
            {
                strUpdateFiels = "fIsBoard";
                entity.fIsBorad = true;
            }
            else if (strType == "CloseBoard")
            {
                strUpdateFiels = "fIsBorad";
                entity.fIsBorad = false;
            }

            List<tGroupUserInfoEntity> list = new List<tGroupUserInfoEntity>();
            list.Add(entity);
            int i = tGroupUserInfoDal.Modify(list, "update", "fID," + strUpdateFiels, "fUserId,fGroupId");
            return i;
        }


        public static int UpdateGroupUserInfo(string strUserId, string strType)
        {


            tGroupUserInfoEntity entity = tGroupUserInfoDal.GetGroupUserInfoLast(strUserId);
            string strUpdateFiels = "";
            if (strType == "online")
            {
                strUpdateFiels = "fIsOnLine,fLastJoinTime";
                entity.fIsOnLine = true;
                entity.fLastJoinTime = DateTime.Now;

                List<tGroupUserJoinHistoryEntity> hisList = new List<tGroupUserJoinHistoryEntity>();
                tGroupUserJoinHistoryEntity his = tGroupUserJoinHistoryDal.GettGroupUserJoinHistory(strUserId, entity.fGroupID);
                if (his.fQuitTime == DateTime.MinValue)
                {
                    his.fQuitTime = DateTime.Now;
                    hisList.Add(his);
                    tGroupUserJoinHistoryDal.Modify(hisList, "update", "fID,fQuitTime", null);
                }

                tGroupUserJoinHistoryEntity histoty = new tGroupUserJoinHistoryEntity();
                histoty.fGroupID = entity.fGroupID;
                histoty.fJoinTime = DateTime.Now;
                histoty.fUserId = strUserId;
                histoty.fUserName = strUserId;
                hisList = new List<tGroupUserJoinHistoryEntity>();
                hisList.Add(histoty);
                tGroupUserJoinHistoryDal.Modify(hisList, "insert", null, null);
            }
            else if (strType == "offline")
            {
                if (entity.fIsOnLine)
                {
                    strUpdateFiels = "fIsOnLine,fIsPush,fIsVideo,fIsAudio,fIsBorad,fLastQuitTime";
                    entity.fIsOnLine = false;
                    entity.fIsPush = false;
                    entity.fIsVideo = false;
                    entity.fIsAudio = false;
                    entity.fIsBorad = false;
                    entity.fLastQuitTime = DateTime.Now;

                    tGroupUserJoinHistoryEntity histoty = tGroupUserJoinHistoryDal.GettGroupUserJoinHistory(strUserId, entity.fGroupID);
                    histoty.fQuitTime = DateTime.Now;
                    List<tGroupUserJoinHistoryEntity> hisList = new List<tGroupUserJoinHistoryEntity>();
                    hisList.Add(histoty);
                    tGroupUserJoinHistoryDal.Modify(hisList, "update", "fID,fQuitTime", null);
                }
                else
                {
                    strUpdateFiels = "fIsPush,fIsVideo,fIsAudio,fIsBorad";
                    entity.fIsPush = false;
                    entity.fIsVideo = false;
                    entity.fIsAudio = false;
                    entity.fIsBorad = false;
                }
            }

            List<tGroupUserInfoEntity> list = new List<tGroupUserInfoEntity>();
            list.Add(entity);
            int i = tGroupUserInfoDal.Modify(list, "update", "fID," + strUpdateFiels, "fUserId,fGroupId");
            return i;
        }
    }
}
