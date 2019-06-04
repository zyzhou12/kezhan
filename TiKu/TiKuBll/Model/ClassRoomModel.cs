using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TiKuBll.Model
{

  public class ClassRoomModel
  {
    public int fID { get; set; }
    public System.String fClassRoomCode { get; set; }
    public System.String fType { get; set; }
    public System.String fClassType { get; set; }
    public System.String fTecharUserName { get; set; }
    public System.String fClassRoomTitle { get; set; }
    public System.String fCoverImg { get; set; }
    public System.String fInfo { get; set; }
    public System.String fDesc { get; set; }
    public System.String fQrCode { get; set; }
    public System.Int32 fMaxNumber { get; set; }
    public System.DateTime fClassRoomDate { get; set; }
    public System.Int32 fClassDateLength { get; set; }
    public System.DateTime fDeadLineDate { get; set; }
    public System.Int32 fEffectDay { get; set; }
    public System.Int32 fFeeLength { get; set; }
    public System.Decimal fPrice { get; set; }
    public System.Decimal fBasePrice { get; set; }
    public System.Int32 fTeacherValidID { get; set; }
    public System.String fPharse { get; set; }
    public System.String fGrade { get; set; }
    public System.String fSubject { get; set; }
    public System.String fKnowLedge { get; set; }
    public System.String fPayType { get; set; }
    public System.Boolean fIsReturn { get; set; }
    public System.String fReturnType { get; set; }
    public System.String fReturnRule { get; set; }
    public System.Boolean fIsRecord { get; set; }
    public System.String fStatus { get; set; }

    public System.Int32 IsBuy { get; set; }

    public System.String fCreateOpr { get; set; }

    public System.String UserName { get; set; }
    public System.String TeacherName { get; set; }
    public System.String TeacherHead { get; set; }
    public System.String TeacherDesc { get; set; }
    public System.String TeacherQrCode { get; set; }
    public System.Int32 IsFocus { get; set; }

    public System.String showType { get; set; }
    public List<DescModel> descList { get; set; }
    public List<CourseModel> courseList { get; set; }
    public List<TeacherValidDetailModel> validList { get; set; }

    public ClassRoomAccountModel account { get; set; }
    public decimal LeftFlow { get; set; }
  }

  public class ClassRoomListModel
  {
      public string strStatus { get; set; }
      public string strPayType { get; set; }
      public string strType { get; set; }
      public string strClassType { get; set; }
    public List<ClassRoomModel> classRoomList { get; set; }
  }

    public class DescListModel
    {
        public bool isEdit { get; set; }
        public string TeacherUserName { get; set; }
        public List<DescModel> descList { get; set; }
    }

  public class DescModel
  {
    public System.Int32 fID { get; set; }
    public System.String fClassRoomCode { get; set; }
    public System.String fType { get; set; }
    public System.String fContent { get; set; }
    public System.Int32 fOrder { get; set; }
  }

    public class CourseListModel
    {
        public bool isEdit { get; set; }
        public string TeacherUserName { get; set; }
        public List<CourseModel> courseList { get; set; }
    }
  public class CourseModel
  {
    public System.Int32 fID { get; set; }
    public System.String fClassType { get; set; }
    public System.String fClassRoomCode { get; set; }
    public System.String fDictTitle { get; set; }
    public System.String fCourseTitle { get; set; }
    public System.DateTime fClassDate { get; set; }

    public System.Decimal fPrice { get; set; }
    public System.String fDocoumentUrl { get; set; }
    public System.Int32 fClassDateLength { get; set; }
    public System.Int32 fOrder { get; set; }
    public System.String fResourceUrl { get; set; }
    public System.String fSource { get; set; }
    public System.DateTime fUploadDate { get; set; }
    public System.String fUploadOpr { get; set; }
    public System.String fAuthor { get; set; }
    public System.String fFileType { get; set; }
    public System.Int32 fFileSize { get; set; }
    public System.String fStatus { get; set; }
    public System.Boolean fIsPay { get; set; }
    public System.String fFileCoverUrl { get; set; }

    public System.Int32 CourseStatus { get; set; }
    public System.Int32 IsBuy { get; set; }
  }
}
