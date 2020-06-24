using EmpresaApp.Domain.Dto;
using System.Threading.Tasks;

namespace EmpresaApp.Domain.Interfaces.Armazenadores
{
    public interface IArmazenadorDeCargo
    {
        Task Armazenar(CargoDto dto);
    }
}
