using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TiKu.Entity
{
  public class tSMSMessageEntity
  {
    public System.Int32 fID { get; set; }
    public System.String fMessageType { get; set; }
    public System.String fMobile { get; set; }
    public System.String fContent { get; set; }
    public System.DateTime fSendDate { get; set; }
    public System.String fStatus { get; set; }
    public System.String fError { get; set; }
    public System.DateTime fCreateDate { get; set; }
    public System.String fCreateOpr { get; set; }
    public System.DateTime fModifyDate { get; set; }
    public System.String fModifyOpr { get; set; }

  }

}
