using System;
using System.Threading;
using System.Threading.Tasks;
using IDAL.Interfaces.Repositories;

namespace IDAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        #region Properties

        IExternalLoginRepository ExternalLoginRepository { get; }
        IRoleRepository RoleRepository { get; }
        IUserRepository UserRepository { get; }
        IEmployerRepository EmployerRepository { get; }
        IAdminRepository AdminRepository { get; }
        IEmployeeRepository EmployeeRepository { get; }
        IAdvisorRepository AdvisorRepository { get; }
        IAnswerRepository AnswerRepository { get; }
        IAlertRepository AlertRepository { get; }
        IPageRepository PageRepository { get; }
        IQuestionRepository QuestionRepository { get; }

        #endregion

        #region Methods

        int SaveChanges();
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        #endregion
    }
}