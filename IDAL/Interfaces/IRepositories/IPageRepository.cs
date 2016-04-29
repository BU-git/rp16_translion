using System.Collections.Generic;
using System.Threading.Tasks;
using IDAL.Models;

namespace IDAL.Interfaces.IRepositories
{
    public interface IPageRepository : IRepository<Page>
    {
        void AddRange(IEnumerable<Page> pages);
    }
}