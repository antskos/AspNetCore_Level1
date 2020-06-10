using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Infrastructure.Interfaces;
using WebStore.Models;
using WebStore.Data;

namespace WebStore.Infrastructure.Services.InMemory
{
    public class InMemoryEmployeesData : IEmployeesData
    {
        private static readonly List<Models.Employee> _employees = TestData.Employees;

        public int Add(Employee emp)
        {
            if (emp is null)
                throw new ArgumentNullException(nameof(Employee));

            if (_employees.Contains(emp)) return emp.Id;

            emp.Id = _employees.Count == 0 ? 1 : _employees.Max(emp => emp.Id) + 1;
            _employees.Add(emp);

            return emp.Id;
        }

        public bool Delete(int id)
        {
            var item = GetById(id);

            if (item is null)
                return false;
            else 
                return _employees.Remove(item);
        }

        public void Edit(Employee emp)
        {
            if (emp is null)
                throw new ArgumentNullException(nameof(Employee));

            if (_employees.Contains(emp)) 
                return;

            var item = GetById(emp.Id);

            item.Name = emp.Name;
            item.Surname = emp.Surname;
            item.Patronymic = emp.Patronymic;
            item.Age = emp.Age;
        }

        public IEnumerable<Employee> Get() => _employees;

        public Employee GetById(int id) => _employees.FirstOrDefault(emp => emp.Id == id);
        

        public void SaveChanges()
        {
        }
    }
}
