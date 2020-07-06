using EmpresaApp.Domain.Dto;
using EmpresaApp.Domain.Entitys;
using EmpresaApp.Domain.Interfaces.Gerais;
using System.Text;
using System.Threading.Tasks;

namespace EmpresaApp.Domain.Interfaces.Repositorios
{
    public interface IEmpresaRepositorio : IRepository<int, Empresa>
    {
        Task<EmpresaDto> ObterPorNomeAsync(string nome);
    }
}
