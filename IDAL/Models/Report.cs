using System;

namespace IDAL.Models
{
    public class Report
    {
        public Guid ReportId { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
