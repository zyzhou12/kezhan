using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeZhanManager.Models
{
    public class GroupModel
    {

        public System.Int32 fID { get; set; }
        public System.String fGroupID { get; set; }
        public System.String fGroupName { get; set; }

        public System.Int32 fCourseId { get; set; }
        public System.String fTeacherID { get; set; }
        public System.DateTime fCreateDate { get; set; }
        public System.DateTime fDestoryDate { get; set; }
        public System.Boolean fIsOpenHand { get; set; }
        public System.Boolean fIsValid { get; set; }

        public System.Int32 fUserCount { get; set; }
        public System.Int32 fSumLength { get; set; }
    }
}