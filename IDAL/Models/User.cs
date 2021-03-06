﻿using System;
using System.Collections.Generic;

namespace IDAL.Models
{
    public class User
    {
        #region Fields

        private ICollection<Claim> _claims;
        private ICollection<ExternalLogin> _externalLogins;
        private ICollection<Role> _roles;
        private ICollection<Alert> _alerts;

        #endregion

        #region Scalar Properties

        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public virtual string PasswordHash { get; set; }
        public virtual string SecurityStamp { get; set; }
        public virtual string Email { get; set; }
        public virtual bool EmailConfirmed { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Admin Admin { get; set; }
        public virtual Advisor Advisor { get; set; }
        public virtual Employer Employer { get; set; }

        public virtual ICollection<Claim> Claims
        {
            get { return _claims ?? (_claims = new List<Claim>()); }
            set { _claims = value; }
        }

        public virtual ICollection<ExternalLogin> Logins
        {
            get
            {
                return _externalLogins ??
                       (_externalLogins = new List<ExternalLogin>());
            }
            set { _externalLogins = value; }
        }

        public virtual ICollection<Role> Roles
        {
            get { return _roles ?? (_roles = new List<Role>()); }
            set { _roles = value; }
        }

        public virtual ICollection<Alert> Alerts
        {
            get { return _alerts ?? (_alerts = new List<Alert>()); }
            set { _alerts = value; }
        }

        #endregion
    }
}