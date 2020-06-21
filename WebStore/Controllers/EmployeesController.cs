using System;
using Microsoft.AspNetCore.Mvc;
using WebStore.Infrastructure.Interfaces;
using WebStore.ViewModels;
using WebStore.Infrastructure.Mapping;

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
                    return View(employee.ToView());
            }
        }

        [HttpPost]
        public IActionResult Edit(EmployeeViewModel model)
        {
            if (model is null)
                throw new ArgumentNullException(nameof(model));

            // валидация модели в программе
            if (model.Patronymic.Length < 5)
                ModelState.AddModelError("Patronymic", "Длина отчества менее пяти символов не допускается");


            if (!ModelState.IsValid)
                    return View(model);

            var employee = model.FromView();

            if (model.Id == 0)
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

            return View(employee.ToView());
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