﻿namespace BusinessLogicLayer.Interfaces
{
    public interface IUnitOfWork
    {
        public IEmployeeRepository Employees { get; }
        public IDepartmentRepository Departments { get; }

        public Task<int> SaveChangesAsync();
    }
}