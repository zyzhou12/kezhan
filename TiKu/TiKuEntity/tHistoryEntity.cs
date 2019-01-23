using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TiKu.Entity
{
  public class tHistoryEntity
  {
    public System.Int32 fID { get; set; }
    public System.String fUserName { get; set; }
    public System.String fType { get; set; }
    public System.String fSystem { get; set; }
    public System.String fContentType { get; set; }
    public System.String fContentCode { get; set; }
    public System.DateTime fDate { get; set; }
    public System.DateTime fCreateDate { get; set; }
    public System.String fCreateOpr { get; set; }
    public System.DateTime fModifyDate { get; set; }
    public System.String fModifyOpr { get; set; }

  }

}
