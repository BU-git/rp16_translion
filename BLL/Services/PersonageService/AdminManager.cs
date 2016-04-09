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


<<<<<<< HEAD
        public override void DeleteEmployee(User user, Employee employee)
=======
        public override async void DeleteEmployee(Employee employee, User user)
>>>>>>> 2c5916570140ab313ca98c9e458212bfdfe9f453
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            if (user.Employer == null)
                throw new ArgumentException("User is not employer", nameof(user));

            if (!user.Employer.Employees.Contains(employee))
                throw new InvalidOperationException("Can't delete employee from current user, because he hasn't this employee");

            _unitOfWork.EmployeeRepository.Remove(employee);
            await _unitOfWork.SaveChangesAsync();
        }

        #region Get all admins
        public override List<Admin> GetAll()
            => _unitOfWork.AdminRepository.GetAll();

        public override Task<List<Admin>> GetAllAsync()
            => _unitOfWork.AdminRepository.GetAllAsync();

        public override Task<List<Admin>> GetAllAsync(CancellationToken cancellationToken)
            => _unitOfWork.AdminRepository.GetAllAsync(cancellationToken);
        #endregion

        #region Get concrete admin
        public override Admin Get(User user)
            => _unitOfWork.AdminRepository.FindById(user.UserId);

        public override Task<Admin> GetAsync(User user)
            => _unitOfWork.AdminRepository.FindByIdAsync(user.UserId);

        public override Task<Admin> GetAsync(CancellationToken cancellationToken, User user)
            => _unitOfWork.AdminRepository.FindByIdAsync(cancellationToken, user.UserId);
        #endregion

        #region Create concrete admin
        public override void Create(Admin entity, User user)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _unitOfWork.UserRepository.AddAdminAsync(entity, user.UserId);
            _unitOfWork.SaveChanges();
        }

        public override async void CreateAsync(Admin entity, User user)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _unitOfWork.UserRepository.AddAdminAsync(entity, user.UserId);
            await _unitOfWork.SaveChangesAsync();
        }

        public override async void CreateAsync(CancellationToken cancellationToken, Admin entity, User user)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _unitOfWork.UserRepository.AddAdminAsync(entity, user.UserId);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        #endregion

        #region Update concrete admin
        public override void Update(Admin entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _unitOfWork.AdminRepository.Update(entity);
            _unitOfWork.SaveChanges();
        }

        public override async void UpdateAsync(Admin entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _unitOfWork.AdminRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public override async void UpdateAsync(CancellationToken cancellationToken, Admin entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _unitOfWork.AdminRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        #endregion

        #region Delete concrete admin
        public override void Delete(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (user.Admin == null)
                throw new InvalidOperationException("Can't delete user. This user is not admin");

            _unitOfWork.UserRepository.Remove(user);
            _unitOfWork.SaveChanges();
        }

        public override void DeleteAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (user.Admin == null)
                throw new InvalidOperationException("Can't delete user. This user is not admin");

            _unitOfWork.UserRepository.Remove(user);
            _unitOfWork.SaveChangesAsync();
        }

        public override void DeleteAsync(CancellationToken cancellationToken, User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (user.Admin == null)
                throw new InvalidOperationException("Can't delete user. This user is not admin");

            _unitOfWork.UserRepository.Remove(user);
            _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        #endregion
    }
}