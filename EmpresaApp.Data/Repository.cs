using EmpresaApp.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EmpresaApp.Data
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DataContext _context;

        public Repository(DataContext context)
        {
            _context = context;
        }

        public List<TEntity> Get(Func<TEntity, bool> filtro, params string[] predicate)
        {
            return List(predicate).Where(filtro).ToList();
        }

        public TEntity GetById(int id)
        {
            return _context.Set<TEntity>().Find(id);
        }

        public List<TEntity> List()
        {
            return _context.Set<TEntity>().ToList();
        }

        public List<TEntity> List(params string[] predicate)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            foreach (var item in predicate)
            {
                query = query.Include(item);
            }

            return query.ToList();
        }

        public void Remove(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public void RemoveRange(List<TEntity> entity)
        {
            _context.Set<TEntity>().RemoveRange(entity);
        }

        public void Save(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
        }
    }
}
