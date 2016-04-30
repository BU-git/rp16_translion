using System.Collections.Generic;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using IDAL.Interfaces.IRepositories;

namespace DAL.Repositories
{
    internal class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _context;
        private DbSet<TEntity> _set;

        internal Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        protected DbSet<TEntity> Set
        {
            get { return _set ?? (_set = _context.Set<TEntity>()); }
        }

        public Task<List<TEntity>> GetAll()
        {
            return Set.ToListAsync();
        }

        public Task<List<TEntity>> GetAll(CancellationToken cancellationToken)
        {
            return Set.ToListAsync(cancellationToken);
        }

        public Task<TEntity> FindById(object id)
        {
            return Set.FindAsync(id);
        }

        public Task<TEntity> FindById(CancellationToken cancellationToken, object id)
        {
            return Set.FindAsync(cancellationToken, id);
        }

        public void Add(TEntity entity)
        {
            Set.Add(entity);
        }

        public void Update(TEntity entity)
        {
            var entry = _context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                Set.Attach(entity);
                entry = _context.Entry(entity);
            }
            entry.State = EntityState.Modified;
        }

        public void Remove(TEntity entity)
        {
            Set.Remove(entity);
        }
    }
}