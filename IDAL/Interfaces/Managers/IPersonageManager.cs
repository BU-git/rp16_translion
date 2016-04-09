using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IDAL.Models;

namespace IDAL.Interfaces.Managers
{
    public interface IPersonageManager<TEntity> where TEntity : class
    {
        #region Get all members

        List<TEntity> GetAll();
        Task<List<TEntity>> GetAllAsync();
        Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken);

        #endregion

        #region Get concrete member

        TEntity Get(User user);
        Task<TEntity> GetAsync(User user);
        Task<TEntity> GetAsync(CancellationToken cancellationToken, User user);

        #endregion

        #region Create

        void Create(TEntity entity, User user);
        void CreateAsync(TEntity entity, User user);
        void CreateAsync(CancellationToken cancellationToken, TEntity entity, User user);

        #endregion

        #region Update

        void Update(TEntity entity);
        void UpdateAsync(TEntity entity);
        void UpdateAsync(CancellationToken cancellationToken, TEntity entity);

        #endregion

        #region Delete

        void Delete(User user);
        void DeleteAsync(User user);
        void DeleteAsync(CancellationToken cancellationToken, User user);

        #endregion
    }
}