using System.Net;

namespace BusinessLogicLayer.Repositories
{
    public class EmployeeRepository : GenaricRepository<Employee>, IEmployeeRepository
    {

        public EmployeeRepository(DataContext dataContext) : base(dataContext) 
        {

        }

        public IEnumerable<Employee> GetAll(string Address)
        {
            return _dbSet.Where(e => e.Address.ToLower() == Address.ToLower()).ToList();
        }

        public IEnumerable<Employee> GetAllWithDepartment()
        {
            return _dbSet.Include(e => e.Department).ToList();
        }
    }
}
