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
    public class FileController : Controller
    {
        //
        // GET: /File/

        public ActionResult MediaQuery()
        {
            return View();
        }

        public ActionResult GetMediaList(string strBeginDate, string strEndDate, string strName)
        {

            List<tMediaEntity> mediaList = tMediaDal.GettMediaList(strBeginDate, strEndDate, strName);
            MediaListModel model = new MediaListModel();
            if (mediaList != null)
            {
                model.iCount = mediaList.Count;
                model.mediaList = mediaList;
            }
            return PartialView("MediaList", model);
        }

        public ActionResult ResourceQuery()
        {
            return View();
        }
        public ActionResult GetFileList(string strBeginDate, string strEndDate, string strName)
        {

            List<tResourceEntity> fileList = tResourceDal.GetResourceList(strBeginDate, strEndDate, strName);
            ResourceListModel model = new ResourceListModel();
            if (fileList != null)
            {
                model.iCount = fileList.Count;
                model.fileList = fileList;
            }
            return PartialView("FileList", model);
        }
    }
}
