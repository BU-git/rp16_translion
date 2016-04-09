using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IDAL.Interfaces;
using IDAL.Models;

namespace BLL.Services.PersonageService
{
    public class EmployerManager : PersonManager<Employer>
    {
        public EmployerManager(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public override void Update(Employer entity)
        {
            if (entity == null)
            {
                throw new ArgumentException("Employer is null. Wrong parameters");
            }
            _unitOfWork.EmployerRepository.Update(entity);
            _unitOfWork.SaveChanges();
        }

        public override async void UpdateAsync(Employer entity)
        {
            if (entity == null)
            {
                throw new ArgumentException("Employer is null. Wrong parameters");
            }
            _unitOfWork.EmployerRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public override async void UpdateAsync(CancellationToken cancellationToken, Employer entity)
        {
            if (entity == null)
            {
                throw new ArgumentException("Employer is null. Wrong parameters");
            }
            _unitOfWork.EmployerRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
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

        public override async void DeleteAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentException("user is null. Wrong parameters");
            }
            _unitOfWork.UserRepository.Remove(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public override async void DeleteAsync(CancellationToken cancellationToken, User user)
        {
            if (user == null)
            {
                throw new ArgumentException("user is null. Wrong parameters");
            }
            _unitOfWork.UserRepository.Remove(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public override void DeleteEmployee(User user, Employee employee)
        {
            if (user == null || employee == null)
            {
                throw new ArgumentException("User is null, employee is absent. Wrong parameters");
            }
            if (user.Employer == null)
            {
                throw new ArgumentException("Only employer can have employees. User isn't employer");
            }
            //TODO Check this!!!
            Employee emp = user.Employer.Employees.FirstOrDefault(x => x.EmployeeId == employee.EmployeeId);
            emp.IsDeleted = true;
            this.UpdateEmployeeAsync(emp);
            _unitOfWork.SaveChanges();
        }

        #region GetAll()

        public override List<Employer> GetAll()
        {
            return _unitOfWork.EmployerRepository.GetAll();
        }

        public override async Task<List<Employer>> GetAllAsync()
        {
            return await _unitOfWork.EmployerRepository.GetAllAsync();
        }

        public override async Task<List<Employer>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _unitOfWork.EmployerRepository.GetAllAsync(cancellationToken);
        }

        #endregion

        #region Get

        public override Employer Get(User user)
        {
            if (user == null)
            {
                throw new ArgumentException("user is null. Wrong parameters");
            }
            return _unitOfWork.EmployerRepository.FindById(user.UserId);
        }

        public override async Task<Employer> GetAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentException("user is null. Wrong parameters");
            }
            return await _unitOfWork.EmployerRepository.FindByIdAsync(user.UserId);
        }

        public override async Task<Employer> GetAsync(CancellationToken cancellationToken, User user)
        {
            if (user == null)
            {
                throw new ArgumentException("user is null. Wrong parameters");
            }
            return await _unitOfWork.EmployerRepository.FindByIdAsync(cancellationToken, user.UserId);
        }

        #endregion

        #region Create

        public override void Create(Employer entity, User user)
        {
            _unitOfWork.UserRepository.AddEmployerAsync(entity, user.UserName);
        }

        public override async void CreateAsync(Employer entity, User user)
        {
            _unitOfWork.UserRepository.AddEmployerAsync(entity, user.UserName);
            await _unitOfWork.SaveChangesAsync();
        }

        public override async void CreateAsync(CancellationToken cancellationToken, Employer entity, User user)
        {
            _unitOfWork.UserRepository.AddEmployerAsync(entity, user.UserName);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        #endregion
    }
}