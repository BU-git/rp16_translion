using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IDAL.Interfaces;
using IDAL.Interfaces.Managers;
using IDAL.Models;

namespace BLL.Services.PersonageService
{
    public class EmployerManager : BaseManager, IPersonageManager<Employer>
    {
        public EmployerManager(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public void Update(Employer entity)
        {
            _unitOfWork.EmployerRepository.Update(entity);
            _unitOfWork.SaveChanges();
        }

        public async void UpdateAsync(Employer entity)
        {
            _unitOfWork.EmployerRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async void UpdateAsync(CancellationToken cancellationToken, Employer entity)
        {
            _unitOfWork.EmployerRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public void Delete(User user)
        {
            _unitOfWork.UserRepository.Remove(user);
            _unitOfWork.SaveChanges();
        }

        public async void DeleteAsync(User user)
        {
            _unitOfWork.UserRepository.Remove(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async void DeleteAsync(CancellationToken cancellationToken, User user)
        {
            _unitOfWork.UserRepository.Remove(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public override bool DeleteEmployee(Employee employee)
        {
            throw new NotImplementedException();
        }

        #region GetAll()

        public List<Employer> GetAll()
        {
            return _unitOfWork.EmployerRepository.GetAll();
        }

        public async Task<List<Employer>> GetAllAsync()
        {
            return await _unitOfWork.EmployerRepository.GetAllAsync();
        }

        public async Task<List<Employer>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _unitOfWork.EmployerRepository.GetAllAsync(cancellationToken);
        }

        #endregion

        #region Get

        public Employer Get(User user)
        {
            return _unitOfWork.EmployerRepository.FindById(user.UserId);
        }

        public async Task<Employer> GetAsync(User user)
        {
            return await _unitOfWork.EmployerRepository.FindByIdAsync(user.UserId);
        }

        public async Task<Employer> GetAsync(CancellationToken cancellationToken, User user)
        {
            return await _unitOfWork.EmployerRepository.FindByIdAsync(cancellationToken, user.UserId);
        }

        #endregion

        #region Create

        public void Create(Employer entity, User user)
        {
            _unitOfWork.UserRepository.AddEmployerAsync(entity, user.UserName);
        }

        public async void CreateAsync(Employer entity, User user)
        {
            _unitOfWork.UserRepository.AddEmployerAsync(entity, user.UserName);
            await _unitOfWork.SaveChangesAsync();
        }

        public async void CreateAsync(CancellationToken cancellationToken, Employer entity, User user)
        {
            _unitOfWork.UserRepository.AddEmployerAsync(entity, user.UserName);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        #endregion
    }
}