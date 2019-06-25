using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TiKu.Entity;

namespace KeZhanManager.Models
{
    public class WithdrawalModel
    {
        public tTeacherWithdrawalEntity withDrawal { get; set; }
        public tUserBankAccountEntity account { get; set; }
    }
}