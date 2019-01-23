using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebEdu.Models
{
  public class OpenClassRoomModel
  {
    public string UserName { get; set; }
    public string Role { get; set; }
    public string ClassRoomCode { get; set; }
    public string ClassRoomType { get; set; }

    /// <summary>
    /// 是否交押金
    /// </summary>
    public bool IsDePosit { get; set; }
  }
}