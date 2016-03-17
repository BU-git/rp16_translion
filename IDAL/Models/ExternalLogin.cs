using System;

namespace IDAL.Models
{
    public class ExternalLogin
    {
        #region Fields

        private User _user;

        #endregion

        #region Navigation Properties

        public virtual User User
        {
            get { return _user; }
            set
            {
                _user = value;
                UserId = value.UserId;
            }
        }

        #endregion

        #region Scalar Properties

        public virtual string LoginProvider { get; set; }
        public virtual string ProviderKey { get; set; }
        public virtual Guid UserId { get; set; }

        #endregion
    }
}