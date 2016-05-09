using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IDAL;
using IDAL.Interfaces;
using IDAL.Models;

namespace BLL.Services.PersonageService
{
    public class EmployerManager : PersonManager<Employer>
    {
        public EmployerManager(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public override async Task<WorkResult> DeleteEmployee(Employee employee)
        {
            if (employee == null)
            {
                return WorkResult.Failed("Employee name null");
            }
            try
            {
                employee.IsDeleted = true;
                UnitOfWork.EmployeeRepository.Update(employee);
                int result = await UnitOfWork.SaveChanges();
                if (result > 0)
                {
                    return WorkResult.Success();
                }
                return WorkResult.Failed("SaveChanges returned result 0");
            }
            catch (Exception ex)
            {
                return WorkResult.Failed(ex.Message);
            }
        }

        #region Get all employers()

        public override async Task<List<Employer>> GetAll()
        {
            return await UnitOfWork.EmployerRepository.GetAll();
        }

        public override async Task<List<Employer>> GetAll(CancellationToken cancellationToken)
        {
            return await UnitOfWork.EmployerRepository.GetAll(cancellationToken);
        }

        #endregion

        #region Get concrete eployer

        public override async Task<Employer> Get(Guid userId)
        {
            if (userId != Guid.Empty)
            {
                return await UnitOfWork.EmployerRepository.FindById(userId);
            }
            return null;
        }

        public override async Task<Employer> Get(CancellationToken cancellationToken, Guid userId)
        {
            if (userId != Guid.Empty)
            {
                return await UnitOfWork.EmployerRepository.FindById(cancellationToken, userId);
            }
            return null;
        }

        public override async Task<Employer> Get(string userName)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                User user = await UnitOfWork.UserRepository.FindByUserName(userName);
                return await UnitOfWork.EmployerRepository.FindById(user.UserId);
            }
            return null;
        }

        public override async Task<Employer> Get(CancellationToken cancellationToken, string userName)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                User user = await UnitOfWork.UserRepository.FindByUserName(cancellationToken, userName);
                return await UnitOfWork.EmployerRepository.FindById(cancellationToken, user.UserId);
            }
            return null;
        }

        #endregion

        #region Create employer

        public override async Task<WorkResult> Create(Employer entity)
        {
            if (entity == null)
            {
                return WorkResult.Failed("Wrong param. Entity is null");
            }
            try
            {
                UnitOfWork.UserRepository.AddEmployer(entity);
                int result = await UnitOfWork.SaveChanges();
                if (result > 0)
                {
                    return WorkResult.Success();
                }
                return WorkResult.Failed("SaveChanges returned result 0");
            }
            catch (Exception ex)
            {
                return WorkResult.Failed(ex.Message);
            }
        }

        public override async Task<WorkResult> Create(CancellationToken cancellationToken, Employer entity)
        {
            if (entity == null)
            {
                return WorkResult.Failed("Wrong param. Entity is null");
            }
            try
            {
                UnitOfWork.UserRepository.AddEmployer(entity);
                int result = await UnitOfWork.SaveChanges(cancellationToken);
                if (result > 0)
                {
                    return WorkResult.Success();
                }
                return WorkResult.Failed("SaveChanges returned result 0");
            }
            catch (Exception ex)
            {
                return WorkResult.Failed(ex.Message);
            }
        }

        #endregion

        #region Update employer

        public override async Task<WorkResult> Update(Employer entity)
        {
            if (entity == null)
            {
                WorkResult.Failed("Wrong param.Entity is null");
            }
            try
            {
                UnitOfWork.EmployerRepository.Update(entity);
                int result = await UnitOfWork.SaveChanges();
                if (result > 0)
                {
                    return WorkResult.Success();
                }
                return WorkResult.Failed("SaveChanges returned result 0");
            }
            catch (Exception ex)
            {
                return WorkResult.Failed(ex.Message);
            }
        }

        public override async Task<WorkResult> Update(CancellationToken cancellationToken, Employer entity)
        {
            if (entity == null)
            {
                WorkResult.Failed("Wrong param.Entity is null");
            }
            try
            {
                UnitOfWork.EmployerRepository.Update(entity);
                int result = await UnitOfWork.SaveChanges(cancellationToken);
                if (result > 0)
                {
                    return WorkResult.Success();
                }
                return WorkResult.Failed("SaveChanges returned result 0");
            }
            catch (Exception ex)
            {
                return WorkResult.Failed(ex.Message);
            }
        }

        #endregion

        #region Delete eployer

        public override async Task<WorkResult> Delete(Employer entity)
        {
            if (entity != null)
            {
                return await Delete(entity.EmployerId);
            }
            return WorkResult.Failed("Admin cannot be null");
        }

        public override async Task<WorkResult> Delete(CancellationToken cancellationToken, Employer entity)
        {
            if (entity != null)
            {
                return await Delete(cancellationToken, entity.EmployerId);
            }
            return WorkResult.Failed("Admin cannot be null");
        }

        public override async Task<WorkResult> Delete(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return WorkResult.Failed("Wrong param.Entity is null");
            }
            try
            {
                // Check is userId owned by Employer
                Employer employer = await UnitOfWork.EmployerRepository.FindById(userId);
                if (employer != null)
                {
                    UnitOfWork.UserRepository.Remove(employer.User);
                    int result = await UnitOfWork.SaveChanges();
                    if (result > 0)
                    {
                        return WorkResult.Success();
                    }
                    return WorkResult.Failed("SaveChanges returned result 0");
                }
                return WorkResult.Failed("userId isn't owned by Employer");
            }
            catch (Exception ex)
            {
                return WorkResult.Failed(ex.Message);
            }
        }

        public override async Task<WorkResult> Delete(CancellationToken cancellationToken, Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return WorkResult.Failed("Wrong param.Entity is null");
            }
            try
            {
                // Check is userId owned by Employer
                Employer employer = await UnitOfWork.EmployerRepository.FindById(cancellationToken, userId);
                if (employer != null)
                {
                    UnitOfWork.UserRepository.Remove(employer.User);
                    int result = await UnitOfWork.SaveChanges(cancellationToken);
                    if (result > 0)
                    {
                        return WorkResult.Success();
                    }
                    return WorkResult.Failed("SaveChanges returned result 0");
                }
                return WorkResult.Failed("userId isn't owned by Employer");
            }
            catch (Exception ex)
            {
                return WorkResult.Failed(ex.Message);
            }
        }

        public override async Task<WorkResult> Delete(string userName)
        {
            if (userName == null)
            {
                return WorkResult.Failed("Wrong param.Entity is null");
            }
            try
            {
                // Check is userId owned by Employer
                User user = await UnitOfWork.UserRepository.FindByUserName(userName);
                if (user.Employer != null)
                {
                    UnitOfWork.UserRepository.Remove(user);
                    int result = await UnitOfWork.SaveChanges();
                    if (result > 0)
                    {
                        return WorkResult.Success();
                    }
                    return WorkResult.Failed("SaveChanges returned result 0");
                }
                return WorkResult.Failed("userId isn't owned by Employer");
            }
            catch (Exception ex)
            {
                return WorkResult.Failed(ex.Message);
            }
        }

        public override async Task<WorkResult> Delete(CancellationToken cancellationToken, string userName)
        {
            if (userName == null)
            {
                return WorkResult.Failed("Wrong param.Entity is null");
            }
            try
            {
                // Check is userId owned by Employer
                User user = await UnitOfWork.UserRepository.FindByUserName(cancellationToken, userName);
                if (user.Employer != null)
                {
                    UnitOfWork.UserRepository.Remove(user);
                    int result = await UnitOfWork.SaveChanges(cancellationToken);
                    if (result > 0)
                    {
                        return WorkResult.Success();
                    }
                    return WorkResult.Failed("SaveChanges returned result 0");
                }
                return WorkResult.Failed("userId isn't owned by Employer");
            }
            catch (Exception ex)
            {
                return WorkResult.Failed(ex.Message);
            }
        }

        #endregion
    }
}