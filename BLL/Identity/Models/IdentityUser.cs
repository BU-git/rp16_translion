using System;
using Microsoft.AspNet.Identity;

namespace BLL.Identity.Models
{
    public class IdentityUser : IUser<Guid>
    {
        public virtual string PasswordHash { get; set; }
        public virtual string SecurityStamp { get; set; }
        public virtual string Email { get; set; }
        public virtual bool EmailConfirmed { get; set; }

        public Guid Id { get; set; }
        public string UserName { get; set; }

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
    }
}