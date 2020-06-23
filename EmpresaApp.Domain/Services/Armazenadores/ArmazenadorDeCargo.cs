using EmpresaApp.Domain.Base;
using EmpresaApp.Domain.Dto;
using EmpresaApp.Domain.Entitys;
using EmpresaApp.Domain.Interfaces;
using EmpresaApp.Domain.Utils;
using System;
using System.Linq;

namespace EmpresaApp.Domain.Services.Armazenadores
{
    public class ArmazenadorDeCargo : DomainService, IArmazenadorDeCargo
    {
        private readonly IRepository<Cargo> _repository;

        public ArmazenadorDeCargo(IRepository<Cargo> repository)
        {
            _repository = repository;
        }

        public Cargo Armazenar(CargoDto dto)
        {
            ValidarCargoComMesmaDescricao(dto);

            var cargo = new Cargo(dto.Descricao);

            if (dto.Id > 0)
            {
                cargo = _repository.GetById(dto.Id);
                cargo.AlterarDescricao(dto.Descricao);
            }

            if (cargo.Validar() && cargo.Id == 0)
            {
                _repository.Save(cargo);
            }
            else
            {
                NotificarValidacoesDeDominio(cargo.ValidationResult);
            }

            return cargo;
        }

        private void ValidarCargoComMesmaDescricao(CargoDto dto)
        {
            var cargoComMesmaDescricao = _repository.Get(x => x.Descricao == dto.Descricao).FirstOrDefault();

            if (cargoComMesmaDescricao != null && cargoComMesmaDescricao.Id != dto.Id)
                NotificarValidacoesDoArmazenador(CommonResources.MsgDominioComMesmoNomeNoMasculino);
        }
    }
}
