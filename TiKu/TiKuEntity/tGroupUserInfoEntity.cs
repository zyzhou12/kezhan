using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TiKu.Entity
{
    public class tGroupUserInfoEntity
    {
        public System.Int32 fID { get; set; }
        public System.String fUserName { get; set; }
        public System.String fUserId { get; set; }
        public System.String fRole { get; set; }
        public System.String fGroupID { get; set; }
        public System.Boolean fIsOnLine { get; set; }
        public System.Boolean fIsPush { get; set; }
        public System.Boolean fIsVideo { get; set; }
        public System.Boolean fIsAudio { get; set; }
        public System.Boolean fIsBorad { get; set; }
        public System.DateTime fLastJoinTime { get; set; }

        public System.DateTime fLastQuitTime { get; set; }

        public System.String fNickName { get; set; }
    }

}
