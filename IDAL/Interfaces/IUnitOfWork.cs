﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace IDAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        #region Properties
        IExternalLoginRepository ExternalLoginRepository { get; }
        IRoleRepository RoleRepository { get; }
        IUserRepository UserRepository { get; }
        IEmployerRepository EmployerRepository { get; }
        IAdminRepository AdminRepository { get; }
        IEmployeeRepository EmployeeRepository { get; }
        #endregion

        #region Methods
        int SaveChanges();
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        #endregion
    }
}