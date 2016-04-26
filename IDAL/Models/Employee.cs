using System;
using System.Collections.Generic;

namespace IDAL.Models
{
    public class Employee
    {
        private ICollection<Alert> _alerts;
        private Employer _employer;


        public Guid EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Prefix { get; set; }
        public bool IsApprove { get; set; }
        public bool IsDeleted { get; set; }
        public Guid EmployerId { get; set; }


        #region Navigation Properties

        public virtual Employer Employer
        {
            get { return _employer; }
            set
            {
                _employer = value;
                if (value != null)
                    EmployerId = value.EmployerId;

            }
        }


        public virtual ICollection<Alert> Alerts
        {
            get { return _alerts ?? (_alerts = new List<Alert>()); }
            set { _alerts = value; }
        }


        #endregion
    }
}