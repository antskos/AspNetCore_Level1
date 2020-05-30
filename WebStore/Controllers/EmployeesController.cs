using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace WebStore.Controllers
{
    public class EmployeesController : Controller
    {
        private static readonly List<Models.Employee> _employees = new List<Models.Employee>
        {
            new Models.Employee
            {
                Id = 1,
                Name = "Ivan",
                Surname = "Ivanov",
                Patronymic = "Ivanovich",
                Age = 26
            },
            new Models.Employee
            {
                Id = 2,
                Name = "Aleksei",
                Surname = "Alekseev",
                Patronymic = "Alekseevich",
                Age = 35
            },
            new Models.Employee
            {
                Id = 3,
                Name = "Konstantin",
                Surname = "Konstantinov",
                Patronymic = "Konstantinovich",
                Age = 41
            },
        };

        public IActionResult Index()
        {
            return View(_employees);
        }

        public IActionResult EmployeeDetails(int id)
        {
            var employee = _employees.FirstOrDefault(emp => emp.Id == id);
            if (employee is null) return NotFound();
            else return View(employee);
        }
    }
}