using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IDAL.Models;

namespace IDAL.Interfaces.IManagers
{
    public interface IPersonageManager<TEntity> where TEntity : class
    {
        #region Get all members
        
        Task<List<TEntity>> GetAll();
        Task<List<TEntity>> GetAll(CancellationToken cancellationToken);

        #endregion

        #region Get concrete member
        
        Task<TEntity> Get(Guid userId);
        Task<TEntity> Get(CancellationToken cancellationToken, Guid userId);
        Task<TEntity> Get(string userName);
        Task<TEntity> Get(CancellationToken cancellationToken, string userName);

        #endregion

        #region Create

        Task<WorkResult> Create(TEntity entity);
        Task<WorkResult> Create(CancellationToken cancellationToken, TEntity entity);

        #endregion

        #region Update
        
        Task<WorkResult> Update(TEntity entity);
        Task<WorkResult> Update(CancellationToken cancellationToken, TEntity entity);

        #endregion

        #region Delete
        
        Task<WorkResult> Delete(Guid userId);
        Task<WorkResult> Delete(CancellationToken cancellationToken, Guid userId);
        Task<WorkResult> Delete(string userName);
        Task<WorkResult> Delete(CancellationToken cancellationToken, string userName);

        #endregion


        #region Get user's roles by id

        Task<List<Role>> GetUserRolesById(string userId);
        Task<List<Role>> GetUserRolesById(CancellationToken cancellationToken, string userId);

        #endregion
    }
}