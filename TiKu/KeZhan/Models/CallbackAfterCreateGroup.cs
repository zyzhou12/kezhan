using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeZhan.Models
{
    public class CallbackAfterCreateGroup
    {
        public string CallbackCommand { get; set; }
        public string GroupId { get; set; }
        public string Operator_Account { get; set; }
        public string Owner_Account { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public List<TICMember> MemberList { get; set; }
    }
}