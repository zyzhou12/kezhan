using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TiKu.Entity
{
  public class tClassRoomEntity
  {
    public System.Int32 fID { get; set; }
    public System.String fClassRoomCode { get; set; }
    public System.String fType { get; set; }
    public System.String fClassType { get; set; }
    public System.String fTecharUserName { get; set; }
    public System.String fClassRoomTitle { get; set; }
    public System.String fCoverImg { get; set; }
    public System.String fInfo { get; set; }
    public System.String fDesc { get; set; }
    public System.Int32 fMaxNumber { get; set; }
    public System.DateTime fClassRoomDate { get; set; }
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
    public System.String fQrCode { get; set; }
    public System.Boolean fIsReturn { get; set; }
    public System.String fReturnType { get; set; }
    public System.String fReturnRule { get; set; }
    public System.Boolean fIsRecord { get; set; }
    public System.String fStatus { get; set; }
    public System.DateTime fCreateDate { get; set; }
    public System.String fCreateOpr { get; set; }
    public System.DateTime fModifyDate { get; set; }
    public System.String fModifyOpr { get; set; }


    public System.String TeacherName { get; set; }
    public System.String TeacherHead { get; set; }
  }

}
