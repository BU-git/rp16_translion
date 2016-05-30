using IDAL.Models;
using System.Collections.Generic;

namespace Web.ViewModels
{
    public class UsersViewModel
    {
        public List<Advisor> Advisors { get; set; }
        public List<Employer> Employers { get; set; }
    }
}