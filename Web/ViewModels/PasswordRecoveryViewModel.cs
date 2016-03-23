using System;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public class PasswordRecoveryViewModel
    {
        [Required]
        [Display(Name = "Nieuwe wachtwoord")]
        public String Password { get; set; }

        [Required]
        [Display(Name = "Bevestig wachtwoord")]
        public String ConfirmationalPassword { get; set; }

        [Required]
        public String Token { get; set; }

        [Required]
        public Guid Id { get; set; }
    }
}
