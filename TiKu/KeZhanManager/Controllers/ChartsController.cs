using KeZhanManager.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeZhanManager.Controllers
{
    [LoginActionFilter]
    public class ChartsController : Controller
    {
        //
        // GET: /Charts/

        public ActionResult FlowChart()
        {
            return View();
        }
        public ActionResult AccountChart()
        {
            return View();
        }
        public ActionResult Charts1()
        {
            return View();
        }
        public ActionResult Charts2()
        {
            return View();
        }
        public ActionResult Charts3()
        {
            return View();
        }
        public ActionResult Charts4()
        {
            return View();
        }
        public ActionResult Charts5()
        {
            return View();
        }
        public ActionResult Charts6()
        {
            return View();
        }
        public ActionResult Charts7()
        {
            return View();
        }
    }
}
