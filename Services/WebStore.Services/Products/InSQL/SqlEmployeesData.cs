using System;
using System.Collections.Generic;
using WebStore.Interfaces.Services;
using WebStore.DAL.Contetxt;
using WebStore.Domain.Entities.Employees;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace WebStore.Services.Products.InSQL
{
    public class SqlEmployeesData : IEmployeesData
    {
        private readonly WebStoreDB _db;
        public SqlEmployeesData(WebStoreDB db) => _db = db;

        public int Add(Employee emp)
        {
            if (emp is null) throw new ArgumentNullException(nameof(emp));
            if (_db.Employees.Contains(emp)) return emp.Id;

            _db.Employees.Add(emp);
            return emp.Id;
        }

        public bool Delete(int id)
        {
            var item = _db.Employees.Find(id);

            if (item is null) return false;

            //_db.Employees.Remove(item);

            //_db.Entry(item).State = EntityState.Deleted;

            _db.Remove(item);
            return true;
        }

        public void Edit(Employee emp)
        {
            if (emp is null) throw new ArgumentNullException(nameof(emp));

            //var item = GetById(emp.Id);
            //if (item is null) return;
            //if (_db.Employees.Contains(emp)) return;

            //item.Name = emp.Name;
            //item.Surname = emp.Surname;
            //item.Patronymic = emp.Patronymic;
            //item.Age = emp.Age;

            // используем, написанные методы EF
            //_db.Attach(emp);
            //_db.Entry(emp).State = EntityState.Modified;

            //ещё способ
            _db.Update(emp);
        }

        public IEnumerable<Employee> Get() => _db.Employees;


        public Employee GetById(int id) => /*_db.Employees.Find(id);*/ _db.Employees.FirstOrDefault(emp => emp.Id == id);

        public void SaveChanges() => _db.SaveChanges();
    }
}
