using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IDAL.Models;

namespace Web.ViewModels
{
    public class ReportViewModel
    {
        public Employee Employee { get; set; }
        public List<Page> Pages { get; set; }
    }
}