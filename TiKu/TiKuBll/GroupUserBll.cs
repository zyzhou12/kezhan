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
            tGroupEntity entity = tGroupDal.GettGroup( strGroupId);
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

        public static int InsertGroupUserInfo(string strUserId, string strGroupId,string strRole)
        {
            if (strRole == "teacher")
            {
                tGroupEntity group = new tGroupEntity();
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
            return i;
        }

        public static int DeleteGroupUserInfo(string strUserId,string strGroupId)
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
            else if (strType == "closegroup")
            {
                strUpdateFiels = "fIsValid";
                entity.fIsValid = false;
            }

            List<tGroupEntity> list = new List<tGroupEntity>();
            list.Add(entity);
            int i = tGroupDal.Modify(list, "update", "fID," + strUpdateFiels, "fGroupId");
            return i;
        }

        public static int UpdateGroupUserInfo(string strUserId,string strGroupId,string strType)
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
            string strUpdateFiels="";
            if (strType == "online")
            {
                strUpdateFiels = "fIsOnLine,fLastJoinTime";
                entity.fIsOnLine = true;
                entity.fLastJoinTime = DateTime.Now;
            }
            else if (strType == "offline")
            {
                strUpdateFiels = "fIsOnLine,fIsPush,fIsVideo,fIsAudio,fIsBorad";
                entity.fIsOnLine = false;
                entity.fIsPush = false;
                entity.fIsVideo = false;
                entity.fIsAudio = false;
                entity.fIsBorad = false;
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
            int i = tGroupUserInfoDal.Modify(list, "update", "fID,"+strUpdateFiels, "fUserId,fGroupId");
            return i;
        }
    }
}
