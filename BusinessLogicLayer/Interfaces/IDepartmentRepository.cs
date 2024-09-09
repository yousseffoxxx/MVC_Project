namespace BusinessLogicLayer.Interfaces
{
    public interface IDepartmentRepository : IGenaricRepository<Department>
    {
        public IEnumerable<Department> GetAll(string name);
    }
}