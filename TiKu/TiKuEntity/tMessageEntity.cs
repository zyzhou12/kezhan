using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TiKu.Entity
{
  public class tMessageEntity
  {
    public System.Int32 fID { get; set; }
    public System.String fFromUser { get; set; }
    public System.String fToUser { get; set; }
    public System.String fTitle { get; set; }
    public System.String fContent { get; set; }
    public System.String fType { get; set; }

    public System.Int32 fTypeID { get; set; }
    public System.DateTime fSendDate { get; set; }
    public System.Int32 fStatus { get; set; }
    public System.DateTime fCreateDate { get; set; }
    public System.String fCreateOpr { get; set; }
    public System.DateTime fModifyDate { get; set; }
    public System.String fModifyOpr { get; set; }

  }

}
