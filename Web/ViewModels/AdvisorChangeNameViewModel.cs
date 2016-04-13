using System;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public class AdvisorChangeNameViewModel
    {
        [Display(Name = "Naam")]
        [Required(ErrorMessage = "Vul naam in")]
        [RegularExpression("[a-zA-Z]*", ErrorMessage = "Naam voldoet niet aan de eisen. Naam mag alleen letters bevatten")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Naam voldoet niet aan de eisen. Minimaal 4 karakters(max - 20)")]
        public String Name { get; set; }

        [Required]
        public Guid Id { get; set; }
    }
}
