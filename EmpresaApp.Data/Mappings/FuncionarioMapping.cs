using EmpresaApp.Domain.Entitys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmpresaApp.Data.Mappings
{
    public class FuncionarioMapping : IEntityTypeConfiguration<Funcionario>
    {
        public void Configure(EntityTypeBuilder<Funcionario> builder)
        {
            builder.Property(fun => fun.Nome)
                .IsRequired();

            builder.Property(fun => fun.Cpf)
                .IsRequired();

            builder.Property(fun => fun.DataContratacao);

            builder.HasOne(fun => fun.Empresa)
              .WithMany()
              .HasForeignKey(fun => fun.EmpresaId)
              .IsRequired(false);

            builder.HasOne(fun => fun.Cargo)
             .WithMany()
             .HasForeignKey(fun => fun.CargoId)
             .IsRequired(false);

            builder.Ignore(fun => fun.CascadeMode);
            builder.Ignore(fun => fun.ValidationResult);
        }
    }
}
