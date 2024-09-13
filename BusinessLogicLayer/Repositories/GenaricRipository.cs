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

        public void Create(TEntity entity) => _dbSet.Add(entity);

        public void Delete(TEntity entity) => _dbSet.Remove(entity);

        public void Update(TEntity entity) => _dbSet.Update(entity);
   
        public TEntity? Get(int id) => _dbSet.Find(id);
        public IEnumerable<TEntity> GetAll() =>_dbSet.ToList();
    }
}
