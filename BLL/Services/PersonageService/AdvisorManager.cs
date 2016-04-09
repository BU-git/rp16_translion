using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IDAL.Interfaces;
using IDAL.Interfaces.Managers;
using IDAL.Models;

namespace BLL.Services.PersonageService
{
    public class AdvisorManager : BaseManager, IPersonageManager<Advisor>
    {
        public AdvisorManager(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public List<Advisor> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<List<Advisor>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<Advisor>> GetAllAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Advisor Get(User user)
        {
            throw new NotImplementedException();
        }

        public Task<Advisor> GetAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<Advisor> GetAsync(CancellationToken cancellationToken, User user)
        {
            throw new NotImplementedException();
        }

        public void Create(Advisor entity, User user)
        {
            throw new NotImplementedException();
        }

        public void CreateAsync(Advisor entity, User user)
        {
            throw new NotImplementedException();
        }

        public void CreateAsync(CancellationToken cancellationToken, Advisor entity, User user)
        {
            throw new NotImplementedException();
        }

        public void Update(Advisor entity)
        {
            throw new NotImplementedException();
        }

        public void UpdateAsync(Advisor entity)
        {
            throw new NotImplementedException();
        }

        public void UpdateAsync(CancellationToken cancellationToken, Advisor entity)
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