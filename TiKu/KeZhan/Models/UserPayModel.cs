using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeZhan.Models
{
    public class UserPayModel
    {
        public string UserName { get; set; }
        public string OpenID { get; set; }
        public decimal ClassFee { get; set; }

        public decimal LeftFlow { get; set; }
    }
}