using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IDAL.Interfaces.IRepositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        #region Methods

        Task<List<TEntity>> GetAll();
        Task<List<TEntity>> GetAll(CancellationToken cancellationToken);

        Task<TEntity> FindById(object id);
        Task<TEntity> FindById(CancellationToken cancellationToken, object id);

        void Add(TEntity entity);
        void Update(TEntity entity);
        void Remove(TEntity entity);

        #endregion
    }
}