namespace PresentationLayer.Controllers
{
    [Authorize]
    public class EmployeesController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public EmployeesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index(string? searchValue)
        {
            var employees = Enumerable.Empty<Employee>();

            if (string.IsNullOrWhiteSpace(searchValue))
                 employees = _unitOfWork.Employees.GetAllWithDepartment();

            else employees = _unitOfWork.Employees.GetAll(searchValue);

            var employeeViewModel = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);

            return View(employeeViewModel);
        }

        public IActionResult Create()
        {
            var employees = _unitOfWork.Departments.GetAll();
            SelectList listItems = new SelectList(employees, "Id", "Name");
            ViewBag.Departments = listItems;
            return View();
        }

        [HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(EmployeeViewModel employeeVM)
        {

            if (!ModelState.IsValid) return View(employeeVM);

            if (employeeVM.Image is not null)
                employeeVM.ImageName = DocumentSittings.UploadFile(employeeVM.Image , "images");
           
            var employee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
            
            _unitOfWork.Employees.Create(employee);
            _unitOfWork.SaveChanges();

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
                if (employeeVM.Image is not null)
                    employeeVM.ImageName = DocumentSittings.UploadFile(employeeVM.Image, "images");

                var employee = _mapper.Map<EmployeeViewModel,Employee>(employeeVM);

                _unitOfWork.Employees.Update(employee);

                if (_unitOfWork.SaveChanges() > 0)
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

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public IActionResult ConfirmDelete(int? id)
        {

            if (!id.HasValue) return BadRequest();

            var employee = _unitOfWork.Employees.Get(id.Value);

            if (employee is null) return NotFound();

            try
            {
                _unitOfWork.Employees.Delete(employee);
                if (_unitOfWork.SaveChanges() > 0 && employee.ImageName is not null)
                {
                    DocumentSittings.DeleteFile("Images" , employee.ImageName);
                }
                

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

                var departments = _unitOfWork.Departments.GetAll();
                SelectList listItems = new SelectList(departments, "Id", "Name");
                ViewBag.Departments = listItems;

            }
            if (!id.HasValue) return BadRequest();

            var employee = _unitOfWork.Employees.Get(id.Value);

            if (employee is null) return NotFound();

            var employeeVM = _mapper.Map<EmployeeViewModel>(employee);

            return View(viewName, employeeVM);
        }
    }
}