using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using Trip8H.Common;
using TuYou.DBUtility;

namespace TiKuDal
{
    public class ConfigDal
    {
        public static string GetToken(string strAppID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT top 1 fToken FROM hotelgroup..tweixintoken where fAppID=@AppID ");
            List<DbParameter> lstParam = new List<DbParameter>();
            lstParam.Add(new DBParam("@AppID", strAppID));
            object dt = DBHelper.ExecuteScalar("TiKu", sb.ToString(), lstParam);
            if (dt == null)
            {
                return "";
            }
            else
            {
                return dt.ToString();
            }
        }
    }
}
