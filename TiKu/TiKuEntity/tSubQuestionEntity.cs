using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TiKu.Entity
{
  public class tSubQuestionEntity
  {
    public System.Int32 fID { get; set; }
    public System.String fTitle { get; set; }
    public System.String fOption_a { get; set; }
    public System.String fOption_b { get; set; }
    public System.String fOption_c { get; set; }
    public System.String fOption_d { get; set; }
    public System.Int32 fPid { get; set; }
    public System.String fAnswer1 { get; set; }
    public System.String fAnswer2 { get; set; }
    public System.String fParse { get; set; }
    public System.String fQtype { get; set; }
    public System.Decimal fDiff { get; set; }
    public System.Int32 fSubjectID { get; set; }
    public System.Int32 fGradeID { get; set; }
    public System.String fKnowledges { get; set; }
    public System.String fSource { get; set; }
    public System.String fTiid { get; set; }
    public System.String fPtiid { get; set; }
    public System.String fFromSite { get; set; }
    public System.String fAnswer_json { get; set; }

  }

}
