using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IDAL.Interfaces;
using IDAL.Interfaces.Managers;
using IDAL.Models;

namespace BLL.Services.PersonageService
{
    public class AdminManager : BaseManager, IPersonageManager<Admin>
    {
        public AdminManager(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public List<Admin> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<List<Admin>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<Admin>> GetAllAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Admin Get(User user)
        {
            throw new NotImplementedException();
        }

        public Task<Admin> GetAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<Admin> GetAsync(CancellationToken cancellationToken, User user)
        {
            throw new NotImplementedException();
        }

        public void Create(Admin entity, User user)
        {
            throw new NotImplementedException();
        }

        public void CreateAsync(Admin entity, User user)
        {
            throw new NotImplementedException();
        }

        public void CreateAsync(CancellationToken cancellationToken, Admin entity, User user)
        {
            throw new NotImplementedException();
        }

        public void Update(Admin entity)
        {
            throw new NotImplementedException();
        }

        public void UpdateAsync(Admin entity)
        {
            throw new NotImplementedException();
        }

        public void UpdateAsync(CancellationToken cancellationToken, Admin entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(User user)
        {
            throw new NotImplementedException();
        }

        public void DeleteAsync(User user)
        {
            throw new NotImplementedException();
        }

        public void DeleteAsync(CancellationToken cancellationToken, User user)
        {
            throw new NotImplementedException();
        }


        public override bool DeleteEmployee(Employee employee)
        {
            throw new NotImplementedException();
        }
    }
}