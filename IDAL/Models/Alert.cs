﻿using System;

namespace IDAL.Models
{
    public class Alert
    {
        #region Scalar Properties

        public Guid AlertId { get; set; }
        public Guid EmployerId { get; set; }
        public Guid? EmployeeId { get; set; }
        public Guid UserId { get; set; }
        public AlertType AlertType { get; set; }
        public virtual bool AlertIsDeleted { get; set; }
        public virtual DateTime AlertCreateTS { get; set; }
        public virtual DateTime AlertUpdateTS { get; set; }

        #endregion
    }

    public enum AlertType
    {
        Employee_Add,
        Employee_Rename,
        Employee_Delete,
        Employee_Profile,
        Employer_Create,
        Employer_Update,
        Employer_Delete,
        Employer_ChangePassw
    }
}