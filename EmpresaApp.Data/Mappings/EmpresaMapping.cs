using EmpresaApp.Domain.Entitys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmpresaApp.Data.Mappings
{
    public class EmpresaMapping : IEntityTypeConfiguration<Empresa>
    {
        public void Configure(EntityTypeBuilder<Empresa> builder)
        {
            builder.Property(emp => emp.Nome)
                .IsRequired();

            builder.Property(emp => emp.Cnpj)
                .IsRequired();

            builder.Property(emp => emp.DataFundacao);

            builder.Ignore(emp => emp.CascadeMode);
            builder.Ignore(emp => emp.ValidationResult);
        }
    }
}
