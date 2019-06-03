using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TiKu.Entity;

namespace KeZhanManager.Models
{
    public class WithDrawalListModel
    {
        public int iCount { get; set; }
        public List<tTeacherWithdrawalEntity> withDrawalList { get; set; }
    }
}