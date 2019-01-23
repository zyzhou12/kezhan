using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiKuBll.Model;
using KeZhan.Filters;
using KeZhan.Models;
using TiKuBll;
using TiKu.Bll;

namespace KeZhan.Controllers
{
  [WeiXingActionFilter]
  public class BookingController : Controller
  {
    //
    // GET: /Booking/
     public ActionResult BookingList(string strStatus=null)
    {
      UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
      BookingListModel model = BookingBll.GetBookingList(userInfo.fUserName, strStatus, DateTime.Now, DateTime.Now);

      return View(model);
    }
       



    public ActionResult UserBooking(string strBookingNo)
    {
      UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
      BookingModel model= BookingBll.GetBookingByNo(strBookingNo);
      model.UserAccountAmount=  UserBll.GetUserAccountAmount(userInfo.fUserName);
      model.UserName = userInfo.fUserName;
      return View(model);
    }

    [HttpPost]
    public JsonResult BookingPay(string strBookingNo, string strTradePass)
    {

      ResponseBaseModel response = new ResponseBaseModel();
      if (string.IsNullOrEmpty(strBookingNo))
      {
        response.iResult = -1;
        response.strMsg = "订单号错误";
      }
      else if (string.IsNullOrEmpty(strTradePass))
      {
        response.iResult = -1;
        response.strMsg = "交易密码错误";
      }
      else
      {
        UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
        string strMsg = null;
        response.iResult = BookingBll.BookingPay(userInfo.fUserName, strTradePass, strBookingNo,"web", ref strMsg);
        response.strMsg = strMsg;
      }
      JsonResult jr = new JsonResult();
      jr.Data = response;
      return jr;
    }


  }
}
