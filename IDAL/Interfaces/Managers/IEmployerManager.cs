using System;

namespace IDAL.Interfaces.Managers
{
    public interface IEmployerManager
    {
        void AddEmployee(string firstName, string prefix, string lastName, Guid employerId);
    }
}