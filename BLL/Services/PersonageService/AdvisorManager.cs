using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IDAL;
using IDAL.Interfaces;
using IDAL.Models;

namespace BLL.Services.PersonageService
{
    public class AdvisorManager : PersonManager<Advisor>
    {
        public AdvisorManager(IUnitOfWork unitOfWork) : base(unitOfWork)
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
                UnitOfWork.EmployeeRepository.Remove(employee);
                int result = await UnitOfWork.SaveChanges();
                if (result > 0)
                {
                    return WorkResult.Success();
                }
                return WorkResult.Failed("SaveChanges result is 0");
            }
            catch (Exception ex)
            {
                return WorkResult.Failed(ex.Message);
            }
        }

        #region Get all advisors

        public override async Task<List<Advisor>> GetAll()
        {
            return await UnitOfWork.AdvisorRepository.GetAll();
        }

        public override async Task<List<Advisor>> GetAll(CancellationToken cancellationToken)
        {
            return await UnitOfWork.AdvisorRepository.GetAll(cancellationToken);
        }

        #endregion

        #region Get concrete advisor

        public override async Task<Advisor> Get(Guid userId)
        {
            if (userId != Guid.Empty)
            {
                return await UnitOfWork.AdvisorRepository.FindById(userId);
            }
            return null;
        }

        public override async Task<Advisor> Get(CancellationToken cancellationToken, Guid userId)
        {
            if (userId != Guid.Empty)
            {
                return await UnitOfWork.AdvisorRepository.FindById(cancellationToken,userId);
            }
            return null;
        }

        public override async Task<Advisor> Get(string userName)
        {
            if (userName != null)
            {
                User user = await UnitOfWork.UserRepository.FindByUserName(userName);
                return await UnitOfWork.AdvisorRepository.FindById(user.UserId);
            }
            return null;
        }

        public override async Task<Advisor> Get(CancellationToken cancellationToken, string userName)
        {
            if (userName != null)
            {
                User user = await UnitOfWork.UserRepository.FindByUserName(userName);
                return await UnitOfWork.AdvisorRepository.FindById(user.UserId);
            }
            return null;
        }

        #endregion

        #region Create advisor

        public override async Task<WorkResult> Create(Advisor entity)
        {
            if (entity == null)
            {
                return WorkResult.Failed("Wrong param. Entity is null");
            }
            try
            {
                UnitOfWork.UserRepository.AddAdvisor(entity);
                int result = await UnitOfWork.SaveChanges();
                if (result > 0)
                {
                    return WorkResult.Success();
                }
                return WorkResult.Failed("SaveChanges result is 0");
            }
            catch (Exception ex)
            {
                return WorkResult.Failed(ex.Message);
            }
        }

        public override async Task<WorkResult> Create(CancellationToken cancellationToken, Advisor entity)
        {
            if (entity == null)
            {
                return WorkResult.Failed("Wrong param. Entity is null");
            }
            try
            {
                UnitOfWork.UserRepository.AddAdvisor(entity);
                int result = await UnitOfWork.SaveChanges(cancellationToken);
                if (result > 0)
                {
                    return WorkResult.Success();
                }
                return WorkResult.Failed("SaveChanges result is 0");
            }
            catch (Exception ex)
            {
                return WorkResult.Failed(ex.Message);
            }
        }

        #endregion

        #region Update advisor

        public override async Task<WorkResult> Update(Advisor entity)
        {
            if (entity == null)
            {
                WorkResult.Failed("Wrong param.Entity is null");
            }
            try
            {
                UnitOfWork.AdvisorRepository.Update(entity);
                int result = await UnitOfWork.SaveChanges();
                if (result > 0)
                {
                    return WorkResult.Success();
                }
                return WorkResult.Failed("SaveChanges result is 0");
            }
            catch (Exception ex)
            {
                return WorkResult.Failed(ex.Message);
            }
        }

        public override async Task<WorkResult> Update(CancellationToken cancellationToken, Advisor entity)
        {
            if (entity == null)
            {
                WorkResult.Failed("Wrong param.Entity is null");
            }
            try
            {
                UnitOfWork.AdvisorRepository.Update(entity);
                int result = await UnitOfWork.SaveChanges(cancellationToken);
                if (result > 0)
                {
                    return WorkResult.Success();
                }
                return WorkResult.Failed("SaveChanges result is 0");
            }
            catch (Exception ex)
            {
                return WorkResult.Failed(ex.Message);
            }
        }


        #endregion

        #region Delete advisor


        public override async Task<WorkResult> Delete(Advisor entity)
        {
            if (entity != null)
            {
                return await Delete(entity.AdvisorId);
            }
            return WorkResult.Failed("Admin cannot be null");
        }

        public override async Task<WorkResult> Delete(CancellationToken cancellationToken, Advisor entity)
        {
            if (entity != null)
            {
                return await Delete(cancellationToken, entity.AdvisorId);
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
                // Check is userId owned Advisor
                Advisor advisor = await UnitOfWork.AdvisorRepository.FindById(userId);
                if (advisor != null)
                {
                    UnitOfWork.UserRepository.Remove(advisor.User);
                    await UnitOfWork.SaveChanges();
                    return WorkResult.Success();
                }
                return WorkResult.Failed("userId isn't owned by Admin");
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
                // Check is userId owned Advisor
                Advisor advisor = await UnitOfWork.AdvisorRepository.FindById(cancellationToken,userId);
                if (advisor != null)
                {
                    UnitOfWork.UserRepository.Remove(advisor.User);
                    await UnitOfWork.SaveChanges(cancellationToken);
                    return WorkResult.Success();
                }
                return WorkResult.Failed("userId isn't owned by Admin");
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
                // Check is userId owned Advisor
                User user = await UnitOfWork.UserRepository.FindByUserName(userName);
                Advisor advisor = await UnitOfWork.AdvisorRepository.FindById(user.UserId);
                if (advisor != null)
                {
                    UnitOfWork.UserRepository.Remove(user);
                    await UnitOfWork.SaveChanges();
                    return WorkResult.Success();
                }
                return WorkResult.Failed("userId isn't owned by Admin");
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
                // Check is userId owned Advisor
                User user = await UnitOfWork.UserRepository.FindByUserName(cancellationToken,userName);
                Advisor advisor = await UnitOfWork.AdvisorRepository.FindById(cancellationToken,user.UserId);
                if (advisor != null)
                {
                    UnitOfWork.UserRepository.Remove(user);
                    await UnitOfWork.SaveChanges(cancellationToken);
                    return WorkResult.Success();
                }
                return WorkResult.Failed("userId isn't owned by Admin");
            }
            catch (Exception ex)
            {
                return WorkResult.Failed(ex.Message);
            }
        }

        #endregion
    }
}