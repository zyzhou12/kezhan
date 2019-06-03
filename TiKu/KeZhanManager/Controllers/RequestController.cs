using KeZhanManager.Filters;
using KeZhanManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiKu.Dal;
using TiKu.Entity;

namespace KeZhanManager.Controllers
{
    [LoginActionFilter]
    public class RequestController : Controller
    {
        //
        // GET: /Request/
        public JsonResult GetMessageCount()
        {
            ResponseBaseModel response = new ResponseBaseModel();
            //UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            //if (userInfo == null)
            //{
            //    response.iResult = -9999;
            //}
            //else
            //{
            //    MessageListModel model = UserBll.GetMessageList(userInfo.fUserName, "0");
            //    response.iResult = model.messageList.Count;
            //}

            JsonResult jr = new JsonResult();
            jr.Data = response;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }

        public JsonResult DoSaveSystemConfig(tSystemConfigEntity config)
        {
            ResponseBaseModel response = new ResponseBaseModel();
            List<tSystemConfigEntity> list=new List<tSystemConfigEntity>();
            list.Add(config);
            if(config.fID>0){
                response.iResult = tSystemConfigDal.Modify(list, "update", null, null);
            }else{
                response.iResult = tSystemConfigDal.Modify(list, "insert", null, null);
            }
            JsonResult jr = new JsonResult();
            jr.Data = response;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }

    }
}
