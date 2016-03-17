﻿using System.Threading;
using System.Threading.Tasks;
using IDAL.Models;

namespace IDAL.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        #region Methods

        User FindByUserName(string username);
        Task<User> FindByUserNameAsync(string username);
        Task<User> FindByUserNameAsync(CancellationToken cancellationToken, string username);

        User FindByEmail(string email);
        Task<User> FindByEmailAsync(string email);
        Task<User> FindByEmailAsync(CancellationToken cancellationToken, string email);

        #endregion
    }
}