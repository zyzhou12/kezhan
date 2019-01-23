﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeZhan.Code
{

  /// <summary>
  /// 类名：Config
  /// 功能：基础配置类
  /// 详细：设置帐户有关信息及返回路径
  /// 版本：3.3
  /// 日期：2012-07-05
  /// 说明：
  /// 以下代码只是为了方便商户测试而提供的样例代码，商户可以根据自己网站的需要，按照技术文档编写,并非一定要使用该代码。
  /// 该代码仅供学习和研究支付宝接口使用，只是提供一个参考。
  /// 
  /// 如何获取安全校验码和合作身份者ID
  /// 1.用您的签约支付宝账号登录支付宝网站(www.alipay.com)
  /// 2.点击“商家服务”(https://b.alipay.com/order/myOrder.htm)
  /// 3.点击“查询合作者身份(PID)”、“查询安全校验码(Key)”
  /// </summary>
  public class Config
  {
    #region 字段
    private static string partner = "";
    private static string key = "";
    private static string private_key = "";
    private static string public_key = "";
    private static string input_charset = "";
    private static string sign_type = "";
    #endregion

    static Config()
    {
      //↓↓↓↓↓↓↓↓↓↓请在这里配置您的基本信息↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓

      //合作身份者ID，以2088开头由16位纯数字组成的字符串
      partner = "2088801353613813";

      //交易安全检验码，由数字和字母组成的32位字符串
      //如果签名方式设置为“MD5”时，请设置该参数
      key = "u8hcqxfvv6k27nas4caaee36sw2szrm4";

      //商户的私钥
      //如果签名方式设置为“0001”时，请设置该参数
      private_key = @"MIICdwIBADANBgkqhkiG9w0BAQEFAASCAmEwggJdAgEAAoGBAKlhmq7w45NGBS/+GV7SAJc3kH16sp8YpAekeOJNkv2SvLiyK+h/+PjqUfCeMRNCJktHVmWZo5/GRwApbvVvo1xgqImPPcR0hvz8gV551+hCIkEUME/7HN6Tw2ZweseTTKZUg6pbcyZSqC0KqtaSC8k0X9BIs5GY+ATQceb2pBDtAgMBAAECgYBdz2IFbAMOySQMXZYdjv+mwK504KTXOqV2WiBdbgtwB3ogbQWmgmIZMmXpdyPE/8kHvl3abYpS7lTagRgkNQ1rL+9QjLu3Spzjemn6DeesG3Bwq20Wys3Jd/YParCt0LYj1edkMWPKUJm4KNGQi0/GqX1GhnvDV8h5taXuJsSvnQJBAOHE+v7T1Q+TRf4wjQ6+D7OzWXQIw3Yz9a/L0LnDjqOzCSf+L+quqp0yANXgKOwMZ2W0EfI+EOi+3GuVhN0XY3sCQQDAD7nXLIZv/RLKynatIBSjs3OOe0eZSBgVWRTvpz50o8Je5+iAv+MGPQ9OfMpRxr9Mi3u7SulLIo6Gybj615y3AkEA1TBJhuoHDe8Wr+GuwVYNw7W+6YgA8pQ8EqWWUmPsRH4iTkaDfssyxr9Z9Jv4aHWKCYqRS7r6Ap1/zINi89dLEwJAXZaMKVTyAZohUR4NbFlIlWh5c8GQ5xlhWu24rPsjDPVLXrHE8xAOEVkpn0Zh1C1zxFnnpiFfwxQIvZeIgvsTywJBAN0jzSaFGEFhjn0DPaecJ2JbywtfYCL1Nh0G7Iod7XHjExTLjc+tdSSZcHBNRq31Dtv8VhrHGceMXK8ING5Ng9g=";

      //支付宝的公钥
      //如果签名方式设置为“0001”时，请设置该参数
      public_key = @"MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCnxj/9qwVfgoUh/y2W89L6BkRAFljhNhgPdyPuBV64bfQNN1PjbCzkIM6qRdKBoLPXmKKMiFYnkd6rAoprih3/PrQEB/VsW8OoM8fxn67UDYuyBTqA23MML9q1+ilIZwBC2AQ2UBVOrFXfFl75p6/B5KsiNG9zpgmLCUYuLkxpLQIDAQAB";

      //↑↑↑↑↑↑↑↑↑↑请在这里配置您的基本信息↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑



      //字符编码格式 目前支持 utf-8
      input_charset = "utf-8";

      //签名方式，选择项：0001(RSA)、MD5
      sign_type = "MD5";
      //无线的产品中，签名方式为rsa时，sign_type需赋值为0001而不是RSA
    }

    #region 属性
    /// <summary>
    /// 获取或设置合作者身份ID
    /// </summary>
    public static string Partner
    {
      get { return partner; }
      set { partner = value; }
    }

    /// <summary>
    /// 获取或设交易安全校验码
    /// </summary>
    public static string Key
    {
      get { return key; }
      set { key = value; }
    }

    /// <summary>
    /// 获取或设置商户的私钥
    /// </summary>
    public static string Private_key
    {
      get { return private_key; }
      set { private_key = value; }
    }

    /// <summary>
    /// 获取或设置支付宝的公钥
    /// </summary>
    public static string Public_key
    {
      get { return public_key; }
      set { public_key = value; }
    }

    /// <summary>
    /// 获取字符编码格式
    /// </summary>
    public static string Input_charset
    {
      get { return input_charset; }
    }

    /// <summary>
    /// 获取签名方式
    /// </summary>
    public static string Sign_type
    {
      get { return sign_type; }
    }
    #endregion
  }
}