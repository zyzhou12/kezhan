using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TiKu.Entity
{
  public class tSystemConfigEntity
  {
    public System.Int32 fID { get; set; }
    public System.String fCity { get; set; }
    public System.Decimal fValidFee { get; set; }
    public System.Decimal fClassFee { get; set; }
    public System.Decimal fVideoFee { get; set; }
    public System.Decimal fProblemFee { get; set; }
    public System.Decimal fAccountMinAmount { get; set; }
    public System.Decimal fSourceFee { get; set; }
    public System.DateTime fCreateDate { get; set; }
    public System.String fCreateOpr { get; set; }
    public System.DateTime fModifyDate { get; set; }
    public System.String fModifyOpr { get; set; }

  }

}
