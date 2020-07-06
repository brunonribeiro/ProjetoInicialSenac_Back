using EmpresaApp.Domain.Entitys;
using EmpresaApp.Domain.Interfaces.Gerais;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmpresaApp.Domain.Interfaces.Repositorios
{
    public interface IFuncionarioRepositorio : IRepository<int, Funcionario>
    {
        Task<IEnumerable<Funcionario>> ObterListaFuncionarioComEmpresaECargo();
        Task<Funcionario> ObterPorNomeAsync(string nome);
        Task<Funcionario> ObterPorCargoIdAsync(int cargoId);
        Task<Funcionario> ObterPorEmpresaIdAsync(int empresaId);
    }
}
