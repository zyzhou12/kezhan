using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiKuDal;

namespace TiKuBll
{
    public class ConfigBll
    {
        public static string GetToken(string strAppID)
        {
            return ConfigDal.GetToken(strAppID);
        }
    }
}
