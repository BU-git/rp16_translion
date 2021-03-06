﻿using System;
using System.Linq;
using System.Threading.Tasks;
using BLL.Identity.Models;
using IDAL.Interfaces;
using IDAL.Models;
using Microsoft.AspNet.Identity;

namespace BLL.Identity.Stores
{
    public class RoleStore : IRoleStore<IdentityRole, Guid>, IQueryableRoleStore<IdentityRole, Guid>, IDisposable
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoleStore(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region IQueryableRoleStore<IdentityRole, Guid> Members

        public IQueryable<IdentityRole> Roles
        {
            get
            {
                return _unitOfWork.RoleRepository.GetAll().Result.Select(x => getIdentityRole(x)).AsQueryable();
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            // Dispose does nothing since we want Unity to manage the lifecycle of our Unit of Work
        }

        #endregion

        #region IRoleStore<IdentityRole, Guid> Members

        public Task CreateAsync(IdentityRole role)
        {
            if (role == null)
                throw new ArgumentNullException("role");

            var r = getRole(role);

            _unitOfWork.RoleRepository.Add(r);
            return _unitOfWork.SaveChanges();
        }

        public Task DeleteAsync(IdentityRole role)
        {
            if (role == null)
                throw new ArgumentNullException("role");

            var r = getRole(role);

            _unitOfWork.RoleRepository.Remove(r);
            return _unitOfWork.SaveChanges();
        }

        public async Task<IdentityRole> FindByIdAsync(Guid roleId)
        {
            var role = await _unitOfWork.RoleRepository.FindById(roleId);
            return  getIdentityRole(role);
        }

        public async Task<IdentityRole> FindByNameAsync(string roleName)
        {
            var role = await _unitOfWork.RoleRepository.FindByName(roleName);
            return getIdentityRole(role);
        }

        public Task UpdateAsync(IdentityRole role)
        {
            if (role == null)
                throw new ArgumentNullException("role");
            var r = getRole(role);
            _unitOfWork.RoleRepository.Update(r);
            return _unitOfWork.SaveChanges();
        }

        #endregion

        #region Private Methods

        private Role getRole(IdentityRole identityRole)
        {
            if (identityRole == null)
                return null;
            return new Role
            {
                RoleId = identityRole.Id,
                Name = identityRole.Name
            };
        }

        private IdentityRole getIdentityRole(Role role)
        {
            if (role == null)
                return null;
            return new IdentityRole
            {
                Id = role.RoleId,
                Name = role.Name
            };
        }

        #endregion
    }
}