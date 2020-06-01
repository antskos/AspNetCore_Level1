using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WebStore.Data;
using WebStore.Infrastructure.Services;
using WebStore.Infrastructure.Interfaces;

namespace WebStore.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IEmployeesData _employeesData;
        public EmployeesController(IEmployeesData employeesData)
        {
            _employeesData = employeesData;
        }

        public IActionResult Index()
        {
            return View(_employeesData.Get());
        }

        public IActionResult EmployeeDetails(int id)
        {
            var employee = _employeesData.GetById(id);

            if (employee is null)
                return NotFound();
            else
                return View(employee);
        }
    }
}