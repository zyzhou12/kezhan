using KeZhanManager.Filters;
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
    public class SystemController : Controller
    {
        //
        // GET: /System/

        public ActionResult ConfigSet()
        {
            tSystemConfigEntity config = tSystemConfigDal.GettSystemConfigByCity("上海");
            return View(config);
        }

        

        public ActionResult DictManager()
        {
            return View();
        }

    }
}
