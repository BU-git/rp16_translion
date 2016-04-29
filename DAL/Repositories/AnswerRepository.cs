using IDAL.Interfaces.IRepositories;
using IDAL.Models;

namespace DAL.Repositories
{
    internal class AnswerRepository : Repository<Answer>, IAnswerRepository
    {
        internal AnswerRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}