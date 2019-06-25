using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeZhan.Controllers
{
    public class HtmlController : Controller
    {
        //
        // GET: /Html/

        public ActionResult WebInfo()
        {
            return View();
        }
        public ActionResult QAInfo()
        {
            return View();
        }
        public ActionResult TeacherQAInfo()
        {
            return View();
        }
        public ActionResult About()
        {
            return View();
        }

        public ActionResult KeZhanDesc()
        {
            return View();
        }
        public ActionResult KeZhanTeachDesc()
        {
            return View();
        }



        public ActionResult UpdateRole()
        {
            return View();
        }


    }
}
