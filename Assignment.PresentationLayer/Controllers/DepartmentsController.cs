using BusinessLogicLayer.Repositories;
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

        public IActionResult Create(Department department) 
        {
            //server side validation
            if (! ModelState.IsValid) return View(department);
            _repository.Create(department);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Details(int? id)
        {
            // Retrieve Department And Send it to the View

            if (!id.HasValue) return BadRequest();

            var department = _repository.Get(id.Value);

            if(department is null) return NotFound();

            return View(department);
        }
        public IActionResult Edit(int? id)
        {
            // Retrieve Department And Send it to the View

            if (!id.HasValue) return BadRequest();

            var department = _repository.Get(id.Value);

            if(department is null) return NotFound();

            return View(department);
        }

        [HttpPost]
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
        public IActionResult Delete(int? id)
        {

            if (!id.HasValue) return BadRequest();

            var department = _repository.Get(id.Value);

            if(department is null) return NotFound();

            return View(department);
        }

        [HttpPost]
        public IActionResult Delete(Department department)
        {

            if (!ModelState.IsValid) return View(department);
            _repository.Delete(department);
            return RedirectToAction(nameof(Index));
        }
    }
}