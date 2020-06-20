using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WebStore.Data;
using WebStore.Infrastructure.Services;
using WebStore.Infrastructure.Interfaces;
using WebStore.ViewModels;
using System.Net.Cache;
using System;
using WebStore.Domain.Entities.Employees;

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

        #region редактирование информации о сотруднике
        public IActionResult Edit(int? id)
        {
            if (id is null)
                return View(new EmployeeViewModel());
            else if (id < 0)
                return BadRequest();
            else
            {
                var employee = _employeesData.GetById((int)id);

                if (employee is null)
                    return NotFound();
                else
                    return View(new EmployeeViewModel
                    {
                        Id = employee.Id,
                        Name = employee.Name,
                        Surname = employee.Surname,
                        Patronymic = employee.Patronymic,
                        Age = employee.Age
                    });
            }
        }

        [HttpPost]
        public IActionResult Edit(EmployeeViewModel Model)
        {
            if (Model is null)
                throw new ArgumentNullException(nameof(Model));

            // валидация модели в программе
            if (Model.Patronymic.Length < 5)
                ModelState.AddModelError("Patronymic", "Длина отчества менее пяти символов не допускается");


            if (!ModelState.IsValid)
                    return View(Model);

            var employee = new Employee
            {
                Id = Model.Id,
                Name = Model.Name,
                Surname = Model.Surname,
                Patronymic = Model.Patronymic,
                Age = Model.Age
            };

            if (Model.Id == 0)
                _employeesData.Add(employee);
            else
                _employeesData.Edit(employee);

            _employeesData.SaveChanges();

            return RedirectToAction("Index");
        }

        #endregion

        #region удаление записи о сотруднике
        public IActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest();

            var employee = _employeesData.GetById(id);
            if (employee is null)
                return NotFound();

            return View(new EmployeeViewModel
            {
                Id = employee.Id,
                Name = employee.Name,
                Surname = employee.Surname,
                Patronymic = employee.Patronymic,
                Age = employee.Age
            });
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            _employeesData.Delete(id);
            _employeesData.SaveChanges();

            return RedirectToAction("Index");
        }


        #endregion
    }
}