using System.Net;

namespace BusinessLogicLayer.Repositories
{
    public class EmployeeRepository : GenaricRepository<Employee>, IEmployeeRepository
    {

        public EmployeeRepository(DataContext dataContext) : base(dataContext) 
        {

        }

        public IEnumerable<Employee> GetAll(string name)
        {
            return _dbSet.Where(e => e.Name.ToLower().Contains( name.ToLower())).Include(e=>e.Department).ToList();
        }

        public IEnumerable<Employee> GetAllWithDepartment()
        {
            return _dbSet.Include(e => e.Department).ToList();
        }
    }
}
