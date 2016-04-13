using System;
using System.Data.Entity;
using System.Security.Claims;
using DAL.Repositories;
using IDAL.Models;
using Claim = IDAL.Models.Claim;

namespace DAL
{
    internal class DBInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            // Add roles
            var roleRepo = new RoleRepository(context);
            roleRepo.Add(new Role {RoleId = Guid.NewGuid(), Name = "Admin"});
            roleRepo.Add(new Role {RoleId = Guid.NewGuid(), Name = "Advisor"});
            roleRepo.Add(new Role {RoleId = Guid.NewGuid(), Name = "Employer"});
            context.SaveChanges();

            // Add Users
            var userRepo = new UserRepository(context);

            #region Admin
            User defaultAdmin = new User()
            {
                UserId = Guid.NewGuid(),
                UserName = "Admin",
                Email = "admin@admin.com",
                PasswordHash = "APBR9EYEz260JqC9otNdi9AQtMa81poBY9UK/Vx7l5qZEjnjJ+LXjeVh/aDH5A+xzQ==",
                SecurityStamp = Guid.NewGuid().ToString()
            };

            // Get Admin role
            Role adminRole = roleRepo.FindByName("Admin");
            defaultAdmin.Roles.Add(adminRole);

            // Add claim
            Claim adminClaim = new Claim()
            {
                User = defaultAdmin,
                UserId = defaultAdmin.UserId,
                ClaimValue = Guid.NewGuid().ToString(),
                ClaimType = Guid.NewGuid().ToString(),
                ClaimId = new Random(103).Next(1,99999999)
            };
            defaultAdmin.Claims.Add(adminClaim);

            // Create Admin information
            Admin adminInfo = new Admin()
            {
                AdminId = defaultAdmin.UserId,
                Name = "Sample name",
                User = defaultAdmin
            };
            defaultAdmin.Admin = adminInfo;
            #endregion

            #region Advisor
            User defaultAdvisor = new User()
            {
                UserId = Guid.NewGuid(),
                UserName = "Advisor",
                Email = "advisor@advisor.com",
                PasswordHash = "APBR9EYEz260JqC9otNdi9AQtMa81poBY9UK/Vx7l5qZEjnjJ+LXjeVh/aDH5A+xzQ==",
                SecurityStamp = Guid.NewGuid().ToString()
            };

            // Get Admin role
            Role advisorRole = roleRepo.FindByName("Advisor");
            defaultAdvisor.Roles.Add(adminRole);

            // Add claim
            Claim advisorClaim = new Claim()
            {
                User = defaultAdvisor,
                UserId = defaultAdvisor.UserId,
                ClaimValue = Guid.NewGuid().ToString(),
                ClaimType = Guid.NewGuid().ToString(),
                ClaimId = new Random(103).Next(1, 99999999)
            };
            defaultAdvisor.Claims.Add(advisorClaim);

            // Create Admin information
            Advisor advisorInfo = new Advisor()
            {
               User = defaultAdvisor,
               AdvisorId = Guid.NewGuid(),
               Name = "Sample name"
            };
            defaultAdvisor.Advisor = advisorInfo;
            #endregion
            
            userRepo.Add(defaultAdmin);
            userRepo.Add(defaultAdvisor);
            context.SaveChanges();
        }
    }
}