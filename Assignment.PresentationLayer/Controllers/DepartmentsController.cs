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

        public IActionResult Create() 
        { 
            return View();
        }

        [HttpPost]
        public IActionResult Create(Department department) 
        {
            //server side validation
            if (! ModelState.IsValid) return View(department);
            _repository.Create(department);
            return RedirectToAction(nameof(Index));
        }
    }
}