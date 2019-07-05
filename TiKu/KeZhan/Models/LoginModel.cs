using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeZhan.Models
{
  public class LoginModel
  {
      public string openid { get; set; }
    public string redirect_uri { get; set; }
  }
}