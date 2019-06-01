using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TiKu.Dal;
using TiKu.Entity;
using TiKuBll.Model;

namespace TiKuBll
{
    public class ResourceBll
    {
        public static int InsertResource(ResourseModel model)
        {
            tResourceEntity resource = new tResourceEntity();
            resource.fCoverImg = model.fCoverImg;
            resource.fCreateDate = model.fCreateDate;
            resource.fCreateOpr = model.fCreateOpr;
            resource.fDateLength = model.fDateLength;
            resource.fDownLoadPrice = model.fDownLoadPrice;
            resource.fFileType = model.fFileType;
            resource.fHeigth = model.fHeigth;
            resource.fIsDownLoad = model.fIsDownLoad;
            resource.fIsTrySee = model.fIsTrySee;
            resource.fPayIsDown = model.fPayIsDown;
            resource.fResourceCode = model.fResourceCode;
            resource.fResourceTitle = model.fResourceTitle;
            resource.fSize = model.fSize;
            resource.fStatus = model.fStatus;
            resource.fType = model.fType;
            resource.fUrl = model.fUrl;
            resource.fUserName = model.fUserName;
            resource.fWidth = model.fWidth;

            List<tResourceEntity> list = new List<tResourceEntity>();
            list.Add(resource);
            int i = tResourceDal.Modify(list, "insert", null, null);
            return i;
        }

        public static ResourseListModel GetResourseList(string strUserName,string strType)
        {
            ResourseListModel model = new ResourseListModel();
            List<tResourceEntity> list = tResourceDal.GettResourceList(strUserName, strType);
            List<ResourseModel> resourceList = new List<ResourseModel>();
            foreach(tResourceEntity entity in list)
            {
                ResourseModel resource = new ResourseModel();
                resource.fID = entity.fID;
                resource.fCoverImg = entity.fCoverImg;
                resource.fCreateDate = entity.fCreateDate;
                resource.fCreateOpr = entity.fCreateOpr;
                resource.fDateLength = entity.fDateLength;
                resource.fDownLoadPrice = entity.fDownLoadPrice;
                resource.fFileType = entity.fFileType;
                resource.fHeigth = entity.fHeigth;
                resource.fIsDownLoad = entity.fIsDownLoad;
                resource.fIsTrySee = entity.fIsTrySee;
                resource.fPayIsDown = entity.fPayIsDown;
                resource.fResourceCode = entity.fResourceCode;
                resource.fResourceTitle = entity.fResourceTitle;
                resource.fSize = entity.fSize;
                resource.fStatus = entity.fStatus;
                resource.fType = entity.fType;
                resource.fUrl = entity.fUrl;
                resource.fUserName = entity.fUserName;
                resource.fWidth = entity.fWidth;
                resourceList.Add(resource);
            }
            model.resourseList = resourceList;
            return model;
        }

        public static ResourseListModel GetDelResourseList(string strUserName)
        {
            ResourseListModel model = new ResourseListModel();
            List<tResourceEntity> list = tResourceDal.GetDelResourceList(strUserName);
            List<ResourseModel> resourceList = new List<ResourseModel>();
            foreach (tResourceEntity entity in list)
            {
                ResourseModel resource = new ResourseModel();
                resource.fID = entity.fID;
                resource.fCoverImg = entity.fCoverImg;
                resource.fCreateDate = entity.fCreateDate;
                resource.fCreateOpr = entity.fCreateOpr;
                resource.fDateLength = entity.fDateLength;
                resource.fDownLoadPrice = entity.fDownLoadPrice;
                resource.fFileType = entity.fFileType;
                resource.fHeigth = entity.fHeigth;
                resource.fIsDownLoad = entity.fIsDownLoad;
                resource.fIsTrySee = entity.fIsTrySee;
                resource.fPayIsDown = entity.fPayIsDown;
                resource.fResourceCode = entity.fResourceCode;
                resource.fResourceTitle = entity.fResourceTitle;
                resource.fSize = entity.fSize;
                resource.fStatus = entity.fStatus;
                resource.fType = entity.fType;
                resource.fUrl = entity.fUrl;
                resource.fUserName = entity.fUserName;
                resource.fWidth = entity.fWidth;
                resourceList.Add(resource);
            }
            model.resourseList = resourceList;
            return model;
        }


        public static ResourseModel GetResource(string strResourceCode)
        {
            ResourseModel model = new ResourseModel();
            tResourceEntity entity= tResourceDal.GettResource(strResourceCode);
            model.fID = entity.fID;
            model.fCoverImg = entity.fCoverImg;
            model.fCreateDate = entity.fCreateDate;
            model.fCreateOpr = entity.fCreateOpr;
            model.fDateLength = entity.fDateLength;
            model.fDownLoadPrice = entity.fDownLoadPrice;
            model.fFileType = entity.fFileType;
            model.fHeigth = entity.fHeigth;
            model.fIsDownLoad = entity.fIsDownLoad;
            model.fIsTrySee = entity.fIsTrySee;
            model.fPayIsDown = entity.fPayIsDown;
            model.fResourceCode = entity.fResourceCode;
            model.fResourceTitle = entity.fResourceTitle;
            model.fSize = entity.fSize;
            model.fStatus = entity.fStatus;
            model.fType = entity.fType;
            model.fUrl = entity.fUrl;
            model.fUserName = entity.fUserName;
            model.fWidth = entity.fWidth;
            return model;
        }

        public static ResourceInfoModel GetResourceInfo(string strUserName)
        {
            ResourceInfoModel rst = new ResourceInfoModel();
            DataTable dt = tResourceDal.GetResourceInfo(strUserName);
            if (dt.Rows.Count > 0)
            {
                Trip8H.Common.PubFun.DataRowToObject(dt.Rows[0], rst);
            }
            return rst;
        }
        public static ResourceInfoModel GetResourceInfoByClassRoomCode(string strClassRoomCode)
        {
            ResourceInfoModel rst = new ResourceInfoModel();
            DataTable dt = tResourceDal.GetResourceInfoByClassRoomCode(strClassRoomCode);
            if (dt.Rows.Count > 0)
            {
                Trip8H.Common.PubFun.DataRowToObject(dt.Rows[0], rst);
            }
            return rst;
        }
        

        public static int DeleteFile(string strResourceCode)
        {
            tResourceEntity entity = tResourceDal.GettResource(strResourceCode);
            entity.fStatus = "已删除";
            entity.fModifyDate = DateTime.Now;
            entity.fModifyOpr = "";
            List<tResourceEntity> list = new List<tResourceEntity>();
            list.Add(entity);
            int i = tResourceDal.Modify(list, "update", "fID,fStatus,fModifyDate,fModifyOpr", null);
            return i;
        }
        public static int RestoreFile(string strResourceCode)
        {
            tResourceEntity entity = tResourceDal.GettResource(strResourceCode);
            entity.fStatus = "已恢复";
            entity.fModifyDate = DateTime.Now;
            entity.fModifyOpr = "";
            List<tResourceEntity> list = new List<tResourceEntity>();
            list.Add(entity);
            int i = tResourceDal.Modify(list, "update", "fID,fStatus,fModifyDate,fModifyOpr", null);
            return i;
        }
        public static int ChangeFileType(string strResourceCode,string strType)
        {
            tResourceEntity entity = tResourceDal.GettResource(strResourceCode);
            entity.fType = strType;
            entity.fModifyDate = DateTime.Now;
            entity.fModifyOpr = "";
            List<tResourceEntity> list = new List<tResourceEntity>();
            list.Add(entity);
            int i = tResourceDal.Modify(list, "update", "fID,fType,fModifyDate,fModifyOpr", null);
            return i;
        }

        public static int DeleteResource()
        {
            return tResourceDal.DeleteResource();
        }
    }
}
