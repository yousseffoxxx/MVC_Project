
namespace BusinessLogicLayer.Repositories
{
    public class DepartmentRepository : GenaricRepository<Department>, IDepartmentRepository
    {
        /*
         //// Dependency Injection
         //// Method Injection => Method ([FromServices]DataContext dataContect)
         //// Property Injection =>
         ////[FromServices]
         ////public int MyProperty { get; set; }


         //private readonly DataContext _dataContext;
         ////CTOR Injection 
         //public DepartmentRepository(DataContext dataContext)
         //{
         //    _dataContext = dataContext;
         //}
        */

        public DepartmentRepository(DataContext dataContext) : base(dataContext)
        {

        }

        public IEnumerable<Department> GetAll(string name)
        {
            return _dbSet.Where(d => d.Name == name).ToList();
        }
    }
}
