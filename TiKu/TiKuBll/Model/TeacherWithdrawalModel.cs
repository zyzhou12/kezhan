using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TiKuBll.Model
{
    public class TeacherWithdrawalModel
    {
        public System.Int32 fID { get; set; }
        public System.String fUserName { get; set; }
        public System.Decimal fAmount { get; set; }
        public System.Int32 fBankAccountId { get; set; }
        public System.DateTime fSubmitDate { get; set; }
        public System.String fApproveOpr { get; set; }
        public System.DateTime fApproveDate { get; set; }
        public System.Boolean fApproveResult { get; set; }
        public System.String fApproveNote { get; set; }
        public System.String fTransCerd { get; set; }
        public System.String fStatus { get; set; }
        public System.DateTime fCreateDate { get; set; }
        public System.String fCreateOpr { get; set; }
        public System.DateTime fModifyDate { get; set; }
        public System.String fModifyOpr { get; set; }

        public UserBankAccountModel bankAccount { get; set; }
    }

    public class TeacherWithdrawalListModel
    {
        public List<TeacherWithdrawalModel> withDrawalList { get; set; }
    }
}
