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
    internal class DepartmentRepository
    {
        /*
         * Get , Get All , Create , Apdate , Delete
         */

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

        //private DataContext dataContext { get; set; } // Hard Codded Dependency

        //public Department Get(int id)
        //{
        //    _dataContext.Deprtments.FirstOrDefault();
        //}
    }
}
