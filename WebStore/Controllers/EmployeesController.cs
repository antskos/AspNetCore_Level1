using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WebStore.Data;

namespace WebStore.Controllers
{
    public class EmployeesController : Controller
    {
        private static readonly List<Models.Employee> _employees = TestData.Employees;
        
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