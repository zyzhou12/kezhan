using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KeZhanManager.Models
{
    public class LoginModel
    {
        [Required]
        [Display(Name="用户名")]
        public string UserName { get; set; }
        [Required]
        [Display(Name = "密码")]
        public string UserPass { get; set; }
    }
}