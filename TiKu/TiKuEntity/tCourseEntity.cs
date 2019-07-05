using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TiKu.Entity
{
  public class tCourseEntity
  {
    public System.Int32 fID { get; set; }
    public System.String fClassType { get; set; }
    public System.String fClassRoomCode { get; set; }
    public System.String fDictTitle { get; set; }
    public System.String fCourseTitle { get; set; }
    public System.DateTime fClassDate { get; set; }

    public System.DateTime fUpdateClassDate { get; set; }
    public System.Int32 fClassDateLength { get; set; }
    public System.Decimal fPrice { get; set; }

    public System.String fDocoumentUrl { get; set; }
    public System.Int32 fOrder { get; set; }
    public System.String fResourceUrl { get; set; }
    public System.String fSource { get; set; }
    public System.DateTime fUploadDate { get; set; }
    public System.String fUploadOpr { get; set; }
    public System.String fAuthor { get; set; }
    public System.String fFileType { get; set; }
    public System.Int32 fFileSize { get; set; }
    public System.String fStatus { get; set; }
    public System.Boolean fIsPay { get; set; }
    public System.String fFileCoverUrl { get; set; }

  }

}
