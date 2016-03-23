using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [Display(Name = "Gebruikersnaam")]
        [StringLength(20, ErrorMessage = "Max length is 20")] //TODO : change message
        public string UserName { get; set; }
    }
}
