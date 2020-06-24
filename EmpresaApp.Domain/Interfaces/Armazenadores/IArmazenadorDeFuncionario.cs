using EmpresaApp.Domain.Dto;
using EmpresaApp.Domain.Entitys;
using System.Threading.Tasks;

namespace EmpresaApp.Domain.Interfaces.Armazenadores
{
    public interface IArmazenadorDeFuncionario
    {
        Task Armazenar(FuncionarioDto dto);
        Task AdicionarEmpresa(int funcionarioId, int empresaId);
        Task AdicionarCargo(int funcionarioId, int cargoId);
    }
}
