using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public class RegistrationEmployerViewModel
    {
        [Required]
        [Display(Name = "Bedrijfsnaam")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Uw voornaam is niet correct ingevoerd, controleer dit aub.")]
        [Display(Name = "Voornaam")]
        public string FirstName { get; set; }

        //[Required(ErrorMessage = "Uw tussenvoegsel mag niet langer zijn dan 6 letters, geen cijfers bevatten en geen spaties")]
        [Display(Name = "Tussenvoegsel")]
        public string Prefix { get; set; }

        [Required(ErrorMessage = "Uw achternaam is niet correct ingevoerd, controleer dit aub.")]
        [Display(Name = "Achternaam")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "There's no phone number correctly entered, please check this.")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(11)]
        [Display(Name = "Telefoon")]
        public string TelephoneNumber { get; set; }

        [Required(ErrorMessage = "Uw emailadres is niet correct, controleer dit aub.")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "E-mailadres")]
        [EmailAddress]
        public string EmailAdress { get; set; }

        [Required(ErrorMessage = "Uw postcode is niet correct. Uw postcode mag niet meer als 6 karakters bevatten.")]
        [Display(Name = "Postcode")]
        [StringLength(6)]
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
        public string UserPassword { get; set; }

        [Required(ErrorMessage = "Komt niet overeen met het opgegeven wachtwoord.")]
        [Compare("UserPassword", ErrorMessage = "Komt niet overeen met het opgegeven wachtwoord.")]
        [DataType(DataType.Password)]
        [Display(Name = "Herhaal wachtwoord")]
        public string RepeatUserPassword { get; set; }
    }
}