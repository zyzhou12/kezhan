using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TiKuBll.Model;

namespace KeZhan.Models
{
  public class TeacherClassRoomListModel
  {
      public string strUserName { get; set; }
    public bool IsValid { get; set; }
    public string strStatus { get; set; }
    public string strPayType { get; set; }
    public string strType { get; set; }
    public ClassRoomListModel list { get; set; }
  }
}