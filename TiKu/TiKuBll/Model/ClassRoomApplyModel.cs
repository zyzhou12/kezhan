using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TiKuBll.Model
{
    public class ClassRoomApplyModel
    {

        public System.Int32 fID { get; set; }
        public System.String fClassRoomCode { get; set; }
        public System.String fStatus { get; set; }
        public System.String fNote { get; set; }
        public System.String fSubmitOpr { get; set; }
        public System.DateTime fSubmitDate { get; set; }
        public System.String fApplyNote { get; set; }
        public System.String fApplyOpr { get; set; }
        public System.DateTime fApplyDate { get; set; }

        public System.String fClassRoomTitle { get; set; }
    }

    public class ClassRoomApplyListModel
    {
        public List<ClassRoomApplyModel> list { get; set; }
    }
}
