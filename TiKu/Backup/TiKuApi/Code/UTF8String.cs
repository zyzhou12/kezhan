using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TiKu.Api.Code
{
  public class UTF8String
  {
    string m_strContent = "";
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="content">要转换的字符串</param>
    public UTF8String(string content)
    {
      m_strContent = content;
    }
    public string getContent()
    {
      return m_strContent;
    }
    /// <summary>
    /// 转换函数
    /// </summary>
    /// <returns>返回转换好的字符串</returns>
    public string ToString()
    {
      string reString = null;
      char[] content = m_strContent.ToCharArray(); //把字符串变为字符数组，以进行处理
      for (int i = 0; i < content.Length; i++) //遍历所有字符
      {
        if (content[i] == '\\') //判断是否转义字符 \ 
        {
          switch (content[i + 1]) //判断转义字符的下一个字符是什么
          {
            case 'u': //转换的是汉字
            case 'U':
              reString += HexArrayToChar(content, i + 2); //获取对应的汉字
              i = i + 5;
              break;
            case '/': //转换的是 /
            case '\\': //转换的是 \
            case '"':
              break;
            default: //其它
              reString += EscapeCharacter(content[i + 1]); //转为其它类型字符
              i = i + 1;
              break;
          }
        }
        else
          reString += content[i]; //非转义字符则直接加入
      }
      return reString;
    }

    /// <summary>
    /// 字符数组转对应汉字字符
    /// </summary>
    /// <param name="content">要转换的数字</param>
    /// <param name="startIndex">起始位置</param>
    /// <returns>对应的汉字</returns>
    private char HexArrayToChar(char[] content, int startIndex)
    {
      char[] ac = new char[4];
      for (int i = 0; i < 4; i++) //获取要转换的部分
        ac[i] = content[startIndex + i];
      string num = new string(ac); //字符数组转为字符串
      return HexStringToChar(num);
    }

    /// <summary>
    /// 转义字符转换函数
    /// 转换字符为对应的转义字符
    /// </summary>
    /// <param name="c">要转的字符</param>
    /// <returns>对应的转义字符</returns>
    private char EscapeCharacter(char c)
    {
      char rc;
      switch (c)
      {
        case 't':
          c = '\t';
          break;
        case 'n':
          c = '\n';
          break;
        case 'r':
          c = '\r';
          break;
        case '\'':
          c = '\'';
          break;
        case '0':
          c = '\0';
          break;
      }
      return c;
    }

    /// <summary>
    /// 字符串转对应汉字字符
    /// 只能处理如"8d34"之类的数字字符为对应的汉字
    /// 例子："9648" 转为 '陈'
    /// </summary>
    /// <param name="content">转换的字符串</param>
    /// <returns>对应的汉字</returns>
    public static char HexStringToChar(string content)
    {
      int num = Convert.ToInt32(content, 16);
      return (char)num;
    }

    /// <summary>
    /// 把string转为UTF8String类型
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    public static UTF8String ValueOf(string content)
    {
      string reString = null;
      char[] ac = content.ToCharArray();
      int num;
      foreach (char c in ac)
      {
        num = (int)c;
        string n = num.ToString("X2");
        if (n.Length == 4)
          reString += "\\u" + n;
        else
          reString += c;
      }
      return new UTF8String(reString);
    }
  }
}