using System.Collections.Generic;
using IDAL.Models;

namespace IDAL.Interfaces.Repositories
{
    public interface IPageRepository : IRepository<Page>
    {
        void AddRange(IEnumerable<Page> pages);
    }
}
