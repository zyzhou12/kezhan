using KeZhan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiKu.Bll;
using TiKuBll.Model;

namespace KeZhan.Controllers
{
    public class RequestController : Controller
    {
        [HttpPost]
        public JsonResult DoSendCode(string strMobile)
        {
            string strMsg = "";
            ResponseBaseModel response = new ResponseBaseModel();
            if (string.IsNullOrEmpty(strMobile))
            {
                response.iResult = -1;
                response.strMsg = "请输入手机号";
            }
            else
            {
                response.iResult = UserBll.UserSendCode(strMobile, ref strMsg);
                response.strMsg = strMsg;
            }

            JsonResult jr = new JsonResult();
            jr.Data = response;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }


        [HttpPost]
        public JsonResult DoValidCode(string strMobile, string strCode, string strRole)
        {
            string strMsg = "";
            ResponseBaseModel response = new ResponseBaseModel();
            if (string.IsNullOrEmpty(strMobile))
            {
                response.iResult = -1;
                response.strMsg = "请输入手机号";
            }
            else if (string.IsNullOrEmpty(strCode))
            {
                response.iResult = -1;
                response.strMsg = "请输入验证码";
            }
            else
            {
                UserInfoModel userInfo = UserBll.UserLogin(strMobile, strCode, strRole, "Web", ref strMsg);
                response.strMsg = strMsg;

                if (userInfo != null)
                {

                    Code.Fun.SetSessionUserInfo(this, userInfo);
                    if (string.IsNullOrEmpty(userInfo.IsPassWord))
                    {
                        response.iResult = 2;
                    }
                    else if (UserBll.CheckUserInfo(userInfo.fUserName, userInfo.fRole))
                    {
                        response.iResult = 0;
                    }
                    else
                    {
                        response.iResult = 1;
                    }
                }
                else
                {
                    response.iResult = -1;
                }
            }

            JsonResult jr = new JsonResult();
            jr.Data = response;
            return jr;
        }
        [HttpPost]
        public JsonResult DoTeacherValidCode(string strMobile, string strCode, string strPass, string strPass2)
        {
            string strMsg = "";
            ResponseBaseModel response = new ResponseBaseModel();
            if (string.IsNullOrEmpty(strMobile))
            {
                response.iResult = -1;
                response.strMsg = "请输入手机号";
            }
            else if (string.IsNullOrEmpty(strCode))
            {
                response.iResult = -1;
                response.strMsg = "请输入验证码";
            }
            else if (string.IsNullOrEmpty(strPass))
            {
                response.iResult = -1;
                response.strMsg = "请输入密码";
            }
            else if (string.IsNullOrEmpty(strPass2))
            {
                response.iResult = -1;
                response.strMsg = "请再次输入密码";
            }
            else
            {
                UserInfoModel userInfo = UserBll.UserLogin(strMobile, strCode, "Teacher", "Web", ref strMsg);
                response.strMsg = strMsg;

                if (userInfo != null)
                {
                    //设置密码
                    UserBll.SaveUserInfo(userInfo.fUserName, "fPassWord", strPass);

                    Code.Fun.SetSessionUserInfo(this, userInfo);
                    if (UserBll.CheckUserInfo(userInfo.fUserName, userInfo.fRole))
                    {
                        response.iResult = 0;
                    }
                    else
                    {
                        response.iResult = 1;
                    }
                }
                else
                {
                    response.iResult = -1;
                }
            }

            JsonResult jr = new JsonResult();
            jr.Data = response;
            return jr;
        }
        [HttpPost]
        public JsonResult DoStudentValidCode(string strMobile, string strCode, string strNickName, string strPass, string strPass2)
        {
            string strMsg = "";
            ResponseBaseModel response = new ResponseBaseModel();
            if (string.IsNullOrEmpty(strMobile))
            {
                response.iResult = -1;
                response.strMsg = "请输入手机号";
            }
            else if (string.IsNullOrEmpty(strCode))
            {
                response.iResult = -1;
                response.strMsg = "请输入验证码";
            }
            else if (string.IsNullOrEmpty(strNickName))
            {
                response.iResult = -1;
                response.strMsg = "请输入昵称";
            }
            else if (string.IsNullOrEmpty(strPass))
            {
                response.iResult = -1;
                response.strMsg = "请输入密码";
            }
            else if (string.IsNullOrEmpty(strPass2))
            {
                response.iResult = -1;
                response.strMsg = "请再次输入密码";
            }
            else
            {
                UserInfoModel userInfo = UserBll.UserLogin(strMobile, strCode, "Student", "Web", ref strMsg);
                response.strMsg = strMsg;

                if (userInfo != null)
                {
                    UserBll.SaveUserInfo(userInfo.fUserName, "fNickName", strNickName);
                    //设置密码
                    UserBll.SaveUserInfo(userInfo.fUserName, "fPassWord", strPass);

                    Code.Fun.SetSessionUserInfo(this, userInfo);
                    if (UserBll.CheckUserInfo(userInfo.fUserName, userInfo.fRole))
                    {
                        response.iResult = 0;
                    }
                    else
                    {
                        response.iResult = 1;
                    }
                }
                else
                {
                    response.iResult = -1;
                }
            }

            JsonResult jr = new JsonResult();
            jr.Data = response;
            return jr;
        }


        [HttpPost]
        public JsonResult DoValidPass(string strMobile, string strPass)
        {
            string strMsg = "";
            ResponseBaseModel response = new ResponseBaseModel();
            if (string.IsNullOrEmpty(strMobile))
            {
                response.iResult = -1;
                response.strMsg = "请输入用户名或手机号";
            }
            else if (string.IsNullOrEmpty(strPass))
            {
                response.iResult = -1;
                response.strMsg = "请输入密码";
            }
            else
            {
                UserInfoModel userInfo = UserBll.UserPassLogin(strMobile, strPass, "Web", ref strMsg);
                response.strMsg = strMsg;



                if (userInfo != null)
                {

                    Code.Fun.SetSessionUserInfo(this, userInfo);
                    if (UserBll.CheckUserInfo(userInfo.fUserName, userInfo.fRole))
                    {

                        response.iResult = 0;
                    }
                    else
                    {
                        response.iResult = 1;
                    }
                }
                else
                {
                    response.iResult = -1;
                }
            }

            JsonResult jr = new JsonResult();
            jr.Data = response;
            return jr;
        }

        public JsonResult GetMessageCount()
        {
            ResponseBaseModel response = new ResponseBaseModel();
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            if (userInfo == null)
            {
                response.iResult = -9999;
            }
            else
            {
                MessageListModel model = UserBll.GetMessageList(userInfo.fUserName, "0");
                response.iResult = model.messageList.Count;
            }

            JsonResult jr = new JsonResult();
            jr.Data = response;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }


        public JsonResult CheckLoginUser(string redirect_uri)
        {
            UserInfoModel user = Code.Fun.GetSessionUserInfo(this);
            ResponseBaseModel response = new ResponseBaseModel();
            if (user == null)
            {
                response.iResult = -9999;
                response.strMsg = redirect_uri;
            }
            else
            {
                response.iResult = 0;
            }

            JsonResult jr = new JsonResult();
            jr.Data = response;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }


    }
}
