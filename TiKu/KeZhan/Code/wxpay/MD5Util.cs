using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace WXPay
{
  public class MD5Util
  {

    public static string GetMD5(string encypStr, string charset)
    {
      string retStr;
      MD5CryptoServiceProvider m5 = new MD5CryptoServiceProvider();

      //创建md5对象
      byte[] inputBye;
      byte[] outputBye;

      //使用GB2312编码方式把字符串转化为字节数组．
      try
      {
        inputBye = Encoding.GetEncoding(charset).GetBytes(encypStr);
      }
      catch (Exception ex)
      {
        inputBye = Encoding.GetEncoding("GB2312").GetBytes(encypStr);
      }
      outputBye = m5.ComputeHash(inputBye);

      retStr = System.BitConverter.ToString(outputBye);
      retStr = retStr.Replace("-", "").ToUpper();
      return retStr;
    }

    public static String getSha1(String str)
    {
      //建立SHA1对象
      //SHA1 sha = new SHA1CryptoServiceProvider();
      ////将mystr转换成byte[] 
      //ASCIIEncoding enc = new ASCIIEncoding();
      //byte[] dataToHash = enc.GetBytes(str);
      ////Hash运算
      //byte[] dataHashed = sha.ComputeHash(dataToHash);
      ////将运算结果转换成string
      //string hash = BitConverter.ToString(dataHashed).Replace("-", "");
      //return hash;

      return System.Web.Security.FormsAuthentication.
          HashPasswordForStoringInConfigFile(str, "sha1").ToLower();  
    }
  }
}