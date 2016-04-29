using System.Collections.Generic;
using IDAL.Interfaces.IRepositories;
using IDAL.Models;

namespace DAL.Repositories
{
    internal class PageRepository : Repository<Page>, IPageRepository
    {
        internal PageRepository(ApplicationDbContext context) : base(context)
        {
        }

        public void AddRange(IEnumerable<Page> pages)
        {
            Set.AddRange(pages);
        }
    }
}