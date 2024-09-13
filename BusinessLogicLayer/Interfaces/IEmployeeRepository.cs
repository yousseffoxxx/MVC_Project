namespace BusinessLogicLayer.Interfaces
{
    public interface IEmployeeRepository : IGenaricRepository<Employee>
    {
        public IEnumerable<Employee> GetAll(string Address);
        public IEnumerable<Employee> GetAllWithDepartment();
    }
}
