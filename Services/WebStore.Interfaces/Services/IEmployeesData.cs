using System.Collections.Generic;
using WebStore.Domain.Entities.Employees;

namespace WebStore.Interfaces.Services
{
    public interface IEmployeesData
    {
        IEnumerable<Employee> Get();

        Employee GetById(int id);

        int Add(Employee emp);

        void Edit(Employee emp);

        bool Delete(int id);

        void SaveChanges();

    }
}
