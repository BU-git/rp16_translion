﻿using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public class AddEmployeeViewModel
    {
        [Required(ErrorMessage = "Vul voornaam in")]
        [Display(Name = "Voornaam")]
        [StringLength(15, ErrorMessage = "Too long username")]
        [RegularExpression("[a-zA-Z]+",
            ErrorMessage = "Veld klopt niet, controleer dit aub. Nummers, symbolen en spaties zijn niet toegestaan")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Vul tussenvoesgsen in")]
        [Display(Name = "Tussenvoesgsen")]
        [StringLength(5, ErrorMessage = "Too long prefix")]
        [RegularExpression("[a-zA-Z]+",
            ErrorMessage = "Veld klopt niet, controleer dit aub. Nummers, symbolen en spaties zijn niet toegestaan")]
        public string Prefix { get; set; }

        [Required(ErrorMessage = "Vul achternaam in")]
        [Display(Name = "Achternaam")]
        [StringLength(15, ErrorMessage = "Too long lastname")]
        [RegularExpression("[a-zA-Z]+",
           ErrorMessage = "Veld klopt niet, controleer dit aub. Nummers, symbolen en spaties zijn niet toegestaan")]
        public string LastName { get; set; }
    }
}