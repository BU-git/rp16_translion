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
    public class BLL_AdcisorManager_Test
    {
        #region Get tests
        [Fact]
        public void Get_returns_null_if_id_is_not_valid()
        {
            //arrange
            var uow = Mock.Of<IUnitOfWork>();
            var manager = new AdvisorManager(uow);
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
            var manager = new AdvisorManager(uow);
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
            var repo = new Mock<IAdvisorRepository>();

            repo.Setup(r => r.FindById(It.Is<Guid>(g => g != Guid.Empty)))
                .ReturnsAsync(new Advisor())
                .Verifiable();

            uow.Setup(u => u.AdvisorRepository)
                .Returns(repo.Object)
                .Verifiable();

            var manager = new AdvisorManager(uow.Object);
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
            var admRepo = new Mock<IAdvisorRepository>();

            repo.Setup(r => r.FindByUserName(It.IsNotNull<string>()))
                .ReturnsAsync(new User { UserId = Guid.NewGuid() })
                .Verifiable();

            admRepo.Setup(a => a.FindById(It.Is<Guid>(g => g != Guid.Empty)))
                .ReturnsAsync(new Advisor())
                .Verifiable();

            uow.Setup(u => u.AdvisorRepository)
                .Returns(admRepo.Object)
                .Verifiable();

            uow.Setup(u => u.UserRepository)
                .Returns(repo.Object)
                .Verifiable();

            var manager = new AdvisorManager(uow.Object);
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
        public void CreateAdvisor_failed_if_Advisor_is_not_valid()
        {
            //arrange
            var uow = Mock.Of<IUnitOfWork>();
            var manager = new AdvisorManager(uow);
            //act
            var result = manager.Create(null).Result;
            //assert
            Assert.False(result.Succeeded);
        }

        [Fact]
        public void CreateAdvisor_succeeded_if_Advisor_is_valid()
        {
            //arrange
            var Advisors = new List<Advisor>();
            var uow = new Mock<IUnitOfWork>();
            var repo = new Mock<IUserRepository>();

            repo.Setup(r => r.AddAdvisor(It.IsNotNull<Advisor>()))
                .Callback<Advisor>(a => Advisors.Add(a))
                .Verifiable();

            uow.Setup(u => u.SaveChanges())
                .ReturnsAsync(1)
                .Verifiable();

            uow.Setup(u => u.UserRepository)
                .Returns(repo.Object)
                .Verifiable();

            var manager = new AdvisorManager(uow.Object);
            //act
            var result = manager.Create(new Advisor()).Result;
            //assert
            repo.Verify();
            uow.Verify();
            Assert.True(result.Succeeded);
            Assert.Equal(1, Advisors.Count);
        }
        #endregion

        #region Update tests
        [Fact]
        public void UpdateAdvisor_failed_if_Advisor_is_not_valid()
        {
            //arrange
            var uow = Mock.Of<IUnitOfWork>();
            var manager = new AdvisorManager(uow);
            //act
            var result = manager.Update(null).Result;
            //assert
            Assert.False(result.Succeeded);
        }

        [Fact]
        public void UpdateAdvisor_succeeded_if_Advisor_is_valid()
        {
            //arrange
            var uow = new Mock<IUnitOfWork>();
            var repo = new Mock<IAdvisorRepository>();

            repo.Setup(r => r.Update(It.IsNotNull<Advisor>()))
                .Verifiable();

            uow.Setup(u => u.SaveChanges())
                .ReturnsAsync(1)
                .Verifiable();

            uow.Setup(u => u.AdvisorRepository)
                .Returns(repo.Object)
                .Verifiable();

            var manager = new AdvisorManager(uow.Object);
            //act
            var result = manager.Update(new Advisor()).Result;
            //assert
            repo.Verify();
            uow.Verify();
            Assert.True(result.Succeeded);
        }
        #endregion

        #region Delete tests
        [Fact]
        public void DeleteAdvisor_failed_if_id_is_not_valid()
        {
            //arrange
            var uow = Mock.Of<IUnitOfWork>();
            var manager = new AdvisorManager(uow);
            //act
            var result = manager.Delete(Guid.Empty).Result;
            //assert
            Assert.False(result.Succeeded);
        }

        [Fact]
        public void DeleteAdvisor_failed_if_Advisor_is_null()
        {
            //arrange
            var uow = Mock.Of<IUnitOfWork>();
            var manager = new AdvisorManager(uow);
            //act
            var result = manager.Delete((Advisor)null).Result;
            //assert
            Assert.False(result.Succeeded);
        }

        [Fact]
        public void DeleteAdvisor_failed_if_name_is_null()
        {
            //arrange
            var uow = Mock.Of<IUnitOfWork>();
            var manager = new AdvisorManager(uow);
            //act
            var result = manager.Delete((String)null).Result;
            //assert
            Assert.False(result.Succeeded);
        }

        [Fact]
        public void DeleteAdvisor_succeeded_if_id_is_valid()
        {
            //arrange
            var AdvisorToDelete = new Advisor { User = new User() };
            var Advisors = new List<Advisor> { AdvisorToDelete };
            var uow = new Mock<IUnitOfWork>();
            var repo = new Mock<IAdvisorRepository>();
            var usrRepo = new Mock<IUserRepository>();

            usrRepo.Setup(u => u.Remove(It.IsNotNull<User>()))
                .Callback<User>(a => Advisors.Remove(AdvisorToDelete))
                .Verifiable();

            repo.Setup(r => r.FindById(It.Is<Guid>(g => g != Guid.Empty)))
                .ReturnsAsync(AdvisorToDelete)
                .Verifiable();

            uow.Setup(u => u.SaveChanges())
                .ReturnsAsync(1)
                .Verifiable();

            uow.Setup(u => u.UserRepository)
                .Returns(usrRepo.Object)
                .Verifiable();

            uow.Setup(u => u.AdvisorRepository)
                .Returns(repo.Object)
                .Verifiable();

            var manager = new AdvisorManager(uow.Object);
            //act
            var result = manager.Delete(Guid.NewGuid()).Result;
            //assert
            usrRepo.Verify();
            repo.Verify();
            uow.Verify();
            Assert.True(result.Succeeded);
            Assert.Equal(0, Advisors.Count);
        }

        [Fact]
        public void DeleteAdvisor_succeeded_if_name_is_valid()
        {
            //arrange
            var AdvisorToDelete = new Advisor { User = new User() };
            var Advisors = new List<Advisor> { AdvisorToDelete };
            var uow = new Mock<IUnitOfWork>();
            var repo = new Mock<IAdvisorRepository>();
            var usrRepo = new Mock<IUserRepository>();

            usrRepo.Setup(u => u.Remove(It.IsNotNull<User>()))
                .Callback<User>(a => Advisors.Remove(AdvisorToDelete))
                .Verifiable();

            usrRepo.Setup(u => u.FindByUserName(It.IsNotNull<string>()))
                .ReturnsAsync(new User { UserId = Guid.NewGuid() })
                .Verifiable();

            repo.Setup(r => r.FindById(It.Is<Guid>(g => g != Guid.Empty)))
                .ReturnsAsync(AdvisorToDelete)
                .Verifiable();

            uow.Setup(u => u.SaveChanges())
                .ReturnsAsync(1)
                .Verifiable();

            uow.Setup(u => u.UserRepository)
                .Returns(usrRepo.Object)
                .Verifiable();

            uow.Setup(u => u.AdvisorRepository)
                .Returns(repo.Object)
                .Verifiable();

            var manager = new AdvisorManager(uow.Object);
            //act
            var result = manager.Delete("asdasd").Result;
            //assert
            usrRepo.Verify();
            repo.Verify();
            uow.Verify();
            Assert.True(result.Succeeded);
            Assert.Equal(0, Advisors.Count);
        }
        #endregion
    }
}
