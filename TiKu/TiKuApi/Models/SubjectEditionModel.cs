using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TiKu.Api.Models
{
  public class SubjectEditionModel
  {
    public string errorCode { get; set; }
    public List<PharseModel> data { get; set; }
  }

  public class PharseModel
  {
    public int id { get; set; }
    public string name { get; set; }
    public int pid { get; set; }
    public string code { get; set; }
    public List<GradeModel> child { get; set; }

  }

  public class GradeModel
  {
    public int id { get; set; }
    public string name { get; set; }
    public int pid { get; set; }
    public string code { get; set; }
    public List<SubjectModel> child { get; set; }
  }

  public class SubjectModel
  {
    public int id { get; set; }
    public string name { get; set; }
    public int pid { get; set; }
    public string code { get; set; }
    public List<EditionModel> child { get; set; }
  }

  public class EditionModel
  {
    public int id { get; set; }
    public string name { get; set; }
    public int pid { get; set; }
    public string code { get; set; }

  }
}