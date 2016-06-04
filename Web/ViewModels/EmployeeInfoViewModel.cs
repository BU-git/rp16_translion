using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using IDAL.Models;

namespace Web.ViewModels
{
    public class EmployeeInfoViewModel
    {
        [Required]
        public IEnumerable<Report> Reports { get; set; }

        [Required]
        public String FullName { get; set; }

        [Required]
        public Guid Id { get; set; }

        public bool IsDeleted { get; set; }
    }
}
