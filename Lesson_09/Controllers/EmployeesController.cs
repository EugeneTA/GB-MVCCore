using Lesson09.DAL.Entities;
using Lesson09.Services;
using Lesson09.ViewModels;
using Microsoft.AspNetCore.Mvc;
using RazorEngine;
using RazorEngine.Templating;
using System.Globalization;
using System.Reflection;

namespace Lesson09.Controllers
{
    public class EmployeesController : Controller
    {

        private readonly IEmployeesRepository _employeesRepository;

        public EmployeesController(IEmployeesRepository employeesRepository)
        {
            _employeesRepository = employeesRepository;
        }

        public IActionResult Index()
        {
            var employees = _employeesRepository.GetAll();

            return View(employees);
        }

        public IActionResult Details(int id)
        {
            var employee = _employeesRepository.GetById(id);
            if (employee is null)
                return NotFound();

            return View(new EmployeesViewModel
            {
                Id = employee.Id,
                Name = employee.Name,
                LastName = employee.LastName,
                Patronymic = employee.Patronymic,
                Birthday = employee.Birthday,
            });
        }

        public IActionResult Create()
        {
            return View("Edit", new EmployeesViewModel());
        }

        public IActionResult Edit(int? id)
        {
            if (id is null)
                return View(new EmployeesViewModel());

            var employee = _employeesRepository.GetById((int)id);
            if (employee == null)
                return NotFound();

            return View(new EmployeesViewModel
            {
                Id = employee.Id,
                Name = employee.Name,
                Birthday = employee.Birthday,
                LastName = employee.LastName,
                Patronymic = employee.Patronymic
            });

        }

        [HttpPost]
        public IActionResult Edit(EmployeesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var employee = new Employee
            {
                Id = model.Id,
                LastName = model.LastName,
                Name = model.Name,
                Patronymic = model.Patronymic,
                Birthday = model.Birthday,
            };

            if (employee.Id == 0)
            {
                var id = _employeesRepository.Add(employee);
                return RedirectToAction("Details", new { id });
            }

            var success = _employeesRepository.Edit(employee);

            if (!success)
                return NotFound();

            return RedirectToAction("Index");
        }


        public IActionResult Delete(int id)
        {
            var employee = _employeesRepository.GetById((int)id);
            if (employee == null)
                return NotFound();

            return View(new EmployeesViewModel
            {
                Id = employee.Id,
                Name = employee.Name,
                Birthday = employee.Birthday,
                LastName = employee.LastName,
                Patronymic = employee.Patronymic
            });
        }


        [HttpPost]
        public IActionResult DeleteConfirm(int id)
        {
            if (!_employeesRepository.Remove(id))
                return NotFound();

            return RedirectToAction("Index");
        }


        public IActionResult Report()
        {
            string templateFile = "Report/ReportTemplate.html";
            string template;
            bool result = false;

            try
            {
                using (var reader = new StreamReader(templateFile))
                {
                    template = reader.ReadToEnd();
                }

                string report = Engine.Razor.RunCompile(template, "myUniqueReport", null, _employeesRepository.GetAll());

                if (!Directory.Exists("Reports"))
                {
                    Directory.CreateDirectory("Reports");
                }

                using (var writer = new StreamWriter("Reports/EmployeeReport.html"))
                {
                    writer.WriteAsync(report);
                }

                result = true;
            }
            catch { }

            return View("Report", result);

        }


    }
}
