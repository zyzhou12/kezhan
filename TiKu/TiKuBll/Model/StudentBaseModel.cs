using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TiKuBll.Model
{
  public class StudentBaseModel
  {

    public string School { get; set; }
    public string Pharse { get; set; }
    public string Grade { get; set; }
  }

  public class ParentsBaseModel
  {
    public string StudentUser { get; set; }
  }
}
