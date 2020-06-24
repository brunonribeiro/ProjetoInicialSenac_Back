using EmpresaApp.Domain.Entitys;
using EmpresaApp.Domain.Interfaces.Gerais;
using System.Threading.Tasks;

namespace EmpresaApp.Domain.Interfaces.Repositorios
{
    public interface ICargoRepositorio : IRepository<int, Cargo>
    {
        Task<Cargo> ObterPorDescricaoAsync(string descricao);
    }
}
