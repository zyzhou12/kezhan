using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TiKu.Api.Models
{
    public class ResponseCourseModel:ResponseBase
    {
        public int CourseID { get; set; }
        public string CourseNo { get; set; }
        public string ClassTitle { get; set; }
        public string ClassDate { get; set; }
        public string ClassLength { get; set; }
        public string Price { get; set; }
    }
}