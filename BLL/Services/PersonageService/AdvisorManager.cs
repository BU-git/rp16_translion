using System;
using System.Collections.Generic;
using System.Linq;
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

        public override void DeleteEmployee(User user, Employee employee)
        {

            if (user == null || employee == null)
            {
                throw new ArgumentException("User is null, employee is absent. Wrong parameters");
            }
            if (user.Employer==null)
            {
                throw new ArgumentException("Only employer can have employees. User isn't employer");
            }
            user.Employer.Employees.Remove(employee);
            _unitOfWork.SaveChanges();
        }

        public override List<Advisor> GetAll()
        {
            return _unitOfWork.AdvisorRepository.GetAll();
        }

        public override async Task<List<Advisor>> GetAllAsync()
        {
            return await _unitOfWork.AdvisorRepository.GetAllAsync();
        }

        public override async Task<List<Advisor>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _unitOfWork.AdvisorRepository.GetAllAsync(cancellationToken);
        }

        public override Advisor Get(User user)
        {
            if (user == null )
            {
                throw new ArgumentException("User is null. Wrong parameters");
            }
            return _unitOfWork.AdvisorRepository.FindById(user.UserId);
        }

        public override async Task<Advisor> GetAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentException("User is null. Wrong parameters");
            }
            return await _unitOfWork.AdvisorRepository.FindByIdAsync(user.UserId);
        }

        public override async Task<Advisor> GetAsync(CancellationToken cancellationToken, User user)
        {
            if (user == null)
            {
                throw new ArgumentException("User is null. Wrong parameters");
            }
            return await _unitOfWork.AdvisorRepository.FindByIdAsync(cancellationToken, user.UserId);
        }

        public override void Create(Advisor entity, User user)
        {
            if (entity == null||user==null)
            {
                throw new ArgumentException("Advisor or user are null. Wrong parameters");
            }
            _unitOfWork.UserRepository.AddAdvisorAsync(entity, user.UserName);
            _unitOfWork.SaveChanges();
        }

        public override Task<int> CreateAsync(Advisor entity, User user)
        {
            if (entity == null || user == null)
            {
                throw new ArgumentException("Advisor or user are null. Wrong parameters");
            }

            _unitOfWork.UserRepository.AddAdvisorAsync(entity, user.UserName);
            return _unitOfWork.SaveChangesAsync();
        }

        public override Task<int> CreateAsync(CancellationToken cancellationToken, Advisor entity, User user)
        {
            if (entity == null || user == null)
            {
                throw new ArgumentException("Advisor or user are null. Wrong parameters");
            }

            _unitOfWork.UserRepository.AddAdvisorAsync(entity, user.UserName);
            return  _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public override void Update(Advisor entity)
        {
            if (entity == null )
            {
                throw new ArgumentException("Advisor is null. Wrong parameters");
            }

            _unitOfWork.AdvisorRepository.Update(entity);
            _unitOfWork.SaveChanges();

        }

        public override Task<int> UpdateAsync(Advisor entity)
        {
            if (entity == null)
            {
                throw new ArgumentException("Advisor is null. Wrong parameters");
            }

            _unitOfWork.AdvisorRepository.Update(entity);
            return _unitOfWork.SaveChangesAsync();
        }

        public override Task<int> UpdateAsync(CancellationToken cancellationToken, Advisor entity)
        {
            if (entity == null)
            {
                throw new ArgumentException("Advisor is null. Wrong parameters");
            }

            _unitOfWork.AdvisorRepository.Update(entity);
            return _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public override void Delete(User user)
        {
            if (user == null)
            {
                throw new ArgumentException("user is null. Wrong parameters");
            }

            _unitOfWork.UserRepository.Remove(user);
             _unitOfWork.SaveChanges();
        }

        public override Task<int> DeleteAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentException("user is null. Wrong parameters");
            }

            _unitOfWork.UserRepository.Remove(user);
            return _unitOfWork.SaveChangesAsync();
        }

        public override Task<int> DeleteAsync(CancellationToken cancellationToken, User user)
        {
            if (user == null)
            {
                throw new ArgumentException("user is null. Wrong parameters");
            }

            _unitOfWork.UserRepository.Remove(user);
            return _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}