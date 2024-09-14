﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface IGenaricRepository<TEntity> where TEntity : class
    {
        void Create(TEntity entity);
        void Delete(TEntity entity);
        void Update(TEntity entity);
        TEntity? Get(int id);
        IEnumerable<TEntity> GetAll();
    }
}
