using System.Collections.Generic;
using WebStore.Infrastructure.Interfaces;
using WebStore.DAL.Contetxt;
using WebStore.Domain.Entities.Employees;

namespace WebStore.Infrastructure.Services.InSQL
{
    public class SqlEmployeesData : IEmployeesData
    {
        private readonly WebStoreDB _db;
        public SqlEmployeesData(WebStoreDB db) => _db = db;

        int IEmployeesData.Add(Employee emp)
        {
            throw new System.NotImplementedException();
        }

        bool IEmployeesData.Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        void IEmployeesData.Edit(Employee emp)
        {
            throw new System.NotImplementedException();
        }

        IEnumerable<Employee> IEmployeesData.Get()
        {
            throw new System.NotImplementedException();
        }

        Employee IEmployeesData.GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        void IEmployeesData.SaveChanges()
        {
            throw new System.NotImplementedException();
        }
    }
}
