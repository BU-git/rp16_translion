using System;
using System.Collections.Generic;
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
            _unitOfWork.EmployerRepository.Update(entity);
            _unitOfWork.SaveChanges();
        }

        public override async void UpdateAsync(Employer entity)
        {
            _unitOfWork.EmployerRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public override async void UpdateAsync(CancellationToken cancellationToken, Employer entity)
        {
            _unitOfWork.EmployerRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public override void Delete(User user)
        {
            _unitOfWork.UserRepository.Remove(user);
            _unitOfWork.SaveChanges();
        }

        public override async void DeleteAsync(User user)
        {
            _unitOfWork.UserRepository.Remove(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public override async void DeleteAsync(CancellationToken cancellationToken, User user)
        {
            _unitOfWork.UserRepository.Remove(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public override bool DeleteEmployee(Employee employee)
        {
            throw new NotImplementedException();
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
            return _unitOfWork.EmployerRepository.FindById(user.UserId);
        }

        public override async Task<Employer> GetAsync(User user)
        {
            return await _unitOfWork.EmployerRepository.FindByIdAsync(user.UserId);
        }

        public override async Task<Employer> GetAsync(CancellationToken cancellationToken, User user)
        {
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