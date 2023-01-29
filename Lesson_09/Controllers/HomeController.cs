using Lesson09.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lesson09.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeesRepository _employeesRepository;

        public HomeController(IEmployeesRepository employeesRepository)
        {
            _employeesRepository = employeesRepository;
        }

        public IActionResult Index()
        {
            var employees = _employeesRepository.GetAll();

            return View(employees);
        }
    }
}
