using EmpresaApp.Domain.Dto;
using EmpresaApp.Domain.Entitys;

namespace EmpresaApp.Domain.Interfaces
{
    public interface IArmazenadorDeEmpresa
    {
        Empresa Armazenar(EmpresaDto dto);
    }
}
