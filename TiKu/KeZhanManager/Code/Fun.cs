using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TiKu.Entity;

namespace KeZhanManager.Code
{

    public class Fun
    {
        public static void SetSessionUserInfo(dynamic context, tUserEntity userInfo)
        {
            context.Session["KeZhanUserInfo"] = userInfo;
        }

        public static tUserEntity GetSessionUserInfo(dynamic context)
        {
            tUserEntity rst = context.Session["KeZhanUserInfo"];
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