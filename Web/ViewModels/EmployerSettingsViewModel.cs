using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public class EmployerSettingsViewModel
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

        [Required(ErrorMessage = "Uw telefoonnummer voldoet niet aan de eisen, het moet minimaal bestaan uit 10 cijfers en maximaal 20")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Telefoon")]
        [StringLength(20, MinimumLength = 10,
            ErrorMessage = "Uw telefoonnummer voldoet niet aan de eisen, het moet minimaal bestaan uit 10 cijfers en maximaal 20")]
        public string TelephoneNumber { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "E-mailadres")]
        [EmailAddress]
        public string EmailAdress { get; set; }

        [Required(ErrorMessage = "Uw postcode is niet correct. Uw postcode mag niet meer als 6 karakters bevatten.")]
        [Display(Name = "Postcode")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "Uw postcode voldoet niet aan de eisen, deze moet uit 6 karakters bestaan")]
        public string PostalCode { get; set; }

        [Required]
        [Display(Name = "Adres")]
        public string Adress { get; set; }

        [Required]
        [Display(Name = "Plaats")]
        public string City { get; set; }

        [Required]
        [Display(Name = "Gebruikersnaam")]
        public string UserName { get; set; }
    }
}
