using EmpresaApp.Domain.Entitys;
using EmpresaApp.Domain.Utils;
using System.ComponentModel.DataAnnotations;

namespace EmpresaApp.Domain.Dto
{
    public class CargoDto : DtoBase
    {
        [Display(Name = "Descrição")]
        [Required(ErrorMessage = ErrorMsg.Required)]
        public string Descricao { get; set; }

        public static CargoDto ConvertEntityToDto(Cargo entity)
        {
            if (entity != null)
            {
                return new CargoDto
                {
                    Id = entity.Id,
                    Descricao = entity.Descricao,
                };
            }

            return null;
        }
    }
}
