using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Gebruikersnaam")]
        public string LoginName { get; set; }

        [Required]
        [Display(Name = "Wachtwoord")]
        public string UserPassword { get; set; }
    }
}