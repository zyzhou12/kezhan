using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeZhanManager.Models
{
    public class UserModel
    {
        public System.Int32 fID { get; set; }
        public System.String fUserName { get; set; }
        public System.String fMobile { get; set; }
        public System.String fOpenID { get; set; }
        public System.String fEmail { get; set; }
        public System.String fPassWord { get; set; }
        public System.String fTradePassWord { get; set; }
        public System.String fCode { get; set; }
        public System.DateTime fCodeEffectDate { get; set; }
        public System.String fEmailCode { get; set; }
        public System.DateTime fEmailCodeEffectDate { get; set; }
        public System.String fNickName { get; set; }
        public System.String fHeadImg { get; set; }
        public System.String fWeiXinUnionID { get; set; }
        public System.String fName { get; set; }
        public System.String fUID { get; set; }
        public System.String fCity { get; set; }
        public System.String fRole { get; set; }
        public System.String fRegSystem { get; set; }
        public System.String fStatus { get; set; }
        public System.DateTime fCreateDate { get; set; }
        public System.String fCreateOpr { get; set; }
        public System.DateTime fModifyDate { get; set; }
        public System.String fModifyOpr { get; set; }


        public System.Decimal fAmount { get; set; }
        public System.Int32 fFlow { get; set; }
    }
}