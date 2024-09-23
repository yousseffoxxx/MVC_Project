using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface IGenaricRepository<TEntity> where TEntity : class
    {
        Task AddAsync(TEntity entity);
        Task<TEntity?> GetAsync(int id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        void Delete(TEntity entity);
        void Update(TEntity entity);
    }
}
