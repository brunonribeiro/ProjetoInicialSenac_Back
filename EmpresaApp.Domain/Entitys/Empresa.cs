using EmpresaApp.Domain.Dto;
using EmpresaApp.Domain.Utils;
using System;

namespace EmpresaApp.Domain.Entitys
{
    public class Empresa : Entity<EmpresaDto>
    {
        public string Nome { get; set; }
        public string Cnpj { get; set; }
        public DateTime DataFundacao { get; set; }
        
        public static Empresa Create(EmpresaDto dto)
        {
            var entity = new Empresa();
            entity.Update(dto);
            return entity;
        }

        public override void Update(EmpresaDto dto)
        {
            Nome = dto.Nome;
            Cnpj = dto.Cnpj;
            DataFundacao = dto.DataFundacao.ToDate();
        }
    }
}
