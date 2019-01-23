using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TiKu.Entity
{
  public class tResourceEntity
  {
    public System.Int32 fID { get; set; }
    public System.String fResourceCode { get; set; }
    public System.String fResourceTitle { get; set; }
    public System.String fCoverImg { get; set; }
    public System.String fResourceInfo { get; set; }
    public System.String fResourceDesc { get; set; }
    public System.Decimal fPrice { get; set; }
    public System.Decimal fBasePrice { get; set; }
    public System.String fPharse { get; set; }
    public System.String fGrade { get; set; }
    public System.String fSubject { get; set; }
    public System.String fKnowLedge { get; set; }
    public System.Boolean fIsDownLoad { get; set; }
    public System.Decimal fDownLoadPrice { get; set; }
    public System.Boolean fIsTrySee { get; set; }
    public System.Boolean fPayIsDown { get; set; }
    public System.String fStatus { get; set; }
    public System.DateTime fCreateDate { get; set; }
    public System.String fCreateOpr { get; set; }
    public System.DateTime fModifyDate { get; set; }
    public System.String fModifyOpr { get; set; }

  }

}
