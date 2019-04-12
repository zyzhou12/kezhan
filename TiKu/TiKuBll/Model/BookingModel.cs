using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TiKuBll.Model
{
    public class BookingModel
    {
        public System.Int32 fID { get; set; }
        public System.String fUserName { get; set; }
        public System.String fBookingNo { get; set; }
        public System.String fType { get; set; }
        public System.String fTypeCode { get; set; }
        public System.Decimal fAmount { get; set; }
        public System.DateTime fBuyDate { get; set; }
        public System.String fBuySystem { get; set; }
        public System.Boolean fIsPay { get; set; }
        public System.String fStatus { get; set; }
        public System.Boolean fIsReturn { get; set; }
        public System.DateTime fCreateDate { get; set; }
        public System.String fCreateOpr { get; set; }
        public System.DateTime fModifyDate { get; set; }
        public System.String fModifyOpr { get; set; }

        public System.String UserName { get; set; }
        public System.String UserHead { get; set; }
        public System.String Mobile { get; set; }
        public System.String Title { get; set; }
        public System.String TeacherName { get; set; }
        public System.String CoverImg { get; set; }
        public System.Decimal UserAccountAmount { get; set; }
        public System.String PayOrderNo { get; set; }

        public System.Int32 MaxReturnAmount { get; set; }

        public ClassRoomModel ClassRoom { get; set; }
    }

    public class BookingListModel
    {
        public System.String fClassRoomCode { get; set; }
        public System.String fClassType { get; set; }

        public System.String fStatus { get; set; }
        public List<BookingModel> list { get; set; }
    }
}
