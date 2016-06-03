﻿using System;
using System.ComponentModel.DataAnnotations;


namespace Web.ViewModels
{
    public class EmployeeChangeNameViewModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid EmployerId { get; set; }

        [Required(ErrorMessage = "Vul voornaam in")]
        [Display(Name = "Voornaam")]
        [StringLength(15, ErrorMessage = "Too long username")]
        [RegularExpression("[a-zA-Z]+",
            ErrorMessage = "Veld klopt niet, controleer dit aub. Nummers, symbolen en spaties zijn niet toegestaan")]
        public String FirstName { get; set; }

        [Required(ErrorMessage = "Vul achternaam in")]
        [Display(Name = "Achternaam")]
        [StringLength(15, ErrorMessage = "Too long lastname")]
        [RegularExpression("[a-zA-Z]+",
            ErrorMessage = "Veld klopt niet, controleer dit aub. Nummers, symbolen en spaties zijn niet toegestaan")]
        public String LastName { get; set; }

        [Display(Name = "Tussenvoesgsen")]
        [StringLength(5, ErrorMessage = "Too long prefix")]
        [RegularExpression("[a-zA-Z]+",
            ErrorMessage = "Veld klopt niet, controleer dit aub. Nummers, symbolen zijn niet toegestaan")]
        public String Prefix { get; set; }
    }
}
