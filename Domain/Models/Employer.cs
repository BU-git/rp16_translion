﻿using System;

namespace Domain.Models
{
    public class Employer : User
    {
        #region Scalar Properties
        public string CompanyName { get; set; }
        public string FirstName { get; set; }
        public string Prefix { get; set; }
        public string LastName { get; set; }
        public string TelephoneNumber { get; set; }
        public string EmailAdress { get; set; }
        public string PostalCode { get; set; }
        public string Adress { get; set; }
        public string City { get; set; }
        #endregion
    }
}