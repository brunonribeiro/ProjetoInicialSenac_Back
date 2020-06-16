using EmpresaApp.Domain.Dto;
using EmpresaApp.Domain.Entitys;
using EmpresaApp.Domain.Interfaces;
using System;
using System.Collections.Generic;

namespace EmpresaApp.Domain.Services
{
    public class FuncionarioService
    {
        private readonly IRepository<Funcionario> _repository;
        private readonly ServiceDefault<FuncionarioDto, Funcionario> _serviceDefault;

        public FuncionarioService(IRepository<Funcionario> repository)
        {
            _repository = repository;
            _serviceDefault = new ServiceDefault<FuncionarioDto, Funcionario>(FuncionarioDto.ConvertEntityToDto, _repository.GetById);
        }

        public FuncionarioDto GetById(int? id)
        {
            return _serviceDefault.GetById(id);
        }

        public List<FuncionarioDto> List()
        {
            return _serviceDefault.List(_repository.List);
        }

        public bool Remove(int? id)
        {
            return _serviceDefault.Remove(id, _repository.Remove);
        }

        public void Save(FuncionarioDto dto)
        {
            _serviceDefault.Save(dto, Funcionario.Create, _repository.Save);
        }

        public void AddCargo(FuncionarioDto dto)
        {
            var entity = _repository.GetById(dto.Id);

            if (entity.EmpresaId != null)
            {
                entity.Update(dto);
            }
            else
            {
                throw new ArgumentException("O funcionário precisa estar vinculado a uma empresa para atribuir um cargo. ");
            }
        }
    }
}
