using System;
using Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace BLL.Identity.Models
{
    public class IdentityUser : IUser<Guid>
    {
        #region CTOR
        public IdentityUser()
        {
            this.Id = Guid.NewGuid();
        }

        public IdentityUser(string userName)
            : this()
        {
            this.UserName = userName;
        }
        #endregion
        
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public virtual string PasswordHash { get; set; }
        public virtual string SecurityStamp { get; set; }
        public virtual string Email { get; set; }
        public virtual bool EmailConfirmed { get; set; }
        public Roles Roles { get; set; }
        public virtual Employer Employer { get; set; }
        public virtual Admin Admin { get; set; }
        }

    public enum Roles
    {
        Admin,
        Employer
    }
}