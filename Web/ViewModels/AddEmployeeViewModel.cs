using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.ViewModels
{
    public class AddEmployeeViewModel
    {
        [Display(Name = "Voornaam")]
        [Required]
        public string FirstName { get; set; }
        [Display(Name = "Tussenvoegsel")]
        [Required]
        public string Prefix { get; set; }
        [Display(Name = "Achternaam")]
        [Required]
        public string LastName { get; set; }
    }
}
