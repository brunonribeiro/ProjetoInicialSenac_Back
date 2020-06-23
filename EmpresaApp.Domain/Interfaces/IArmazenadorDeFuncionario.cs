using EmpresaApp.Domain.Dto;
using EmpresaApp.Domain.Entitys;

namespace EmpresaApp.Domain.Interfaces
{
    public interface IArmazenadorDeFuncionario
    {
        Funcionario Armazenar(FuncionarioDto dto);
        void AdicionarEmpresa(int funcionarioId, int empresaId);
        void AdicionarCargo(int funcionarioId, int cargoId);
    }
}
