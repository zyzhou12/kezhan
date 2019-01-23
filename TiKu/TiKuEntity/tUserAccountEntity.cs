using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TiKu.Entity
{
  public class tUserAccountEntity
  {
    public System.Int32 fID { get; set; }
    public System.String fUserName { get; set; }
    public System.Decimal fAmount { get; set; }
    public System.String fRemark { get; set; }
    public System.DateTime fDate { get; set; }
    public System.String fSystem { get; set; }
    public System.String fTradingType { get; set; }
    public System.Int32 fTradingId { get; set; }
    public System.String fNote { get; set; }
    public System.DateTime fCreateDate { get; set; }
    public System.String fCreateOpr { get; set; }
    public System.DateTime fModifyDate { get; set; }
    public System.String fModifyOpr { get; set; }

  }

}
