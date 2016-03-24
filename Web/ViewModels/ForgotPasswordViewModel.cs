using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Web.ViewModels
{
    public class ForgotPasswordViewModel
    {     
        [Display(Name = "Gebruikersnaam")]
        [Required(ErrorMessage = "This is required field")]
        [Remote("CheckUserName", "Home", ErrorMessage = "Uw gebruinkersnaam is incorrect, controleer dit aub")]
        [StringLength(20, ErrorMessage = "Max length is 20")] //TODO : change message
        public string UserName { get; set; }
    }
}
