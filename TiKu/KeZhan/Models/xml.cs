using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeZhan.Models
{
    public class xml
    {
        public string return_code { get; set; }
        public string result_code { get; set; }
        public string return_msg { get; set; }
        public string err_code { get; set; }
        public string err_code_des { get; set; }

        public string refund_channel { get; set; }
    }
}