using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public class AnonymousRegistrationEmployerVM
    {
        [Required(ErrorMessage = "Deze bedrijfsnaam is niet goed ingevoerd.")]
        [StringLength(20, MinimumLength = 1,
            ErrorMessage = "Deze bedrijfsnaam is niet goed ingevoerd.")]
        [Display(Name = "Bedrijfsnaam")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Uw voornaam is niet correct ingevoerd, controleer dit aub.")]
        [Display(Name = "Voornaam")]
        [StringLength(20, MinimumLength = 1,
            ErrorMessage = "Uw voornaam is niet correct ingevoerd, controleer dit aub.")]
        public string FirstName { get; set; }
        
        [Display(Name = "Tussenvoegsel")]
        [RegularExpression("^[A-Za-z]{1,10}",
            ErrorMessage =
                "Uw telefoonnummer voldoet niet aan de eisen, het moet minimaal bestaan uit 10 cijfers en maximaal 20.")
        ]
        public string Prefix { get; set; }

        [Required(ErrorMessage = "Uw achternaam is niet correct ingevoerd, controleer dit aub.")]
        [Display(Name = "Achternaam")]
        public string LastName { get; set; }

        [Required(
            ErrorMessage =
                "Uw telefoonnummer voldoet niet aan de eisen, het moet minimaal bestaan uit 10 cijfers en maximaal 20")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Telefoon")]
        [RegularExpression("^\\d{10,20}$",
            ErrorMessage =
                "Uw telefoonnummer voldoet niet aan de eisen, het moet minimaal bestaan uit 10 cijfers en maximaal 20.")
        ]
        public string TelephoneNumber { get; set; }

        [Required(ErrorMessage = "Uw emailadres is niet correct, controleer dit aub.")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "E-mailadres")]
        [EmailAddress]
        public string EmailAdress { get; set; }

        [Required(ErrorMessage = "Uw postcode is niet correct. Uw postcode mag niet meer als 6 karakters bevatten.")]
        [Display(Name = "Postcode")]
        [StringLength(6, MinimumLength = 6,
            ErrorMessage = "Uw postcode voldoet niet aan de eisen, deze moet uit 6 karakters bestaan")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Uw adres is niet correct ingevuld, controleer dit aub.")]
        [Display(Name = "Adres")]
        public string Adress { get; set; }

        [Required(ErrorMessage = "Uw plaats is niet correct ingevuld, controleer dit aub.")]
        [Display(Name = "Plaats")]
        public string City { get; set; }

        [Required(
            ErrorMessage =
                "Uw gebruikersnaam komt niet overeen met de voorwaarden. Uw gebruikersnaam moet minimaal 8 karakters bevatten, u mag symbolen en cijfers gebruiken"
            )]
        [Display(Name = "Gebruikersnaam")]
        public string LoginName { get; set; }

        [Required(
            ErrorMessage =
                "Uw wachtwoord komt niet overeen met de voorwaarden. Uw wachtwoord moet minimaal 8 karakters bevatten, met minimaal 1 symbool, 1 cijfer en 1 hoofdletter"
            )]
        [DataType(DataType.Password)]
        [Display(Name = "Wachtwoord")]
        [MinLength(8, ErrorMessage = "Wachtwoord voldoet niet aan eisen. Minimaal 1 hoofdletter, 1 symbool en 8 karakters")]
        public string UserPassword { get; set; }

        [Required(ErrorMessage = "Komt niet overeen met het opgegeven wachtwoord.")]
        [Compare("UserPassword", ErrorMessage = "Komt niet overeen met het opgegeven wachtwoord.")]
        [DataType(DataType.Password)]
        [Display(Name = "Herhaal wachtwoord")]
        public string RepeatUserPassword { get; set; }
    }
}