using DAL.Repositories;
using System;
using System.Data.Entity;
using Domain.Models;

namespace DAL
{
    internal class DBInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            RoleRepository roleRepo = new RoleRepository(context);
            roleRepo.Add(new Role { RoleId = Guid.NewGuid(), Name = "Admin" });
            roleRepo.Add(new Role { RoleId = Guid.NewGuid(), Name = "Employer" });
            base.Seed(context);
        }
    }
}
