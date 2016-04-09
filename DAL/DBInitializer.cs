using System;
using System.Data.Entity;
using DAL.Repositories;
using IDAL.Models;

namespace DAL
{
    internal class DBInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            var roleRepo = new RoleRepository(context);
            roleRepo.Add(new Role {RoleId = Guid.NewGuid(), Name = "Admin"});
            roleRepo.Add(new Role {RoleId = Guid.NewGuid(), Name = "Advisor"});
            roleRepo.Add(new Role {RoleId = Guid.NewGuid(), Name = "Employer"});
            base.Seed(context);
        }
    }
}