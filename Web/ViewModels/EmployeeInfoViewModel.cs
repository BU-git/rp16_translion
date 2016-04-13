using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public class EmployeeInfoViewModel
    {
        [Required]
        public IEnumerable<String> Reports { get; set; }

        [Required]
        public String FullName { get; set; }

        [Required]
        public Guid Id { get; set; }
    }
}
