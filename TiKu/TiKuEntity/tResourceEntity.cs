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
      public System.String fUserName { get; set; }
      public System.String fResourceTitle { get; set; }
      public System.String fType { get; set; }
      public System.String fMediaId { get; set; }
      public System.String fUrl { get; set; }
      public System.String fCoverImg { get; set; }
      public System.String fFileType { get; set; }
      public System.Int32 fSize { get; set; }
      public System.Int32 fWidth { get; set; }
      public System.Int32 fHeigth { get; set; }
      public System.String fDateLength { get; set; }
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
