using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TiKuBll.Model
{
    public class UserMemberModel
    {

        public System.Int32 fID { get; set; }
        public System.String fUserName { get; set; }
        public System.String fMemberUserName { get; set; }
        public System.DateTime fCreateDateTime { get; set; }
        public System.String fNote { get; set; }
        public System.String fStatus { get; set; }

        public System.String MemberName { get; set; }
        public System.String MemberHead { get; set; }
    }

    public class UserMemberListModel
    {
        public List<UserMemberModel> userMemberList { get; set; }
    }
}
