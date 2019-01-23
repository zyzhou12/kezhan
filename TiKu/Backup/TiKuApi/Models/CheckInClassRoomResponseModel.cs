using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TiKu.Api.Models
{
  public class CheckInClassRoomResponseModel
  {
    public System.String SDKAppId { get; set; }
    public System.String RoomId { get; set; }

    public System.String UserSig { get; set; }
    public System.String PrivateMapKey { get; set; }
  }
}