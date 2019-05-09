using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TiKuBll.Model
{
    public class UserRefundModel
    {
        public System.Int32 fID { get; set; }
        public System.String fOrderNo { get; set; }
        public System.String fBookingNo { get; set; }
        public System.String fPayType { get; set; }
        public System.String fUserName { get; set; }
        public System.Decimal fApplyAmount { get; set; }
        public System.String fApplyRemark { get; set; }
        public System.DateTime fApplyDate { get; set; }
        public System.Decimal fRefundAmount { get; set; }
        public System.String fRefundNote { get; set; }
        public System.DateTime fRefundDate { get; set; }
        public System.String fRefundUserName { get; set; }
        public System.Int32 fStatus { get; set; }
        public System.DateTime fCreateDate { get; set; }
        public System.String fCreateOpr { get; set; }
        public System.DateTime fModifyDate { get; set; }
        public System.String fModifyOpr { get; set; }

        public System.String fClassRoomTitle { get; set; }
        public System.String fNickName { get; set; }
    }

    public class UserRefundListModel
    {
        public string iStatus { get; set; }
        public List<UserRefundModel> refundList { get; set; }
    }
}
