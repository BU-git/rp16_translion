using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IDAL.Models
{
    public class Alert
    {
        #region Fields

        private ICollection<Employee> _employees;

        #endregion



        #region Scalar Properties

        public Guid AlertId { get; set; }
        public Guid AlertEmployerId { get; set; }
        public AlertType AlertType { get; set; }
        public string AlertComment { get; set; }
        public virtual bool AlertIsDeleted { get; set; }
        public virtual DateTime AlertCreateTS { get; set; }
        public virtual DateTime AlertUpdateTS { get; set; }


        #endregion

        #region Navigation Properties

        public virtual Employer Employer { get; set; }
       
        #endregion

        public ICollection<Employee> Employees
        {
            get { return _employees ?? (_employees = new List<Employee>()); }
            set { _employees = value; }
        }
        
    }
    public enum AlertType { Employee_Add,
        Employee_Rename,
        Employee_Delete,
        Employee_Profile,
        Employer_Create,
        Employer_Update,
        Employer_Delete,
        Employer_ChangePassw
    };

}