using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeZhan.Models
{
    public class PublicTokenModel
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public string openid { get; set; }
        public string scope { get; set; }
        public string expires_in { get; set; }
    }

    public class PublicUserModel
    {
        public System.Int32 subscribe { get; set; }
        public System.String openid { get; set; }
        public System.String nickname { get; set; }
        public System.Int32 sex { get; set; }
        public System.String province { get; set; }
        public System.String city { get; set; }
        public System.String country { get; set; }
        public System.String headimgurl { get; set; }
        public System.String unionid { get; set; }
    }
}