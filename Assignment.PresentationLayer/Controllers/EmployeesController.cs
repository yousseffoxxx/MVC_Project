using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    public class EmployeesController : Controller
    {
        private IEmployeeRepository _repository;

        public EmployeesController(IEmployeeRepository Repository)
        {
            _repository = Repository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            //ViewData["Message"] = new Employee { Name = "Youssef" };

            //C# 4 Feature ViewBag

            //ViewBag.Department = new Department { Name = "IT" };

            var departments = _repository.GetAll();
            return View(departments);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if (!ModelState.IsValid) return View(employee);
            _repository.Create(employee);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int? id) => EmployeeControllerHandler(id, nameof(Details));

        public IActionResult Edit(int? id) => EmployeeControllerHandler(id, nameof(Edit));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, Employee employee)
        {
            if (id != employee.Id) return BadRequest();

            if (!ModelState.IsValid)
            {
                return View(employee);
            }
            try
            {
                if (_repository.Update(employee) > 0)
                    TempData["Message"] = "Employee Updated Successfuly";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(employee);
        }

        public IActionResult Delete(int? id) => EmployeeControllerHandler(id, nameof(Delete));

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public IActionResult ConfirmDelete(int? id)
        {

            if (!id.HasValue) return BadRequest();

            var employee = _repository.Get(id.Value);

            if (employee is null) return NotFound();

            try
            {
                _repository.Delete(employee);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
                return View(employee);
            }
        }

        private IActionResult EmployeeControllerHandler(int? id, string viewName)
        {

            if (!id.HasValue) return BadRequest();

            var employee = _repository.Get(id.Value);

            if (employee is null) return NotFound();

            return View(viewName, employee);
        }
    }
}