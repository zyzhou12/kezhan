using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeZhan.Models;
using TiKu.Bll;
using KeZhan.Filters;
using TiKuBll.Model;
using System.Web.Script.Serialization;
using TiKuBll;

namespace KeZhan.Controllers
{
    [UserActionFilter]
    public class UserController : Controller
    {
        //
        // GET: /User/
        public ActionResult UserInfoEdit()
        {


            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            if (UserBll.CheckUserInfo(userInfo.fUserName, userInfo.fRole))//已补全信息
            {
                return RedirectToAction("ClassRoomList", "Open");
            }
            else
            {

                UserBaseModel model = new UserBaseModel();
                model.userRole = userInfo.fRole;
                model.userInfo = userInfo;
                if (userInfo.fRole == "Teacher")
                {
                    model.teacherInfo = UserBll.GetUserTeacherBase(userInfo.fUserName);
                }
                else if (userInfo.fRole == "Student")
                {
                    model.studentInfo = UserBll.GetUserStudentBase(userInfo.fUserName);
                }
                else if (userInfo.fRole == "Parents")
                {
                    model.parentsInfo = UserBll.GetUserParentsBase(userInfo.fUserName);
                }
                return View(model);
            }

        }




        public JsonResult DoSaveTeacherInfo(TeacherBaseModel model)
        {
            ResponseBaseModel response = new ResponseBaseModel();

            if (string.IsNullOrEmpty(model.fDesc))
            {
                response.iResult = -1;
                response.strMsg = "请输入介绍";
            }
            else if (model.fIsLive && model.fVideoFee <= 0)
            {
                response.iResult = -1;
                response.strMsg = "开通一对一必须填写费用";
            }
            else if (model.fIsProblem && model.fProblemFee <= 0)
            {
                response.iResult = -1;
                response.strMsg = "开通答疑必须填写费用";
            }
            else
            {
                UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
                response.iResult = UserBll.SaveTeacherInfo(userInfo.fUserName, model);


            }


            JsonResult jr = new JsonResult();
            jr.Data = response;
            return jr;
        }


        [HttpPost]
        public JsonResult DoSaveStudentInfo(StudentBaseModel model)
        {
            ResponseBaseModel response = new ResponseBaseModel();

            if (string.IsNullOrEmpty(model.Pharse))
            {
                response.iResult = -1;
                response.strMsg = "请选择学段";
            }
            else if (string.IsNullOrEmpty(model.Grade))
            {
                response.iResult = -1;
                response.strMsg = "请选择年级";
            }
            else if (string.IsNullOrEmpty(model.School))
            {
                response.iResult = -1;
                response.strMsg = "请填写您的学校名称";
            }
            else
            {
                UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
                response.iResult = UserBll.SaveStudentInfo(userInfo.fUserName, model);


            }
            JsonResult jr = new JsonResult();
            jr.Data = response;
            return jr;
        }


        [HttpPost]
        public JsonResult DoSaveParentsInfo()
        {
            JsonResult jr = new JsonResult();
            //jr.Data = response;
            return jr;
        }


        public JsonResult SaveTeacherValidFile(string jsonData)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            ResponseBaseModel response = new ResponseBaseModel();
            try
            {
                TeacherValidModel model = new JavaScriptSerializer().Deserialize<TeacherValidModel>(jsonData);
                TeacherBaseModel teacher = UserBll.GetUserTeacherBase(userInfo.fUserName);
                if (teacher.fStatus == "未认证" && string.IsNullOrEmpty(model.fIDCard1))
                {
                    response.iResult = -1;
                    response.strMsg = "请上传证件照";
                }
                else if (string.IsNullOrEmpty(model.fName))
                {
                    response.iResult = -1;
                    response.strMsg = "请输入真实姓名";
                }
                else if (string.IsNullOrEmpty(model.fIDType))
                {
                    response.iResult = -1;
                    response.strMsg = "请选择证件类型";
                }
                else if (string.IsNullOrEmpty(model.fUID))
                {
                    response.iResult = -1;
                    response.strMsg = "请输入证件号码";
                }
                else if (model.fIDType=="1" && model.fUID.Length!=18)
                {
                    response.iResult = -1;
                    response.strMsg = "证件号码错误";
                }
                else if (model.detailList == null || model.detailList.Count <= 0)
                {
                    response.iResult = -1;
                    response.strMsg = "请上传证书文件";
                }
                else
                {

                    response.iResult = UserBll.SaveTeacherValidInfo(userInfo.fUserName, model);

                }
            }
            catch (Exception ex)
            {
                response.iResult = -1;
                response.strMsg = ex.Message;
            }
            JsonResult jr = new JsonResult();
            jr.Data = response;
            return jr;
        }

        public ActionResult ValidInfo(int iValidID)
        {

            ValidDetailListModel model = UserBll.GetTeachValidDetailListByID(iValidID);
            return View(model);
        }

        public ActionResult UpdatePassWord(string redirect_uri = null)
        {
            FileModel model = new FileModel();
            if (string.IsNullOrEmpty(redirect_uri))
            {
                model.FileUrl = "";
            }
            else
            {
                model.FileUrl = redirect_uri;
            }
            return View(model);
        }

        public ActionResult UserInfo()
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);

            UserBaseModel model = new UserBaseModel();
            model.userRole = userInfo.fRole;
            model.userInfo = UserBll.GetUserInfo(userInfo.fUserName);
            if (userInfo.fRole == "Teacher")
            {
                model.teacherInfo = UserBll.GetUserTeacherBase(userInfo.fUserName);
                model.validInfo = UserBll.GettTeachValidList(userInfo.fUserName);
            }
            else if (userInfo.fRole == "Student")
            {
                model.studentInfo = UserBll.GetUserStudentBase(userInfo.fUserName);
            }
            else if (userInfo.fRole == "Parents")
            {
                model.parentsInfo = UserBll.GetUserParentsBase(userInfo.fUserName);
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult DoSaveInfo(string strType, string strInfo)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            ResponseBaseModel response = new ResponseBaseModel();
            int i = UserBll.SaveUserInfo(userInfo.fUserName, strType, strInfo);

            JsonResult jr = new JsonResult();
            jr.Data = response;
            return jr;
        }

        [HttpPost]
        public JsonResult DoUpdateMobile(string strMobile, string strCode)
        {
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
                UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);

                //校验验证码
                UserInfoModel newUser = UserBll.GetUserInfo("1" + strMobile);
                if (newUser.fCode == strCode)
                {
                    UserBll.SaveUserInfo(userInfo.fUserName, "fMobile", strMobile);

                    response.iResult = 0;
                }
                else
                {
                    response.strMsg = "验证码不对";
                    response.iResult = -1;
                }



            }

            JsonResult jr = new JsonResult();
            jr.Data = response;
            return jr;
        }


        /// <summary>
        /// 银行账户管理
        /// </summary>
        /// <returns></returns>
        public ActionResult UserBankManager()
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            UserBankAccountListModel model = UserBll.GetUserBankAccountList(userInfo.fUserName);

            return View(model);
        }

        public ActionResult UserBankEdit(int iBankId)
        {
            UserBankAccountModel model = new UserBankAccountModel();
            if (iBankId > 0)
            {
                model = UserBll.GetUserBankAccount(iBankId);
            }
            return View(model);
        }


        public JsonResult DoSaveUserBank(UserBankAccountModel model)
        {
            ResponseBaseModel response = new ResponseBaseModel();
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            if (model.fID <= 0)
            {
                model.fUserName = userInfo.fUserName;
                model.fCreateDate = DateTime.Now;
                model.fCreateOpr = userInfo.fUserName;
            }
            int i = UserBll.SaveUserBankAccount(model);

            JsonResult jr = new JsonResult();
            jr.Data = response;
            return jr;

        }


        public ActionResult WeiXinLogin(string redirect_uri = null)
        {

            if (string.IsNullOrEmpty(redirect_uri))
            {
                redirect_uri = Request.Url.ToString();
            }
            string strAppID = "";
            string strState = "kezhanlogin";

            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);


            string oauthUrl = string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_base&state={2}#wechat_redirect", strAppID, "http://weixin.aizhusoft.com/AuthoRedirect.aspx?url=http://hotellife.aizhusoft.com/usercount/DoWeiXinLogin?strParams=" + strAppID, strState);


            return Redirect(oauthUrl);
        }




        //public ActionResult DoWeiXinLogin(string code, string strParams, string state)
        //{
        //  string redirecturi = "http://www.baidu.com?1=" + code + strParams + state;

        //  try
        //  {
        //    string strAppID = "";
        //    string strAppSecret = "";

        //    string sUrl = String.Format(APIRequest.GetUrl("getaccesstoken"), strAppID, strAppSecret, code);

        //    String strJson = APIRequest.GetData(sUrl);

        //    JavaScriptSerializer jss = new JavaScriptSerializer();

        //    PublicTokenModel token = jss.Deserialize<PublicTokenModel>(strJson);

        //    if (token.access_token != null)
        //    {
        //      //更新用户信息
        //      ResponseModel<UserModel> Response = UserBll.getUser(microPublic.fPublicID, token.openid);

        //      //先登录

        //      UserInfoModel userInfo = new UserInfoModel();
        //      userInfo.User = Response.ResultObj;


        //      if (!string.IsNullOrEmpty(token.access_token))
        //      {
        //        string strtoken = HotelLife.Bll.ConfigBll.GetToken(strAppID);

        //        //获取个人信息
        //        //sUrl = String.Format(APIRequest.GetUrl("getuserinfo"), strtoken, token.openid);
        //        //strJson = APIRequest.GetData(sUrl);
        //        //PublicUserModel user = jss.Deserialize<PublicUserModel>(strJson);
        //        //userInfo.User.fNickName = user.nickname;
        //        //userInfo.User.fHeadImg = user.headimgurl;
        //        //userInfo.User.fsubscribe = user.subscribe;
        //      }
        //      Code.Fun.SetSessionUserInfo(this, userInfo);



        //    }


        //    return Redirect("");
        //  }
        //  catch (Exception ex)
        //  {
        //    return Redirect(redirecturi + "&" + ex.Message);
        //  }
        //}




        public ActionResult UpdateRole()
        {
            return View();
        }

        public ActionResult UserPay()
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            ConfigModel config = ManagerBll.GetSystemConfig("上海");
            UserPayModel model = new UserPayModel();
            model.UserName = userInfo.fUserName;
            model.ClassFee = config.fClassFee;
            return View(model);
        }

        public JsonResult SubmitBuyFlowOrder(int iNum, decimal dPrice)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            JsonResult jr = new JsonResult();
            jr.Data = UserBll.SubmitBuyFlowOrder(userInfo.fUserName, iNum, dPrice, "购买", Convert.ToDateTime("2099-12-31"), userInfo.fUserName);
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }



        public ActionResult UserAccount()
        {
            return View();
        }

        public ActionResult QueryUserAccount(string strTradingType, string strSystem, string strType, DateTime beginDate, DateTime endDate)
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            UserAccountListModel model = UserBll.GetUserAccountData(userInfo.fUserName, strTradingType, strSystem, strType, beginDate, endDate);

            return PartialView("UserAccountListControl", model);
        }


        public ActionResult TeacherWithdrawalApply()
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            UserBankAccountListModel model = UserBll.GetUserBankAccountList(userInfo.fUserName);

            return View(model);
        }

        public JsonResult DoSaveTeacherWithDrawal(TeacherWithdrawalModel model)
        {
            ResponseBaseModel response = new ResponseBaseModel();
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            string strMsg = "";
            int checkResult = UserBll.CheckWithdrawal(Convert.ToInt32(model.fAmount), userInfo.fUserName, ref strMsg);
            if (checkResult == 0)
            {

                model.fUserName = userInfo.fUserName;
                model.fSubmitDate = DateTime.Now;
                model.fStatus = "0";
                model.fCreateDate = DateTime.Now;
                model.fCreateOpr = userInfo.fUserName;

                int i = UserBll.SaveTeacherWithdrawal(model);
                response.iResult = 1;
            }
            else
            {
                response.iResult = -1;
                response.strMsg = strMsg;
            }

            JsonResult jr = new JsonResult();
            jr.Data = response;
            return jr;
        }

        public ActionResult UserMemberList()
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            UserMemberListModel model = UserBll.GetUserMemberList(userInfo.fUserName, "", null, null);
            return View(model);
        }

        public ActionResult QueryMemberList()
        {
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            UserMemberListModel model = UserBll.GetMemberList(userInfo.fUserName, "");

            return PartialView("UserMemberListControl", model);
        }

        public JsonResult CheckUser(string strMobile)
        {
            ResponseBaseModel response = new ResponseBaseModel();

            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            UserInfoModel model = UserBll.GetUserInfoByMobile(strMobile);
            if (model == null)
            {
                response.iResult = -1;
                response.strMsg = "该手机号还未注册，请确认手机号是否正确";
            }
            else if (model.fRole != "Teacher")
            {
                response.iResult = -1;
                response.strMsg = "该用户不是老师，不能邀请";
            }
            else
            {

                UserMemberModel um = UserBll.GetUserMember(userInfo.fUserName, strMobile);
                if (um != null && um.fStatus.Trim() == "2")
                {
                    response.iResult = -1;
                    response.strMsg = "该用户已是你的授课老师，不用重复邀请";
                }
                else
                {
                    response.iResult = 0;
                }
            }

            JsonResult jr = new JsonResult();
            jr.Data = response;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }

        /// <summary>
        /// 邀请授课老师
        /// </summary>
        /// <param name="strMobile"></param>
        /// <param name="strNote"></param>
        /// <returns></returns>
        public JsonResult InviteMember(string strMobile, string strNote)
        {
            ResponseBaseModel response = new ResponseBaseModel();
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
            string strMsg = "";
            strNote = userInfo.fNickName + "邀请您成为他的授课老师，是否同意" + strNote;
            response.iResult = UserBll.UserInviteMember(userInfo.fUserName, strMobile, strNote, ref strMsg);
            response.strMsg = strMsg;

            JsonResult jr = new JsonResult();
            jr.Data = response;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }

        public JsonResult InviteAgree(int id)
        {
            ResponseBaseModel response = new ResponseBaseModel();
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);

            response.iResult = UserBll.UserInviteAgree(userInfo.fUserName, id);

            JsonResult jr = new JsonResult();
            jr.Data = response;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }
        public JsonResult InviteRefused(int id)
        {
            ResponseBaseModel response = new ResponseBaseModel();
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);

            response.iResult = UserBll.UserInviteRefused(userInfo.fUserName, id);

            JsonResult jr = new JsonResult();
            jr.Data = response;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }


        public JsonResult InviteCancel(int id)
        {
            ResponseBaseModel response = new ResponseBaseModel();
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);

            response.iResult = UserBll.UserInviteCancel(userInfo.fUserName, id);

            JsonResult jr = new JsonResult();
            jr.Data = response;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }


        public JsonResult InviteRemove(int id)
        {
            ResponseBaseModel response = new ResponseBaseModel();
            UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);

            response.iResult = UserBll.UserInviteRemove(userInfo.fUserName, id);

            JsonResult jr = new JsonResult();
            jr.Data = response;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }

    }
}
