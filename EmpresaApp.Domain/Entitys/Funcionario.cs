using EmpresaApp.Domain.Dto;
using EmpresaApp.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmpresaApp.Domain.Entitys
{
    public class Funcionario : Entity<FuncionarioDto>
    {
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public DateTime? DataContratacao { get; set; }
        public int? EmpresaId { get; set; }
        public Empresa Empresa { get; set; }
        public int? CargoId { get; set; }
        public Cargo Cargo { get; set; }

        public static Funcionario Create(FuncionarioDto dto)
        {
            var entity = new Funcionario();
            entity.Update(dto);
            return entity;
        }

        public override void Update(FuncionarioDto dto)
        {
            Nome = dto.Nome;
            Cpf = dto.Cpf;
            DataContratacao = dto.DataContratacao?.ToDate();
        }
    }
}
