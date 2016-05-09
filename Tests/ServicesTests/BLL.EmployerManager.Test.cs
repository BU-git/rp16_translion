using BLL.Services.PersonageService;
using IDAL.Interfaces;
using IDAL.Interfaces.IRepositories;
using IDAL.Models;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace Tests.ServicesTests
{
    public class BLL_EmployerManager_Test
    {
        #region Get tests
        [Fact]
        public void Get_returns_null_if_id_is_not_valid()
        {
            //arrange
            var uow = Mock.Of<IUnitOfWork>();
            var manager = new EmployerManager(uow);
            //act
            var result = manager.Get(Guid.Empty).Result;
            //assert
            Assert.Null(result);
        }

        [Fact]
        public void Get_returns_null_if_username_is_not_valid()
        {
            //arrange
            var uow = Mock.Of<IUnitOfWork>();
            var manager = new EmployerManager(uow);
            //act
            var result = manager.Get(null).Result;
            //assert
            Assert.Null(result);
        }

        [Fact]
        public void Get_succeeded_if_id_is_valid()
        {
            //arrange
            var uow = new Mock<IUnitOfWork>();
            var repo = new Mock<IEmployerRepository>();

            repo.Setup(r => r.FindById(It.Is<Guid>(g => g != Guid.Empty)))
                .ReturnsAsync(new Employer())
                .Verifiable();

            uow.Setup(u => u.EmployerRepository)
                .Returns(repo.Object)
                .Verifiable();

            var manager = new EmployerManager(uow.Object);
            //act
            var result = manager.Get(Guid.NewGuid()).Result;
            //assert
            repo.Verify();
            uow.Verify();
            Assert.NotNull(result);
        }

        [Fact]
        public void Get_succeeded_if_username_is_valid()
        {
            //arrange
            var uow = new Mock<IUnitOfWork>();
            var repo = new Mock<IUserRepository>();
            var admRepo = new Mock<IEmployerRepository>();

            repo.Setup(r => r.FindByUserName(It.IsNotNull<string>()))
                .ReturnsAsync(new User { UserId = Guid.NewGuid() })
                .Verifiable();

            admRepo.Setup(a => a.FindById(It.Is<Guid>(g => g != Guid.Empty)))
                .ReturnsAsync(new Employer())
                .Verifiable();

            uow.Setup(u => u.EmployerRepository)
                .Returns(admRepo.Object)
                .Verifiable();

            uow.Setup(u => u.UserRepository)
                .Returns(repo.Object)
                .Verifiable();

            var manager = new EmployerManager(uow.Object);
            //act
            var result = manager.Get("name").Result;
            //assert
            repo.Verify();
            admRepo.Verify();
            uow.Verify();
            Assert.NotNull(result);
        }
        #endregion

        #region Create tests
        [Fact]
        public void CreateEmployer_failed_if_Employer_is_not_valid()
        {
            //arrange
            var uow = Mock.Of<IUnitOfWork>();
            var manager = new EmployerManager(uow);
            //act
            var result = manager.Create(null).Result;
            //assert
            Assert.False(result.Succeeded);
        }

        [Fact]
        public void CreateEmployer_succeeded_if_Employer_is_valid()
        {
            //arrange
            var Employers = new List<Employer>();
            var uow = new Mock<IUnitOfWork>();
            var repo = new Mock<IUserRepository>();

            repo.Setup(r => r.AddEmployer(It.IsNotNull<Employer>()))
                .Callback<Employer>(a => Employers.Add(a))
                .Verifiable();

            uow.Setup(u => u.SaveChanges())
                .ReturnsAsync(1)
                .Verifiable();

            uow.Setup(u => u.UserRepository)
                .Returns(repo.Object)
                .Verifiable();

            var manager = new EmployerManager(uow.Object);
            //act
            var result = manager.Create(new Employer()).Result;
            //assert
            repo.Verify();
            uow.Verify();
            Assert.True(result.Succeeded);
            Assert.Equal(1, Employers.Count);
        }
        #endregion

        #region Update tests
        [Fact]
        public void UpdateEmployer_failed_if_Employer_is_not_valid()
        {
            //arrange
            var uow = Mock.Of<IUnitOfWork>();
            var manager = new EmployerManager(uow);
            //act
            var result = manager.Update(null).Result;
            //assert
            Assert.False(result.Succeeded);
        }

        [Fact]
        public void UpdateEmployer_succeeded_if_Employer_is_valid()
        {
            //arrange
            var uow = new Mock<IUnitOfWork>();
            var repo = new Mock<IEmployerRepository>();

            repo.Setup(r => r.Update(It.IsNotNull<Employer>()))
                .Verifiable();

            uow.Setup(u => u.SaveChanges())
                .ReturnsAsync(1)
                .Verifiable();

            uow.Setup(u => u.EmployerRepository)
                .Returns(repo.Object)
                .Verifiable();

            var manager = new EmployerManager(uow.Object);
            //act
            var result = manager.Update(new Employer()).Result;
            //assert
            repo.Verify();
            uow.Verify();
            Assert.True(result.Succeeded);
        }
        #endregion

        #region Delete tests
        [Fact]
        public void DeleteEmployer_failed_if_id_is_not_valid()
        {
            //arrange
            var uow = Mock.Of<IUnitOfWork>();
            var manager = new EmployerManager(uow);
            //act
            var result = manager.Delete(Guid.Empty).Result;
            //assert
            Assert.False(result.Succeeded);
        }

        [Fact]
        public void DeleteEmployer_failed_if_Employer_is_null()
        {
            //arrange
            var uow = Mock.Of<IUnitOfWork>();
            var manager = new EmployerManager(uow);
            //act
            var result = manager.Delete((Employer)null).Result;
            //assert
            Assert.False(result.Succeeded);
        }

        [Fact]
        public void DeleteEmployer_failed_if_name_is_null()
        {
            //arrange
            var uow = Mock.Of<IUnitOfWork>();
            var manager = new EmployerManager(uow);
            //act
            var result = manager.Delete((String)null).Result;
            //assert
            Assert.False(result.Succeeded);
        }

        [Fact]
        public void DeleteEmployer_succeeded_if_id_is_valid()
        {
            //arrange
            var EmployerToDelete = new Employer { User = new User() };
            var Employers = new List<Employer> { EmployerToDelete };
            var uow = new Mock<IUnitOfWork>();
            var repo = new Mock<IEmployerRepository>();
            var usrRepo = new Mock<IUserRepository>();

            usrRepo.Setup(u => u.Remove(It.IsNotNull<User>()))
                .Callback<User>(a => Employers.Remove(EmployerToDelete))
                .Verifiable();

            repo.Setup(r => r.FindById(It.Is<Guid>(g => g != Guid.Empty)))
                .ReturnsAsync(EmployerToDelete)
                .Verifiable();

            uow.Setup(u => u.SaveChanges())
                .ReturnsAsync(1)
                .Verifiable();

            uow.Setup(u => u.UserRepository)
                .Returns(usrRepo.Object)
                .Verifiable();

            uow.Setup(u => u.EmployerRepository)
                .Returns(repo.Object)
                .Verifiable();

            var manager = new EmployerManager(uow.Object);
            //act
            var result = manager.Delete(Guid.NewGuid()).Result;
            //assert
            usrRepo.Verify();
            repo.Verify();
            uow.Verify();
            Assert.True(result.Succeeded);
            Assert.Equal(0, Employers.Count);
        }

        [Fact]
        public void DeleteEmployer_succeeded_if_name_is_valid()
        {
            //arrange
            var EmployerToDelete = new Employer { User = new User() };
            var Employers = new List<Employer> { EmployerToDelete };
            var uow = new Mock<IUnitOfWork>();
            var repo = new Mock<IEmployerRepository>();
            var usrRepo = new Mock<IUserRepository>();

            usrRepo.Setup(u => u.Remove(It.IsNotNull<User>()))
                .Callback<User>(a => Employers.Remove(EmployerToDelete))
                .Verifiable();

            usrRepo.Setup(u => u.FindByUserName(It.IsNotNull<string>()))
                .ReturnsAsync(new User { UserId = Guid.NewGuid() })
                .Verifiable();

            repo.Setup(r => r.FindById(It.Is<Guid>(g => g != Guid.Empty)))
                .ReturnsAsync(EmployerToDelete)
                .Verifiable();

            uow.Setup(u => u.SaveChanges())
                .ReturnsAsync(1)
                .Verifiable();

            uow.Setup(u => u.UserRepository)
                .Returns(usrRepo.Object)
                .Verifiable();

            uow.Setup(u => u.EmployerRepository)
                .Returns(repo.Object)
                .Verifiable();

            var manager = new EmployerManager(uow.Object);
            //act
            var result = manager.Delete("asdasd").Result;
            //assert
            usrRepo.Verify();
            repo.Verify();
            uow.Verify();
            Assert.True(result.Succeeded);
            Assert.Equal(0, Employers.Count);
        }
        #endregion
    }
}
