using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.ViewModels;

namespace PresentationLayer.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IEmployeeRepository _employeerepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public EmployeesController(IEmployeeRepository EmployeeRepository, IDepartmentRepository departmentRepository, IMapper mapper)
        {
            _employeerepository = EmployeeRepository;
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }

        [HttpGet]        
        public IActionResult Index(string? searchValue)
        {
            var employees = Enumerable.Empty<Employee>();

            if (string.IsNullOrWhiteSpace(searchValue))
                 employees = _employeerepository.GetAllWithDepartment();

            else employees = _employeerepository.GetAll(searchValue);

            var employeeViewModel = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);

            return View(employeeViewModel);
        }

        public IActionResult Create()
        {
            var employees = _departmentRepository.GetAll();
            SelectList listItems = new SelectList(employees, "Id", "Name");
            ViewBag.Departments = listItems;
            return View();
        }

        [HttpPost]
        public IActionResult Create(EmployeeViewModel employeeVM)
        {
            var employee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);

            if (!ModelState.IsValid) return View(employeeVM);
                _employeerepository.Create(employee);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int? id) => EmployeeControllerHandler(id, nameof(Details));

        public IActionResult Edit(int? id) => EmployeeControllerHandler(id, nameof(Edit));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, EmployeeViewModel employeeVM)
        {
            if (id != employeeVM.Id) return BadRequest();

            if (!ModelState.IsValid)
            {
                return View(employeeVM);
            }
            try
            {
                var employee = _mapper.Map<EmployeeViewModel,Employee>(employeeVM);

                if (_employeerepository.Update(employee) > 0)
                    TempData["Message"] = "Employee Updated Successfuly";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(employeeVM);
        }

        public IActionResult Delete(int? id) => EmployeeControllerHandler(id, nameof(Delete));

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public IActionResult ConfirmDelete(int? id)
        {

            if (!id.HasValue) return BadRequest();

            var employee = _employeerepository.Get(id.Value);

            if (employee is null) return NotFound();

            try
            {
                _employeerepository.Delete(employee);
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

            if(viewName == nameof(Edit))
            {

                var departments = _departmentRepository.GetAll();
                SelectList listItems = new SelectList(departments, "Id", "Name");
                ViewBag.Departments = listItems;

            }
            if (!id.HasValue) return BadRequest();

            var employee = _employeerepository.Get(id.Value);

            if (employee is null) return NotFound();

            var employeeVM = _mapper.Map<EmployeeViewModel>(employee);

            return View(viewName, employeeVM);
        }
    }
}