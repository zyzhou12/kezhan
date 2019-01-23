using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace TiKu.Api.Models
{
  public class ResponseBase
  {
    /// <summary>
    /// 返回值, 大于等于0 success
    /// </summary>
    public int resultCode { get; set; }
    [DefaultValue("")]
    public string resultMessage { get; set; }
  }
}