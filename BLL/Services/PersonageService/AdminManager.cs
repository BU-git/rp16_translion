using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IDAL;
using IDAL.Interfaces;
using IDAL.Models;

namespace BLL.Services.PersonageService
{
    public class AdminManager : PersonManager<Admin>
    {
        public AdminManager(IUnitOfWork unitOfWork) : base(unitOfWork)
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

        #region Get all admins

        public override Task<List<Admin>> GetAll()
        {
            return UnitOfWork.AdminRepository.GetAll();
        }

        public override Task<List<Admin>> GetAll(CancellationToken cancellationToken)
        {
            return UnitOfWork.AdminRepository.GetAll(cancellationToken);
        }

        #endregion

        #region Get concrete admin

        public override async Task<Admin> Get(Guid userId)
        {
            if (userId != Guid.Empty)
            {
                return await UnitOfWork.AdminRepository.FindById(userId);
            }
            return null;
        }

        public override async Task<Admin> Get(CancellationToken cancellationToken, Guid userId)
        {
            if (userId != Guid.Empty)
            {
                return await UnitOfWork.AdminRepository.FindById(userId);
            }
            return null;
        }

        public override async Task<Admin> Get(string userName)
        {
            if (userName != null)
            {
                User user = await UnitOfWork.UserRepository.FindByUserName(userName);
                return await UnitOfWork.AdminRepository.FindById(user.UserId);
            }
            return null;
        }

        public override async Task<Admin> Get(CancellationToken cancellationToken, string userName)
        {
            if (userName != null)
            {
                User user = await UnitOfWork.UserRepository.FindByUserName(cancellationToken, userName);
                return await UnitOfWork.AdminRepository.FindById(cancellationToken, user.UserId);
            }
            return null;
        }

        #endregion

        #region Create admin

        public override async Task<WorkResult> Create(Admin entity)
        {
            if (entity == null)
            {
                return WorkResult.Failed("Wrong param. Entity is null");
            }
            try
            {
                UnitOfWork.UserRepository.AddAdmin(entity);
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

        public override async Task<WorkResult> Create(CancellationToken cancellationToken, Admin entity)
        {
            if (entity == null)
            {
                return WorkResult.Failed("Wrong param. Entity is null");
            }
            try
            {
                UnitOfWork.UserRepository.AddAdmin(entity);
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

        #endregion

        #region Update admin

        public override async Task<WorkResult> Update(Admin entity)
        {
            if (entity == null)
            {
                WorkResult.Failed("Wrong param.Entity is null");
            }
            try
            {
                UnitOfWork.AdminRepository.Update(entity);
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

        public override async Task<WorkResult> Update(CancellationToken cancellationToken, Admin entity)
        {
            if (entity == null)
            {
                WorkResult.Failed("Wrong param.Entity is null");
            }
            try
            {
                UnitOfWork.AdminRepository.Update(entity);
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

        #region Delete admin

        public override async Task<WorkResult> Delete(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return WorkResult.Failed("Wrong param.Entity is null");
            }
            try
            {
                User user = await UnitOfWork.UserRepository.FindById(userId);
                UnitOfWork.UserRepository.Remove(user);
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

        public override async Task<WorkResult> Delete(CancellationToken cancellationToken, Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return WorkResult.Failed("Wrong param.Entity is null");
            }
            try
            {
                User user = await UnitOfWork.UserRepository.FindById(cancellationToken,userId);
                UnitOfWork.UserRepository.Remove(user);
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

        public override async Task<WorkResult> Delete(string userName)
        {
            if (userName == null)
            {
                return WorkResult.Failed("Wrong param.Entity is null");
            }
            try
            {
                User user = await UnitOfWork.UserRepository.FindByUserName(userName);
                UnitOfWork.UserRepository.Remove(user);
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

        public override async Task<WorkResult> Delete(CancellationToken cancellationToken, string userName)
        {
            if (userName == null)
            {
                return WorkResult.Failed("Wrong param.Entity is null");
            }
            try
            {
                User user = await UnitOfWork.UserRepository.FindByUserName(cancellationToken, userName);
                UnitOfWork.UserRepository.Remove(user);
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
    }
}