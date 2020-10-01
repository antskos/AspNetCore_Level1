using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using WebStore.Clients.Base;
using WebStore.Domain;
using WebStore.Domain.Entities.Employees;
using WebStore.Interfaces.Services;

namespace WebStore.Clients.Employees
{
    public class EmployeesClient : BaseClient, IEmployeesData
    {
        public EmployeesClient(IConfiguration configuration) : base(configuration, WebAPI.Employees) { }

        public int Add(Employee emp) => Post(_serviceAddress, emp).Content.ReadAsAsync<int>().Result;
        
        public IEnumerable<Employee> Get() => Get<IEnumerable<Employee>>(_serviceAddress);        

        public Employee GetById(int id) => Get<Employee>($"{_serviceAddress}/{id}");

        public void Edit(Employee emp) => Put(_serviceAddress, emp);

        public bool Delete(int id) => Delete($"{_serviceAddress}/{id}").IsSuccessStatusCode;

        public void SaveChanges() { }
    }
}
