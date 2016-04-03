using System;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public class PasswordRecoveryViewModel
    {
        [Required(ErrorMessage = "Vul nieuw wachtwoord in")]
        [Display(Name = "Nieuwe wachtwoord")]
        [MinLength(8, ErrorMessage = "Wachtwoord voldoet niet aan eisen. Minimaal 1 hoofdletter, 1 symbool en 8 karakters")]
        [RegularExpression(@"^([a-z]+)([A-Z]+)(\d+)", ErrorMessage = "Wachtwoord voldoet niet aan eisen. Minimaal 1 hoofdletter, 1 symbool en 8 karakters")]
        public String Password { get; set; }

        [Required(ErrorMessage = "Wachtwoord kom niet overeen")]
        [Display(Name = "Bevestig wachtwoord")]
        [Compare(nameof(Password), ErrorMessage = "Wachtwoord kom niet overeen")]
        public String ConfirmationalPassword { get; set; }

        [Required]
        public String Token { get; set; }

        [Required]
        public Guid Id { get; set; }
    }
}
