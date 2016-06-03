using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using FromDataAnnotations = System.ComponentModel.DataAnnotations;
namespace Web.ViewModels
{
    public class CreateAdvisorViewModel
    {
        [Display(Name= "Naam")]
        [Required(ErrorMessage = "Vul naam in")]
        [RegularExpression(@"[a-zA-Z\s]*", ErrorMessage = "Naam voldoet niet aan de eisen. Naam mag alleen letters bevatten")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Naam voldoet niet aan de eisen. Minimaal 4 karakters(max - 20)")]
        public String Name { get; set; }

        [Display(Name = "E-mailadres")]
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string EmailAdress { get; set; }

        [Display(Name= "Gebruikersnaam")]
        [Required(ErrorMessage = "Vul gebruikersnaam in")]
        [Remote("CheckAdvisorName", "Admin", ErrorMessage = "Uw gebruikersnaam is incorrect, controleer dit aub.(In use)")]
        [StringLength(15, MinimumLength = 8, ErrorMessage = "Gebruikersnaam voldoet niet aan de eisen. De gebruikersnaam moet minimaal 8 karakters bevatten, max. - 15")]
        public String Username { get; set; }

        [Required(ErrorMessage = "Vul wachwoord in")]
        [Display(Name= "Wachtwoord")]
        [StringLength(15, MinimumLength = 8, ErrorMessage = "Wachtwoord voldoet niet aan de eisen. Minimaal 1 hoofdletter, 1 symbool en 8 karakters(max - 15)")]
        public String Password { get; set; }

        [Display(Name = "Bevestig wachtwoord")]
        [Required(ErrorMessage = "Vul Bevestig wachtwoord in")]
        [FromDataAnnotations.Compare(nameof(Password), ErrorMessage = "Wachtwoord komt niet overeen")]
        public String ConfirmPassword { get; set; }
    }
}
