using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeZhan.Models
{
    public class TICUserResponseModel : TICBaseModel
    {

        public List<TICUser> user_list { get; set; }
        public List<string> repeats { get; set; }
    }

    public class TICUser
    {
        public string user_id { get; set; }
        public string user_token { get; set; }
    }

    public class TICUserToken : TICBaseModel
    {
        public string user_token { get; set; }
    }
}