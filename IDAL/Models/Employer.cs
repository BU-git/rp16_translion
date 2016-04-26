using System;
using System.Collections.Generic;

namespace IDAL.Models
{
    public class Employer
    {
        #region Fields

        private ICollection<Employee> _employees;
        private ICollection<Alert> _alerts; 

        #endregion

        #region Scalar Properties

        public Guid EmployerId { get; set; }
        public string CompanyName { get; set; }
        public string FirstName { get; set; }
        public string Prefix { get; set; }
        public string LastName { get; set; }
        public string TelephoneNumber { get; set; }
        public string PostalCode { get; set; }
        public string Adress { get; set; }
        public string City { get; set; }
        public bool IsDeleted { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ICollection<Employee> Employees
        {
            get { return _employees ?? (_employees = new List<Employee>()); }
            set { _employees = value; }
        }

        public virtual ICollection<Alert> Alerts
        {
            get { return _alerts ?? (_alerts = new List<Alert>()); }
            set { _alerts = value; }
        }

        public virtual User User { get; set; }

        #endregion
    }
}