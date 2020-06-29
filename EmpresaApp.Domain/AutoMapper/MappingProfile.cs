using AutoMapper;
using EmpresaApp.Domain.Dto;
using EmpresaApp.Domain.Entitys;

namespace EmpresaApp.Domain.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Empresa, EmpresaDto>().ReverseMap();
            CreateMap<Funcionario, FuncionarioDto>().ReverseMap();
            CreateMap<Cargo, CargoDto>().ReverseMap();
        }
    }
}
