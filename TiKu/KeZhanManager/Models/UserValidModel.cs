using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TiKu.Entity;

namespace KeZhanManager.Models
{
    public class UserValidModel
    {
        public tTeachValidEntity valid { get; set; }
        public List<tTeacherValidDetailEntity> validDetail { get; set; }
    }
}