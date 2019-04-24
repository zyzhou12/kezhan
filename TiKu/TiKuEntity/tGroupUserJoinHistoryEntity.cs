using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TiKu.Entity
{
    public class tGroupUserJoinHistoryEntity
    {
        public System.Int32 fID { get; set; }
        public System.String fUserName { get; set; }
        public System.String fUserId { get; set; }
        public System.String fGroupID { get; set; }
        public System.DateTime fJoinTime { get; set; }
        public System.DateTime fQuitTime { get; set; }

    }

}
