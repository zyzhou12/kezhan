using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TiKuBll.Model
{
  public class ValidListModel
  {
    public List<ValidModel> validList { get; set; }
  }

  public class ValidModel
  {
    public System.Int32 fID { get; set; }
    public System.String fUserName { get; set; }
    public System.String fIDCard1 { get; set; }
    public System.String fIDCard2 { get; set; }
    public System.String fTeachCert1 { get; set; }
    public System.String fTeachCert2 { get; set; }
    public System.String fCertType { get; set; }
    public System.String fCertNo { get; set; }
    public System.String fPharse { get; set; }
    public System.String fSubject { get; set; }
    public System.DateTime fUploadDate { get; set; }
    public System.String fValidUser { get; set; }
    public System.DateTime fValidDate { get; set; }
    public System.String fNote { get; set; }
    public System.String fStatus { get; set; }
  }
}
