using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public class AccountEmployerViewModel
    {
        [Required]
        [Display(Name = "Bedrijfsnaam")]
        public string CompanyName { get; set; }

        [Required]
        [Display(Name = "Voornaam")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Tussenvoegsel")]
        public string Prefix { get; set; }

        [Required]
        [Display(Name = "Achternaam")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Telefoon")]
        public string TelephoneNumber { get; set; }

        [Required]
        [Display(Name = "Email-adres")]
        public string EmailAdress { get; set; }

        [Required]
        [Display(Name = "Postcode")]
        public string PostalCode { get; set; }

        [Required]
        [Display(Name = "Adres")]
        public string Adress { get; set; }

        [Required]
        [Display(Name = "Plaats")]
        public string City { get; set; }

        [Required]
        [Display(Name = "Gebruikersnaam")]
        public string LoginName { get; set; }

        [Required]
        [Display(Name = "Wachtwoord")]
        public string UserPassword { get; set; }

        [Required]
        [Display(Name = "Herhaal wachtwoord")]
        public string RepeatUserPassword { get; set; }
    }
}