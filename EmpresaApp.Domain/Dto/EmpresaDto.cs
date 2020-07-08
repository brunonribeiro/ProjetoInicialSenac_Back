using EmpresaApp.Domain.Attributes;
using EmpresaApp.Domain.Entitys;
using EmpresaApp.Domain.Utils;
using System.ComponentModel.DataAnnotations;

namespace EmpresaApp.Domain.Dto
{
    public class EmpresaDto : DtoBase
    {
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Display(Name = "CNPJ")]
        [CNPJ(ErrorMessage = ErrorMsg.Invalid)]
        public string Cnpj { get; set; }

        [Display(Name = "Data da Fundação")]
        [Date(ErrorMessage = ErrorMsg.Invalid)]
        public string DataFundacao { get; set; }

        public static EmpresaDto ConvertEntityToDto(Empresa entity)
        {
            if (entity != null)
            {
                return new EmpresaDto
                {
                    Id = entity.Id,
                    Nome = entity.Nome,
                    Cnpj = entity.Cnpj,
                    DataFundacao = entity.DataFundacao?.ToShortDateString()
                };
            }

            return null;
        }
    }
}
