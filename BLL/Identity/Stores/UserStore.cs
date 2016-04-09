﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Identity.Models;
using IDAL.Interfaces;
using IDAL.Models;
using Microsoft.AspNet.Identity;
using Claim = System.Security.Claims.Claim;

namespace BLL.Identity.Stores
{
    public class UserStore : IUserLoginStore<IdentityUser, Guid>, IUserClaimStore<IdentityUser, Guid>,
        IUserRoleStore<IdentityUser, Guid>, IUserPasswordStore<IdentityUser, Guid>,
        IUserSecurityStampStore<IdentityUser, Guid>, IUserStore<IdentityUser, Guid>, IUserEmailStore<IdentityUser, Guid>,
        IDisposable
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserStore(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region IDisposable Members

        public void Dispose()
        {
            // Dispose does nothing since we want Unity to manage the lifecycle of our Unit of Work
        }

        #endregion

        #region IUserStore<IdentityUser, Guid> Members

        public Task CreateAsync(IdentityUser identityUser)
        {
            if (identityUser == null)
            {
                throw new ArgumentNullException("user");
            }
            var user = GetUser(identityUser);
            _unitOfWork.UserRepository.Add(user);
            return _unitOfWork.SaveChangesAsync();
        }

        public Task DeleteAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var u = GetUser(user);

            _unitOfWork.UserRepository.Remove(u);
            return _unitOfWork.SaveChangesAsync();
        }

        public Task<IdentityUser> FindByIdAsync(Guid userId)
        {
            var user = _unitOfWork.UserRepository.FindById(userId);
            return Task.FromResult(getIdentityUser(user));
        }

        public Task<IdentityUser> FindByNameAsync(string userName)
        {
            var user = _unitOfWork.UserRepository.FindByUserName(userName);
            return Task.FromResult(getIdentityUser(user));
        }

        public Task UpdateAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentException("user");

            var u = _unitOfWork.UserRepository.FindById(user.Id);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");

            PopulateUser(u, user);

            _unitOfWork.UserRepository.Update(u);
            return _unitOfWork.SaveChangesAsync();
        }

        #endregion

        #region IUserClaimStore<IdentityUser, Guid> Members

        public Task AddClaimAsync(IdentityUser user, Claim claim)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (claim == null)
                throw new ArgumentNullException("claim");

            var u = _unitOfWork.UserRepository.FindById(user.Id);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");

            var c = new IDAL.Models.Claim
            {
                ClaimType = claim.Type,
                ClaimValue = claim.Value,
                User = u
            };
            u.Claims.Add(c);

            _unitOfWork.UserRepository.Update(u);
            return _unitOfWork.SaveChangesAsync();
        }

        public Task<IList<Claim>> GetClaimsAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var u = _unitOfWork.UserRepository.FindById(user.Id);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");

            return Task.FromResult<IList<Claim>>(u.Claims.Select(x => new Claim(x.ClaimType, x.ClaimValue)).ToList());
        }

        public Task RemoveClaimAsync(IdentityUser user, Claim claim)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (claim == null)
                throw new ArgumentNullException("claim");

            var u = _unitOfWork.UserRepository.FindById(user.Id);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");

            var c = u.Claims.FirstOrDefault(x => x.ClaimType == claim.Type && x.ClaimValue == claim.Value);
            u.Claims.Remove(c);

            _unitOfWork.UserRepository.Update(u);
            return _unitOfWork.SaveChangesAsync();
        }

        #endregion

        #region IUserLoginStore<IdentityUser, Guid> Members

        public Task AddLoginAsync(IdentityUser user, UserLoginInfo login)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (login == null)
                throw new ArgumentNullException("login");

            var u = _unitOfWork.UserRepository.FindById(user.Id);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");

            var l = new ExternalLogin
            {
                LoginProvider = login.LoginProvider,
                ProviderKey = login.ProviderKey,
                User = u
            };
            u.Logins.Add(l);

            _unitOfWork.UserRepository.Update(u);
            return _unitOfWork.SaveChangesAsync();
        }

        public Task<IdentityUser> FindAsync(UserLoginInfo login)
        {
            if (login == null)
                throw new ArgumentNullException("login");

            var identityUser = default(IdentityUser);

            var l = _unitOfWork.ExternalLoginRepository.GetByProviderAndKey(login.LoginProvider, login.ProviderKey);
            if (l != null)
                identityUser = getIdentityUser(l.User);

            return Task.FromResult(identityUser);
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var u = _unitOfWork.UserRepository.FindById(user.Id);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");

            return
                Task.FromResult<IList<UserLoginInfo>>(
                    u.Logins.Select(x => new UserLoginInfo(x.LoginProvider, x.ProviderKey)).ToList());
        }

        public Task RemoveLoginAsync(IdentityUser user, UserLoginInfo login)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (login == null)
                throw new ArgumentNullException("login");

            var u = _unitOfWork.UserRepository.FindById(user.Id);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");

            var l =
                u.Logins.FirstOrDefault(
                    x => x.LoginProvider == login.LoginProvider && x.ProviderKey == login.ProviderKey);
            u.Logins.Remove(l);

            _unitOfWork.UserRepository.Update(u);
            return _unitOfWork.SaveChangesAsync();
        }

        #endregion

        #region IUserRoleStore<IdentityUser, Guid> Members

        public Task AddToRoleAsync(IdentityUser user, string roleName)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("Argument cannot be null, empty, or whitespace: roleName.");

            var u = _unitOfWork.UserRepository.FindById(user.Id);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");
            var r = _unitOfWork.RoleRepository.FindByName(roleName);
            if (r == null)
                throw new ArgumentException("roleName does not correspond to a Role entity.", "roleName");

            u.Roles.Add(r);
            _unitOfWork.UserRepository.Update(u);

            return _unitOfWork.SaveChangesAsync();
        }

        public Task<IList<string>> GetRolesAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var u = _unitOfWork.UserRepository.FindById(user.Id);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");

            return Task.FromResult<IList<string>>(u.Roles.Select(x => x.Name).ToList());
        }

        public Task<bool> IsInRoleAsync(IdentityUser user, string roleName)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("Argument cannot be null, empty, or whitespace: role.");

            var u = _unitOfWork.UserRepository.FindById(user.Id);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");

            return Task.FromResult(u.Roles.Any(x => x.Name == roleName));
        }

        public Task RemoveFromRoleAsync(IdentityUser user, string roleName)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("Argument cannot be null, empty, or whitespace: role.");

            var u = _unitOfWork.UserRepository.FindById(user.Id);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");

            var r = u.Roles.FirstOrDefault(x => x.Name == roleName);
            u.Roles.Remove(r);

            _unitOfWork.UserRepository.Update(u);
            return _unitOfWork.SaveChangesAsync();
        }

        #endregion

        #region IUserPasswordStore<IdentityUser, Guid> Members

        public Task<string> GetPasswordHashAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            return Task.FromResult(!string.IsNullOrWhiteSpace(user.PasswordHash));
        }

        public Task SetPasswordHashAsync(IdentityUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        #endregion

        #region IUserSecurityStampStore<IdentityUser, Guid> Members

        public Task<string> GetSecurityStampAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            return Task.FromResult(user.SecurityStamp);
        }

        public Task SetSecurityStampAsync(IdentityUser user, string stamp)
        {
            user.SecurityStamp = stamp;
            return Task.FromResult(0);
        }

        #endregion

        #region IEmailStore<IdentityUser, Guid> Members

        public async Task SetEmailAsync(IdentityUser user, string email)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException($"Argument cannot be null, empty, or whitespace: {nameof(email)}.");

            var foundUser = await _unitOfWork.UserRepository.FindByIdAsync(user.Id);

            if (foundUser == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", nameof(user));

            foundUser.Email = email;

            _unitOfWork.UserRepository.Update(foundUser);

            await _unitOfWork.SaveChangesAsync();
        }

        public Task<string> GetEmailAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return _unitOfWork.UserRepository.FindByIdAsync(user.Id)
                .ContinueWith(u => u.Result.Email ?? null, TaskContinuationOptions.ExecuteSynchronously);
        }

        public Task<bool> GetEmailConfirmedAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return _unitOfWork.UserRepository.FindByIdAsync(user.Id)
                .ContinueWith(u =>
                {
                    if (u.Result == null)
                        throw new ArgumentException("IdentityUser does not correspond to a User entity.", nameof(user));
                    return u.Result.EmailConfirmed;
                }, TaskContinuationOptions.ExecuteSynchronously);
        }

        public async Task SetEmailConfirmedAsync(IdentityUser user, bool confirmed)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var foundUser = await _unitOfWork.UserRepository.FindByIdAsync(user.Id);

            if (foundUser == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", nameof(user));

            foundUser.EmailConfirmed = confirmed;

            _unitOfWork.UserRepository.Update(foundUser);

            await _unitOfWork.SaveChangesAsync();
        }

        public Task<IdentityUser> FindByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException($"Argument cannot be null, empty, or whitespace: {nameof(email)}.");

            return _unitOfWork.UserRepository.FindByEmailAsync(email)
                .ContinueWith(u => getIdentityUser(u.Result), TaskContinuationOptions.ExecuteSynchronously);
        }

        #endregion

        #region Private Methods

        private User GetUser(IdentityUser identityUser)
        {
            if (identityUser == null)
                return null;

            var user = new User();
            PopulateUser(user, identityUser);

            return user;
        }

        private void PopulateUser(User user, IdentityUser identityUser)
        {
            user.UserId = identityUser.Id;
            user.UserName = identityUser.UserName;
            user.PasswordHash = identityUser.PasswordHash;
            user.SecurityStamp = identityUser.SecurityStamp;
            user.Email = identityUser.Email;
            user.EmailConfirmed = identityUser.EmailConfirmed;
        }

        private IdentityUser getIdentityUser(User user)
        {
            if (user == null)
                return null;

            var identityUser = new IdentityUser();
            populateIdentityUser(identityUser, user);

            return identityUser;
        }

        private void populateIdentityUser(IdentityUser identityUser, User user)
        {
            identityUser.Id = user.UserId;
            identityUser.UserName = user.UserName;
            identityUser.PasswordHash = user.PasswordHash;
            identityUser.SecurityStamp = user.SecurityStamp;
            identityUser.Email = user.Email;
            identityUser.EmailConfirmed = user.EmailConfirmed;
        }

        #endregion
    }
}