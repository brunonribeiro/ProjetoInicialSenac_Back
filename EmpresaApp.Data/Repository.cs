using EmpresaApp.Domain.Entitys;
using EmpresaApp.Domain.Interfaces.Gerais;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EmpresaApp.Data
{
    public class Repository<TId, TEntity> :
        IReadOnlyRepositoryAsync<TId, TEntity>, 
        IWriteOnlyRepository<TEntity>
        where TId : struct
        where TEntity : Entity<TId, TEntity>
    {
        private readonly DbSet<TEntity> _dbSet;

        public Repository(DataContext context)
        {
            _dbSet = context.Set<TEntity>();
        }     

        public async Task AdicionarAsync(TEntity obj) => await _dbSet.AddAsync(obj);
        public void Adicionar(TEntity obj) => _dbSet.Add(obj);

        public void Atualizar(TEntity obj) => _dbSet.Update(obj);

        public void Remover(TEntity obj) => _dbSet.Remove(obj);

        public async Task<IEnumerable<TEntity>> ListarAsyncAsNoTracking() => await _dbSet.AsNoTracking().ToListAsync();

        public async Task<IEnumerable<TEntity>> BuscarAsync(Expression<Func<TEntity, bool>> predicate) =>
            await _dbSet.Where(predicate).ToListAsync();

        public async Task<IEnumerable<TEntity>> BuscarAsyncAsNoTracking(Expression<Func<TEntity, bool>> predicate) =>
            await _dbSet.AsNoTracking().Where(predicate).ToListAsync();

        public async Task<TEntity> ObterPorIdAsyncAsNoTracking(TId id) =>
            await _dbSet.AsNoTracking().FirstOrDefaultAsync(e => EqualityComparer<TId>.Default.Equals(e.Id, id));

        public async Task<IEnumerable<TEntity>> ListarAsync() => await _dbSet.ToListAsync();

        public async Task<TEntity> ObterPorIdAsync(TId id) =>
            await _dbSet.FirstOrDefaultAsync(e => EqualityComparer<TId>.Default.Equals(e.Id, id));
    }
}
