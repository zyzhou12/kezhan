using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TiKuBll.Model
{
   public class UserBankAccountModel
   {
       public System.Int32 fID { get; set; }
       public System.String fUserName { get; set; }

       public System.String fName { get; set; }
       public System.String fAccountName { get; set; }
       public System.String fOpenBank { get; set; }
       public System.String fAccountNo { get; set; }
       public System.DateTime fCreateDate { get; set; }
       public System.String fCreateOpr { get; set; }
    }

   public class UserBankAccountListModel
   {
       public List<UserBankAccountModel> accountList { get; set; }
   }
}
