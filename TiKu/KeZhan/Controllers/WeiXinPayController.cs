using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net;
using System.IO;
using System.Text;
using WXPay;
using System.Xml;
using KeZhan.Models;
using TiKu.Bll;
using TiKuBll.Model;
using TiKuBll;
using System.Configuration;

namespace KeZhan.Controllers
{
    public class WeiXinPayController : Controller
    {

        public static string tenpay = "1";  //人民币
        //public static string partnerid = "1225312802"; //PartnerID
        //public static string partnerkey = "111111111111111111111111111111111111"; //PartnerKey

        //商户号，密钥
        //艾住
        //  public static string mchid = "1235339102"; //mchid
        //  public static string appkey = "5AEC0662D38A44DB9A7CCB13CA6E9D04"; //paysignkey(非appkey 在微信商户平台设置 (md5)111111111111) 

        public static string mchid = "1270015901"; //mchid
        public static string appkey = "4C891C0C71DE4DDC9953F3197C5CA91F";


        public static string timeStamp = ""; //时间戳 
        public static string nonceStr = ""; //随机字符串 

        public static string notify_url =ConfigurationManager.AppSettings["PayCallBack"].ToString()+"/weixinpay/call_back_url"; //支付完成后的回调处理页面,*替换成notify_url.asp所在路径

        public static string rrrcode = "";     //微信端传来的code
        public static string prepayId = "";     //预支付ID
        public static string sign = "";     //为了获取预支付ID的签名
        public static string paySign = "";  //进行支付需要的签名
        public static string package = "";  //进行支付需要的包



        public string GetUnifiedOrder(string trade_type, string sp_billno, string strBody, decimal strFee)
        {
            //TenPayModel model = new TenPayModel();
            prepayId = "";
            JsonResult rst = new JsonResult();
            try
            {
                string appId = "wx67d892b056103f43"; //appid
                // string appsecret = "af19b09cee2a4a2834ad69914d11b238"; //appsecret   


                //当前时间 yyyyMMdd
                string date = DateTime.Now.ToString("yyyyMMdd");

                //创建支付应答对象
                var packageReqHandler = new RequestHandler(HttpContext);
                //初始化
                packageReqHandler.init();

                timeStamp = TenpayUtil.getTimestamp();
                nonceStr = TenpayUtil.getNoncestr();



                //设置package订单参数
                packageReqHandler.setParameter("body", strBody); //商品信息 127字符
                packageReqHandler.setParameter("out_trade_no", sp_billno); //商家订单号
                packageReqHandler.setParameter("total_fee", Convert.ToInt32(strFee).ToString()); //商品金额,以分为单位(money * 100).ToString()
                packageReqHandler.setParameter("appid", appId);
                packageReqHandler.setParameter("mch_id", mchid);
                packageReqHandler.setParameter("nonce_str", nonceStr.ToLower());
                packageReqHandler.setParameter("notify_url", notify_url);
                //   packageReqHandler.setParameter("openid", openid);
                packageReqHandler.setParameter("spbill_create_ip", Request.UserHostAddress); //用户的公网ip，不是商户服务器IP
                packageReqHandler.setParameter("trade_type", trade_type);
                packageReqHandler.setParameter("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));
                packageReqHandler.setParameter("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));



                //获取package包
                sign = packageReqHandler.CreateMd5Sign("key", appkey);
                packageReqHandler.setParameter("sign", sign);
                string data = "";

                data = packageReqHandler.parseXML();

                WriteFile(sp_billno, "4   " + data);


                WriteFile(sp_billno, "预支付begin   " + DateTime.Now.ToString());

                string prepayXml = HttpUtil.Send(data, "https://api.mch.weixin.qq.com/pay/unifiedorder");

                WriteFile(sp_billno, "预支付end   " + DateTime.Now.ToString());
                WriteFile(sp_billno, "5   " + prepayXml);
                while (string.IsNullOrEmpty(prepayXml))
                {
                    prepayXml = HttpUtil.Send(data, "https://api.mch.weixin.qq.com/pay/unifiedorder");
                    WriteFile(sp_billno, "5   " + prepayXml);
                }
                //        <xml><return_code><![CDATA[SUCCESS]]></return_code>
                //<return_msg><![CDATA[OK]]></return_msg>
                //<appid><![CDATA[wxcdfd15d903566d66]]></appid>
                //<mch_id><![CDATA[1235339102]]></mch_id>
                //<nonce_str><![CDATA[Cg8VUTTqXa7s3arA]]></nonce_str>
                //<sign><![CDATA[C35514D2D74585671E4AE88464F475B7]]></sign>
                //<result_code><![CDATA[SUCCESS]]></result_code>
                //<prepay_id><![CDATA[wx22143604919832e41a0975bc3745441052]]></prepay_id>
                //<trade_type><![CDATA[NATIVE]]></trade_type>
                //<code_url><![CDATA[weixin://wxpay/bizpayurl?pr=lrlTex3]]></code_url>
                //</xml>
                //获取预支付ID
                var xdoc = new XmlDocument();
                xdoc.LoadXml(prepayXml);
                XmlNode xn = xdoc.SelectSingleNode("xml");
                XmlNodeList xnl = xn.ChildNodes;
                if (xnl.Count > 9)
                {
                    prepayId = xnl[9].InnerText;
                    package = string.Format("prepay_id={0}", prepayId);
                    WriteFile(sp_billno, "6   " + package);
                }

                //设置支付参数
                var paySignReqHandler = new RequestHandler(HttpContext);
                paySignReqHandler.setParameter("appId", appId);
                paySignReqHandler.setParameter("timeStamp", timeStamp);
                paySignReqHandler.setParameter("nonceStr", nonceStr);
                paySignReqHandler.setParameter("package", package);
                paySignReqHandler.setParameter("signType", "MD5");
                paySign = paySignReqHandler.CreateMd5Sign("key", appkey);


                WriteFile(sp_billno, "7   " + paySign);
                WriteFile(sp_billno, "enddate   " + DateTime.Now.ToString());



                // model.orderNo = sp_billno;
                //// model.appId = appId;
                // //获取package包
                // model.packageValue = package;
                // model.nonceStr = nonceStr;
                // model.paySign = paySign;
                // model.timeStamp = timeStamp;
            }
            catch (Exception ex)
            {
                // model.orderNo = ex.Message;
                WriteFile(sp_billno, "error   " + ex.Message);
            }
            // rst.Data = model;
            rst.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            // return rst;
            string callbackparam = Request.QueryString["callbackparam"];
            //return callbackparam + "(" + new JavaScriptSerializer().Serialize(model) + ")";
            return prepayId;
        }



        [HttpPost]
        public JsonResult UserPay(string strUserName, string strType, string strBookingNo, string strFee)
        {
            string sp_billno = UserBll.UserPay(strUserName, "weixinpay",strType, strBookingNo, Convert.ToDecimal(strFee), "web");

            ResponseBaseModel response = new ResponseBaseModel();
            response.iResult = 0;
            response.strMsg = GetUnifiedOrder("NATIVE", sp_billno, strType, Convert.ToDecimal(strFee) * 100);

            JsonResult jr = new JsonResult();
            jr.Data = response;
            return jr;
        }
       
       

        [HttpPost]
        public JsonResult UserWapPay(string strUserName, string strType, string strBookingNo, string strFee)
        {
            string sp_billno = UserBll.UserPay(strUserName, "weixinpay",strType, strBookingNo, Convert.ToDecimal(strFee), "web");

            ResponseBaseModel response = new ResponseBaseModel();
            response.iResult = 0;
            response.strMsg = GetUnifiedOrder("MWEB", sp_billno, strType, Convert.ToDecimal(strFee) * 100);

            JsonResult jr = new JsonResult();
            jr.Data = response;
            return jr;
        }
        [HttpPost]
        public JsonResult UserWeiXinPay(string openid, string strUserName, string strType, string strBookingNo, string strFee)
        {
            string sp_billno = UserBll.UserPay(strUserName, "weixinpay", strType, strBookingNo, Convert.ToDecimal(strFee), "web");


            return GetJSUnifiedOrder(openid, sp_billno, strType, Convert.ToInt32(Convert.ToDecimal(strFee) * 100));
        }
        /// <summary>
        /// 公众号支付
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="strUserName"></param>
        /// <param name="strBody"></param>
        /// <param name="strFee"></param>
        /// <returns></returns>
        public JsonResult GetJSUnifiedOrder(string openid,  string sp_billno, string strBody, int strFee)
        {

            TenPayModel model = new TenPayModel();
            JsonResult rst = new JsonResult();
            try
            {

                string appId = "wx67d892b056103f43"; //appid
                // string appsecret = "af19b09cee2a4a2834ad69914d11b238"; //appsecret


                //获取支付Openid（打开产品页面时获取存入）



                //当前时间 yyyyMMdd
                string date = DateTime.Now.ToString("yyyyMMdd");




                //创建支付应答对象
                var packageReqHandler = new RequestHandler(HttpContext);
                //初始化
                packageReqHandler.init();

                timeStamp = TenpayUtil.getTimestamp();
                nonceStr = TenpayUtil.getNoncestr();


                WriteFile(sp_billno, "3   " + openid + " " + sp_billno + " " + strFee.ToString());
                //设置package订单参数
                packageReqHandler.setParameter("body", strBody); //商品信息 127字符
                packageReqHandler.setParameter("out_trade_no", sp_billno); //商家订单号
                packageReqHandler.setParameter("total_fee", strFee.ToString()); //商品金额,以分为单位(money * 100).ToString()
                packageReqHandler.setParameter("appid", appId);
                packageReqHandler.setParameter("mch_id", mchid);
                packageReqHandler.setParameter("nonce_str", nonceStr.ToLower());
                packageReqHandler.setParameter("notify_url", notify_url);
                packageReqHandler.setParameter("openid", openid);
                packageReqHandler.setParameter("spbill_create_ip", Request.UserHostAddress); //用户的公网ip，不是商户服务器IP
                packageReqHandler.setParameter("trade_type", "JSAPI");
                packageReqHandler.setParameter("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));
                packageReqHandler.setParameter("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));



                //获取package包
                sign = packageReqHandler.CreateMd5Sign("key", appkey);
                packageReqHandler.setParameter("sign", sign);
                string data = "";

                data = packageReqHandler.parseXML();

                WriteFile(sp_billno, "4   " + data);


                WriteFile(sp_billno, "预支付begin   " + DateTime.Now.ToString());

                string prepayXml = HttpUtil.Send(data, "https://api.mch.weixin.qq.com/pay/unifiedorder");

                WriteFile(sp_billno, "预支付end   " + DateTime.Now.ToString());
                WriteFile(sp_billno, "5   " + prepayXml);
                while (string.IsNullOrEmpty(prepayXml))
                {
                    prepayXml = HttpUtil.Send(data, "https://api.mch.weixin.qq.com/pay/unifiedorder");
                    WriteFile(sp_billno, "5   " + prepayXml);
                }

                //获取预支付ID
                var xdoc = new XmlDocument();
                xdoc.LoadXml(prepayXml);
                XmlNode xn = xdoc.SelectSingleNode("xml");
                XmlNodeList xnl = xn.ChildNodes;
                if (xnl.Count > 7)
                {
                    prepayId = xnl[7].InnerText;
                    package = string.Format("prepay_id={0}", prepayId);
                    WriteFile(sp_billno, "6   " + package);
                }

                //设置支付参数
                var paySignReqHandler = new RequestHandler(HttpContext);
                paySignReqHandler.setParameter("appId", appId);
                paySignReqHandler.setParameter("timeStamp", timeStamp);
                paySignReqHandler.setParameter("nonceStr", nonceStr);
                paySignReqHandler.setParameter("package", package);
                paySignReqHandler.setParameter("signType", "MD5");
                paySign = paySignReqHandler.CreateMd5Sign("key", appkey);


                WriteFile(sp_billno, "7   " + paySign);
                WriteFile(sp_billno, "enddate   " + DateTime.Now.ToString());



                model.orderNo = sp_billno;
                model.appId = appId;
                //获取package包
                model.packageValue = package;
                model.nonceStr = nonceStr;
                model.paySign = paySign;
                model.timeStamp = timeStamp;
            }
            catch (Exception ex)
            {
                model.orderNo = ex.Message;
                WriteFile(sp_billno, "error   " + ex.Message);
            }
            rst.Data = model;
            rst.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return rst;

        }


        //public JsonResult XCXGetUnifiedOrder(string openid, string hotelcode, string publicID, string sp_billno, string strBody, int strFee)
        //{
        //  TenPayModel model = new TenPayModel();
        //  JsonResult rst = new JsonResult();
        //  try
        //  {
        //    tWeiXinPayAccountEntity account = tWeiXinPayAccountDal.GettWeiXinPayAccountByPublicID(publicID);
        //    string mchid = account.fWeiXinMCHID;
        //    string appkey = account.fWeiXinAppKey;


        //    string appId = account.fAppID; //appid
        //    // string appsecret = "af19b09cee2a4a2834ad69914d11b238"; //appsecret


        //    //获取支付Openid（打开产品页面时获取存入）



        //    //当前时间 yyyyMMdd
        //    string date = DateTime.Now.ToString("yyyyMMdd");




        //    //创建支付应答对象
        //    var packageReqHandler = new RequestHandler(HttpContext);
        //    //初始化
        //    packageReqHandler.init();

        //    timeStamp = TenpayUtil.getTimestamp();
        //    nonceStr = TenpayUtil.getNoncestr();


        //    WriteFile(sp_billno, "3   " + openid + " " + sp_billno + " " + strFee.ToString());
        //    //设置package订单参数
        //    packageReqHandler.setParameter("body", strBody); //商品信息 127字符
        //    packageReqHandler.setParameter("out_trade_no", sp_billno); //商家订单号
        //    packageReqHandler.setParameter("total_fee", strFee.ToString()); //商品金额,以分为单位(money * 100).ToString()
        //    packageReqHandler.setParameter("appid", appId);
        //    packageReqHandler.setParameter("mch_id", mchid);
        //    packageReqHandler.setParameter("nonce_str", nonceStr.ToLower());
        //    packageReqHandler.setParameter("notify_url", dinnerNotify_url);
        //    packageReqHandler.setParameter("openid", openid);
        //    packageReqHandler.setParameter("spbill_create_ip", Request.UserHostAddress); //用户的公网ip，不是商户服务器IP
        //    packageReqHandler.setParameter("trade_type", "JSAPI");
        //    packageReqHandler.setParameter("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));
        //    packageReqHandler.setParameter("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));



        //    //获取package包
        //    sign = packageReqHandler.CreateMd5Sign("key", appkey);
        //    packageReqHandler.setParameter("sign", sign);
        //    string data = "";

        //    data = packageReqHandler.parseXML();

        //    WriteFile(sp_billno, "4   " + data);


        //    WriteFile(sp_billno, "预支付begin   " + DateTime.Now.ToString());

        //    string prepayXml = HttpUtil.Send(data, "https://api.mch.weixin.qq.com/pay/unifiedorder");

        //    WriteFile(sp_billno, "预支付end   " + DateTime.Now.ToString());
        //    WriteFile(sp_billno, "5   " + prepayXml);
        //    while (string.IsNullOrEmpty(prepayXml))
        //    {
        //      prepayXml = HttpUtil.Send(data, "https://api.mch.weixin.qq.com/pay/unifiedorder");
        //      WriteFile(sp_billno, "5   " + prepayXml);
        //    }

        //    //获取预支付ID
        //    var xdoc = new XmlDocument();
        //    xdoc.LoadXml(prepayXml);
        //    XmlNode xn = xdoc.SelectSingleNode("xml");
        //    XmlNodeList xnl = xn.ChildNodes;
        //    if (xnl.Count > 7)
        //    {
        //      prepayId = xnl[7].InnerText;
        //      package = string.Format("prepay_id={0}", prepayId);
        //      WriteFile(sp_billno, "6   " + package);
        //    }

        //    //设置支付参数
        //    var paySignReqHandler = new RequestHandler(HttpContext);
        //    paySignReqHandler.setParameter("appId", appId);
        //    paySignReqHandler.setParameter("timeStamp", timeStamp);
        //    paySignReqHandler.setParameter("nonceStr", nonceStr);
        //    paySignReqHandler.setParameter("package", package);
        //    paySignReqHandler.setParameter("signType", "MD5");
        //    paySign = paySignReqHandler.CreateMd5Sign("key", appkey);


        //    WriteFile(sp_billno, "7   " + paySign);
        //    WriteFile(sp_billno, "enddate   " + DateTime.Now.ToString());


        //    package = prepayId;

        //    model.orderNo = sp_billno;
        //    model.appId = appId;
        //    //获取package包
        //    model.packageValue = package;
        //    model.nonceStr = nonceStr;
        //    model.paySign = paySign;
        //    model.timeStamp = timeStamp;
        //  }
        //  catch (Exception ex)
        //  {
        //    model.orderNo = ex.Message;
        //    WriteFile(sp_billno, "error   " + ex.Message);
        //  }
        //  rst.Data = model;
        //  rst.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
        //  return rst;
        //}

        /// <summary>
        /// log
        /// </summary>
        /// <param name="pathWrite"></param>
        /// <param name="content"></param>
        public static void WriteFile(string pathWrite, string content)
        {
            try
            {
                StreamWriter sw = new StreamWriter(@"\WXPayLog\" + DateTime.Now.ToString("yyyyMMdd") + ".txt", true);
                sw.WriteLine(content + "\r\n----------------------------------------\r\n");
                sw.Close();//写入
            }
            catch
            {

            }


        }

        /// <summary>
        /// 支付回调
        /// </summary>
        /// <returns></returns>
        public void call_back_url()
        {
            //<xml><appid><![CDATA[wx62fb46176fda9393]]></appid>
            //<bank_type><![CDATA[CCB_CREDIT]]></bank_type>
            //<cash_fee><![CDATA[1]]></cash_fee>
            //<fee_type><![CDATA[CNY]]></fee_type>
            //<is_subscribe><![CDATA[Y]]></is_subscribe>
            //<mch_id><![CDATA[1225312802]]></mch_id>
            //<nonce_str><![CDATA[5e9f92a01c986bafcabbafd145520b13]]></nonce_str>
            //<openid><![CDATA[oiE6kjhjiCRpUfIU2-M2wiPwsV70]]></openid>
            //<out_trade_no><![CDATA[20150108110920]]></out_trade_no>
            //<result_code><![CDATA[SUCCESS]]></result_code>
            //<return_code><![CDATA[SUCCESS]]></return_code>
            //<sign><![CDATA[87BD1D8E8F7C986CDAAA8742C63B570A]]></sign>
            //<time_end><![CDATA[20150108110933]]></time_end>
            //<total_fee>1</total_fee>
            //<trade_type><![CDATA[JSAPI]]></trade_type>
            //<transaction_id><![CDATA[1005320752201501080009547966]]></transaction_id>
            //</xml>


            StreamReader reader = new StreamReader(Request.InputStream);
            String xmlData = reader.ReadToEnd();
            WriteFile("Error.txt", "9   " + xmlData);

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.XmlResolver = null;
                doc.LoadXml(xmlData);
                XmlNode root = doc.FirstChild;
                if (root["result_code"].InnerText == "SUCCESS")
                {
                    string out_trade_no = root["out_trade_no"].InnerText;
                    string trade_no = root["transaction_id"].InnerText;
                    string openid = root["openid"].InnerText;

                    UserBll.UserPaySuccess("weixinpay", out_trade_no, trade_no);


                    WriteFile("Error.txt", "订单（" + out_trade_no + "）支付成功【" + trade_no + "】");

                }
            }
            catch (Exception ex)
            {
                WriteFile("Error.txt", "回调异常   " + ex.Message);
            }

            Response.Write("SUCCESS");
            //  return View(xmlData);
        }



        public JsonResult GetBookingStatus(string strBookingNo)
        {
            ResponseBaseModel response = new ResponseBaseModel();
             BookingModel model = BookingBll.GetBookingByNo(strBookingNo);
             if (model.fStatus == "已支付")
             {
                 response.iResult = 0;
                 response.strMsg = "";
             }
             else
             {
                 response.iResult = -1;
             }
             JsonResult jr = new JsonResult();
             jr.Data = response;
             return jr;
        }

        public JsonResult GetFlowStatus(string strBookingNo)
        {
            ResponseBaseModel response = new ResponseBaseModel();
            response.iResult = UserBll.GetFlowStoredStatus(strBookingNo);
           
            JsonResult jr = new JsonResult();
            jr.Data = response;
            return jr;
        }
        /// <summary>
        /// 支付回调
        /// </summary>
        /// <returns></returns>
        //public void dinner_call_back_url()
        //{


        //  StreamReader reader = new StreamReader(Request.InputStream);
        //  String xmlData = reader.ReadToEnd();
        //  WriteFile("Error.txt", "9   " + xmlData);

        //  try
        //  {
        //    WriteFile("Error.txt", "dddd ");
        //    XmlDocument doc = new XmlDocument();
        //    doc.LoadXml(xmlData);
        //    XmlNode root = doc.FirstChild;
        //    if (root["result_code"].InnerText == "SUCCESS")
        //    {
        //      string out_trade_no = root["out_trade_no"].InnerText;
        //      string trade_no = root["transaction_id"].InnerText;
        //      string openid = root["openid"].InnerText;

        //      tDinnerOrderDal.OrderPay(out_trade_no, trade_no, "sys");
        //      WriteFile("Error.txt", "订单（" + out_trade_no + "）支付成功【" + trade_no + "】");

        //    }
        //  }
        //  catch (Exception ex)
        //  {
        //    WriteFile("Error.txt", "回调异常   " + ex.Message);
        //  }

        //  Response.Write("SUCCESS");
        //  //  return View(xmlData);
        //}


        /// <summary>
        /// 退款
        /// </summary>
        /// <param name="transactionid"></param>
        /// <param name="outtradeno"></param>
        /// <param name="outrefundno"></param>
        /// <param name="totalfee"></param>
        /// <param name="refundfee"></param>
        /// <returns></returns>
        public string RefundAmount(string outtradeno, string outrefundno, int totalfee, int refundfee, string strUserHostAddress)
        {

            string resp = string.Empty;
            try
            {

                string appId = "wx67d892b056103f43";
                //当前时间 yyyyMMdd
                string date = DateTime.Now.ToString("yyyyMMdd");
                //创建支付应答对象
                var packageReqHandler = new RequestHandler(HttpContext);
                //初始化
                packageReqHandler.init();
                string nonceStr = TenpayUtil.getNoncestr();
                //设置package订单参数
                //获取package包
                packageReqHandler.setParameter("appid", appId);
                packageReqHandler.setParameter("mch_id", mchid);
                packageReqHandler.setParameter("nonce_str", nonceStr.ToLower());
               // packageReqHandler.setParameter("transaction_id", "4200000290201904164708356519");//微信订单号
                packageReqHandler.setParameter("out_trade_no", outtradeno);//商户订单号
                packageReqHandler.setParameter("out_refund_no", outrefundno);//商户退款订单号
                packageReqHandler.setParameter("total_fee", totalfee.ToString()); //总金额,以分为单位(money * 100).ToString()
                packageReqHandler.setParameter("refund_fee", refundfee.ToString()); //退款金额,以分为单位(money * 100).ToString()
               

                string sign = packageReqHandler.CreateMd5Sign("key", appkey);
                packageReqHandler.setParameter("sign", sign);
                string data = "";

                data = packageReqHandler.parseXML();
                string url = "https://api.mch.weixin.qq.com/secapi/pay/refund";
                string password = mchid;

                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);

                string WeixinCertUrl= ConfigurationManager.AppSettings["WeixinCertUrl"].ToString();
                X509Certificate2 cer = new X509Certificate2(WeixinCertUrl, password, X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet);
                HttpWebRequest webrequest = (HttpWebRequest)HttpWebRequest.Create(url);
                webrequest.ClientCertificates.Add(cer);
                webrequest.Method = "post";

                Stream requestStream = webrequest.GetRequestStream();
                requestStream.Write(Encoding.GetEncoding("UTF-8").GetBytes(data), 0, data.Length);
                requestStream.Close();

                HttpWebResponse webreponse = (HttpWebResponse)webrequest.GetResponse();
                requestStream = webreponse.GetResponseStream();
                using (StreamReader reader = new StreamReader(requestStream))
                {
                    resp = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                resp = ex.Message;
            }

            WriteFile("Error.txt", resp + "\r\n----------------------------------------\r\n");
        
            return resp;

            //<xml>
            //<return_code><![CDATA[SUCCESS]]></return_code> 
            //<return_msg><![CDATA[OK]]></return_msg>
            //<appid><![CDATA[wx67d892b056103f43]]></appid>
            //<mch_id><![CDATA[1270015901]]></mch_id> 
            //<nonce_str><![CDATA[uM7ST4BDUaL57g46]]></nonce_str> 
            //<sign><![CDATA[089E52099C453CD3E1358AB3C37D528E]]></sign> 
            //<result_code><![CDATA[SUCCESS]]></result_code>
            //<transaction_id><![CDATA[1003640943201512162118664995]]></transaction_id> 
            //<out_trade_no><![CDATA[820151216161142]]></out_trade_no> 
            //<out_refund_no><![CDATA[8201512161

            //string prepayXml = HttpUtil.Send(data, "https://api.mch.weixin.qq.com/secapi/pay/refund");



        }


        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            if (errors == SslPolicyErrors.None)
                return true;
            return false;
        }

    }
}
