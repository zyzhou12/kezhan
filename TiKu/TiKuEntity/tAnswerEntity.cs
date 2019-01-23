using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TiKu.Entity
{
  public class tAnswerEntity
  {
    public System.Int32 fID { get; set; }
    public System.Int32 fProblemID { get; set; }
    public System.String fUserName { get; set; }
    public System.String fAnswerText { get; set; }
    public System.String fAnswerImg { get; set; }
    public System.String fAnswerVoice { get; set; }
    public System.DateTime fDate { get; set; }
    public System.Int32 fOrder { get; set; }
    public System.DateTime fCreateDate { get; set; }
    public System.String fCreateOpr { get; set; }
    public System.DateTime fModifyDate { get; set; }
    public System.String fModifyOpr { get; set; }

  }

}
