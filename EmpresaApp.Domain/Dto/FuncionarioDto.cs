using EmpresaApp.Domain.Attributes;
using EmpresaApp.Domain.Entitys;
using EmpresaApp.Domain.Utils;
using System.ComponentModel.DataAnnotations;

namespace EmpresaApp.Domain.Dto
{
    public class FuncionarioDto : DtoBase
    {
        [Display(Name = "Nome")]
        [Required(ErrorMessage = ErrorMsg.Required)]
        public string Nome { get; set; }

        [Display(Name = "CPF")]
        [CPF(ErrorMessage = ErrorMsg.Invalid)]
        [Required(ErrorMessage = ErrorMsg.Required)]
        public string Cpf { get; set; }

        [Display(Name = "Data da Contratação")]
        [Date(ErrorMessage = ErrorMsg.Invalid)]
        public string DataContratacao { get; set; }

        [Display(Name = "Empresa")]
        public int? EmpresaId { get; set; }

        [Display(Name = "Cargo")]
        public int? CargoId { get; set; }

        public static FuncionarioDto ConvertEntityToDto(Funcionario entity)
        {
            if (entity != null)
            {
                return new FuncionarioDto
                {
                    Id = entity.Id,
                    Nome = entity.Nome,
                    Cpf = entity.Cpf,
                    DataContratacao = entity.DataContratacao.ToShortDateString(),
                    EmpresaId = entity.EmpresaId,
                    CargoId = entity.CargoId
                };
            }

            return null;
        }
    }
}
