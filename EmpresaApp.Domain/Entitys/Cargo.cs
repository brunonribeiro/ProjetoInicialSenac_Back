using EmpresaApp.Domain.Dto;

namespace EmpresaApp.Domain.Entitys
{
    public class Cargo : Entity<CargoDto>
    {
        public string Descricao { get; set; }

        public static Cargo Create(CargoDto dto)
        {
            var entity = new Cargo();
            entity.Update(dto);
            return entity;
        }

        public override void Update(CargoDto dto)
        {
            Descricao = dto.Descricao;
        }
    }
}
