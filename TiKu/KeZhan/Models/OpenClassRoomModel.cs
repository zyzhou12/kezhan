using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeZhan.Models
{
  public class OpenClassRoomModel
  {
      public string TeacherUserName { get; set; }
    public string UserName { get; set; }
    public string NickName { get; set; }
    public string UserSig { get; set; }
    public string Role { get; set; }
    public string ClassRoomCode { get; set; }

    public string ClassRoomName { get; set; }
    public string ClassRoomType { get; set; }

    /// <summary>
    /// 是否交押金
    /// </summary>
    public bool IsDePosit { get; set; }
  }
}