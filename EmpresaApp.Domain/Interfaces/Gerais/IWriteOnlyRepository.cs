using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EmpresaApp.Domain.Interfaces.Gerais
{
    public interface IWriteOnlyRepository<in TEntity> where TEntity : class
    {
        void Adicionar(TEntity obj);
        Task AdicionarAsync(TEntity obj);
        void Atualizar(TEntity obj);
        void Remover(TEntity obj);
    }
}
