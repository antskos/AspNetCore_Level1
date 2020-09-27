using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using WebStore.Domain;
using WebStore.Domain.Entities.Employees;
using WebStore.Interfaces.Services;

namespace WebStore.ServiceHosting.Controllers
{
    /// <summary>
    /// API управления сотрудниками
    /// </summary>
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

        /// <summary>
        /// Добавление нового сотрудника
        /// </summary>
        /// <param name="emp">Добавляемый сотрудник</param>
        /// <returns>Возвращает, присвоенный id сотрудника</returns>
        [HttpPost]
        public int Add(Employee emp)
        {
            var id = _employeesData.Add(emp);
            SaveChanges();
            return id;
        }

        /// <summary>
        /// Метод получения информации обо всех сотрудниках
        /// </summary>
        /// <returns></returns>
        [HttpGet]   // GET http://localhost:5001/api/employees
        //[HttpGet("all")]        // GET http://localhost:5001/api/employees/all
        public IEnumerable<Employee> Get()
        {
            return _employeesData.Get();
        }


        /// <summary>
        /// Получить данные сотрудника по его идентификатору
        /// </summary>
        /// <param name="id">Идентификатор сотрудника</param>
        /// <returns>Найденный сотрудник</returns>
        [HttpGet("{id}")]        // GET http://localhost:5001/api/employees/3 -- сотрудник с id=3
        public Employee GetById(int id)
        {
            return _employeesData.GetById(id);
        }


        /// <summary>
        /// Редактирование информации сотруднрика
        /// </summary>
        /// <param name="emp">Редактируемый сотрудник</param>
        [HttpPut]
        public void Edit(Employee emp)
        {
            _employeesData.Edit(emp);
            SaveChanges();
        }

        /// <summary>
        /// Удаление данных сотрудника
        /// </summary>
        /// <param name="id">Идентификатор удаляемого сотрудника</param>
        /// <returns></returns>
        [HttpDelete("{id}")]    // DELETE http://localhost:5001/api/employees/3
        //[HttpDelete("delete/{id}")]    // DELETE http://localhost:5001/api/employees/delete/3 -- вариант оформления маршрута конечной точки
        //[HttpDelete("delete({id})")]    // DELETE http://localhost:5001/api/employees/delete(3) --  другой вариант
        public bool Delete(int id)
        {
            var result = _employeesData.Delete(id);
            SaveChanges();
            return result;
        }

       
        
        [NonAction]  // чтобы не было ошибки автоматизированной генерации документации по WebApi посредством swagger
        public void SaveChanges()
        {
            _employeesData.SaveChanges();
        }
    }
}
