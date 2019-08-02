using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeZhan.Models
{
    public class StateChange
    {
        public string CallbackCommand { get; set; }
        public StateChangeInfo Info { get; set; }
    }

    public class StateChangeInfo
    {
        public string Action { get; set; }
        public string To_Account { get; set; }
        public string Reason { get; set; }
    }
}