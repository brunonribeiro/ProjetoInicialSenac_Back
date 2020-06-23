using EmpresaApp.Domain.Base;
using EmpresaApp.Domain.Dto;
using EmpresaApp.Domain.Entitys;
using EmpresaApp.Domain.Interfaces;
using EmpresaApp.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuncionarioApp.Domain.Services.Armazenadores
{
    public class ArmazenadorDeFuncionario : DomainService, IArmazenadorDeFuncionario
    {
        private readonly IRepository<Funcionario> _repository;
        private readonly IRepository<Empresa> _repositoryEmpresa;
        private readonly IRepository<Cargo> _repositoryCargo;

        public ArmazenadorDeFuncionario(IRepository<Funcionario> repository, IRepository<Empresa> repositoryEmpresa, IRepository<Cargo> repositoryCargo)
        {
            _repository = repository;
            _repositoryEmpresa = repositoryEmpresa;
            _repositoryCargo = repositoryCargo;
        }
      
        public Funcionario Armazenar(FuncionarioDto dto)
        {
            ValidarFuncionarioComMesmoNome(dto);

            var funcionario = new Funcionario(dto.Nome, dto.Cpf);

            if (dto.Id > 0)
            {
                funcionario = _repository.GetById(dto.Id);
                funcionario.AlterarNome(dto.Nome);
                funcionario.AlterarCpf(dto.Cpf);
            }

            funcionario.AlterarDataContratacao(dto.DataContratacao);

            if (funcionario.Validar() && funcionario.Id == 0)
            {
                _repository.Save(funcionario);
            }
            else
            {
                NotificarValidacoesDeDominio(funcionario.ValidationResult);
            }

            return funcionario;
        }

        public void AdicionarEmpresa(int funcionarioId, int empresaId)
        {
            var funcionario = _repository.GetById(funcionarioId);

            ValidarFuncionaroNaoCadastrado(funcionario);
            ValidarEmpresaNaoCadastrada(empresaId);

            funcionario.AlterarEmpresa(empresaId);
        }

        public void AdicionarCargo(int funcionarioId, int cargoId)
        {
            var funcionario = _repository.GetById(funcionarioId);

            ValidarFuncionaroNaoCadastrado(funcionario);
            ValidarFuncionaroComEmpresaCadastrada(funcionario);
            ValidarCargoNaoCadastrado(cargoId);

            funcionario.AlterarCargo(funcionarioId);
        }      

        private void ValidarFuncionarioComMesmoNome(FuncionarioDto dto)
        {
            var funcionarioComMesmaNome = _repository.Get(x => x.Nome == dto.Nome).FirstOrDefault();

            if (funcionarioComMesmaNome != null && funcionarioComMesmaNome.Id != dto.Id)
                NotificarValidacoesDoArmazenador(CommonResources.MsgDominioComMesmoNomeNoMasculino);
        }

        private void ValidarFuncionaroNaoCadastrado(Funcionario funcionario)
        {
            if (funcionario == null)
                NotificarValidacoesDoArmazenador("O funcionário informado não está cadastrado.");
        }

        private void ValidarEmpresaNaoCadastrada(int empresaId)
        {
            var empresa = _repositoryEmpresa.GetById(empresaId);
            if (empresa == null)
                NotificarValidacoesDoArmazenador("A empresa informada não está cadastrada.");
        }

        private void ValidarCargoNaoCadastrado(int cargoId)
        {
            var cargo = _repositoryCargo.GetById(cargoId);
            if (cargo == null)
                NotificarValidacoesDoArmazenador("O cargo informado não está cadastrado.");
        }
        private void ValidarFuncionaroComEmpresaCadastrada(Funcionario funcionario)
        {
            if (!funcionario.EmpresaId.HasValue)
                NotificarValidacoesDoArmazenador("O funcionário precisa estar vinculado a uma empresa para atribuir um cargo.");
        }

    }
}