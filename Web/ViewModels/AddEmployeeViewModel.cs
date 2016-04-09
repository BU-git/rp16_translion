using System.ComponentModel.DataAnnotations;

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