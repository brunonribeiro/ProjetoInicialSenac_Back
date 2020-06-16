using System;
using System.Collections.Generic;

namespace EmpresaApp.Domain.Interfaces
{
    public interface IRepository<TEntity>
    {
        List<TEntity> List();
        List<TEntity> List(params string[] predicate);
        TEntity GetById(int id);
        List<TEntity> Get(Func<TEntity, bool> filtro, params string[] predicate);
        void Save(TEntity entity);
        void Remove(TEntity entity);
        void RemoveRange(List<TEntity> entity);
    }
}
