using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TiKu.Api.Models
{
  public class UserResponseModel:ResponseBase
  {
    public System.Int32 UserID { get; set; }
    public System.String UserName { get; set; }
    public System.String Role { get; set; }
    public System.String OpenID { get; set; }
    public System.String SessionKey { get; set; }
    public System.String UnionID { get; set; }
    public System.String NickName { get; set; }
    public System.String Mobile { get; set; }
    public System.String Gender { get; set; }

  }

  public class UserLoginModel
  {
      public string openid { get; set; }
      public string session_key { get; set; }
      public string unionid { get; set; }    
  }
}