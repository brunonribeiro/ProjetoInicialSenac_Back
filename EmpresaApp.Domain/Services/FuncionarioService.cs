using EmpresaApp.Domain.Dto;
using EmpresaApp.Domain.Entitys;
using EmpresaApp.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EmpresaApp.Domain.Services
{
    public class FuncionarioService
    {
        private readonly IRepository<Funcionario> _repository;
        private readonly IRepository<Empresa> _repositoryEmpresa;
        private readonly IRepository<Cargo> _repositoryCargo;
        private readonly ServiceDefault<FuncionarioDto, Funcionario> _serviceDefault;

        public FuncionarioService(IRepository<Funcionario> repository, IRepository<Empresa> repositoryEmpresa, IRepository<Cargo> repositoryCargo)
        {
            _repository = repository;
            _repositoryEmpresa = repositoryEmpresa;
            _repositoryCargo = repositoryCargo;
            _serviceDefault = new ServiceDefault<FuncionarioDto, Funcionario>(FuncionarioDto.ConvertEntityToDto, _repository.GetById);
        }

        public FuncionarioDto GetById(int? id)
        {
            return _serviceDefault.GetById(id);
        }

        public List<FuncionarioListarDto> List()
        {
            return _repository.List("Empresa", "Cargo").Select(FuncionarioListarDto.ConvertEntityToDto).ToList();
        }

        public bool Remove(int? id)
        {
            return _serviceDefault.Remove(id, _repository.Remove);
        }

        public void Save(FuncionarioDto dto)
        {
            _serviceDefault.Save(dto, Funcionario.Create, _repository.Save);
        }

        public void AddEmpresa(int funcionarioId, int empresaId)
        {
            var entity = _repository.GetById(funcionarioId);

            if (entity != null)
            {
                var empresa = _repositoryEmpresa.GetById(empresaId);
                if (empresa == null)
                    throw new ArgumentException("A empresa informada não está cadastrada.");

                entity.EmpresaId = empresaId;
            }
            else
            {
                throw new ArgumentException("O funcionário informado não está cadastrado. ");
            }
        }

        public void AddCargo(int funcionarioId, int cargoId)
        {
            var entity = _repository.GetById(funcionarioId);

            if (entity != null)
            {
                if (entity.EmpresaId == null)
                    throw new ArgumentException("O funcionário precisa estar vinculado a uma empresa para atribuir um cargo. ");

                var cargo = _repositoryCargo.GetById(cargoId);
                if (cargo == null)
                    throw new ArgumentException("O cargo informado não está cadastrado.");

                entity.CargoId = cargoId;
            }
            else
            {
                throw new ArgumentException("O funcionário informado não está cadastrado. ");
            }
        }
    }
}
