using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TiKu.Api.Models
{
    public class ResponseOnLineClassModel:ResponseBase
    {
        public string strUserName { get; set; }
        public string strSig { get; set; }
        public string strRoomID { get; set; }
        public string strRole { get; set; }
    }
}