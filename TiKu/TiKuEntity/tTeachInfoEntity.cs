using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TiKu.Entity
{
  public class tTeachInfoEntity
  {
    public System.Int32 fID { get; set; }
    public System.String fUserName { get; set; }
    public System.String fDesc { get; set; }
    public System.String fQrCode { get; set; }
    public System.Decimal fDePositAmount { get; set; }
    public System.String fDepositNo { get; set; }
    public System.String fIDCard1 { get; set; }
    public System.String fIDCard2 { get; set; }
    public System.Boolean fIsClassRoom { get; set; }
    public System.Boolean fIsLive { get; set; }
    public System.Decimal fVideoFee { get; set; }
    public System.Boolean fIsProblem { get; set; }
    public System.Decimal fProblemFee { get; set; }
    public System.String fStatus { get; set; }
    public System.DateTime fCreateDate { get; set; }
    public System.String fCreateOpr { get; set; }
    public System.DateTime fModifyDate { get; set; }
    public System.String fModifyOpr { get; set; }

  }

}
