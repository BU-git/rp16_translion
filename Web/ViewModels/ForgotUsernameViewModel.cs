using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Web.ViewModels
{
    public class ForgotUsernameViewModel
    {
        [Display(Name = "Emailadres")]
        [Required(ErrorMessage = "Vul emailadres in")]
        [Remote("CheckEmail", "Home", ErrorMessage = "Uw emailadres is incorrect, controleer dit aub")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Onjuist emailadress")]
        public string Email { get; set; }
    }
}
