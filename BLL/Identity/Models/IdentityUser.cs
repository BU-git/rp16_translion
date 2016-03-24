﻿using System;
using Domain.Models;
using Microsoft.AspNet.Identity;

namespace BLL.Identity.Models
{
    public class IdentityUser : IUser<Guid>
    {
        #region CTOR
        public IdentityUser()
        {
            Id = Guid.NewGuid();
        }

        public IdentityUser(string userName)
            : this()
        {
            UserName = userName;
        }
        #endregion
        
        public virtual string PasswordHash { get; set; }
        public virtual string SecurityStamp { get; set; }
        public Roles Roles { get; set; }
        public virtual Employer Employer { get; set; }
        public virtual Admin Admin { get; set; }
        public Guid Id { get; set; }
        public string UserName { get; set; }
        }

    public enum Roles
    {
        Admin,
        Employer
    }
}