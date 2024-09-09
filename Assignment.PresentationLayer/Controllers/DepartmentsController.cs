using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly IDepartmentRepository _repository;

        public DepartmentsController(IDepartmentRepository departmentRepository)
        {
            _repository = departmentRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // Retrieve All Departments
            var departments = _repository.GetAll();
            return View(departments);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Department department) 
        {
            //server side validation
            if (! ModelState.IsValid) return View(department);
            _repository.Create(department);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int? id) => DepartmentControllerHandler(id, nameof(Details));

        public IActionResult Edit(int? id) => DepartmentControllerHandler(id, nameof(Edit));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute]int id,Department department)
        {
            if (id != department.Id) return BadRequest();

            if (!ModelState.IsValid)
            {
                try
                {
                    _repository.Update(department);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // log Exception
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(department);
        }

        public IActionResult Delete(int? id) => DepartmentControllerHandler(id,nameof(Delete));

        [HttpPost, ActionName("Delete")]
        public IActionResult ConfirmDelete(int? id)
        {

            if (!id.HasValue) return BadRequest();

            var department = _repository.Get(id.Value);

            if (department is null) return NotFound();

            try
            {
                _repository.Delete(department);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
                return View(department);
            }
        }

        private IActionResult DepartmentControllerHandler(int? id , string viewName)
        {

            if (!id.HasValue) return BadRequest();

            var department = _repository.Get(id.Value);

            if (department is null) return NotFound();

            return View(viewName, department);
        }
    }
}