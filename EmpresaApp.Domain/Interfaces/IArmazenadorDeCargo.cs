using EmpresaApp.Domain.Dto;
using EmpresaApp.Domain.Entitys;

namespace EmpresaApp.Domain.Interfaces
{
    public interface IArmazenadorDeCargo
    {
        Cargo Armazenar(CargoDto dto);
    }
}
