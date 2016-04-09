using System;
using IDAL.Interfaces;
using IDAL.Interfaces.Managers;
using IDAL.Models;

namespace BLL
{
    public class EmployerManager : IEmployerManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployerManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void AddEmployee(string firstName, string prefix, string lastName, Guid employerId)
        {
            var employee = new Employee
            {
                EmployeeId = Guid.NewGuid(),
                FirstName = firstName,
                Prefix = prefix,
                LastName = lastName,
                EmployerId = employerId
            };

            _unitOfWork.EmployeeRepository.Add(employee);
            _unitOfWork.SaveChanges();

            //TODO: Add send mail to Trans Lion
        }
    }
}