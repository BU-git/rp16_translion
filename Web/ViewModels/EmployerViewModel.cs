using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using IDAL.Models;

namespace Web.ViewModels
{
    public class EmployerViewModel
    {
        public EmployerViewModel()
        {
            Employees = new List<Employee>();
        }
        public Guid EmployerId { get; set; }

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
        [StringLength(11)]
        public string TelephoneNumber { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "E-mailadres")]
        [EmailAddress]
        public string EmailAdress { get; set; }

        [Required]
        [Display(Name = "Postcode")]
        [StringLength(6)]
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

        public IEnumerable<Employee> Employees { get; set; }

        // TODO CHECK AND DELETE
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