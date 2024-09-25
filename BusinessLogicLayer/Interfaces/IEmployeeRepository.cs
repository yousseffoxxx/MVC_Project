namespace BusinessLogicLayer.Interfaces
{
    public interface IEmployeeRepository : IGenaricRepository<Employee>
    {
        public Task<IEnumerable<Employee>> GetAllAsync(string name);
        public Task<IEnumerable<Employee>> GetAllWithDepartmentAsync();
    }
}
