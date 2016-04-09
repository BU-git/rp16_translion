using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IDAL.Interfaces;
using IDAL.Interfaces.Managers;
using IDAL.Models;

namespace BLL.Services.PersonageService
{
    public class AdminManager : PersonManager<Admin>
    {
        public AdminManager(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }


        public override void DeleteEmployee(User user, Employee employee)
        {
            throw new NotImplementedException();
        }

        public override List<Admin> GetAll()
        {
            throw new NotImplementedException();
        }

        public override Task<List<Admin>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public override Task<List<Admin>> GetAllAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Admin Get(User user)
        {
            throw new NotImplementedException();
        }

        public override Task<Admin> GetAsync(User user)
        {
            throw new NotImplementedException();
        }

        public override Task<Admin> GetAsync(CancellationToken cancellationToken, User user)
        {
            throw new NotImplementedException();
        }

        public override void Create(Admin entity, User user)
        {
            throw new NotImplementedException();
        }

        public override void CreateAsync(Admin entity, User user)
        {
            throw new NotImplementedException();
        }

        public override void CreateAsync(CancellationToken cancellationToken, Admin entity, User user)
        {
            throw new NotImplementedException();
        }

        public override void Update(Admin entity)
        {
            throw new NotImplementedException();
        }

        public override void UpdateAsync(Admin entity)
        {
            throw new NotImplementedException();
        }

        public override void UpdateAsync(CancellationToken cancellationToken, Admin entity)
        {
            throw new NotImplementedException();
        }

        public override void Delete(User user)
        {
            throw new NotImplementedException();
        }

        public override void DeleteAsync(User user)
        {
            throw new NotImplementedException();
        }

        public override void DeleteAsync(CancellationToken cancellationToken, User user)
        {
            throw new NotImplementedException();
        }
    }
}