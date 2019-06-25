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
    public class BookingController : Controller
    {
        //
        // GET: /Booking/

        public ActionResult BookingQuery()
        {
            return View();
        }

        public ActionResult GetBookingList(string strBeginDate, string strEndDate, string strUserName, string strClassCode)
        {

            List<tBookingEntity> bookingList = tBookingDal.GetBookingList(strBeginDate, strEndDate, strUserName, strClassCode);
            BookingListModel model = new BookingListModel();
            if (bookingList != null)
            {
                model.iCount = bookingList.Count;
                model.bookingList = bookingList;
            }
            return PartialView("BookingList", model);
        }
        

        public ActionResult UserPayQuery()
        {
            return View();
        }

        public ActionResult GetUserPayList(string strBeginDate, string strEndDate, string strUserName)
        {

            List<tUserPayEntity> payList = tUserPayDal.GettUserPayList(strBeginDate, strEndDate, strUserName);
            UserPayListModel model = new UserPayListModel();
            if (payList != null)
            {
                model.iCount = payList.Count;
                model.payList = payList;
            }
            return PartialView("UserPayList", model);
        }
        public ActionResult UserRefundQuery()
        {
            return View();
        }

        public ActionResult GetUserRefundList(string strBeginDate, string strEndDate, string strUserName, string strClassCode)
        {

            List<tUserRefundEntity> refundList = tUserRefundDal.UserRefundListQuery(strBeginDate, strEndDate, strUserName, strClassCode);
            UserRefundListModel model = new UserRefundListModel();
            if (refundList != null)
            {
                model.iCount = refundList.Count;
                model.refundList = refundList;
            }
            return PartialView("UserRefundList", model);
        }
    }
}
