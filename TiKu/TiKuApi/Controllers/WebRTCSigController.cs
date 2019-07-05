using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiKu.Api.Code;

namespace TiKu.Api.Controllers
{
    public class WebRTCSigController : Controller
    {
        //
        // GET: /WebRTCSig/

        public JsonResult Sig(string strUserName)
        {
            JsonResult jr = new JsonResult();
            jr.Data=WebRTCSig.GetSig(strUserName);
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }

    }
}
