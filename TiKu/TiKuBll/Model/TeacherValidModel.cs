using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TiKuBll.Model
{
  public class TeacherValidModel
  {
    public System.String fUserName { get; set; }


    public System.Int32 fID { get; set; }
    public System.String fName { get; set; }
    public System.String fIDType { get; set; }
    public System.String fUID { get; set; }
    public System.String fIDCard1 { get; set; }
    public System.String fIDCard2 { get; set; }
    public System.String fStatus { get; set; }
    public System.DateTime fCreateDate { get; set; }

    public List<TeacherValidDetailModel> detailList { get; set; }
  }

  public class TeacherValidListModel
  {
    public List<TeacherValidModel> validList { get; set; }
  }

    public class ValidDetailListModel
    {

        public List<TeacherValidDetailModel> detailList { get; set; }
    }

  public class TeacherValidDetailModel
  {
      public System.Int32 fID { get; set; }
      public System.Int32 fTeacherValidID { get; set; }
      public System.String fCertType { get; set; }
      public System.String fTeachCert1 { get; set; }
      public System.String fTeachCert2 { get; set; }
      public System.DateTime fUploadDate { get; set; }
      public System.String fPharse { get; set; }
      public System.String fSubject { get; set; }

      public System.String fValidUser { get; set; }
      public System.DateTime fValidDate { get; set; }
      public System.String fNote { get; set; }

      public System.String fStatus { get; set; }

  }
}