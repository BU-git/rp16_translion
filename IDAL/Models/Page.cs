using System.Collections.Generic;

namespace IDAL.Models
{
    public class Page
    {
        public Page()
        {
            Questions = new List<Question>();
        }

        public int Id { get; set; }

        #region Properties
        public int Order { get; set; }

        public string Name { get; set; }
        #endregion

        #region Navigation properties
        public virtual ICollection<Question> Questions { get; set; }
        #endregion
    }
}
