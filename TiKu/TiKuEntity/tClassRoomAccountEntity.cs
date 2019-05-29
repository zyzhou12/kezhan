using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TiKu.Entity
{
    public class tClassRoomAccountEntity
    {
        public System.Int32 fID { get; set; }
        public System.String fClassRoomCode { get; set; }
        public System.Int32 fBookingCount { get; set; }
        public System.Int32 fPayCount { get; set; }
        public System.Decimal fIncome { get; set; }
        public System.Decimal fRefund { get; set; }
        public System.Decimal fFee { get; set; }
        public System.Decimal fAmount { get; set; }
        public System.Int32 fUsedFlow { get; set; }
        public System.Int32 fStatus { get; set; }
        public System.String fConfirmOpr { get; set; }
        public System.DateTime fConfirmDate { get; set; }
        public System.DateTime fCreateDate { get; set; }
        public System.String fCreateOpr { get; set; }


        public System.String fClassRoomTitle { get; set; }
    }

}
