using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BusinessLogicLayer.Repositories
{
    public class GenaricRepository<TEntity> : IGenaricRepository<TEntity> where TEntity : class
    {

        protected DataContext _dataContext;

        protected DbSet<TEntity> _dbSet;

        public GenaricRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
            _dbSet = _dataContext.Set<TEntity>();
        }

        public int Create(TEntity entity)
        {
            _dbSet.Add(entity);
            return _dataContext.SaveChanges();
        }

        public int Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
            return _dataContext.SaveChanges();
        }
        public int Update(TEntity entity)
        {
            _dbSet.Update(entity);
            return _dataContext.SaveChanges();
        }

        public TEntity? Get(int id) => _dbSet.Find(id);
        public IEnumerable<TEntity> GetAll() =>_dbSet.ToList();

    }
}
