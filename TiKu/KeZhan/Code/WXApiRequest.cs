using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace KeZhan.Code
{
    public class WXApiRequest
    {

        /// <summary>
        /// 获取接口地址
        /// </summary>
        /// <param name="strType"></param>
        /// <returns></returns>
        public static string GetUrl(string strType)
        {
            string strUrl = "";
            if (strType == "authorize")//授权
            {
                strUrl = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_base&state={2}#wechat_redirect";
            }
            else if (strType == "authorize2")//授权
            {
                strUrl = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_userinfo&state={2}#wechat_redirect";
            }
            else if (strType == "getaccesstoken")
            {
                strUrl = "https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code";
            }
            else if (strType == "refrestaccesstoken")
            {
                strUrl = "https://api.weixin.qq.com/sns/oauth2/refresh_token?appid={0}&grant_type=refresh_token&refresh_token={1}";
            }
            else if (strType == "gettoken")
            {
                strUrl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";
            }
            else if (strType == "getuserinfo")
            {
                // strUrl = "https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN";
                strUrl = "https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang=zh_CN";
            }
            return strUrl;
        }


        public static String GetData(string strUrl)
        {
            WebClient MyWebClient = new WebClient();


            MyWebClient.Credentials = CredentialCache.DefaultCredentials;
            Byte[] pageData = MyWebClient.DownloadData(strUrl);

            String strJson = Encoding.UTF8.GetString(pageData) ?? "";
            return strJson;

        }


        public static String PostData(string strUrl, string strBody)
        {
            WebClient MyWebClient = new WebClient();


            MyWebClient.Encoding = Encoding.UTF8;

            byte[] post = Encoding.UTF8.GetBytes(strBody);
            byte[] pageData = MyWebClient.UploadData(strUrl, "post", post);

            String strJson = Encoding.UTF8.GetString(pageData) ?? "";
            return strJson;

        }
    }
}