using System;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public class PasswordRecoveryViewModel
    {
        [Required(ErrorMessage = "This is required field")]
        [Display(Name = "Nieuwe wachtwoord")]
        public String Password { get; set; }

        [Required(ErrorMessage = "This is required field")]
        [Display(Name = "Bevestig wachtwoord")]
        [Compare(nameof(Password), ErrorMessage = "Password and conf. password not match")]
        public String ConfirmationalPassword { get; set; }

        [Required]
        public String Token { get; set; }

        [Required]
        public Guid Id { get; set; }
    }
}
