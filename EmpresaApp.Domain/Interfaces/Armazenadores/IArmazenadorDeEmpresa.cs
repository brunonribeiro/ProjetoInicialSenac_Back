using EmpresaApp.Domain.Dto;
using System.Threading.Tasks;

namespace EmpresaApp.Domain.Interfaces.Armazenadores
{
    public interface IArmazenadorDeEmpresa
    {
        Task Armazenar(EmpresaDto dto);
    }
}
