using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Web;
using System.Security.Cryptography;

namespace TiKuService
{
  public class AliSign
  {

    private const String ALGORITHM = "HmacSHA1";
    private const String HTTP_METHOD = "GET";
    private const String SEPARATOR = "&";
    private const String EQUAL = "=";


    private static String percentEncode(String value)
    {

      return UrlEncode(value).Replace("+", "%20").Replace("*", "%2A").Replace("%7E", "~");
    }

    public static string UrlEncode(string str)
    {
      StringBuilder builder = new StringBuilder();
      foreach (char c in str)
      {
        if (HttpUtility.UrlEncode(c.ToString(), Encoding.UTF8).Length > 1)
        {
          builder.Append(HttpUtility.UrlEncode(c.ToString(), Encoding.UTF8).ToUpper());
        }
        else
        {
          builder.Append(c);
        }
      }
      return builder.ToString();
    }
    public static String encodeByRFC3986(String obj)
    {
      StringBuilder builder = new StringBuilder();
      String[] segments = obj.Split('/');
      for (int i = 0; i < segments.Length; i++)
      {
        builder.Append(percentEncode(segments[i]));
        if (i != segments.Length - 1)
        {
          builder.Append("/");
        }
      }
      return builder.ToString();
    }
    public static String buildCanonicalizedQueryString(Dictionary<string, string> parameterMap)
    {
      // 对参数进行排序
      List<String> sortedKeys = parameterMap.Keys.ToList();
      sortedKeys.Sort();

      StringBuilder temp = new StringBuilder();
      foreach (String key in sortedKeys)
      {
        // 此处需要对key和value进行编码
        String value = parameterMap[key];
        temp.Append(SEPARATOR).Append(percentEncode(key)).Append(EQUAL).Append(percentEncode(value));
      }
      return temp.ToString().Substring(1);
    }

    public static String buildStringToSign(String canonicalizedQueryString)
    {
      // 生成stringToSign字符
      StringBuilder temp = new StringBuilder();
      temp.Append(HTTP_METHOD).Append(SEPARATOR);
      temp.Append(percentEncode("/")).Append(SEPARATOR);
      // 此处需要对canonicalizedQueryString进行编码
      temp.Append(percentEncode(canonicalizedQueryString));
      return temp.ToString();
    }


    public static String buildSignature(String keySecret, String stringToSign)
    {
      byte[] byt_key = UTF8Encoding.UTF8.GetBytes(keySecret);
      //byte[] byt_key = UTF8Encoding.UTF8.GetBytes("fdFww0u5zcZhcvi1X+3KJ5bN1VY=&");//请使用您自己的key和secret

      //将StringToSign转化为Byte数组
      byte[] byt_ToSign = UTF8Encoding.UTF8.GetBytes(stringToSign);

      //设置HMAC SHA1的密钥（Access Key Secret的Byte数组）
      HMACSHA1 hmac = new HMACSHA1(byt_key);
      //进行哈希运算
      byte[] hashValue = hmac.ComputeHash(byt_ToSign);
      //进行Base64编码
      string signature = Convert.ToBase64String(hashValue);
      //对生成的签名进行URL编码（UTF8）
      signature = UrlEncode(signature);

      return signature;
     
    }

    public static String buildRequestURL(String signature, Dictionary<string, string> parameterMap)
    {
      // 生成请求URL
      StringBuilder temp = new StringBuilder("http://mts.aliyuncs.com/?");
      temp.Append(UrlEncode("Signature")).Append("=").Append(signature);
      foreach (var e in parameterMap)
      {
        temp.Append("&").Append(percentEncode(e.Key)).Append("=").Append(percentEncode(e.Value));
      }
      return temp.ToString();
    }

  
  }
}
