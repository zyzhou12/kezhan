using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TiKu.Entity;

namespace KeZhanManager.Models
{
    public class UserFlowListModel
    {
        public int iCount { get; set; }
        public List<tFlowStoredEntity> flowList { get; set; }
    }
}