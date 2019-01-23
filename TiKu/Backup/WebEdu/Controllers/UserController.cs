using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebEdu.Models;
using TiKu.Bll;
using WebEdu.Filters;
using TiKuBll.Model;
using System.Web.Script.Serialization;

namespace WebEdu.Controllers
{
  [UserActionFilter]
  public class UserController : Controller
  {
    //
    // GET: /User/
    public ActionResult UserInfoEdit()
    {
      UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);


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


    public JsonResult SaveTeacherValidFile(TeacherValidModel model)
    {
      UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
      ResponseBaseModel response = new ResponseBaseModel();

      if (string.IsNullOrEmpty(model.fIDCard1))
      {
        response.iResult = -1;
        response.strMsg = "请上传身份证正面";
      }
      else if (string.IsNullOrEmpty(model.fIDCard2))
      {
        response.iResult = -1;
        response.strMsg = "请上传身份证反面";
      }
      else if (string.IsNullOrEmpty(model.fPharse))
      {
        response.iResult = -1;
        response.strMsg = "请选择选段";
      }
      else if (string.IsNullOrEmpty(model.fSubject))
      {
        response.iResult = -1;
        response.strMsg = "请选择学科";
      }
      else if (string.IsNullOrEmpty(model.fCertType))
      {
        response.iResult = -1;
        response.strMsg = "请选择证书类型";
      }
      else if (string.IsNullOrEmpty(model.fTeachCert1))
      {
        response.iResult = -1;
        response.strMsg = "请上传证书文件";
      }
      else
      {
        response.iResult = UserBll.SaveTeacherValidInfo(userInfo.fUserName, model);


      }

      JsonResult jr = new JsonResult();
      jr.Data = response;
      return jr;
    }

    public ActionResult ValidInfo(int iValidID)
    {

      TeacherValidModel model = UserBll.GetTeacherValid(iValidID);
      return View(model);
    }

    public ActionResult UpdatePassWord(string redirect_uri=null)
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
      return View(userInfo);
    }



    public ActionResult UserAccount()
    {
      return View();
    }

    public ActionResult QueryUserAccount()
    {
      UserInfoModel userInfo = Code.Fun.GetSessionUserInfo(this);
      UserAccountListModel model = UserBll.GetUserAccountData(userInfo.fUserName);

      return PartialView("UserAccountListControl", model);
    }

  }
}
