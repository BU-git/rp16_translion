using System.Collections.Generic;

namespace IDAL.Models
{
    public class Employer : User
    {
        private ICollection<Employee> _employees; 

        #region Scalar Properties
        public string CompanyName { get; set; }
        public string FirstName { get; set; }
        public string Prefix { get; set; }
        public string LastName { get; set; }
        public string TelephoneNumber { get; set; }
        public string PostalCode { get; set; }
        public string Adress { get; set; }
        public string City { get; set; }
        #endregion

        public virtual ICollection<Employee> Employees
        {
            get { return _employees ?? (_employees = new List<Employee>()); }
            set { _employees = value; }
        }
    }
}