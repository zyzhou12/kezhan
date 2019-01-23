using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TiKuBll.Model;

namespace WebEdu.Models
{
  public class UserBaseModel
  {
    public string userRole { get; set; }
    public UserInfoModel userInfo { get; set; }
    public TeacherBaseModel teacherInfo { get; set; }
    public StudentBaseModel studentInfo { get; set; }
    public ParentsBaseModel parentsInfo { get; set; }

    public TeacherValidListModel validInfo { get; set; }
  }
}