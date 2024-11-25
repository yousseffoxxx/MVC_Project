using System.Net;

namespace BusinessLogicLayer.Repositories
{
    public class EmployeeRepository : GenaricRepository<Employee>, IEmployeeRepository
    {

        public EmployeeRepository(DataContext dataContext) : base(dataContext) 
        {

        }

        public async Task<IEnumerable<Employee>> GetAllAsync(string name)
        {
            return await _dbSet.Where(e => e.Name.ToLower().Contains( name.ToLower())).Include(e=>e.Department).ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetAllWithDepartmentAsync()
        {
            return await _dbSet.Include(e => e.Department).ToListAsync();
        }
    }
}