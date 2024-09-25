
namespace PresentationLayer.Controllers
{
    public class DepartmentsController : Controller
    {
        //private IGenaricRepository<Department> _repository;

        private IDepartmentRepository _repository;

        public DepartmentsController(IDepartmentRepository Repository)
        {
            _repository = Repository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            /// ViewData => Dictionary<String,object>
            ///ViewData["Message"] = "Hello From viewData";

            // Retrieve All Departments
            var departments = _repository.GetAllAsync();
            return View(departments);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Department department) 
        {
            //server side validation
            if (! ModelState.IsValid) return View(department);
            _repository.AddAsync(department);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id) => await DepartmentControllerHandler(id, nameof(Details));

        public async Task<IActionResult> Edit(int? id) => await DepartmentControllerHandler(id, nameof(Edit));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute]int id,Department department)
        {
            if (id != department.Id) return BadRequest();

            if (!ModelState.IsValid)
            {
                return View(department);
            }
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
            return View(department);
        }

        public async Task<IActionResult> DeleteAsync(int? id) => await DepartmentControllerHandler(id,nameof(DeleteAsync));

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {

            if (!id.HasValue) return BadRequest();

            var department = await _repository.GetAsync(id.Value);

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

        private async Task<IActionResult> DepartmentControllerHandler(int? id , string viewName)
        {

            if (!id.HasValue) return BadRequest();

            var department = await _repository.GetAsync(id.Value);

            if (department is null) return NotFound();

            return View(viewName, department);
        }
    }
}