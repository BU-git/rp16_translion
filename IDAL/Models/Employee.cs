using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL.Models
{
    public class Employee
    {
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
                if (value == null)
                    throw new ArgumentNullException("value");
                _employer = value;
                EmployerId = value.UserId;
            }
        }
        #endregion

    }
}
