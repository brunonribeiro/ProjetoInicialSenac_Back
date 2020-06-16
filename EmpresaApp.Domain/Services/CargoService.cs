using EmpresaApp.Domain.Dto;
using EmpresaApp.Domain.Entitys;
using EmpresaApp.Domain.Interfaces;
using System.Collections.Generic;

namespace EmpresaApp.Domain.Services
{
    public class CargoService
    {
        private readonly IRepository<Cargo> _repository;
        private readonly ServiceDefault<CargoDto, Cargo> _serviceDefault;

        public CargoService(IRepository<Cargo> repository)
        {
            _repository = repository;
            _serviceDefault = new ServiceDefault<CargoDto, Cargo>(CargoDto.ConvertEntityToDto, _repository.GetById);
        }

        public CargoDto GetById(int? id)
        {
            return _serviceDefault.GetById(id);
        }

        public List<CargoDto> List()
        {
            return _serviceDefault.List(_repository.List);
        }

        public bool Remove(int? id)
        {
            return _serviceDefault.Remove(id, _repository.Remove);
        }

        public void Save(CargoDto dto)
        {
            _serviceDefault.Save(dto, Cargo.Create, _repository.Save);
        }
    }
}
