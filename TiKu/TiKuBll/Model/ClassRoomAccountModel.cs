using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TiKuBll.Model
{
    public class ClassRoomAccountModel
    {
        public int fID { get; set; }
        public string fClassRoomCode { get; set; }
        public int fBookingCount { get; set; }
        public int fPayCount { get; set; }
        public decimal fIncome { get; set; }
        public decimal fRefund { get; set; }
        public decimal fFee { get; set; }
        public decimal fAmount { get; set; }
        public int fUsedFlow { get; set; }
        public int fStatus { get; set; }
        public DateTime fConfirmDate { get; set; }
        public string fConfirmOpr { get; set; }
        public DateTime fCreateDate { get; set; }
        public string fCreateOpr { get; set; }
    }
}
