using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeZhan.Models
{
    public class TICClassResponseModel : TICBaseModel
    {
        public string class_id { get; set; }
        public string teacher_url { get; set; }
        public string student_url { get; set; }
    }
}