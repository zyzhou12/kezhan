using KeZhan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiKu.Bll;
using TiKuBll;
using TiKuBll.Model;

namespace KeZhan.Controllers
{
    public class AiZhuPayController : Controller
    {
        //
        // GET: /AiZhuPay/


        [HttpPost]
        public JsonResult BookingPay(string strBookingType,string strBookingNo, string strTradePass)
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
                response.iResult = BookingBll.BookingPay(userInfo.fUserName, strTradePass, strBookingType, strBookingNo, "web", ref strMsg);
                response.strMsg = strMsg;
            }
            JsonResult jr = new JsonResult();
            jr.Data = response;
            return jr;
        }

        public string RefundAmount(int iRefundID,string outtradeno, string note, int refundfee,string oprUser)
        {
            string strMsg = "";
            int i = UserBll.UserAmountRefund(iRefundID, outtradeno, refundfee, note, oprUser, ref strMsg);
            if(i==0)
            {
                strMsg = "SUCCESS";
            }
            return strMsg;
        }
    }
}
