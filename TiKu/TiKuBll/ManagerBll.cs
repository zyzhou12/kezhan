using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiKuBll.Model;
using System.Data;
using TiKu.Dal;
using Trip8H.Common;

namespace TiKuBll
{
  public class ManagerBll
  {
    public static ValidListModel TeacherValidQuery(string strUserName)
    {
      ValidListModel model = new ValidListModel();
      DataTable dt = tTeachValidDal.TeacherValidQuery(strUserName);
      List<ValidModel> validList = PubFun.DataTableToObjects<ValidModel>(dt);
      model.validList = validList;
      return model;
    }

    public static int TeacherValid(string strUserName, int iValidFid, bool ValidResult,string strName,string strUID,string strCertNo,string strEffect, string ValidMessage)
    {
      int i = tTeachValidDal.TeacherValid(strUserName, iValidFid, ValidResult,strName,strUID,strCertNo,strEffect, ValidMessage);
      return i;
    }
  }
}
