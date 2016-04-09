using System;

namespace IDAL.Models
{
    public class Admin
    {
        #region Navigation Properties

        public virtual User User { get; set; }

        #endregion

        #region Scalar Properties

        public Guid AdminId { get; set; }
        public string Name { get; set; }

        #endregion
    }
}