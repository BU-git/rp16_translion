using System.Threading;
using System.Threading.Tasks;
using DAL.Repositories;
using IDAL.Interfaces;
using IDAL.Interfaces.Repositories;

namespace DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        #region IDisposable Members

        public void Dispose()
        {
            _externalLoginRepository = null;
            _roleRepository = null;
            _userRepository = null;
            _employerRepository = null;
            _adminRepository = null;
            _employeeRepository = null;
            _advisorRepository = null;
            _alertRepository = null;
            _context.Dispose();
        }

        #endregion

        #region Fields

        private readonly ApplicationDbContext _context;
        private IExternalLoginRepository _externalLoginRepository;
        private IRoleRepository _roleRepository;
        private IUserRepository _userRepository;
        private IEmployerRepository _employerRepository;
        private IAdminRepository _adminRepository;
        private IEmployeeRepository _employeeRepository;
        private IAdvisorRepository _advisorRepository;
        private IAlertRepository _alertRepository;
        private IAnswerRepository _answerRepository;
        private IPageRepository _pageRepository;
        private IQuestionRepository _questionRepository;

        #endregion

        #region Constructors

        public UnitOfWork() : this("DefaultConnection")
        {
        }

        public UnitOfWork(string nameOrConnectionString)
        {
            _context = new ApplicationDbContext(nameOrConnectionString);
        }

        #endregion

        #region IUnitOfWork Members

        public IExternalLoginRepository ExternalLoginRepository
        {
            get
            {
                return _externalLoginRepository ?? (_externalLoginRepository = new ExternalLoginRepository(_context));
            }
        }

        public IRoleRepository RoleRepository
        {
            get { return _roleRepository ?? (_roleRepository = new RoleRepository(_context)); }
        }

        public IUserRepository UserRepository
        {
            get { return _userRepository ?? (_userRepository = new UserRepository(_context)); }
        }

        public IEmployerRepository EmployerRepository
        {
            get { return _employerRepository ?? (_employerRepository = new EmployerRepository(_context)); }
        }

        public IAdminRepository AdminRepository
        {
            get { return _adminRepository ?? (_adminRepository = new AdminRepository(_context)); }
        }

        public IEmployeeRepository EmployeeRepository
        {
            get { return _employeeRepository ?? (_employeeRepository = new EmployeeRepository(_context)); }
        }

        public IAdvisorRepository AdvisorRepository
        {
            get { return _advisorRepository ?? (_advisorRepository = new AdvisorRepository(_context)); }
        }

        public IAlertRepository AlertRepository
        {
            get { return _alertRepository ?? (_alertRepository = new AlertRepository(_context)); }
        }

        public IPageRepository PageRepository
        {
            get { return _pageRepository ?? (_pageRepository = new PageRepository(_context)); }
        }

        public IAnswerRepository AnswerRepository
        {
            get { return _answerRepository ?? (_answerRepository = new AnswerRepository(_context)); }
        }

        public IQuestionRepository QuestionRepository
        {
            get { return _questionRepository ?? (_questionRepository = new QuestionRepository(_context)); }
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }

        #endregion
    }
}