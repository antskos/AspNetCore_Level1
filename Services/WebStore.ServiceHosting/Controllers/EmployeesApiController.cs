using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using WebStore.Domain;
using WebStore.Domain.Entities.Employees;
using WebStore.Interfaces.Services;

namespace WebStore.ServiceHosting.Controllers
{
    //[Route("api/[controller]")]      http://localhost:5001/api/employeesapi -- такой путь до контроллера по маршруту задаваемому этой строкой
    //[Route("api/employees")]        // http://localhost:5001/api/employees
    [Route(WebAPI.Employees)]        // http://localhost:5001/api/employees
    [Produces("application/json")]      // явное задание формата передачи данных клиенту
    [ApiController]
    public class EmployeesApiController : ControllerBase, IEmployeesData
    {
        private readonly IEmployeesData _employeesData;

        public EmployeesApiController(IEmployeesData employeesData)
        {
            _employeesData = employeesData;
        }

        [HttpPost]
        public int Add(Employee emp)
        {
            var id = _employeesData.Add(emp);
            SaveChanges();
            return id;
        }

        [HttpGet]   // GET http://localhost:5001/api/employees
        //[HttpGet("all")]        // GET http://localhost:5001/api/employees/all
        public IEnumerable<Employee> Get()
        {
            return _employeesData.Get();
        }

        [HttpGet("{id}")]        // GET http://localhost:5001/api/employees/3 -- сотрудник с id=3
        public Employee GetById(int id)
        {
            return _employeesData.GetById(id);
        }

        [HttpPut]
        public void Edit(Employee emp)
        {
            _employeesData.Edit(emp);
            SaveChanges();
        }

        [HttpDelete("{id}")]    // DELETE http://localhost:5001/api/employees/3
        //[HttpDelete("delete/{id}")]    // DELETE http://localhost:5001/api/employees/delete/3 -- вариант оформления маршрута конечной точки
        //[HttpDelete("delete({id})")]    // DELETE http://localhost:5001/api/employees/delete(3) --  другой вариант
        public bool Delete(int id)
        {
            var result = _employeesData.Delete(id);
            SaveChanges();
            return result;
        }

       
        // будет ошибка при автоматизированной генерации документации по WebApi, правка позже
        // [NonAction]  -- правка
        public void SaveChanges()
        {
            _employeesData.SaveChanges();
        }
    }
}
