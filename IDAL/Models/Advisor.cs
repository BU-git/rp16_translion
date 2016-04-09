using System;

namespace IDAL.Models
{
    public class Advisor
    {
        #region Navigation Properties

        public User User { get; set; }

        #endregion

        #region Scalar Properties

        public Guid AdvisorId { get; set; }
        public string Name { get; set; }

        #endregion
    }
}