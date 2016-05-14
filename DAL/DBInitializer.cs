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
                // Password: qwerty1234
                PasswordHash = "AHjaRq2q/FfDHi0bYjtDfZqWQWmedFAQcrVpuBeh5mRTk0nlWhQlfE+aiW/4zp/67Q==",
                SecurityStamp = Guid.NewGuid().ToString()
            };

            // Get Admin role
            Role adminRole = roleRepo.FindByName("Admin").Result;
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
                Name = "Default Admin",
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
                // Password: qwerty1234
                PasswordHash = "AHjaRq2q/FfDHi0bYjtDfZqWQWmedFAQcrVpuBeh5mRTk0nlWhQlfE+aiW/4zp/67Q==",
                SecurityStamp = Guid.NewGuid().ToString()
            };

            // Get Admin role
            Role advisorRole = roleRepo.FindByName("Advisor").Result;
            defaultAdvisor.Roles.Add(advisorRole);

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
               Name = "Default Advisor"
            };
            defaultAdvisor.Advisor = advisorInfo;
            #endregion
            
            userRepo.Add(defaultAdmin);
            userRepo.Add(defaultAdvisor);
            context.SaveChanges();
        }
    }
}