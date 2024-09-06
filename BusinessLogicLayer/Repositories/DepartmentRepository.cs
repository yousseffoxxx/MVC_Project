using DataAccessLayer.Data;
using DataAccessLayer.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        // Dependency Injection
        // Method Injection => Method ([FromServices]DataContext dataContect)
        // Property Injection =>
        //[FromServices]
        //public int MyProperty { get; set; }


        private readonly DataContext _dataContext;
        //CTOR Injection 
        public DepartmentRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        /*
        * Get , Get All , Create , Apdate , Delete
        */

        public Department? Get(int id) => _dataContext.Deprtments.Find(id);
        public IEnumerable<Department> GetAll() => _dataContext.Deprtments.ToList();

        public int Create(Department entity)
        {
            _dataContext.Deprtments.Add(entity);
            return _dataContext.SaveChanges();
        }
        public int Update(Department entity)
        {
            _dataContext.Deprtments.Update(entity);
            return _dataContext.SaveChanges();
        }
        public int Delete(Department entity)
        {
            _dataContext.Deprtments.Remove(entity);
            return _dataContext.SaveChanges();
        }
    }
}
