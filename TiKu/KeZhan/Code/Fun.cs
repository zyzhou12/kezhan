using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TiKuBll.Model;

namespace KeZhan.Code
{
  public class Fun
  {
    public static void SetSessionUserInfo(dynamic context, UserInfoModel userInfo)
    {
      context.Session["userInfo"] = userInfo;
    }

    public static UserInfoModel GetSessionUserInfo(dynamic context)
    {
      UserInfoModel rst = context.Session["userInfo"];
      return rst;
    }

  

    //public static ResponseModel<T> makeFailResponse<T>(string msg, T val = default(T))
    //{
    //  ResponseModel<T> rst = new ResponseModel<T>();
    //  rst.ResultObj = val;
    //  rst.setFail(msg);
    //  return rst;
    //}
  }
}