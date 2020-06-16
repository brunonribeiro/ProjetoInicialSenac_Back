using EmpresaApp.Domain.Entitys;
using EmpresaApp.Domain.Utils;
using System.ComponentModel.DataAnnotations;

namespace EmpresaApp.Domain.Dto
{
    public class EmpresaDto : DtoBase
    {
        [Display(Name = "Nome")]
        [Required(ErrorMessage = ErrorMsg.Required)]
        public string Nome { get; set; }

        [Display(Name = "CNPJ")]
        public string Cnpj { get; set; }

        [Display(Name = "Data da Fundação")]
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
                    DataFundacao = entity.DataFundacao.ToShortDateString()
                };
            }

            return null;
        }
    }
}
