using EmpresaApp.Domain.Base;
using EmpresaApp.Domain.Dto;
using EmpresaApp.Domain.Entitys;
using EmpresaApp.Domain.Interfaces;
using EmpresaApp.Domain.Utils;
using System;
using System.Linq;

namespace EmpresaApp.Domain.Services.Armazenadores
{
    public class ArmazenadorDeEmpresa : DomainService, IArmazenadorDeEmpresa
    {
        private readonly IRepository<Empresa> _repository;

        public ArmazenadorDeEmpresa(IRepository<Empresa> repository)
        {
            _repository = repository;
        }

        public Empresa Armazenar(EmpresaDto dto)
        {
            ValidarEmpresaComMesmoNome(dto);

            var empresa = new Empresa(dto.Nome, dto.Cnpj);

            if (dto.Id > 0)
            {
                empresa = _repository.GetById(dto.Id);
                empresa.AlterarNome(dto.Nome);
                empresa.AlterarCnpj(dto.Cnpj);
            }

            empresa.AlterarDataFundacao(dto.DataFundacao);

            if (empresa.Validar() && empresa.Id == 0)
            {
                _repository.Save(empresa);
            }
            else
            {
                NotificarValidacoesDeDominio(empresa.ValidationResult);
            }

            return empresa;
        }

        private void ValidarEmpresaComMesmoNome(EmpresaDto dto)
        {
            var empresaComMesmaNome = _repository.Get(x => x.Nome == dto.Nome).FirstOrDefault();

            if (empresaComMesmaNome != null && empresaComMesmaNome.Id != dto.Id)
                NotificarValidacoesDoArmazenador(CommonResources.MsgDominioComMesmoNomeNoFeminino);
        }
    }
}
