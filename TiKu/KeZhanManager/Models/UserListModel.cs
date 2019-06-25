using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TiKu.Entity;

namespace KeZhanManager.Models
{
    public class UserListModel
    {
        public int iCount { get; set; }
        public List<UserModel> userList { get; set; }
    }
}