
namespace EmpresaApp.Domain.Interfaces.Gerais
{
    public interface IRepository<TId, TEntity> : IReadOnlyRepositoryAsync<TId, TEntity>, IWriteOnlyRepository<TEntity>
        where TId : struct
        where TEntity : class
    {
    }
}
