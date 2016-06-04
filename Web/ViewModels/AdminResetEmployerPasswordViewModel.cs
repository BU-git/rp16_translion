using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public sealed class AdminResetEmployerPasswordViewModel
    {
        [Required]
        public string ResultMessage { get; set; }
    }
}