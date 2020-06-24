using EmpresaApp.Domain.Entitys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmpresaApp.Data.Mappings
{
    public class CargoMapping : IEntityTypeConfiguration<Cargo>
    {
        public void Configure(EntityTypeBuilder<Cargo> builder)
        {
            builder.Property(tc => tc.Descricao)
              .IsRequired();

            builder.Ignore(tc => tc.CascadeMode);
            builder.Ignore(tc => tc.ValidationResult);
        }
    }
}
