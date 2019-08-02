using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeZhan.Models
{
    public class SaasClassOverModel
    {
        public string @event { get; set; }
        public ClassOverModel data { get; set; }
    }

    public class ClassBeginModel
    {
        public string class_id { get; set; }
        public Int64 real_start_time { get; set; }
    }

    public class ClassOverModel
    {
        public string class_id { get; set; }
        public Int64 real_stop_time { get; set; }
    }
}