using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TiKu.Api.Models
{
  public class ChapterModel
  {
    public string errorCode { get; set; }
    public List<ChapterData> data { get; set; }
  }

  public class ChapterData
  {
    public int id { get; set; }
    public string name { get; set; }
    public int pid { get; set; }
    public int subjectId { get; set; }
    public int pharseId { get; set; }
    public int editionId { get; set; }
    public int gradeId { get; set; }
    public int sort { get; set; }
    public string oldId { get; set; }
    public List<ChapterData> child { get; set; }
  }
}