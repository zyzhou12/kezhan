using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TiKuBll.Model;

namespace WebEdu.Models
{
  public class TeacherClassRoomListModel
  {
    public bool IsValid { get; set; }
    public ClassRoomListModel list { get; set; }
  }
}