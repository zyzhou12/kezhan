using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TiKu.Entity
{
  public class tBookingEntity
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
    public System.String fIsReturn { get; set; }
    public System.DateTime fCreateDate { get; set; }
    public System.String fCreateOpr { get; set; }
    public System.DateTime fModifyDate { get; set; }
    public System.String fModifyOpr { get; set; }

  }

}
