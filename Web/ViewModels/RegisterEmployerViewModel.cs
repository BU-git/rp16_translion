﻿using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Web.Security;

namespace Web.ViewModels
{
    public class RegisterEmployerViewModel
    {
        [Required]
        [Display(Name = "Bedrijfsnaam")]
        public string CompanyName { get; set; }

        [Required]
        [Display(Name = "Voornaam")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Tussenvoegsel")]
        public string Prefix { get; set; }

        [Required]
        [Display(Name = "Achternaam")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Telefoon")]
        public string TelephoneNumber { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email-adres")]
        [EmailAddress]
        public string EmailAdress { get; set; }

        [Required]
        [Display(Name = "Postcode")]
        public string PostalCode { get; set; }

        [Required]
        [Display(Name = "Adres")]
        public string Adress { get; set; }

        [Required]
        [Display(Name = "Plaats")]
        public string City { get; set; }

        [Required]
        [Display(Name = "Gebruikersnaam")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Wachtwoord")]
        public string Password { get; set; } = "default";

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Herhaal wachtwoord")]
        public string ConfirmPassword { get; set; } = "default";
    }
}