using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TiKu.Api.Models
{
  public class OtherBasicModel
  {
    public string errorCode { get; set; }
    public List<QTypeModel> qTypes { get; set; }
    public List<PaperTypeModel> paperTypes { get; set; }
    public List<DiffTypeModel> diffTypes { get; set; }
  }

  public class QTypeModel
  {
    public int id { get; set; }
    public int subjectId { get; set; }
    public int pharseId { get; set; }
    public string typeName { get; set; }
  }

  public class PaperTypeModel
  {
    public int id { get; set; }
    public string name { get; set; }
  }

  public class DiffTypeModel
  {
    public int id { get; set; }
    public string name { get; set; }
  }
}