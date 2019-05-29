using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeZhan.Models
{
  public class FileModel
  {
    public string UserName { get; set; }
    public string FileType { get; set; }
    public string Name { get; set; }
    public string ID { get; set; }
    public string FileUrl { get; set; }

    public int FreeDateLength { get; set; }
  }
}