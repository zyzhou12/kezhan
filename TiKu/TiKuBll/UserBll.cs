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

        public static int UserSendEmailCode(string strUserName,string strEmail, ref string strMsg)
        {
            string strCode = new Random().Next(100000, 999999).ToString();
            return tUserDal.UserSendEmailCode(strUserName,strEmail, strCode, ref strMsg);
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
                model.fOpenID = user.fOpenID;
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
                model.fOpenID = user.fOpenID;
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

        public static UserInfoModel UserWeiChatLogin(string strOpenID,string strUserName, ref string strMsg)
        {
            UserInfoModel model = null;
            tUserEntity user = tUserDal.UserWeiChatLogin(strOpenID, strUserName,  ref strMsg);
            if (user != null)
            {
                model = new UserInfoModel();
                model.fCity = user.fCity;
                model.fEmail = user.fEmail;
                model.fHeadImg = user.fHeadImg;
                model.fMobile = user.fMobile;
                model.fOpenID = user.fOpenID;
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

        public static UserInfoModel GetUserInfo(string strUserName)
        {
            UserInfoModel model = new UserInfoModel();
            tUserEntity user = tUserDal.GettUser(strUserName);
            model.fCity = user.fCity;
            model.fEmail = user.fEmail;
            model.fHeadImg = user.fHeadImg;
            model.fMobile = user.fMobile;
            model.fOpenID = user.fOpenID;
            model.fCode = user.fCode;
            model.fEmailCode = user.fEmailCode;
            model.fEmailCodeEffectDate = user.fEmailCodeEffectDate;
            model.fName = user.fName;
            model.fNickName = user.fNickName;
            model.fRegSystem = user.fRegSystem;
            model.fRole = user.fRole;
            model.fStatus = user.fStatus;
            model.fUID = user.fUID;
            model.fUserName = user.fUserName;
            return model;
        }

        public static UserInfoModel GetUserInfoByMobile(string strMobile)
        {
            UserInfoModel model = new UserInfoModel();
            tUserEntity user = tUserDal.GettUserByMobile(strMobile);
            model.fCity = user.fCity;
            model.fEmail = user.fEmail;
            model.fHeadImg = user.fHeadImg;
            model.fMobile = user.fMobile;
            model.fOpenID = user.fOpenID;
            model.fCode = user.fCode;
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

        public static ValidModel GetTeacherValid(int iValidID, int detialID)
        {
            tTeachValidEntity valid = tTeachValidDal.GettTeachValid(iValidID);
            tTeacherValidDetailEntity detail = tTeacherValidDetailDal.GettTeacherValidDetail(detialID);
            ValidModel model = new ValidModel();
            if (valid != null)
            {
                model.fIDCard1 = valid.fIDCard1;
                model.fIDCard2 = valid.fIDCard2;
                model.fName = valid.fName;
                model.fIDType = valid.fIDType;
                model.fUID = valid.fUID;
                model.fUserName = valid.fUserName;
                model.fTeacherValidID = valid.fID;

                model.fID = detail.fID;
                model.fCertNo = detail.fCertNo;
                model.fCertType = detail.fCertType;
                model.fNote = detail.fNote;
                model.fPharse = detail.fPharse;
                model.fSubject = detail.fSubject;
                model.fTeachCert1 = detail.fTeachCert1;
                model.fTeachCert2 = detail.fTeachCert2;
                model.fUploadDate = detail.fUploadDate;
                model.fValidDate = detail.fValidDate;
                model.fValidUser = detail.fValidUser;

                model.fStatus = valid.fStatus;
            }
            return model;
        }

        public static TeacherValidListModel GettTeachValidList(string strUserName, string strStatus = null)
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
                valid.fName = entity.fName;
                valid.fIDType = entity.fIDType;
                valid.fUID = entity.fUID;
                valid.fStatus = entity.fStatus;
                valid.fCreateDate = entity.fCreateDate;
                validList.Add(valid);
            }
            model.validList = validList;
            return model;
        }


        public static ValidDetailListModel GetTeachValidDetailList(string strUserName, string strStatus = null)
        {
            List<tTeacherValidDetailEntity> entityList = tTeacherValidDetailDal.GettTeacherValidDetailList(strUserName, strStatus);
            ValidDetailListModel model = new ValidDetailListModel();
            List<TeacherValidDetailModel> validList = new List<TeacherValidDetailModel>();
            foreach (var entity in entityList)
            {
                TeacherValidDetailModel valid = new TeacherValidDetailModel();
                valid.fID = entity.fID;
                valid.fCertType = entity.fCertType;
                valid.fNote = entity.fNote;
                valid.fPharse = entity.fPharse;
                valid.fStatus = entity.fStatus;
                valid.fSubject = entity.fSubject;
                valid.fTeachCert1 = entity.fTeachCert1;
                valid.fTeachCert2 = entity.fTeachCert2;
                valid.fTeacherValidID = entity.fTeacherValidID;
                valid.fUploadDate = entity.fUploadDate;
                valid.fValidDate = entity.fValidDate;

                validList.Add(valid);
            }
            model.detailList = validList;
            return model;
        }

        public static ValidDetailListModel GetTeachValidDetailListByID(int iValidID)
        {
            List<tTeacherValidDetailEntity> entityList = tTeacherValidDetailDal.GettTeacherValidDetailList(iValidID);
            ValidDetailListModel model = new ValidDetailListModel();
            List<TeacherValidDetailModel> validList = new List<TeacherValidDetailModel>();
            foreach (var entity in entityList)
            {
                TeacherValidDetailModel valid = new TeacherValidDetailModel();
                valid.fID = entity.fID;
                valid.fCertType = entity.fCertType;
                valid.fNote = entity.fNote;
                valid.fPharse = entity.fPharse;
                valid.fStatus = entity.fStatus;
                valid.fSubject = entity.fSubject;
                valid.fTeachCert1 = entity.fTeachCert1;
                valid.fTeachCert2 = entity.fTeachCert2;
                valid.fTeacherValidID = entity.fTeacherValidID;
                valid.fUploadDate = entity.fUploadDate;
                valid.fValidDate = entity.fValidDate;
                valid.fValidUser = entity.fValidUser;
                validList.Add(valid);
            }
            model.detailList = validList;
            return model;
        }


        public static TeacherValidDetailModel GetTeachValidDetail(int iDetailID)
        {
            tTeacherValidDetailEntity entity = tTeacherValidDetailDal.GettTeacherValidDetail(iDetailID);

            TeacherValidDetailModel valid = new TeacherValidDetailModel();
            valid.fID = entity.fID;
            valid.fCertType = entity.fCertType;
            valid.fNote = entity.fNote;
            valid.fPharse = entity.fPharse;
            valid.fStatus = entity.fStatus;
            valid.fSubject = entity.fSubject;
            valid.fTeachCert1 = entity.fTeachCert1;
            valid.fTeachCert2 = entity.fTeachCert2;
            valid.fTeacherValidID = entity.fTeacherValidID;
            valid.fUploadDate = entity.fUploadDate;
            valid.fValidDate = entity.fValidDate;

            return valid;
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
                //else if (string.IsNullOrEmpty(teacher.fDesc))
                //{
                //    IsCheck = false;
                //}
                else if (teacher.fStatus != "已认证")
                {
                    IsCheck = false;
                }
            }
            //else if (strRole == "Student")
            //{
            //    tStudentInfoEntity student = tStudentInfoDal.GettStudentInfoByUserName(strUserName);
            //    if (student == null)
            //    {
            //        IsCheck = false;
            //    }
            //    else if (string.IsNullOrEmpty(student.fSchool))
            //    {
            //        IsCheck = false;
            //    }
            //}
            return IsCheck;
        }

        public static int SaveUserInfo(string strUserName, string InfoType, string strValue)
        {
            tUserEntity user = tUserDal.GettUser(strUserName);
            if (InfoType == "fMobile")
            {
                user.fMobile = strValue;
            }
            else if (InfoType == "fEmail")
            {
                user.fEmail = strValue;
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
            entity.fIDCard1 = model.fIDCard1;
            entity.fIDCard2 = model.fIDCard2;
            entity.fName = model.fName;
            entity.fIDType = model.fIDType;
            entity.fUID = model.fUID;
            entity.fCreateDate = DateTime.Now;
            entity.fCreateOpr = strUserName;
            entity.fStatus = "提交";
            list.Add(entity);
            int i = tTeachValidDal.Modify(list, "insert", null, null);

            List<tTeacherValidDetailEntity> detailList = new List<tTeacherValidDetailEntity>();
            foreach (TeacherValidDetailModel d in model.detailList)
            {
                if (!string.IsNullOrEmpty(d.fTeachCert1))
                {
                    tTeacherValidDetailEntity detail = new tTeacherValidDetailEntity();
                    detail.fTeacherValidID = i;
                    detail.fTeachCert1 = d.fTeachCert1;
                    detail.fStatus = "提交";
                    detail.fUploadDate = DateTime.Now;
                    detail.fCreateDate = DateTime.Now;
                    detail.fCreateOpr = strUserName;
                    detailList.Add(detail);
                }
            }
            int j = tTeacherValidDetailDal.Modify(detailList, "insert", null, null);

            //tTeachInfoEntity teacher = tTeachInfoDal.GettTeachInfoByUserName(strUserName);
            //teacher.fStatus = "已提交认证";
            //List<tTeachInfoEntity> infolist = new List<tTeachInfoEntity>();
            //infolist.Add(teacher);
            //tTeachInfoDal.Modify(infolist, "update", "fID,fStatus", null);
            return i;
        }

        /// <summary>
        /// 购买流量
        /// </summary>
        /// <param name="strUserName"></param>
        /// <param name="iNum"></param>
        /// <param name="dPrice"></param>
        /// <param name="strSource"></param>
        /// <param name="effectDate"></param>
        /// <param name="strOpr"></param>
        /// <returns></returns>
        public static string SubmitBuyFlowOrder(string strUserName,int iNum,decimal dPrice,string strSource,DateTime effectDate,string strOpr)
        {
            Random rd = new Random();
            tFlowStoredEntity entity = new tFlowStoredEntity();
            entity.fUserName = strUserName;
            entity.fNum = iNum;
            entity.fPrice = dPrice;
            entity.fSource = strSource;
            entity.fStoredNo = "F"+DateTime.Now.ToString("yyyyMMddHHmmssssss") + rd.Next(10, 100).ToString(); 
            entity.fStatus = 0;
            entity.fEffectDate = effectDate.ToShortDateString();
            entity.fCreateDate = DateTime.Now;
            entity.fCreateOpr = strOpr;
            List<tFlowStoredEntity> list = new List<tFlowStoredEntity>();
            list.Add(entity);
            int i=tFlowStoredDal.Modify(list, "insert", null, null);
            if (i >= 0)
            {
                return entity.fStoredNo;
            }
            else
            {
                return "";
            }
        }

        public static int GetFlowStoredStatus(string strStoredNo)
        {
            tFlowStoredEntity entity = tFlowStoredDal.GettFlowStored(strStoredNo);
            return entity.fStatus;
        }


        public static string UserPay(string strUserName, string strPayType,string strType, string strBookingNo, decimal iAmount, string strSystem)
        {
            Random rd = new Random();
            tUserPayEntity pay = new tUserPayEntity();
            pay.fAmount = iAmount;
            pay.fCreateDate = DateTime.Now;
            pay.fCreateOpr = strUserName;
            pay.fOrderNo = DateTime.Now.ToString("yyyyMMddHHmmssssss") + rd.Next(10, 100).ToString();            
            pay.fRemark = strBookingNo;            
            pay.fType = strType;
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


        public static int UserAmountRefund(int iRefundID, string strBookingNo, decimal refundAmount, string strNote, string strApplyOpr, ref string strMsg)
        {
            return tUserRefundDal.UserAmountRefund(iRefundID, strBookingNo, refundAmount, strNote, strApplyOpr, ref strMsg);
        }

        public static UserBankAccountListModel GetUserBankAccountList(string strUserName)
        {
            List<tUserBankAccountEntity> list = tUserBankAccountDal.GettUserBankAccountList(strUserName);
            UserBankAccountListModel model = new UserBankAccountListModel();
            List<UserBankAccountModel> accountList = new List<UserBankAccountModel>();
            foreach (tUserBankAccountEntity entity in list)
            {
                UserBankAccountModel account = new UserBankAccountModel();
                account.fName = entity.fName;
                account.fAccountName = entity.fAccountName;
                account.fAccountNo = entity.fAccountNo;
                account.fCreateDate = entity.fCreateDate;
                account.fCreateOpr = entity.fCreateOpr;
                account.fID = entity.fID;
                account.fOpenBank = entity.fOpenBank;
                account.fUserName = entity.fUserName;
                accountList.Add(account);
            }
            model.accountList = accountList;
            return model;
        }

        public static UserBankAccountModel GetUserBankAccount(int id)
        {
            tUserBankAccountEntity entity = tUserBankAccountDal.GettUserBankAccount(id);
            UserBankAccountModel account = new UserBankAccountModel();
            account.fName = entity.fName;
            account.fAccountName = entity.fAccountName;
            account.fAccountNo = entity.fAccountNo;
            account.fCreateDate = entity.fCreateDate;
            account.fCreateOpr = entity.fCreateOpr;
            account.fID = entity.fID;
            account.fOpenBank = entity.fOpenBank;
            account.fUserName = entity.fUserName;

            return account;
        }

        public static int SaveUserBankAccount(UserBankAccountModel model)
        {
            tUserBankAccountEntity entity = new tUserBankAccountEntity();
            entity.fName = model.fName;
            entity.fAccountName = model.fAccountName;
            entity.fAccountNo = model.fAccountNo;
            entity.fCreateDate = model.fCreateDate;
            entity.fCreateOpr = model.fCreateOpr;
            entity.fID = model.fID;
            entity.fOpenBank = model.fOpenBank;
            entity.fUserName = model.fUserName;
            List<tUserBankAccountEntity> list = new List<tUserBankAccountEntity>();
            list.Add(entity);
            int i = 0;
            if (entity.fID > 0)
            {
                i = tUserBankAccountDal.Modify(list, "update", null, null);
            }
            else
            {
                i = tUserBankAccountDal.Modify(list, "insert", null, null);
            }
            return i;
        }

        public static Decimal GetUserAccountAmount(string strUserName)
        {
            Decimal AccountAmount = tFlowStoredDal.GetUserFlowAccount(strUserName);
            return AccountAmount;
        }



        public static UserAccountListModel GetUserAccountData(string strUserName, string strTradingType, string strSystem, string strType, DateTime beginDate, DateTime endDate)
        {
            UserAccountListModel model = new UserAccountListModel();
            DataTable dt = tUserAccountDal.GettUserAccountList(strUserName, strTradingType, strSystem, strType, beginDate, endDate);

            List<UserAccountModel> lstRst = PubFun.DataTableToObjects<UserAccountModel>(dt);
            model.accountList = lstRst;
            model.LeftAmount = tUserPayDal.GetUserAccountAmount(strUserName);
            model.LeftFlow = tFlowStoredDal.GetUserFlowAccount(strUserName);
            return model;
        }


        /// <summary>
        /// 获取课程最大可能费用（时长，人数，价格）
        /// </summary>
        /// <param name="strClassRoomCode"></param>
        /// <returns></returns>
        //public static Decimal GetClassRoomFlootPrice(string strClassRoomCode)
        //{
        //    Decimal flootPrice = tClassRoomDal.GetClassRoomFlootPrice(strClassRoomCode);
        //    return flootPrice;
        //}


        public static TeacherWithdrawalListModel GetTeacherWithdrawalList(string strStatus, string strUserName)
        {
            List<tTeacherWithdrawalEntity> list = tTeacherWithdrawalDal.GettTeacherWithdrawalList(strStatus, strUserName);
            TeacherWithdrawalListModel model = new TeacherWithdrawalListModel();
            List<TeacherWithdrawalModel> withDrawalList = new List<TeacherWithdrawalModel>();
            foreach (tTeacherWithdrawalEntity entity in list)
            {
                TeacherWithdrawalModel w = new TeacherWithdrawalModel();
                w.fAmount = entity.fAmount;
                w.fApproveDate = entity.fApproveDate;
                w.fApproveNote = entity.fApproveNote;
                w.fApproveOpr = entity.fApproveOpr;
                w.fApproveResult = entity.fApproveResult;
                w.fBankAccountId = entity.fBankAccountId;
                w.fCreateDate = entity.fCreateDate;
                w.fCreateOpr = entity.fCreateOpr;
                w.fID = entity.fID;
                w.fModifyDate = entity.fModifyDate;
                w.fModifyOpr = entity.fModifyOpr;
                w.fSubmitDate = entity.fSubmitDate;
                w.fStatus = entity.fStatus;
                w.fTransCerd = entity.fTransCerd;
                w.fUserName = entity.fUserName;

                withDrawalList.Add(w);
            }
            model.withDrawalList = withDrawalList;
            return model;
        }

        public static TeacherWithdrawalModel GetTeacherWithdrawal(int id)
        {
            tTeacherWithdrawalEntity entity = tTeacherWithdrawalDal.GettTeacherWithdrawal(id);
            TeacherWithdrawalModel model = new TeacherWithdrawalModel();
            model.fAmount = entity.fAmount;
            model.fApproveDate = entity.fApproveDate;
            model.fApproveNote = entity.fApproveNote;
            model.fApproveOpr = entity.fApproveOpr;
            model.fApproveResult = entity.fApproveResult;
            model.fBankAccountId = entity.fBankAccountId;
            model.fCreateDate = entity.fCreateDate;
            model.fCreateOpr = entity.fCreateOpr;
            model.fID = entity.fID;
            model.fModifyDate = entity.fModifyDate;
            model.fModifyOpr = entity.fModifyOpr;
            model.fSubmitDate = entity.fSubmitDate;
            model.fStatus = entity.fStatus;
            model.fTransCerd = entity.fTransCerd;
            model.fUserName = entity.fUserName;

            model.bankAccount = GetUserBankAccount(model.fBankAccountId);
            return model;
        }

        public static int SaveTeacherWithdrawal(TeacherWithdrawalModel model)
        {
            tTeacherWithdrawalEntity entity = new tTeacherWithdrawalEntity();
            entity.fAmount = model.fAmount;
            entity.fApproveDate = model.fApproveDate;
            entity.fApproveNote = model.fApproveNote;
            entity.fApproveOpr = model.fApproveOpr;
            entity.fApproveResult = model.fApproveResult;
            entity.fBankAccountId = model.fBankAccountId;
            entity.fCreateDate = model.fCreateDate;
            entity.fCreateOpr = model.fCreateOpr;
            entity.fID = model.fID;
            entity.fModifyDate = model.fModifyDate;
            entity.fModifyOpr = model.fModifyOpr;
            entity.fSubmitDate = model.fSubmitDate;
            entity.fStatus = model.fStatus;
            entity.fTransCerd = model.fTransCerd;
            entity.fUserName = model.fUserName;
            List<tTeacherWithdrawalEntity> list = new List<tTeacherWithdrawalEntity>();
            list.Add(entity);
            int i = 0;
            if (entity.fID > 0)
            {
                i = tTeacherWithdrawalDal.Modify(list, "update", null, null);
            }
            else
            {
                i = tTeacherWithdrawalDal.Modify(list, "insert", null, null);
            }
            return i;
        }

        public static int CheckWithdrawal(int iAmount, string UserName, ref string strMsg)
        {
            return tTeacherWithdrawalDal.CheckWithdrawal(iAmount, UserName, ref strMsg);
        }


        public static int TeacherWithdrawalAgree(int iWithID, string strApplyNote, string strApplyOpr, DateTime applyDate, string strTransCerd, ref string strMsg)
        {
            return tTeacherWithdrawalDal.TeacherWithdrawalAgree(iWithID, strApplyNote, strApplyOpr, applyDate, strTransCerd, ref strMsg);
        }

        public static int TeacherWithdrawalRefuse(int iWithID, string strApplyNote, string strApplyOpr, DateTime applyDate, ref string strMsg)
        {
            return tTeacherWithdrawalDal.TeacherWithdrawalRefuse(iWithID, strApplyNote, strApplyOpr, applyDate,  ref strMsg);
        }


        /// <summary>
        /// 获取消息
        /// </summary>
        /// <param name="strUser"></param>
        /// <param name="strStatus"></param>
        /// <returns></returns>
        public static MessageListModel GetMessageList(string strUser, string strStatus)
        {
            List<tMessageEntity> list = tMessageDal.GettMessageList(strUser, strStatus);
            MessageListModel model = new MessageListModel();
            List<MessageModel> messageList = new List<MessageModel>();
            foreach (tMessageEntity entity in list)
            {
                MessageModel message = new MessageModel();
                message.fContent = entity.fContent;
                message.fCreateDate = entity.fCreateDate;
                message.fCreateOpr = entity.fCreateOpr;
                message.fFromUser = entity.fFromUser;
                message.fID = entity.fID;
                message.fModifyDate = entity.fModifyDate;
                message.fModifyOpr = entity.fModifyOpr;
                message.fSendDate = entity.fSendDate;
                message.fStatus = entity.fStatus;
                message.fTitle = entity.fTitle;
                message.fToUser = entity.fToUser;
                message.fType = entity.fType;
                message.fTypeID = entity.fTypeID;
                messageList.Add(message);
            }
            model.messageList = messageList;

            return model;
        }

        public static int MessageUpdate(int iMessageID)
        {
            tMessageEntity message = new tMessageEntity();
            message.fID = iMessageID;
            message.fStatus = 1;
            List<tMessageEntity> list = new List<tMessageEntity>();
            list.Add(message);
            int i = tMessageDal.Modify(list, "update", "fID,fStatus", null);
            return i;
        }

        public static UserMemberModel GetUserMember(string strUserName, string strMobile)
        {
            tUserMemberEntity entity = tUserMemberDal.GetUserMember(strUserName, strMobile);
            UserMemberModel model = null;
            if (entity != null)
            {
                model = new UserMemberModel();
                model.fCreateDateTime = entity.fCreateDateTime;
                model.fID = entity.fID;
                model.fMemberUserName = entity.fMemberUserName;
                model.fNote = entity.fNote;
                model.fStatus = entity.fStatus;
                model.fUserName = entity.fStatus;
            }
            return model;


        }

        /// <summary>
        /// 获取授课老师集合
        /// </summary>
        /// <param name="strUser"></param>
        /// <param name="strStatus"></param>
        /// <returns></returns>
        public static UserMemberListModel GetUserMemberList(string strUser, string strStatus, string strPharse, string strSubject)
        {
            UserMemberListModel model = new UserMemberListModel();
            DataTable dt = tUserMemberDal.GettUserMemberList(strUser, strStatus,strPharse,strSubject);
            List<UserMemberModel> lstRst = PubFun.DataTableToObjects<UserMemberModel>(dt);
            model.userMemberList = lstRst;
            return model;
        }

        public static UserMemberListModel GetTeacherList(string strUser, string strPharse, string strSubject)
        {
            UserMemberListModel model = new UserMemberListModel();
            DataTable dt = tUserMemberDal.GetTeacherList(strUser, strPharse, strSubject);
            List<UserMemberModel> lstRst = PubFun.DataTableToObjects<UserMemberModel>(dt);
            model.userMemberList = lstRst;
            return model;
        }

        public static UserMemberListModel GetMemberList(string strUser, string strStatus)
        {
            UserMemberListModel model = new UserMemberListModel();
            DataTable dt = tUserMemberDal.GetMemberList(strUser);
            List<UserMemberModel> lstRst = PubFun.DataTableToObjects<UserMemberModel>(dt);
            model.userMemberList = lstRst;
            return model;
        }


        public static int UserInviteMember(string UserName, string Mobile, string strNote, ref string strMsg)
        {
            int i = tUserMemberDal.UserInviteMember(UserName, Mobile, strNote, ref strMsg);
            return i;
        }

        public static int UserInviteAgree(string UserName, int UserMemberID)
        {
            int i = tUserMemberDal.InviteAgree(UserName, UserMemberID);
            return i;
        }
        public static int UserInviteRefused(string UserName, int UserMemberID)
        {
            int i = tUserMemberDal.InviteRefused(UserName, UserMemberID);
            return i;
        }

        public static int UserInviteCancel(string UserName, int UserMemberID)
        {
            int i = tUserMemberDal.InviteCancel(UserName, UserMemberID);
            return i;
        }
        public static int UserInviteRemove(string UserName, int UserMemberID)
        {
            int i = tUserMemberDal.InviteRemove(UserName, UserMemberID);
            return i;
        }

        public static int FlowAdjust(string strMobile, int iFlow, DateTime effectDate, string strUserName, string strNote, ref string strMsg)
        {
            int i = tFlowStoredDal.FlowAdjust(strMobile, iFlow, effectDate, strUserName, strNote, ref strMsg);
            return i;
        }

        public static int CheckFlowEffect()
        {
            return tFlowStoredDal.CheckFlowEffect();
        }

        public static int TeacherFocus(string strUserName, string strTeacherUser, bool IsFocus)
        {
            return tUserFocusDal.TeacherFocus(strUserName, strTeacherUser, IsFocus);
        }

        public static UserListModel GetFocusTeacherList(string strUserName)
        {
            List<tUserEntity> list = tUserDal.GetFocusTeacherList(strUserName);
            UserListModel model = new UserListModel();
            List<UserInfoModel> userList = new List<UserInfoModel>();
            foreach(tUserEntity user in list)
            {
                UserInfoModel info = new UserInfoModel();
                info.fUserName = user.fUserName;
                info.fHeadImg = user.fHeadImg;
                info.fNickName = user.fNickName;
                userList.Add(info);
            }
            model.userList = userList;
            return model;
        }
    }
}
