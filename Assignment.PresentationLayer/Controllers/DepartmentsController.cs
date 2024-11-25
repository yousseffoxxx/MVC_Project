namespace PresentationLayer.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DepartmentsController : Controller
    {

        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentsController( IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Retrieve All Departments
            var departments =await  _unitOfWork.Departments.GetAllAsync();
            var departmentViewModel = _mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel>>(departments);

            return View(departments);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> CreateAsync(DepartmentViewModel departmentVM) 
        {
            //server side validation
            if (! ModelState.IsValid) return View(departmentVM);
            var department = _mapper.Map<DepartmentViewModel, Department>(departmentVM);

            await _unitOfWork.Departments.AddAsync(department);
            await _unitOfWork.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id) => await DepartmentControllerHandler(id, nameof(Details));
        public async Task<IActionResult> Edit(int? id) => await DepartmentControllerHandler(id, nameof(Edit));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync([FromRoute]int id, DepartmentViewModel departmentVM)
        {
            if (id != departmentVM.Id) return BadRequest();

            if (!ModelState.IsValid) return View(departmentVM);

            try
            {
                var department = _mapper.Map<DepartmentViewModel, Department>(departmentVM);

                _unitOfWork.Departments.Update(department);

                if (await _unitOfWork.SaveChangesAsync() > 0)
                    TempData["Message"] = "Employee Updated Successfuly";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // log Exception
                ModelState.AddModelError("", ex.Message);
            }
            return View(departmentVM);
        }

        public async Task<IActionResult> Delete(int? id) => await DepartmentControllerHandler(id,nameof(Delete));

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {

            if (!id.HasValue) return BadRequest();

            var department = await _unitOfWork.Departments.GetAsync(id.Value);

            if (department is null) return NotFound();

            try
            {
                _unitOfWork.Departments.Delete(department);
                await _unitOfWork.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
                return View(department);
            }
        }

        private async Task<IActionResult> DepartmentControllerHandler(int? id , string viewName)
        {

            if (!id.HasValue) return BadRequest();

            var department = await _unitOfWork.Departments.GetAsync(id.Value);

            if (department is null) return NotFound();
            var departmentVM = _mapper.Map<DepartmentViewModel>(department);


            return View(viewName, department);
        }
    }
}