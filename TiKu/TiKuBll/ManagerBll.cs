using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiKuBll.Model;
using System.Data;
using TiKu.Dal;
using Trip8H.Common;
using TiKu.Entity;

namespace TiKuBll
{
  public class ManagerBll
  {

      public static ConfigModel GetSystemConfig(string strCity)
      {
          tSystemConfigEntity entity = tSystemConfigDal.GettSystemConfigByCity(strCity);
          ConfigModel model = new ConfigModel();
          model.fAccountMinAmount = entity.fAccountMinAmount;
          model.fCity = entity.fCity;
          model.fClassFee = entity.fClassFee;
          model.fCreateDate = entity.fCreateDate;
          model.fCreateOpr = entity.fCreateOpr;
          model.fID = entity.fID;
          model.fModifyDate = entity.fModifyDate;
          model.fModifyOpr = entity.fModifyOpr;
          model.fProblemFee = entity.fProblemFee;
          model.fSourceFee = entity.fSourceFee;
          model.fValidFee = entity.fValidFee;
          model.fVideoFee = entity.fVideoFee;
          return model;
      }

    public static ValidListModel TeacherValidQuery(string strUserName)
    {
      ValidListModel model = new ValidListModel();
      DataTable dt = tTeachValidDal.TeacherValidQuery(strUserName);
      List<ValidModel> validList = PubFun.DataTableToObjects<ValidModel>(dt);
      model.validList = validList;
      return model;
    }

    public static int TeacherValid(string strUserName, int iValidFid, int iValidDetailFid, bool ValidResult, string strName, string strIDType, string strUID, string strCertType, string strCertNo, string strEffect, string strPharse, string strSubject, string ValidMessage)
    {
        int i = tTeachValidDal.TeacherValid(strUserName, iValidFid, iValidDetailFid, ValidResult, strName,strIDType, strUID,strCertType, strCertNo, strEffect,strPharse,strSubject, ValidMessage);
      return i;
    }

    public static ClassRoomApplyListModel ClassRoomApplyQuery()
    {
        ClassRoomApplyListModel model = new ClassRoomApplyListModel();
        DataTable dt = tClassRoomApplyDal.GettClassRoomApplyList();
        List<ClassRoomApplyModel> applyList = PubFun.DataTableToObjects<ClassRoomApplyModel>(dt);
        model.list = applyList;
        return model;
    }

      public static ClassRoomApplyModel GetClassRoomApply(int fid)
    {
        tClassRoomApplyEntity entity = tClassRoomApplyDal.GettClassRoomApply(fid);
        ClassRoomApplyModel model = new ClassRoomApplyModel();
        model.fID = entity.fID;
        model.fApplyDate = entity.fApplyDate;
        model.fApplyNote = entity.fApplyNote;
        model.fApplyOpr = entity.fApplyOpr;
        model.fClassRoomCode = entity.fClassRoomCode;
        model.fStatus = entity.fStatus;
        model.fSubmitDate = entity.fSubmitDate;
        model.fSubmitOpr = entity.fSubmitOpr;
        return model;
    }

    public static int ClassRoomApplyAgree(int iApplyID, string strApplyNote, string strApplyOpr, DateTime applyDate, ref string strMsg)
    {
        int i = tClassRoomApplyDal.ClassRoomApplyAgree(iApplyID, strApplyNote, strApplyOpr, applyDate, ref strMsg);
        return i;
    }

    public static int ClassRoomApplyRefuse(int iApplyID, string strApplyNote, string strApplyOpr, DateTime applyDate, ref string strMsg)
    {
        int i = tClassRoomApplyDal.ClassRoomApplyRefuse(iApplyID, strApplyNote, strApplyOpr, applyDate, ref strMsg);
        return i;
    }
  }
}
