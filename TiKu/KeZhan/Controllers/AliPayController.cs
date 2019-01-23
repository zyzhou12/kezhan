using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections.Specialized;
using KeZhan.Code;
using System.Xml;
using System.Text;
using TiKu.Bll;
using Aop.Api;
using Aop.Api.Request;
using Aop.Api.Response;
using KeZhan.Models;
using Aop.Api.Domain;
using Aop.Api.Util;
using System.IO;
using TiKuBll;
using TiKuBll.Model;
using System.Configuration;

namespace KeZhan.Controllers
{
  public class AliPayController : Controller
  {
    //
    // GET: /AliPay/
      [HttpPost]
    public JsonResult UserPay(string strUserName, string strBookingNo, string strFee)
    {
      string privatekey = @"MIIEpQIBAAKCAQEAweqH4zUrohyvhIQCj5XNcdVz/1jd669oP/pc8gla9fTApwrU
                            wy4AQqPinR38w0uk8/kUGCqetZAUfmyBEjXptqI3Lh5i1Jag7t+Psa5m8Zsi2f8q
                            0ZqUrxXKJZF/rBum+3pLI1uPtK8HLgLI2K+/2eDK9xL9AqaHF5tR1Ml3IJjAhZYU
                            FDsCeY8wnf5TywvzhgA//n/eKvvxyUBrs2Mfwu46sEWVC82JUII8Ww9mUL0dVqWk
                            8eAnyYvgTxZMkeLHz8vWXP0tjoAS40sDRxo0bXisuXtgoF+U6rFCAhviZLRJOKMu
                            +QvTKh0vFVZnOcxD/DWIursk9ij6Beua2XmRPQIDAQABAoIBAQC8DfniOfoqqK7+
                            UBc7sAcg0eRASapNmjn7cY0ZnED+LXF3jWVwMvhFqDFoNWCe9IjvoSn/lbV8VlHJ
                            mOhDBM22M/JXY1hs2fcQMPZlVcC3pb9SscaQptxyPyte649pFRgG4T5k97KRgvvv
                            fvvQSABCB2JN4bhEDcMM/a+KMCa7EDfTihGSvwAh/bu9+9hB5v6nx1CotHsrp/mW
                            5QfWDq4lYnajjwAi3Sj1cIkKuREEj9fkz29uHQlg57Zxv9pSLhSHCft8wLNN0ab8
                            yC6d4ygIYABwkwSwBc4rOfX8B/k4mEKDja981tawUnQdkTMg9wJtUVqTPlSvNh7d
                            JzIzwgrxAoGBAOHqn38HJIutx4WKpyP84FNi3zRC5XK/TJyOJ8g8nURaSV/vrZ8Z
                            suz2P9j0PgbRe9NBIqgCxMRYl8yR+ChIVXgpzIN2Y/Gy4ZTmfCTrMDXcvM++iXVO
                            xdlZCpH2rTGOkkRpLJ/vkM6h9Vz0l3g42AIvfQRxMvU26QsCdsyyl9jfAoGBANu9
                            CDAbiabSO7bi+YUcM+edLxfyClD/LO9w8oXvS4DgXjyK6y3k97ae0zUAsWRnlVe6
                            Ytw0av+vdnTKSbUsHToJvDlmo1oOrGjSrduk2EvCPsSiDvrSd/Qd+/JBi0B8nCIu
                            0/JKoI9e0wG0tgl/toTnZxq4cRk/spfBSwORDq1jAoGAVa6+v1beLXvDaIqlyahn
                            DDk7nn6gt1yGmfnwbKlQnFQB8DjfCLCeg/EBVi+MNtnMtNrHYiFqr21KZXQXQm/s
                            up5fypxkW48Ur3ybKQVqS4NkuQXy7GLr9vsmXyXHmjwQjZG2MxKRQU172b2KlTY5
                            9to7+CtWwFoLGPneRNSXctECgYEAqcN5N/GOf1uc1sa0j6oiT6aYY5+TaNA8HyDb
                            va4KXx74rz2ERIjI+EXsVt6aLj/4mTZeelHk+HTOx5whJd9XFSfAS3iIa2M6wCFE
                            QJUyphUD+VZazF0eX3Nq2tbYhpG+7onPJTmSojFYQ9EhcmVA1Z0RgwtMFX2otKWZ
                            FsBS3pECgYEAse5fSm7eWxEAThpmvdqAysRRoUxum80gDkacbXJl62ZPTSiT4jDG
                            ybZxVxITwfH23XCf9z52H/fb2TEUdE6c8cnwbnKsYHSAMw8Kg6bwRKhdItiPc7Kd
                            jNbVzfmmNrbwttR1WDPtgLd8u3z+pjqcP5y82Y9e9Znj21S2VuoJOQY=
                            ";

    

      DefaultAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", "2018113062388410", privatekey, "json", "1.0", "RSA2", "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAlse4dU81/Q0QZa5lgwkcPPUFUfim+ZXYLosPaOJU4ZBDPUvxQnyVuHjvQdnAoxqUX2jmEi/XQfDHCtgcFMO4c7UpiI/DTqybBUMb5yKoAtPipKw7W4XAauXoFWqGF/dV2WrrDggNHVKKZYrpWFdyN8B32b5JP5J9TorGhNELl7RHm7QsOSCMFgMdd/IVIEsDr1V9HtJa3DMj2J6k+vbJAickXsmJBI4ix74pGSUYKG8tx2t016tkaIjRdI5DLIseEgqhV8jvqnk5qI9kziMJdXZr3QhIbXX/7Sf0JcJPQwXbqokSB1NFU++fIk1s367l1xqKsAZUd29c/9c7FlAF+QIDAQAB", "UTF-8", false);
      
      string sp_billno = UserBll.UserPay(strUserName, "alipay",strBookingNo, Convert.ToDecimal(strFee), "web");

      // 外部订单号，商户网站订单系统中唯一的订单号
      string out_trade_no = sp_billno;

      // 订单名称
      string subject = "充值";

      // 付款金额
      string total_amout = strFee;

      // 商品描述
      string body = "测试商品";

      // 组装业务参数model
      AlipayTradePagePayModel model = new AlipayTradePagePayModel();
      model.Body = body;
      model.Subject = subject;
      model.TotalAmount = total_amout;
      model.OutTradeNo = out_trade_no;
      model.ProductCode = "FAST_INSTANT_TRADE_PAY";

      AlipayTradePagePayRequest request = new AlipayTradePagePayRequest();
      // 设置同步回调地址
      request.SetReturnUrl(ConfigurationManager.AppSettings["PayCallBack"].ToString() + "/alipay/payReturn?payType=classroom&strBookingNo=" + strBookingNo);
      // 设置异步通知接收地址
      request.SetNotifyUrl(ConfigurationManager.AppSettings["PayCallBack"].ToString() + "/alipay/PayNotify");
      // 将业务model载入到request
      request.SetBizModel(model);

      AlipayTradePagePayResponse res = null;
      try
      {
        res = client.pageExecute(request, null, "post");
        //Response.Write(response.Body);
      }
      catch (Exception exp)
      {
        throw exp;
      }
       


      ResponseBaseModel response = new ResponseBaseModel();
      response.iResult = 0;
      response.strMsg =res.Body;

      JsonResult jr = new JsonResult();
      jr.Data = response;
      return jr;
    }





    public ActionResult PayReturn(string payType, string strBookingNo=null)
    {
      WriteFile("Error.txt", "支付回调");
      /* 实际验证过程建议商户添加以下校验。
         1、商户需要验证该通知数据中的out_trade_no是否为商户系统中创建的订单号，
         2、判断total_amount是否确实为该订单的实际金额（即商户订单创建时的金额），
         3、校验通知中的seller_id（或者seller_email) 是否为out_trade_no这笔单据的对应的操作方（有的时候，一个商户可能有多个seller_id/seller_email）
         4、验证app_id是否为该商户本身。
         */
      try
      {
        Dictionary<string, string> sArray = GetRequestPost();
        if (sArray.Count != 0)
        {
          bool flag = AlipaySignature.RSACheckV1(sArray, "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAlse4dU81/Q0QZa5lgwkcPPUFUfim+ZXYLosPaOJU4ZBDPUvxQnyVuHjvQdnAoxqUX2jmEi/XQfDHCtgcFMO4c7UpiI/DTqybBUMb5yKoAtPipKw7W4XAauXoFWqGF/dV2WrrDggNHVKKZYrpWFdyN8B32b5JP5J9TorGhNELl7RHm7QsOSCMFgMdd/IVIEsDr1V9HtJa3DMj2J6k+vbJAickXsmJBI4ix74pGSUYKG8tx2t016tkaIjRdI5DLIseEgqhV8jvqnk5qI9kziMJdXZr3QhIbXX/7Sf0JcJPQwXbqokSB1NFU++fIk1s367l1xqKsAZUd29c/9c7FlAF+QIDAQAB", "UTF-8", "RSA2", false);
          if (flag)
          {
            //交易状态
            //判断该笔订单是否在商户网站中已经做过处理
            //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
            //请务必判断请求时的total_amount与通知时获取的total_fee为一致的
            //如果有做过处理，不执行商户的业务程序

            //注意：
            //退款日期超过可退款期限后（如三个月可退款），支付宝系统发送该交易状态通知

            //XmlDocument xmlDoc = new XmlDocument();

            //WriteFile("Error.txt", xmlDoc.OuterXml);
            //xmlDoc.LoadXml(sArray["notify_data"]);
            ////商户订单号
            //string out_trade_no = xmlDoc.SelectSingleNode("/notify/out_trade_no").InnerText;
            ////支付宝交易号
            //string trade_no = xmlDoc.SelectSingleNode("/notify/trade_no").InnerText;
            ////交易状态
            //string trade_status = xmlDoc.SelectSingleNode("/notify/trade_status").InnerText;

              BookingModel model = BookingBll.GetBookingByNo(strBookingNo);
              if (model.fStatus == "提交")
              {
                 

                  string out_trade_no = Request.Form["out_trade_no"];
                  string trade_no = Request.Form["trade_no"];
                  string trade_status = Request.Form["trade_status"];


                  WriteFile("Error.txt", "订单（" + out_trade_no + "）支付成功【" + trade_no + "】");
                  UserBll.UserPaySuccess("alipay", out_trade_no, trade_no);

              }

          }
          else
          {
            WriteFile("Error.txt", "回调失败");
          }
        }
      }
      catch (Exception ex)
      {

        WriteFile("Error.txt", "回调失败" + ex.Message);
      }

      if (payType == "pay")
      {
        return RedirectToAction("UserPay", "User");
      }
      else
      {
        return RedirectToAction("UserBooking", "Booking", new { strBookingNo = strBookingNo });
      }
    }


    public string PayNotify()
    {
      WriteFile("Error.txt", "支付回调");
     /* 实际验证过程建议商户添加以下校验。
        1、商户需要验证该通知数据中的out_trade_no是否为商户系统中创建的订单号，
        2、判断total_amount是否确实为该订单的实际金额（即商户订单创建时的金额），
        3、校验通知中的seller_id（或者seller_email) 是否为out_trade_no这笔单据的对应的操作方（有的时候，一个商户可能有多个seller_id/seller_email）
        4、验证app_id是否为该商户本身。
        */
      try
      {
        Dictionary<string, string> sArray = GetRequestPost();
        if (sArray.Count != 0)
        {
          bool flag = AlipaySignature.RSACheckV1(sArray, "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAlse4dU81/Q0QZa5lgwkcPPUFUfim+ZXYLosPaOJU4ZBDPUvxQnyVuHjvQdnAoxqUX2jmEi/XQfDHCtgcFMO4c7UpiI/DTqybBUMb5yKoAtPipKw7W4XAauXoFWqGF/dV2WrrDggNHVKKZYrpWFdyN8B32b5JP5J9TorGhNELl7RHm7QsOSCMFgMdd/IVIEsDr1V9HtJa3DMj2J6k+vbJAickXsmJBI4ix74pGSUYKG8tx2t016tkaIjRdI5DLIseEgqhV8jvqnk5qI9kziMJdXZr3QhIbXX/7Sf0JcJPQwXbqokSB1NFU++fIk1s367l1xqKsAZUd29c/9c7FlAF+QIDAQAB", "UTF-8", "RSA2", false);
          if (flag)
          {
            //交易状态
            //判断该笔订单是否在商户网站中已经做过处理
            //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
            //请务必判断请求时的total_amount与通知时获取的total_fee为一致的
            //如果有做过处理，不执行商户的业务程序

            //注意：
            //退款日期超过可退款期限后（如三个月可退款），支付宝系统发送该交易状态通知

            //XmlDocument xmlDoc = new XmlDocument();

            //WriteFile("Error.txt", xmlDoc.OuterXml);
            //xmlDoc.LoadXml(sArray["notify_data"]);
            ////商户订单号
            //string out_trade_no = xmlDoc.SelectSingleNode("/notify/out_trade_no").InnerText;
            ////支付宝交易号
            //string trade_no = xmlDoc.SelectSingleNode("/notify/trade_no").InnerText;
            ////交易状态
            //string trade_status = xmlDoc.SelectSingleNode("/notify/trade_status").InnerText;

            string out_trade_no = Request.Form["out_trade_no"];
            string trade_no = Request.Form["trade_no"];
            string trade_status = Request.Form["trade_status"];

            WriteFile("Error.txt", "订单（" + out_trade_no + "）支付成功【" + trade_no + "】");
            UserBll.UserPaySuccess("alipay", out_trade_no, trade_no);



            return "success";
          }
          else
          {
            WriteFile("Error.txt", "回调失败");
            return "fail";
          }
        }
      }
      catch (Exception ex)
      {

        WriteFile("Error.txt", "回调失败" + ex.Message);
      }

        return "fail";
    }


      [HttpPost]
    public JsonResult UserWapPay(string strUserName, string strBookingNo, string strFee)
    {
      string privatekey = @"MIIEpQIBAAKCAQEAweqH4zUrohyvhIQCj5XNcdVz/1jd669oP/pc8gla9fTApwrU
                            wy4AQqPinR38w0uk8/kUGCqetZAUfmyBEjXptqI3Lh5i1Jag7t+Psa5m8Zsi2f8q
                            0ZqUrxXKJZF/rBum+3pLI1uPtK8HLgLI2K+/2eDK9xL9AqaHF5tR1Ml3IJjAhZYU
                            FDsCeY8wnf5TywvzhgA//n/eKvvxyUBrs2Mfwu46sEWVC82JUII8Ww9mUL0dVqWk
                            8eAnyYvgTxZMkeLHz8vWXP0tjoAS40sDRxo0bXisuXtgoF+U6rFCAhviZLRJOKMu
                            +QvTKh0vFVZnOcxD/DWIursk9ij6Beua2XmRPQIDAQABAoIBAQC8DfniOfoqqK7+
                            UBc7sAcg0eRASapNmjn7cY0ZnED+LXF3jWVwMvhFqDFoNWCe9IjvoSn/lbV8VlHJ
                            mOhDBM22M/JXY1hs2fcQMPZlVcC3pb9SscaQptxyPyte649pFRgG4T5k97KRgvvv
                            fvvQSABCB2JN4bhEDcMM/a+KMCa7EDfTihGSvwAh/bu9+9hB5v6nx1CotHsrp/mW
                            5QfWDq4lYnajjwAi3Sj1cIkKuREEj9fkz29uHQlg57Zxv9pSLhSHCft8wLNN0ab8
                            yC6d4ygIYABwkwSwBc4rOfX8B/k4mEKDja981tawUnQdkTMg9wJtUVqTPlSvNh7d
                            JzIzwgrxAoGBAOHqn38HJIutx4WKpyP84FNi3zRC5XK/TJyOJ8g8nURaSV/vrZ8Z
                            suz2P9j0PgbRe9NBIqgCxMRYl8yR+ChIVXgpzIN2Y/Gy4ZTmfCTrMDXcvM++iXVO
                            xdlZCpH2rTGOkkRpLJ/vkM6h9Vz0l3g42AIvfQRxMvU26QsCdsyyl9jfAoGBANu9
                            CDAbiabSO7bi+YUcM+edLxfyClD/LO9w8oXvS4DgXjyK6y3k97ae0zUAsWRnlVe6
                            Ytw0av+vdnTKSbUsHToJvDlmo1oOrGjSrduk2EvCPsSiDvrSd/Qd+/JBi0B8nCIu
                            0/JKoI9e0wG0tgl/toTnZxq4cRk/spfBSwORDq1jAoGAVa6+v1beLXvDaIqlyahn
                            DDk7nn6gt1yGmfnwbKlQnFQB8DjfCLCeg/EBVi+MNtnMtNrHYiFqr21KZXQXQm/s
                            up5fypxkW48Ur3ybKQVqS4NkuQXy7GLr9vsmXyXHmjwQjZG2MxKRQU172b2KlTY5
                            9to7+CtWwFoLGPneRNSXctECgYEAqcN5N/GOf1uc1sa0j6oiT6aYY5+TaNA8HyDb
                            va4KXx74rz2ERIjI+EXsVt6aLj/4mTZeelHk+HTOx5whJd9XFSfAS3iIa2M6wCFE
                            QJUyphUD+VZazF0eX3Nq2tbYhpG+7onPJTmSojFYQ9EhcmVA1Z0RgwtMFX2otKWZ
                            FsBS3pECgYEAse5fSm7eWxEAThpmvdqAysRRoUxum80gDkacbXJl62ZPTSiT4jDG
                            ybZxVxITwfH23XCf9z52H/fb2TEUdE6c8cnwbnKsYHSAMw8Kg6bwRKhdItiPc7Kd
                            jNbVzfmmNrbwttR1WDPtgLd8u3z+pjqcP5y82Y9e9Znj21S2VuoJOQY=
                            ";



      DefaultAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", "2018113062388410", privatekey, "json", "1.0", "RSA2", "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAlse4dU81/Q0QZa5lgwkcPPUFUfim+ZXYLosPaOJU4ZBDPUvxQnyVuHjvQdnAoxqUX2jmEi/XQfDHCtgcFMO4c7UpiI/DTqybBUMb5yKoAtPipKw7W4XAauXoFWqGF/dV2WrrDggNHVKKZYrpWFdyN8B32b5JP5J9TorGhNELl7RHm7QsOSCMFgMdd/IVIEsDr1V9HtJa3DMj2J6k+vbJAickXsmJBI4ix74pGSUYKG8tx2t016tkaIjRdI5DLIseEgqhV8jvqnk5qI9kziMJdXZr3QhIbXX/7Sf0JcJPQwXbqokSB1NFU++fIk1s367l1xqKsAZUd29c/9c7FlAF+QIDAQAB", "UTF-8", false);

      string sp_billno = UserBll.UserPay(strUserName, "alipay", strBookingNo, Convert.ToDecimal(strFee), "web");

      // 外部订单号，商户网站订单系统中唯一的订单号
      string out_trade_no = sp_billno;

      // 订单名称
      string subject = "充值";

      // 付款金额
      string total_amout = strFee;

      // 商品描述
      string body = "测试商品";

      string quit_url = ConfigurationManager.AppSettings["PayCallBack"].ToString() + "/alipay/payReturn?payType=pay";

      // 组装业务参数model
      AlipayTradeWapPayModel model = new AlipayTradeWapPayModel();
      model.Body = body;
      model.Subject = subject;
      model.TotalAmount = total_amout;
      model.OutTradeNo = out_trade_no;
      model.ProductCode = "QUICK_WAP_WAY";
      model.QuitUrl = quit_url;

      AlipayTradeWapPayRequest request = new AlipayTradeWapPayRequest();
      // 设置同步回调地址
      request.SetReturnUrl(ConfigurationManager.AppSettings["PayCallBack"].ToString() + "/alipay/payReturn?payType=classroom&strBookingNo=" + strBookingNo);
      // 设置异步通知接收地址
      request.SetNotifyUrl(ConfigurationManager.AppSettings["PayCallBack"].ToString() + "/alipay/PayNotify");
      // 将业务model载入到request
      request.SetBizModel(model);

      AlipayTradeWapPayResponse res = null;
      try
      {
        res = client.pageExecute(request, null, "post");
        //Response.Write(response.Body);
      }
      catch (Exception exp)
      {
        throw exp;
      }



      ResponseBaseModel response = new ResponseBaseModel();
      response.iResult = 0;
      response.strMsg = res.Body;

      JsonResult jr = new JsonResult();
      jr.Data = response;
      return jr;
    }



    public static void WriteFile(string pathWrite, string content)
    {
      try
      {
        StreamWriter sw = new StreamWriter(@"\AliPayLog\" + DateTime.Now.ToString("yyyyMMdd") + ".txt", true);
        sw.WriteLine(content + "\r\n----------------------------------------\r\n");
        sw.Close();//写入
      }
      catch
      {

      }


    }

    public void PayOrder(string strUserName,int strFee)
    {
      string bookingno = UserBll.UserPay(strUserName, "alipay",null, strFee, "web");
      //支付宝网关地址
      string GATEWAY_NEW = "http://wappaygw.alipay.com/service/rest.htm?";

      ////////////////////////////////////////////调用授权接口alipay.wap.trade.create.direct获取授权码token////////////////////////////////////////////

      //返回格式
      string format = "xml";
      //必填，不需要修改

      //返回格式
      string v = "2.0";
      //必填，不需要修改

      //请求号
      string req_id = DateTime.Now.ToString("yyyyMMddHHmmss");
      //必填，须保证每次请求都是唯一

      //req_data详细信息

      //服务器异步通知页面路径
      string notify_url = "http://test.xyk.hotel8h.com/AliPay/notify_url";
      //需http://格式的完整路径，不允许加?id=123这类自定义参数

      //页面跳转同步通知页面路径
      string call_back_url = "http://test.xyk.hotel8h.com/AliPay/call_back_url";
      //需http://格式的完整路径，不允许加?id=123这类自定义参数

      //操作中断返回地址
      string merchant_url = "http://127.0.0.1:64704/WS_WAP_PAYWAP-CSHARP-UTF-8/xxxxx.aspx";
      //用户付款中途退出返回商户的地址。需http://格式的完整路径，不允许加?id=123这类自定义参数

      //卖家支付宝帐户
      string seller_email = "pay@hotel8h.com";
      //必填

      //商户订单号
      string strOrderNo = bookingno;
      string out_trade_no = strOrderNo;
      //商户网站订单系统中唯一订单号，必填

      //订单名称
      string subject = "酒店预定";
      //必填

      //付款金额
      string total_fee = "0.1";
      //必填

      //请求业务参数详细
      string req_dataToken = "<direct_trade_create_req><notify_url>" + notify_url + "</notify_url><call_back_url>" + call_back_url + "</call_back_url><seller_account_name>" + seller_email + "</seller_account_name><out_trade_no>" + out_trade_no + "</out_trade_no><subject>" + subject + "</subject><total_fee>" + total_fee + "</total_fee><merchant_url>" + merchant_url + "</merchant_url></direct_trade_create_req>";
      //必填

      //把请求参数打包成数组
      Dictionary<string, string> sParaTempToken = new Dictionary<string, string>();
      sParaTempToken.Add("partner", Config.Partner);
      sParaTempToken.Add("_input_charset", Config.Input_charset.ToLower());
      sParaTempToken.Add("sec_id", Config.Sign_type.ToUpper());
      sParaTempToken.Add("service", "alipay.wap.trade.create.direct");
      sParaTempToken.Add("format", format);
      sParaTempToken.Add("v", v);
      sParaTempToken.Add("req_id", req_id);
      sParaTempToken.Add("req_data", req_dataToken);

      //建立请求
      string sHtmlTextToken = Submit.BuildRequest(GATEWAY_NEW, sParaTempToken);
      //URLDECODE返回的信息
      Encoding code = Encoding.GetEncoding(Config.Input_charset);
      sHtmlTextToken = HttpUtility.UrlDecode(sHtmlTextToken, code);

      //解析远程模拟提交后返回的信息
      Dictionary<string, string> dicHtmlTextToken = Submit.ParseResponse(sHtmlTextToken);

      //获取token
      string request_token = dicHtmlTextToken["request_token"];

      ////////////////////////////////////////////根据授权码token调用交易接口alipay.wap.auth.authAndExecute////////////////////////////////////////////


      //业务详细
      string req_data = "<auth_and_execute_req><request_token>" + request_token + "</request_token></auth_and_execute_req>";
      //必填

      //把请求参数打包成数组
      Dictionary<string, string> sParaTemp = new Dictionary<string, string>();
      sParaTemp.Add("partner", Config.Partner);
      sParaTemp.Add("_input_charset", Config.Input_charset.ToLower());
      sParaTemp.Add("sec_id", Config.Sign_type.ToUpper());
      sParaTemp.Add("service", "alipay.wap.auth.authAndExecute");
      sParaTemp.Add("format", format);
      sParaTemp.Add("v", v);
      sParaTemp.Add("req_data", req_data);

      //建立请求
      string sHtmlText = Submit.BuildRequest(GATEWAY_NEW, sParaTemp, "get", "确认");
      Response.Write(sHtmlText);

    }


 

    public ActionResult notify_url()
    {
      Dictionary<string, string> sPara = GetRequestPost();

      if (sPara.Count > 0)//判断是否有带返回参数
      {
        Notify aliNotify = new Notify();
        bool verifyResult = aliNotify.VerifyNotify(sPara, Request.Form["sign"]);

        if (verifyResult)//验证成功
        {
          /////////////////////////////////////////////////////////////////////////////////////////////////////////////
          //请在这里加上商户的业务逻辑程序代码


          //——请根据您的业务逻辑来编写程序（以下代码仅作参考）——
          //获取支付宝的通知返回参数，可参考技术文档中服务器异步通知参数列表

          //解密（如果是RSA签名需要解密，如果是MD5签名则下面一行清注释掉）
          //  sPara = aliNotify.Decrypt(sPara);

          //XML解析notify_data数据
          try
          {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(sPara["notify_data"]);
            //商户订单号
            string out_trade_no = xmlDoc.SelectSingleNode("/notify/out_trade_no").InnerText;
            //支付宝交易号
            string trade_no = xmlDoc.SelectSingleNode("/notify/trade_no").InnerText;
            //交易状态
            string trade_status = xmlDoc.SelectSingleNode("/notify/trade_status").InnerText;

            if (trade_status == "TRADE_FINISHED")
            {
              //判断该笔订单是否在商户网站中已经做过处理
              //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
              //如果有做过处理，不执行商户的业务程序





              //注意：
              //该种交易状态只在两种情况下出现
              //1、开通了普通即时到账，买家付款成功后。
              //2、开通了高级即时到账，从该笔交易成功时间算起，过了签约时的可退款时限（如：三个月以内可退款、一年以内可退款等）后。

              Response.Write("success");  //请不要修改或删除
            }
            else if (trade_status == "TRADE_SUCCESS")
            {
              //判断该笔订单是否在商户网站中已经做过处理
              //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
              //如果有做过处理，不执行商户的业务程序

              //注意：
              //该种交易状态只在一种情况下出现——开通了高级即时到账，买家付款成功后。


              Response.Write("success");  //请不要修改或删除
            }
            else
            {
              Response.Write(trade_status);
            }

          }
          catch (Exception exc)
          {
            Response.Write(exc.ToString());
          }



          //——请根据您的业务逻辑来编写程序（以上代码仅作参考）——

          /////////////////////////////////////////////////////////////////////////////////////////////////////////////
        }
        else//验证失败
        {
          Response.Write("fail");
        }
      }
      else
      {
        Response.Write("无通知参数");
      }
      return View();
    }



    /// <summary>
    /// 获取支付宝POST过来通知消息，并以“参数名=参数值”的形式组成数组
    /// </summary>
    /// <returns>request回来的信息组成的数组</returns>
    public Dictionary<string, string> GetRequestPost()
    {
      int i = 0;
      Dictionary<string, string> sArray = new Dictionary<string, string>();
      NameValueCollection coll;
      //Load Form variables into NameValueCollection variable.
      coll = Request.Form;

      // Get names of all forms into a string array.
      String[] requestItem = coll.AllKeys;

      for (i = 0; i < requestItem.Length; i++)
      {
        sArray.Add(requestItem[i], Request.Form[requestItem[i]]);
      }

      return sArray;
    }

    public ActionResult call_back_url()
    {
      string strMessage = "";
      string strBookingNo = "";
      Dictionary<string, string> sPara = GetRequestGet();

      if (sPara.Count > 0)//判断是否有带返回参数
      {
        Notify aliNotify = new Notify();
        bool verifyResult = aliNotify.VerifyReturn(sPara, Request.QueryString["sign"]);

        if (verifyResult)//验证成功
        {
          /////////////////////////////////////////////////////////////////////////////////////////////////////////////
          //请在这里加上商户的业务逻辑程序代码


          //——请根据您的业务逻辑来编写程序（以下代码仅作参考）——
          //获取支付宝的通知返回参数，可参考技术文档中页面跳转同步通知参数列表

          //商户订单号
          string out_trade_no = Request.QueryString["out_trade_no"];

          //支付宝交易号
          string trade_no = Request.QueryString["trade_no"];

          //交易状态
          string result = Request.QueryString["result"];

          strBookingNo = out_trade_no;

          //打印页面
          // Response.Write("验证成功<br />");

          //——请根据您的业务逻辑来编写程序（以上代码仅作参考）——

         
          UserBll.UserPaySuccess("weixinpay", out_trade_no, trade_no);
          
          /////////////////////////////////////////////////////////////////////////////////////////////////////////////
        }
        else//验证失败
        {
          strMessage = "验证失败";
          // Response.Write("验证失败");
        }
      }
      else
      {
        strMessage = "无返回参数";
        // Response.Write("无返回参数");
      }

      return RedirectToAction("OrderDetail", "Acct", new { id = strBookingNo });
    }

    /// <summary>
    /// 获取支付宝GET过来通知消息，并以“参数名=参数值”的形式组成数组
    /// </summary>
    /// <returns>request回来的信息组成的数组</returns>
    public Dictionary<string, string> GetRequestGet()
    {
      int i = 0;
      Dictionary<string, string> sArray = new Dictionary<string, string>();
      NameValueCollection coll;
      //Load Form variables into NameValueCollection variable.
      coll = Request.QueryString;

      // Get names of all forms into a string array.
      String[] requestItem = coll.AllKeys;

      for (i = 0; i < requestItem.Length; i++)
      {
        sArray.Add(requestItem[i], Request.QueryString[requestItem[i]]);
      }

      return sArray;
    }

  }
}
