namespace PresentationLayer.Controllers
{
    [Authorize(Roles ="Admin")]
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
        public async Task<IActionResult> Index(string? searchValue)
        {
            var employees = Enumerable.Empty<Employee>();

            if (string.IsNullOrWhiteSpace(searchValue))
                 employees = await _unitOfWork.Employees.GetAllWithDepartmentAsync();

            else employees = await _unitOfWork.Employees.GetAllAsync(searchValue);

            var employeeViewModel = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);

            return View(employeeViewModel);
        }

        public async Task<IActionResult> Create()
        {
            var employees = await _unitOfWork.Departments.GetAllAsync();
            SelectList listItems = new SelectList(employees, "Id", "Name");
            ViewBag.Departments = listItems;
            return View();
        }

        [HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(EmployeeViewModel employeeVM)
        {

            if (!ModelState.IsValid) return View(employeeVM);

            if (employeeVM.Image is not null)
                employeeVM.ImageName = await DocumentSittings.UploadFileAsync(employeeVM.Image , "Images");
           

            var employee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
            await _unitOfWork.Employees.AddAsync(employee);
            await _unitOfWork.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id) => await EmployeeControllerHandler(id, nameof(Details));
        public Task<IActionResult> Edit(int? id) => EmployeeControllerHandler(id, nameof(Edit));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, EmployeeViewModel employeeVM)
        {
            if (id != employeeVM.Id) return BadRequest();

            if (!ModelState.IsValid) return View(employeeVM);

            try
            {
                if (employeeVM.Image is not null)
                    employeeVM.ImageName = await DocumentSittings.UploadFileAsync(employeeVM.Image, "images");

                var employee = _mapper.Map<EmployeeViewModel,Employee>(employeeVM);

                _unitOfWork.Employees.Update(employee);

                if (await _unitOfWork.SaveChangesAsync() > 0)
                    TempData["Message"] = "Employee Updated Successfuly";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(employeeVM);
        }

        public async Task<IActionResult> Delete(int? id) => await EmployeeControllerHandler(id, nameof(Delete));

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {

            if (!id.HasValue) return BadRequest();

            var employee = await _unitOfWork.Employees.GetAsync(id.Value);

            if (employee is null) return NotFound();

            try
            {
                _unitOfWork.Employees.Delete(employee);
                if (await _unitOfWork.SaveChangesAsync() > 0 && employee.ImageName is not null)
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

        private async Task<IActionResult> EmployeeControllerHandler(int? id, string viewName)
        {

            if(viewName == nameof(Edit))
            {

                var departments = await _unitOfWork.Departments.GetAllAsync();
                SelectList listItems = new SelectList(departments, "Id", "Name");
                ViewBag.Departments = listItems;

            }
            if (!id.HasValue) return BadRequest();

            var employee = await _unitOfWork.Employees.GetAsync(id.Value);

            if (employee is null) return NotFound();

            var employeeVM = _mapper.Map<EmployeeViewModel>(employee);

            return View(viewName, employeeVM);
        }
    }
}