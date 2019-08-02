using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TiKuBll.Model
{
  public class UserInfoModel
  {
    public System.String fUserName { get; set; }
    public System.String fMobile { get; set; }
    public System.String fOpenID { get; set; }
    public System.String fUserToken { get; set; }

    public System.String fCode { get; set; }
    public System.String fEmailCode { get; set; }
    public System.DateTime fEmailCodeEffectDate { get; set; }
    public System.String IsPassWord { get; set; }
    public System.String fEmail { get; set; }
    public System.String fNickName { get; set; }
    public System.String fHeadImg { get; set; }
    public System.String fName { get; set; }
    public System.String fUID { get; set; }

    public System.String fWeiXinUnionID { get; set; }
    public System.String fCity { get; set; }
    public System.String fRole { get; set; }
    public System.String fRegSystem { get; set; }
    public System.String fStatus { get; set; }

    public System.String NowRole { get; set; }
  }

    public class UserListModel
    {
        public List<UserInfoModel> userList { get; set; }
    }
}
