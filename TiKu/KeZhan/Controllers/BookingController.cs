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
using System.IO;
using System.Xml.Serialization;

namespace KeZhan.Controllers
{
    [WeiXingActionFilter]
    public class BookingController : Controller
    {
        //
        // GET: /Booking/
        public ActionResult BookingList(string strStatus = null)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            BookingListModel model = BookingBll.GetBookingList(userInfo.fUserName, strStatus, DateTime.Now, DateTime.Now);

            return View(model);
        }




        public ActionResult UserBooking(string strBookingNo)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            BookingModel model = BookingBll.GetBookingByNo(strBookingNo);
            model.UserAccountAmount = UserBll.GetUserAccountAmount(userInfo.fUserName);
            model.UserName = userInfo.fUserName;


            return View(model);
        }


        public ActionResult UserRefundList(string strUserName = "", string strStatus = "0")
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            UserRefundListModel model = BookingBll.GetBookingRefundList(strUserName, strStatus, userInfo.fUserName);

            return View(model);
        }


        public ActionResult UserRefund(int iRefundID)
        {
            UserRefundModel model = BookingBll.GetBookingRefund(iRefundID);
            return View(model);
        }

        [HttpPost]
        public JsonResult DoConfrimRefund(int refundID, string strPayType, decimal refundPrice, string applyStatus, string applyNote)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            ResponseBaseModel response = new ResponseBaseModel();

            string strMessage = "";
            try
            {
                UserRefundModel refund = BookingBll.GetBookingRefund(refundID);

                //业务订单号转换成支付订单号
                //select @BookingNo=fOrderNo  from tuserpay where fType='BuyClass' and fPayNo is not null and fRemark=@BookingNo
                string strPayNo = BookingBll.GetPayNoByBookingNo(refund.fBookingNo);
                BookingModel booking=BookingBll.GetBookingByNo(refund.fBookingNo);

                if (applyStatus == "1")
                {
                    bool issuccess = false;

                    if (refund.fPayType == "weixinpay")
                    {
                        WeiXinPayController pay = new WeiXinPayController();
                        string RefundResult = pay.RefundAmount(strPayNo, refund.fOrderNo, Convert.ToInt32(booking.fAmount * 100), Convert.ToInt32(refund.fApplyAmount * 100), Request.UserHostAddress);


                        xml result = Deserialize(typeof(xml), RefundResult) as xml;
                        if (result.result_code == "SUCCESS")
                        {
                            issuccess = true;
                        }
                        else
                        {
                            if (result.return_code == "SUCCESS")
                            {
                                strMessage = "退款失败，" + result.err_code_des;
                            }
                            else
                            {
                                strMessage = "退款失败，" + result.return_msg;
                            }

                            response.iResult = -1;
                        }
                    }
                    else if (refund.fPayType == "alipay")
                    {
                        AliPayController pay = new AliPayController();
                        string RefundResult = pay.RefundAmount(strPayNo, refund.fOrderNo, refund.fApplyAmount);

                        if (RefundResult == "SUCCESS")
                        {
                            issuccess = true;
                        }
                        else
                        {
                            strMessage = RefundResult;

                            response.iResult = -1;
                        }
                    }
                    else if (refund.fPayType == "aizhupay")
                    {
                        AiZhuPayController pay = new AiZhuPayController();
                        string RefundResult = pay.RefundAmount(refund.fID, refund.fBookingNo, "applyNote", Convert.ToInt32(refund.fRefundAmount), userInfo.fUserName);

                        if (RefundResult == "SUCCESS")
                        {
                            issuccess = true;
                        }
                        else
                        {
                            strMessage = RefundResult;
                            response.iResult = -1;
                        }
                    }


                    if (issuccess)
                    {
                        response.iResult = BookingBll.ConfirmUserRefund(refundID, refundPrice, applyNote, userInfo.fUserName, ref strMessage);

                        strMessage = "退款成功";

                    }
                }
                else
                {

                    strMessage = "退款已驳回";
                    response.iResult = BookingBll.RefusedUserRefund(refundID, applyNote, userInfo.fUserName);


                }

            }
            catch (Exception ex)
            {
                response.iResult = -1;
                strMessage = ex.Message;
            }
            response.strMsg = strMessage;
            JsonResult jr = new JsonResult();
            jr.Data = response;
            return jr;
        }


        public static object Deserialize(Type type, string xml)
        {
            try
            {
                using (StringReader sr = new StringReader(xml))
                {
                    XmlSerializer xmldes = new XmlSerializer(type);
                    return xmldes.Deserialize(sr);
                }
            }
            catch (Exception e)
            {

                return null;
            }
        }

    }
}
