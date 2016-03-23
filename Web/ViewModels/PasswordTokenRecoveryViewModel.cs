using System;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public class PasswordTokenRecoveryViewModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public String Token { get; set; }
    }
}
