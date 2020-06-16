using EmpresaApp.Domain.Dto;
using EmpresaApp.Domain.Entitys;
using EmpresaApp.Domain.Interfaces;
using System.Collections.Generic;

namespace EmpresaApp.Domain.Services
{
    public class EmpresaService
    {
        private readonly IRepository<Empresa> _repository;
        private readonly ServiceDefault<EmpresaDto, Empresa> _serviceDefault;

        public EmpresaService(IRepository<Empresa> repository)
        {
            _repository = repository;
            _serviceDefault = new ServiceDefault<EmpresaDto, Empresa>(EmpresaDto.ConvertEntityToDto, _repository.GetById);
        }

        public EmpresaDto GetById(int? id)
        {
            return _serviceDefault.GetById(id);
        }

        public List<EmpresaDto> List()
        {
            return _serviceDefault.List(_repository.List);
        }

        public bool Remove(int? id)
        {
            return _serviceDefault.Remove(id, _repository.Remove);
        }

        public void Save(EmpresaDto dto)
        {
            _serviceDefault.Save(dto, Empresa.Create, _repository.Save);
        }
    }
}
