using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.Interfaces;
using IDAL.Models;

namespace DAL.Repositories
{
    internal class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        internal EmployeeRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}
