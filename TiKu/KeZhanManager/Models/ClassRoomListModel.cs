using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TiKu.Entity;

namespace KeZhanManager.Models
{
    public class ClassRoomListModel
    {
        public int iCount { get; set; }
        public List<tClassRoomEntity> classRoomList { get; set; }
    }
}