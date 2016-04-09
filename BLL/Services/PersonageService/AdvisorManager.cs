using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IDAL.Interfaces;
using IDAL.Interfaces.Managers;
using IDAL.Models;

namespace BLL.Services.PersonageService
{
    public class AdvisorManager : PersonManager<Advisor>
    {
        public AdvisorManager(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public override void DeleteEmployee(Employee employee, User user)
        {
            throw new NotImplementedException();
        }

        public override List<Advisor> GetAll()
        {
            throw new NotImplementedException();
        }

        public override Task<List<Advisor>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public override Task<List<Advisor>> GetAllAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Advisor Get(User user)
        {
            throw new NotImplementedException();
        }

        public override Task<Advisor> GetAsync(User user)
        {
            throw new NotImplementedException();
        }

        public override Task<Advisor> GetAsync(CancellationToken cancellationToken, User user)
        {
            throw new NotImplementedException();
        }

        public override void Create(Advisor entity, User user)
        {
            throw new NotImplementedException();
        }

        public override void CreateAsync(Advisor entity, User user)
        {
            throw new NotImplementedException();
        }

        public override void CreateAsync(CancellationToken cancellationToken, Advisor entity, User user)
        {
            throw new NotImplementedException();
        }

        public override void Update(Advisor entity)
        {
            throw new NotImplementedException();
        }

        public override void UpdateAsync(Advisor entity)
        {
            throw new NotImplementedException();
        }

        public override void UpdateAsync(CancellationToken cancellationToken, Advisor entity)
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