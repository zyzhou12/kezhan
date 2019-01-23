using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace WXPay
{
    public class HttpUtil
    {

        private const string sContentType = "application/x-www-form-urlencoded";
        private const string sUserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.2; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
        public static string Send(string data, string url)
        {
          return TXSend(System.Text.Encoding.UTF8.GetBytes(data), url);
          //return Send(System.Text.Encoding.UTF8.GetBytes(data), url);
        }

        public static string Send(byte[] data, string url)
        {
          System.GC.Collect();//垃圾回收，回收没有正常关闭的http连接

            Stream responseStream;
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            if (request == null)
            {
                throw new ApplicationException(string.Format("Invalid url string: {0}", url));
            }
            // request.UserAgent = sUserAgent;  
            request.ContentType = sContentType;
            request.Method = "POST";
           // request.Timeout = 3000;
            request.ContentLength = data.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(data, 0, data.Length);
            requestStream.Close();
            try
            {
                responseStream = request.GetResponse().GetResponseStream();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            string str = string.Empty;
            using (StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("UTF-8")))
            {
                str = reader.ReadToEnd();
            }
            responseStream.Close();
            return str;
        }


        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
          //直接确认，否则打不开    
          return true;
        }
        public static string TXSend(byte[] data, string url, int timeout=3)
        {
          //System.GC.Collect();//垃圾回收，回收没有正常关闭的http连接

          string result = "";//返回结果

          HttpWebRequest request = null;
          HttpWebResponse response = null;
          Stream reqStream = null;

          try
          {
            //设置最大连接数
            ServicePointManager.DefaultConnectionLimit = 200;
            //设置https验证方式
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
              ServicePointManager.ServerCertificateValidationCallback =
                      new RemoteCertificateValidationCallback(CheckValidationResult);
            }

            /***************************************************************
            * 下面设置HttpWebRequest的相关属性
            * ************************************************************/
            request = (HttpWebRequest)WebRequest.Create(url);

            request.Method = "POST";
            request.Timeout = timeout * 1000;
            request.KeepAlive = true;

            //设置POST的数据类型和长度
            request.ContentType = "text/xml";
          
            request.ContentLength = data.Length;

            

            //往服务器写入数据
            reqStream = request.GetRequestStream();
            reqStream.Write(data, 0, data.Length);
            reqStream.Close();

            //获取服务端返回
            response = (HttpWebResponse)request.GetResponse();

            //获取服务端返回数据
            StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            result = sr.ReadToEnd().Trim();
            sr.Close();
          }
          catch (System.Threading.ThreadAbortException e)
          {
            System.Threading.Thread.ResetAbort();
          }
          catch (WebException e)
          {
           
            //throw new WxPayException(e.ToString());
          }
          catch (Exception e)
          {
           // throw new WxPayException(e.ToString());
          }
          finally
          {
            //关闭连接和流
            if (response != null)
            {
              response.Close();
            }
            if (request != null)
            {
              request.Abort();
            }
          }
          return result;
        }
    }
}