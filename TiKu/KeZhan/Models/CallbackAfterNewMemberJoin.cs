﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeZhan.Models
{
    public class CallbackAfterNewMemberJoin
    {
        public string CallbackCommand { get; set; }
        public string GroupId { get; set; }
        public string Type { get; set; }
        public string JoinType { get; set; }
        public string Operator_Account { get; set; }
        public List<TICMember> NewMemberList { get; set; }
    }
}