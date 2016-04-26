using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IDAL.Models;

namespace Web.ViewModels
{
    public class AdminAlertPanelViewModel
    {

        public Alert alert { get; set; }
        public string EmployerName { get; set; }

        public string Company { get; set; }

        public string EmployeeName { get; set; }

        public string AlertType { get; set; }

        public string Comment { get; set; }
        
    }
}