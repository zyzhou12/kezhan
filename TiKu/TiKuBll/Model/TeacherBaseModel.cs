using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TiKuBll.Model
{
  public class TeacherBaseModel
  {

    public string fDesc { get; set; }
    public string fQrCode { get; set; }
    public string fStatus { get; set; }

    public string fIDCard1 { get; set; }
    public string fIDCard2 { get; set; }


    public bool fIsClassRoom { get; set; }
    public bool fIsLive { get; set; }
    public bool fIsProblem { get; set; }

    public decimal fVideoFee { get; set; }
    public decimal fProblemFee { get; set; }

  }
}
