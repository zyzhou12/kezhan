using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiKu.Dal;
using TiKu.Entity;
using TiKuBll.Model;
using System.Data;
using Trip8H.Common;

namespace TiKu.Bll
{
  public class UserBll
  {
    /// <summary>
    /// 发送验证码
    /// </summary>
    /// <param name="strMobile"></param>
    /// <param name="strMsg"></param>
    /// <returns></returns>
    public static int UserSendCode(string strMobile, ref string strMsg)
    {
      string strCode = new Random().Next(100000, 999999).ToString();
      return tUserDal.UserSendCode(strMobile, strCode, ref strMsg);
    }

    /// <summary>
    /// 验证验证码登录
    /// </summary>
    /// <param name="strMobile"></param>
    /// <param name="strCode"></param>
    /// <param name="strRole"></param>
    /// <param name="strMsg"></param>
    /// <returns></returns>
    public static UserInfoModel UserLogin(string strMobile, string strCode, string strRole, string strSystem, ref string strMsg)
    {
      UserInfoModel model = null;
      tUserEntity user = tUserDal.UserLogin(strMobile, strCode, strRole, strSystem, ref strMsg);
      if (user != null)
      {
        model = new UserInfoModel();
        model.fCity = user.fCity;
        model.fEmail = user.fEmail;
        model.fHeadImg = user.fHeadImg;
        model.fMobile = user.fMobile;
        model.fName = user.fName;
        model.fNickName = user.fNickName;
        model.fRegSystem = user.fRegSystem;
        model.fRole = user.fRole;
        model.fStatus = user.fStatus;
        model.fUID = user.fUID;
        model.fUserName = user.fUserName;
        if (!string.IsNullOrEmpty(user.fPassWord))
        {
          model.IsPassWord = "true";
        }
      }
      return model;
    }

    public static UserInfoModel UserPassLogin(string strMobile, string strPass, string strSystem, ref string strMsg)
    {
      UserInfoModel model = null;
      tUserEntity user = tUserDal.UserPassLogin(strMobile, strPass, strSystem, ref strMsg);
      if (user != null)
      {
        model = new UserInfoModel();
        model.fCity = user.fCity;
        model.fEmail = user.fEmail;
        model.fHeadImg = user.fHeadImg;
        model.fMobile = user.fMobile;
        model.fName = user.fName;
        model.fNickName = user.fNickName;
        model.fRegSystem = user.fRegSystem;
        model.fRole = user.fRole;
        model.fStatus = user.fStatus;
        model.fUID = user.fUID;
        model.fUserName = user.fUserName;
      }
      return model;
    }

    public static UserInfoModel GetUserInfo(string strUserName)
    {
      UserInfoModel model = new UserInfoModel();
      tUserEntity user = tUserDal.GettUser(strUserName);
      model.fCity = user.fCity;
      model.fEmail = user.fEmail;
      model.fHeadImg = user.fHeadImg;
      model.fMobile = user.fMobile;
      model.fName = user.fName;
      model.fNickName = user.fNickName;
      model.fRegSystem = user.fRegSystem;
      model.fRole = user.fRole;
      model.fStatus = user.fStatus;
      model.fUID = user.fUID;
      model.fUserName = user.fUserName;
      return model;
    }


    public static TeacherBaseModel GetUserTeacherBase(string strUserName)
    {
      DataTable dt = tUserDal.GetUserInfoByRole(strUserName, "Teacher", "Base");

      List<TeacherBaseModel> lstRst = PubFun.DataTableToObjects<TeacherBaseModel>(dt);
      TeacherBaseModel model = new TeacherBaseModel();
      if (lstRst != null && lstRst.Count > 0)
      {
        model = lstRst[0];
      }
      return model;
    }

    public static TeacherValidModel GetTeacherValid(int iFID)
    {
      tTeachValidEntity valid = tTeachValidDal.GettTeachValid(iFID);
      TeacherValidModel model = new TeacherValidModel();
      if (valid != null)
      {
        model.fID = valid.fID;
        model.fIDCard1 = valid.fIDCard1;
        model.fIDCard2 = valid.fIDCard2;
        model.fCertType = valid.fCertType;
        model.fPharse = valid.fPharse;
        model.fSubject = valid.fSubject;
        model.fTeachCert1 = valid.fTeachCert1;
        model.fTeachCert2 = valid.fTeachCert2;
        model.fValidDate = valid.fValidDate;
        model.fNote = valid.fNote;
        model.fStatus = valid.fStatus;
      }
      return model;
    }

    public static TeacherValidListModel GettTeachValidList(string strUserName,string strStatus=null)
    {
      List<tTeachValidEntity> entityList = tTeachValidDal.GettTeachValidList(strUserName, strStatus);
      TeacherValidListModel model = new TeacherValidListModel();
      List<TeacherValidModel> validList = new List<TeacherValidModel>();
      foreach (var entity in entityList)
      {
        TeacherValidModel valid = new TeacherValidModel();
        valid.fID = entity.fID;
        valid.fIDCard1 = entity.fIDCard1;
        valid.fIDCard2 = entity.fIDCard1;
        valid.fCertType = entity.fCertType;
        valid.fPharse = entity.fPharse;
        valid.fSubject = entity.fSubject;
        valid.fTeachCert1 = entity.fIDCard1;
        valid.fTeachCert2 = entity.fIDCard1;
        valid.fStatus = entity.fStatus;

        valid.fValidDate = entity.fValidDate;
        valid.fNote = entity.fNote;
        validList.Add(valid);
      }
      model.validList = validList;
      return model;
    }

    public static StudentBaseModel GetUserStudentBase(string strUserName)
    {
      DataTable dt = tUserDal.GetUserInfoByRole(strUserName, "Student", "Base");

      List<StudentBaseModel> lstRst = PubFun.DataTableToObjects<StudentBaseModel>(dt);

      StudentBaseModel model = new StudentBaseModel();
      if (lstRst != null && lstRst.Count > 0)
      {
        model = lstRst[0];
      }
      return model;
    }

    public static ParentsBaseModel GetUserParentsBase(string strUserName)
    {
      DataTable dt = tUserDal.GetUserInfoByRole(strUserName, "Parents", "Base");

      List<ParentsBaseModel> lstRst = PubFun.DataTableToObjects<ParentsBaseModel>(dt);

      ParentsBaseModel model = new ParentsBaseModel();
      if (lstRst != null && lstRst.Count > 0)
      {
        model = lstRst[0];
      }
      return model;
    }

    /// <summary>
    /// 检查信息是否补全
    /// </summary>
    /// <param name="strUserName"></param>
    /// <param name="strRole"></param>
    /// <returns></returns>
    public static bool CheckUserInfo(string strUserName, string strRole)
    {
      bool IsCheck = true;
      if (strRole == "Teacher")
      {
        tTeachInfoEntity teacher = tTeachInfoDal.GettTeachInfoByUserName(strUserName);
        if (teacher == null)
        {
          IsCheck = false;
        }
        else if (string.IsNullOrEmpty(teacher.fDesc))
        {
          IsCheck = false;
        }
        else if (teacher.fStatus != "已认证")
        {
          IsCheck = false;
        }
      }
      else if (strRole == "Student")
      {
        tStudentInfoEntity student = tStudentInfoDal.GettStudentInfoByUserName(strUserName);
        if (student == null)
        {
          IsCheck = false;
        }
        else if (string.IsNullOrEmpty(student.fSchool))
        {
          IsCheck = false;
        }
      }
      return IsCheck;
    }

    public static int SaveUserInfo(string strUserName, string InfoType, string strValue)
    {
      tUserEntity user = tUserDal.GettUser(strUserName);
      if (InfoType == "fMobile")
      {
        user.fMobile = strValue;
      }
      else if (InfoType == "fNickName")
      {
        user.fNickName = strValue;
      }
      else if (InfoType == "fHeadImg")
      {
        user.fHeadImg = strValue;
      }
      else if (InfoType == "fPassWord")
      {
        user.fPassWord = strValue;
      }
      else if (InfoType == "fTradePassWord")
      {
        user.fTradePassWord = strValue;
      }
      else if (InfoType == "fCity")
      {
        user.fCity = strValue;
      }
      List<tUserEntity> list = new List<tUserEntity>();
      list.Add(user);
      int i = tUserDal.Modify(list, "update", "fID," + InfoType, null);
      return i;
    }


    public static int SaveTeacherInfo(string strUserName, TeacherBaseModel model)
    {
      tTeachInfoEntity teacher = tTeachInfoDal.GettTeachInfoByUserName(strUserName);
      if (teacher == null)
      {
        teacher = new tTeachInfoEntity();
      }
      teacher.fDesc = model.fDesc;
      teacher.fQrCode = model.fQrCode;
      teacher.fIDCard1 = model.fIDCard1;
      teacher.fIDCard2 = model.fIDCard2;
      teacher.fIsClassRoom = model.fIsClassRoom;
      teacher.fIsLive = model.fIsLive;
      teacher.fIsProblem = model.fIsProblem;

      teacher.fVideoFee = model.fVideoFee;
      teacher.fProblemFee = model.fProblemFee;

      int i = 0;
      List<tTeachInfoEntity> list = new List<tTeachInfoEntity>();
      if (teacher.fID == 0)
      {
        teacher.fCreateDate = DateTime.Now;
        teacher.fCreateOpr = strUserName;
        teacher.fUserName = strUserName;
        teacher.fStatus = "未认证";
        list.Add(teacher);
        i = tTeachInfoDal.Modify(list, "insert", null, null);
      }
      else
      {
        teacher.fModifyDate = DateTime.Now;
        teacher.fModifyOpr = strUserName;
        list.Add(teacher);
        i = tTeachInfoDal.Modify(list, "update", null, null);//修改字段
      }

      return i;
    }

    public static int SaveStudentInfo(string strUserName, StudentBaseModel model)
    {
      tStudentInfoEntity student = tStudentInfoDal.GettStudentInfoByUserName(strUserName);
      if (student == null)
      {
        student = new tStudentInfoEntity();
      }
      student.fGrade = model.Grade;
      student.fPharse = model.Pharse;
      student.fSchool = model.School;

      int i = 0;
      List<tStudentInfoEntity> list = new List<tStudentInfoEntity>();
      if (student.fID == 0)
      {
        student.fCreateDate = DateTime.Now;
        student.fCreateOpr = strUserName;
        student.fUserName = strUserName;
        list.Add(student);
        i = tStudentInfoDal.Modify(list, "insert", null, null);
      }
      else
      {
        student.fModifyDate = DateTime.Now;
        student.fModifyOpr = strUserName;
        list.Add(student);
        i = tStudentInfoDal.Modify(list, "update", null, null);//修改字段
      }

      return i;
    }

    public static int SaveTeacherValidInfo(string strUserName, TeacherValidModel model)
    {

      tTeachValidEntity entity = new tTeachValidEntity();
      List<tTeachValidEntity> list = new List<tTeachValidEntity>();
      entity.fUserName = strUserName;
      entity.fTeachCert1 = model.fTeachCert1;
      entity.fTeachCert2 = model.fTeachCert2;
      entity.fIDCard1 = model.fIDCard1;
      entity.fIDCard2 = model.fIDCard2;
      entity.fCertType = model.fCertType;
      entity.fPharse = model.fPharse;
      entity.fSubject = model.fSubject;
      entity.fCreateDate = DateTime.Now;
      entity.fCreateOpr = strUserName;
      entity.fUploadDate = DateTime.Now;
      entity.fStatus = "提交";
      list.Add(entity);
      int i = tTeachValidDal.Modify(list, "insert", null, null);

      //tTeachInfoEntity teacher = tTeachInfoDal.GettTeachInfoByUserName(strUserName);
      //teacher.fStatus = "已提交认证";
      //List<tTeachInfoEntity> infolist = new List<tTeachInfoEntity>();
      //infolist.Add(teacher);
      //tTeachInfoDal.Modify(infolist, "update", "fID,fStatus", null);
      return i;
    }

    public static string UserPay(string strUserName, string strPayType, string strBookingNo, decimal iAmount, string strSystem)
    {
      Random rd = new Random();
      tUserPayEntity pay = new tUserPayEntity();
      pay.fAmount = iAmount;
      pay.fCreateDate = DateTime.Now;
      pay.fCreateOpr = strUserName;
      if (strBookingNo == null)
      {
        pay.fNote = "充值";
        pay.fRemark = "充值";
      }
      else
      {
        pay.fNote = "购买课程";
        pay.fRemark = strBookingNo;
      }
      pay.fOrderNo = DateTime.Now.ToString("yyyyMMddHHmmssssss") + rd.Next(10, 100).ToString();
      pay.fSystem = strSystem;
      pay.fUserName = strUserName;
      pay.fPayType = strPayType;

      List<tUserPayEntity> list = new List<tUserPayEntity>();
      list.Add(pay);
      int i = tUserPayDal.Modify(list, "insert", null, null);
      return pay.fOrderNo;
    }

    public static int UserPaySuccess(string strPayType, string strPayNo, string strTradeNo)
    {
      int i = tUserPayDal.UserPaySuccess(strPayType, strPayNo, strTradeNo);
      return i;
    }

    public static Decimal GetUserAccountAmount(string strUserName)
    {
      Decimal AccountAmount = tUserPayDal.GetUserAccountAmount(strUserName);
      return AccountAmount;
    }

    public static UserAccountListModel GetUserAccountData(string strUserName)
    {
      UserAccountListModel model = new UserAccountListModel();
      DataTable dt = tUserAccountDal.GettUserAccountList(strUserName);

      List<UserAccountModel> lstRst = PubFun.DataTableToObjects<UserAccountModel>(dt);
      model.accountList = lstRst;
      return model;
    }

  }
}
