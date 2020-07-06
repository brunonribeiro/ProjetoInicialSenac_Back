using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EmpresaApp.Domain.Interfaces.Gerais
{
    public interface IReadOnlyRepositoryAsync<in TId, TEntity>
       where TId : struct
       where TEntity : class
    {
        Task<IEnumerable<TEntity>> BuscarAsync(Expression<Func<TEntity, bool>> predicate);
        //Task<IEnumerable<TEntity>> BuscarAsyncAsNoTracking(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> ListarAsync();
        //Task<IEnumerable<TEntity>> ListarAsyncAsNoTracking();
        Task<TEntity> ObterPorIdAsync(TId id);
        //Task<TEntity> ObterPorIdAsyncAsNoTracking(TId id);
    }
}
