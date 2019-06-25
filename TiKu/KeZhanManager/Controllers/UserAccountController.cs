using KeZhanManager.Filters;
using KeZhanManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiKu.Dal;
using TiKu.Entity;

namespace KeZhanManager.Controllers
{
    [LoginActionFilter]
    public class UserAccountController : Controller
    {
        //
        // GET: /UserAccount/

        public ActionResult SettlementQuery()
        {
            return View();
        }

        public ActionResult GetSettlementList(string strBeginDate, string strEndDate, string strUserName)
        {

            List<tClassRoomAccountEntity> accountList = tClassRoomAccountDal.GettClassRoomAccountList(strBeginDate, strEndDate, strUserName);
            SettlementListModel model = new SettlementListModel();
            if (accountList != null)
            {
                model.iCount = accountList.Count;
                model.accountList = accountList;
            }
            return PartialView("SettlementList", model);
        }
        

        public ActionResult UserAmountQuery()
        {
            return View();
        }

        public ActionResult GetUserAmountList(string strBeginDate, string strEndDate, string strUserName)
        {

            List<tUserAmountEntity> amountList = tUserAmountDal.GettUserAmountList(strBeginDate, strEndDate, strUserName);
            UserAmountListModel model = new UserAmountListModel();
            if (amountList != null)
            {
                model.iCount = amountList.Count;
                model.amountList = amountList;
            }
            return PartialView("UserAmountList", model);
        }
        public ActionResult UserWithdrawalQuery()
        {
            return View();
        }

         public ActionResult GetWithdrawalList(string strBeginDate, string strEndDate, string strUserName)
        {

            List<tTeacherWithdrawalEntity> withDrawalList = tTeacherWithdrawalDal.GetWithdrawalList(strBeginDate, strEndDate, strUserName);
            WithDrawalListModel model = new WithDrawalListModel();
            if (withDrawalList != null)
            {
                model.iCount = withDrawalList.Count;
                model.withDrawalList = withDrawalList;
            }
            return PartialView("UserWithDrawalList", model);
        }
        public ActionResult UserFlowQuery()
        {
            return View();
        }
        public ActionResult GetUserFlowList(string strBeginDate, string strEndDate, string strUserName)
        {

            List<tFlowStoredEntity> flowList = tFlowStoredDal.GettFlowStoredList(strBeginDate, strEndDate, strUserName);
            UserFlowListModel model = new UserFlowListModel();
            if (flowList != null)
            {
                model.iCount = flowList.Count;
                model.flowList = flowList;
            }
            return PartialView("UserFlowList", model);
        }

        public ActionResult UserFlowAdjust()
        {
            return View();
        }

        public JsonResult DoFlowAdjust(string strMobile, int iFlow, DateTime effectDate, string strNote)
        {
            tUserEntity userInfo = Code.Fun.GetSessionUserInfo(this);
            ResponseBaseModel response = new ResponseBaseModel();
            string strMsg = "";

            response.iResult = tFlowStoredDal.FlowAdjust(strMobile, iFlow, effectDate, userInfo.fUserName, strNote, ref strMsg);
            response.strMsg = strMsg;
            JsonResult jr = new JsonResult();
            jr.Data = response;
            return jr;
        }

        public ActionResult WithDrawalApply(int iWithDrawalId)
        {
            WithdrawalModel model = new WithdrawalModel();
            model.withDrawal = tTeacherWithdrawalDal.GettTeacherWithdrawal(iWithDrawalId);
            model.account= tUserBankAccountDal.GettUserBankAccount(model.withDrawal.fBankAccountId);
            return View(model);
        }

        public JsonResult DoAgreeWithDrawal(int iWithID, string strNote, string TransCerd)
        {
            tUserEntity userInfo = Code.Fun.GetSessionUserInfo(this);
            ResponseBaseModel response = new ResponseBaseModel();
            string strMsg = "";
            response.iResult = tTeacherWithdrawalDal.TeacherWithdrawalAgree(iWithID, strNote, userInfo.fUserName, DateTime.Now,TransCerd, ref strMsg);
            response.strMsg = strMsg;
            JsonResult jr = new JsonResult();
            jr.Data = response;
            return jr;
        }

        public JsonResult DoRefuseWithDrawal(int iWithID, string strNote)
        {
            tUserEntity userInfo = Code.Fun.GetSessionUserInfo(this);
            ResponseBaseModel response = new ResponseBaseModel();
            string strMsg = null;


            response.iResult = tTeacherWithdrawalDal.TeacherWithdrawalRefuse(iWithID, strNote, userInfo.fUserName, DateTime.Now, ref strMsg);
            JsonResult jr = new JsonResult();
            jr.Data = response;
            return jr;
        }

    }
}
