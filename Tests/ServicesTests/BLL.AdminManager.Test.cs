using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Services.PersonageService;
using IDAL.Interfaces;
using IDAL.Interfaces.IRepositories;
using IDAL.Models;
using Xunit;
using Moq;

namespace Tests.ServicesTests
{
    public class BLL_AdminManager_Test
    {
        #region GetBaseUserByName tests
        [Fact]
        public async Task GetBaseUserByName_throws_if_name_is_null()
        {
            //arrange
            var uow = new Mock<IUnitOfWork>();
            var manager = new AdminManager(uow.Object);
            //act
            //assert   
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await manager.GetBaseUserByName(null));
        }

        [Fact]
        public void GetBaseUserByName_succeded_if_correct_value()
        {
            //arrange
            var repo = new Mock<IUserRepository>();
            var uow = new Mock<IUnitOfWork>();

            repo.Setup(r => r.FindByUserName(It.IsNotNull<string>()))
                .ReturnsAsync(new User())
                .Verifiable();

            uow.Setup(u => u.UserRepository).Returns(repo.Object);

            var manager = new AdminManager(uow.Object);
            //act
            var user = manager.GetBaseUserByName("aaaa").Result;
            //assert
            repo.Verify();
            Assert.NotNull(user);
        }
        #endregion

        #region GetBaseUserByGuid tests
        [Fact]
        public async Task GetBaseUserByGuid_throws_exception_if_guid_is_not_valid()
        {
            //arrange
            var uow = new Mock<IUnitOfWork>();
            var manager = new AdminManager(uow.Object);
            //act
            //assert   
            await Assert.ThrowsAsync<ArgumentException>(async () => await manager.GetBaseUserByGuid(Guid.Empty));
        }

        [Fact]
        public void GetBaseUserByGuid_succeeded_if_valid_param()
        {
            //arrange
            var repo = new Mock<IUserRepository>();
            var uow = new Mock<IUnitOfWork>();

            repo.Setup(r => r.FindById(It.Is<Guid>(g => g != Guid.Empty)))
                .ReturnsAsync(new User())
                .Verifiable();

            uow.Setup(u => u.UserRepository).Returns(repo.Object);

            var manager = new AdminManager(uow.Object);
            //act
            var user = manager.GetBaseUserByGuid(Guid.NewGuid()).Result;
            //assert
            repo.Verify();
            Assert.NotNull(user);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        [InlineData("asdasd")]
        public async Task GetBaseUserByGuid_throws_exception_if_str_is_not_valid(string id)
        {
            //arrange
            var uow = new Mock<IUnitOfWork>();
            var manager = new AdminManager(uow.Object);
            //act
            //assert   
            await Assert.ThrowsAsync<ArgumentException>(async () => await manager.GetBaseUserByGuid(id));
        }

        [Fact]
        public void GetBaseUserByGuid_succeeded_if_valid_str_param()
        {
            //arrange
            var repo = new Mock<IUserRepository>();
            var uow = new Mock<IUnitOfWork>();

            repo.Setup(r => r.FindById(It.Is<Guid>(g => g != Guid.Empty)))
                .ReturnsAsync(new User())
                .Verifiable();

            uow.Setup(u => u.UserRepository).Returns(repo.Object);

            var manager = new AdminManager(uow.Object);
            //act
            var user = manager.GetBaseUserByGuid(Guid.NewGuid().ToString()).Result;
            //assert
            repo.Verify();
            Assert.NotNull(user);
        }
        #endregion

        #region GetAllEmployees tests
        [Fact]
        public async Task GetAllEmployeesTest_throws_if_id_is_not_valid()
        {
            //arrange
            var manager = new AdminManager(Mock.Of<IUnitOfWork>());
            //act
            //assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await manager.GetAllEmployees(Guid.Empty));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("ZZZZZZ")]
        [InlineData(null)]
        public async Task GetAllEmployeesTest_throws_if_str_id_is_not_valid(string id)
        {
            //arrange
            var uow = new Mock<IUnitOfWork>();
            var manager = new AdminManager(uow.Object);
            //act
            //assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await manager.GetAllEmployees(id));
        }

        [Fact]
        public void GetAllEmployees_returns_all_employees_if_id_is_correct()
        {
            //arrange
            var allEmployees = new List<Employee>();
            var emplRepo = new Mock<IEmployeeRepository>();
            var uow = new Mock<IUnitOfWork>();

            emplRepo.Setup(e => e.GetAllEmployees(It.Is<Guid>(g => g != Guid.Empty)))
                .ReturnsAsync(allEmployees)
                .Verifiable();

            uow.Setup(u => u.EmployeeRepository).Returns(emplRepo.Object);

            var manager = new AdminManager(uow.Object);
            //act
            var res = manager.GetAllEmployees(Guid.NewGuid()).Result;
            //assert
            emplRepo.Verify();
            Assert.Equal(allEmployees, res);
        }

        [Fact]
        public void GetAllEmployees_returns_all_employees_if_str_id_is_correct()
        {
            //arrange
            var allEmployees = new List<Employee>();
            var emplRepo = new Mock<IEmployeeRepository>();
            var userRepo = new Mock<IUserRepository>();
            var uow = new Mock<IUnitOfWork>();

            emplRepo.Setup(e => e.GetAllEmployees(It.Is<Guid>(g => g != Guid.Empty)))
                .ReturnsAsync(allEmployees)
                .Verifiable();

            userRepo.Setup(u => u.FindById(It.Is<Guid>(g => g != Guid.Empty)))
                .ReturnsAsync(new User { UserId = Guid.NewGuid() })
                .Verifiable();

            uow.Setup(u => u.EmployeeRepository).Returns(emplRepo.Object);

            uow.Setup(u => u.UserRepository).Returns(userRepo.Object);

            var manager = new AdminManager(uow.Object);
            //act
            var res = manager.GetAllEmployees(Guid.NewGuid().ToString()).Result;
            //assert
            emplRepo.Verify();
            userRepo.Verify();
            Assert.Equal(allEmployees, res);
        }
        #endregion

        #region GetEmployee tests
        [Fact]
        public async Task GetEmployee_throws_if_id_not_valid()
        {
            //arrange
            var uow = Mock.Of<IUnitOfWork>();
            var manager = new AdminManager(uow);
            //act
            //assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await manager.GetEmployee(Guid.Empty));
        }

        [Fact]
        public void GetEmployee_succeded_if_id_is_valid()
        {
            //arrange
            var repo = new Mock<IEmployeeRepository>();
            var uow = new Mock<IUnitOfWork>();

            repo.Setup(r => r.FindById(It.Is<Guid>(g => g != Guid.Empty)))
                .ReturnsAsync(new Employee())
                .Verifiable();

            uow.Setup(u => u.EmployeeRepository)
                .Returns(repo.Object)
                .Verifiable();

            var manager = new AdminManager(uow.Object);
            //act
            var result = manager.GetEmployee(Guid.NewGuid()).Result;
            //assert
            repo.Verify();
            uow.Verify();
            Assert.NotNull(result);
        }
        #endregion

        #region CreateEmployee
        [Fact]
        public void CreateEmployee_failed_if_employee_is_null()
        {
            //arrange
            var uow = Mock.Of<IUnitOfWork>();
            var manager = new AdminManager(uow);
            //act
            var result = manager.CreateEmployee(null).Result;
            //assert
            Assert.False(result.Succeeded);
        }

        [Fact]
        public void CreateEmployee_succeded_if_employee_is_valid()
        {
            //arrange
            var db = new List<Employee>();
            var repo = new Mock<IEmployerRepository>();
            var uow = new Mock<IUnitOfWork>();

            repo.Setup(r => r.AddEmployee(It.IsNotNull<Employee>()))
                .Callback<Employee>(e => db.Add(e))
                .Verifiable();

            uow.Setup(u => u.EmployerRepository)
                .Returns(repo.Object)
                .Verifiable();

            uow.Setup(u => u.SaveChanges())
                .ReturnsAsync(1)
                .Verifiable();

            var manager = new AdminManager(uow.Object);
            //act
            var result = manager.CreateEmployee(new Employee()).Result;
            //assert
            repo.Verify();
            uow.Verify();
            Assert.True(result.Succeeded);
            Assert.Equal(1, db.Count);
        }
        #endregion

        #region UpdateEmployee tests
        [Fact]
        public void UpdateEmployee_failed_if_employee_is_null()
        {
            //arrange
            var uow = Mock.Of<IUnitOfWork>();
            var manager = new AdminManager(uow);
            //act
            var result = manager.UpdateEmployee(null).Result;
            //assert
            Assert.False(result.Succeeded);
        }

        [Fact]
        public void UpdateEmployee_succeded_if_employee_is_valid()
        {
            //arrange
            var repo = new Mock<IEmployeeRepository>();
            var uow = new Mock<IUnitOfWork>();

            repo.Setup(r => r.Update(It.IsNotNull<Employee>()))
                .Verifiable();

            uow.Setup(u => u.EmployeeRepository)
                .Returns(repo.Object)
                .Verifiable();

            var manager = new AdminManager(uow.Object);
            //act
            var result = manager.UpdateEmployee(new Employee()).Result;
            //assert
            repo.Verify();
            uow.Verify();
            Assert.True(result.Succeeded);
        }
        #endregion

        #region Get tests
        [Fact]
        public void Get_returns_null_if_id_is_not_valid()
        {
            //arrange
            var uow = Mock.Of<IUnitOfWork>();
            var manager = new AdminManager(uow);
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
            var manager = new AdminManager(uow);
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
            var repo = new Mock<IAdminRepository>();

            repo.Setup(r => r.FindById(It.Is<Guid>(g => g != Guid.Empty)))
                .ReturnsAsync(new Admin())
                .Verifiable();

            uow.Setup(u => u.AdminRepository)
                .Returns(repo.Object)
                .Verifiable();

            var manager = new AdminManager(uow.Object);
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
            var admRepo = new Mock<IAdminRepository>();

            repo.Setup(r => r.FindByUserName(It.IsNotNull<string>()))
                .ReturnsAsync(new User { UserId = Guid.NewGuid()})
                .Verifiable();

            admRepo.Setup(a => a.FindById(It.Is<Guid>(g => g != Guid.Empty)))
                .ReturnsAsync(new Admin())
                .Verifiable();

            uow.Setup(u => u.AdminRepository)
                .Returns(admRepo.Object)
                .Verifiable();

            uow.Setup(u => u.UserRepository)
                .Returns(repo.Object)
                .Verifiable();

            var manager = new AdminManager(uow.Object);
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
        public void CreateAdmin_failed_if_admin_is_not_valid()
        {
            //arrange
            var uow = Mock.Of<IUnitOfWork>();
            var manager = new AdminManager(uow);
            //act
            var result = manager.Create(null).Result;
            //assert
            Assert.False(result.Succeeded);
        }

        [Fact]
        public void CreateAdmin_succeeded_if_admin_is_valid()
        {
            //arrange
            var admins = new List<Admin>();
            var uow = new Mock<IUnitOfWork>();
            var repo = new Mock<IUserRepository>();

            repo.Setup(r => r.AddAdmin(It.IsNotNull<Admin>()))
                .Callback<Admin>(a => admins.Add(a))
                .Verifiable();

            uow.Setup(u => u.SaveChanges())
                .ReturnsAsync(1)
                .Verifiable();

            uow.Setup(u => u.UserRepository)
                .Returns(repo.Object)
                .Verifiable();

            var manager = new AdminManager(uow.Object);
            //act
            var result = manager.Create(new Admin()).Result;
            //assert
            repo.Verify();
            uow.Verify();
            Assert.True(result.Succeeded);
            Assert.Equal(1, admins.Count);
        }
        #endregion

        #region Update tests
        [Fact]
        public void UpdateAdmin_failed_if_admin_is_not_valid()
        {
            //arrange
            var uow = Mock.Of<IUnitOfWork>();
            var manager = new AdminManager(uow);
            //act
            var result = manager.Update(null).Result;
            //assert
            Assert.False(result.Succeeded);
        }

        [Fact]
        public void UpdateAdmin_succeeded_if_admin_is_valid()
        {
            //arrange
            var uow = new Mock<IUnitOfWork>();
            var repo = new Mock<IAdminRepository>();

            repo.Setup(r => r.Update(It.IsNotNull<Admin>()))
                .Verifiable();

            uow.Setup(u => u.SaveChanges())
                .ReturnsAsync(1)
                .Verifiable();

            uow.Setup(u => u.AdminRepository)
                .Returns(repo.Object)
                .Verifiable();

            var manager = new AdminManager(uow.Object);
            //act
            var result = manager.Update(new Admin()).Result;
            //assert
            repo.Verify();
            uow.Verify();
            Assert.True(result.Succeeded);
        }
        #endregion

        #region Delete tests
        [Fact]
        public void DeleteAdmin_failed_if_id_is_not_valid()
        {
            //arrange
            var uow = Mock.Of<IUnitOfWork>();
            var manager = new AdminManager(uow);
            //act
            var result = manager.Delete(Guid.Empty).Result;
            //assert
            Assert.False(result.Succeeded);
        }

        [Fact]
        public void DeleteAdmin_failed_if_admin_is_null()
        {
            //arrange
            var uow = Mock.Of<IUnitOfWork>();
            var manager = new AdminManager(uow);
            //act
            var result = manager.Delete((Admin)null).Result;
            //assert
            Assert.False(result.Succeeded);
        }

        [Fact]
        public void DeleteAdmin_failed_if_name_is_null()
        {
            //arrange
            var uow = Mock.Of<IUnitOfWork>();
            var manager = new AdminManager(uow);
            //act
            var result = manager.Delete((String)null).Result;
            //assert
            Assert.False(result.Succeeded);
        }

        [Fact]
        public void DeleteAdmin_succeeded_if_id_is_valid()
        {
            //arrange
            var adminToDelete = new Admin { User = new User()};
            var admins = new List<Admin> { adminToDelete };
            var uow = new Mock<IUnitOfWork>();
            var repo = new Mock<IAdminRepository>();
            var usrRepo = new Mock<IUserRepository>();

            usrRepo.Setup(u => u.Remove(It.IsNotNull<User>()))
                .Callback<User>(a => admins.Remove(adminToDelete))
                .Verifiable();

            repo.Setup(r => r.FindById(It.Is<Guid>(g => g != Guid.Empty)))
                .ReturnsAsync(adminToDelete)
                .Verifiable();

            uow.Setup(u => u.SaveChanges())
                .ReturnsAsync(1)
                .Verifiable();

            uow.Setup(u => u.UserRepository)
                .Returns(usrRepo.Object)
                .Verifiable();

            uow.Setup(u => u.AdminRepository)
                .Returns(repo.Object)
                .Verifiable();

            var manager = new AdminManager(uow.Object);
            //act
            var result = manager.Delete(Guid.NewGuid()).Result;
            //assert
            usrRepo.Verify();
            repo.Verify();
            uow.Verify();
            Assert.True(result.Succeeded);
            Assert.Equal(0, admins.Count);
        }

        [Fact]
        public void DeleteAdmin_succeeded_if_name_is_valid()
        {
            //arrange
            var adminToDelete = new Admin { User = new User() };
            var admins = new List<Admin> { adminToDelete };
            var uow = new Mock<IUnitOfWork>();
            var repo = new Mock<IAdminRepository>();
            var usrRepo = new Mock<IUserRepository>();

            usrRepo.Setup(u => u.Remove(It.IsNotNull<User>()))
                .Callback<User>(a => admins.Remove(adminToDelete))
                .Verifiable();

            usrRepo.Setup(u => u.FindByUserName(It.IsNotNull<string>()))
                .ReturnsAsync(new User { UserId = Guid.NewGuid() })
                .Verifiable();

            repo.Setup(r => r.FindById(It.Is<Guid>(g => g != Guid.Empty)))
                .ReturnsAsync(adminToDelete)
                .Verifiable();

            uow.Setup(u => u.SaveChanges())
                .ReturnsAsync(1)
                .Verifiable();

            uow.Setup(u => u.UserRepository)
                .Returns(usrRepo.Object)
                .Verifiable();

            uow.Setup(u => u.AdminRepository)
                .Returns(repo.Object)
                .Verifiable();

            var manager = new AdminManager(uow.Object);
            //act
            var result = manager.Delete("asdasd").Result;
            //assert
            usrRepo.Verify();
            repo.Verify();
            uow.Verify();
            Assert.True(result.Succeeded);
            Assert.Equal(0, admins.Count);
        }
        #endregion
    }
}
