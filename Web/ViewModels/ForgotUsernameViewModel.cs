using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Web.ViewModels
{
    public class ForgotUsernameViewModel
    {
        [Required]
        [Display(Name = "Emailadres")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Ongeldig emailadres")]
        public string Email { get; set; }
    }
}
