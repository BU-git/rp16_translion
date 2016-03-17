using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace Web.Models
{
    public class LoginViewModel
    {
        public string LoginName { get; set; }
        public string UserPassword { get; set; }
    }
}