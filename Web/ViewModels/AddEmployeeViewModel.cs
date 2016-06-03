using System;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public class AddEmployeeViewModel
    {
        public Guid EmployerID { get; set; }
        [Required(ErrorMessage = "Vul voornaam in")]
        [Display(Name = "Voornaam")]
        [StringLength(15, ErrorMessage = "Gebruikersnaam te lang")]
        [RegularExpression("[a-zA-Z]+",
            ErrorMessage = "Veld klopt niet, controleer dit aub. Nummers, symbolen en spaties zijn niet toegestaan")]
        public string FirstName { get; set; }

        [Display(Name = "Tussenvoegsel")]
        [StringLength(20, ErrorMessage = "Tussenvoegsel te lang")]
        [RegularExpression("[a-zA-Z\\s\"\']+",
            ErrorMessage = "Veld klopt niet, controleer dit aub. Nummers, symbolen en spaties zijn niet toegestaan")]
        public string Prefix { get; set; }

        [Required(ErrorMessage = "Vul achternaam in")]
        [Display(Name = "Achternaam")]
        [StringLength(15, ErrorMessage = "Achternaam te lang")]
        [RegularExpression("[a-zA-Z]+",
           ErrorMessage = "Veld klopt niet, controleer dit aub. Nummers, symbolen en spaties zijn niet toegestaan")]
        public string LastName { get; set; }
    }
}