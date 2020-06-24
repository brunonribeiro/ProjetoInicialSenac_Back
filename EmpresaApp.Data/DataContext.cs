using EmpresaApp.Data.Mappings;
using EmpresaApp.Domain.Entitys;
using Microsoft.EntityFrameworkCore;

namespace EmpresaApp.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> option) : base(option)
        {

        }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<Cargo> Cargos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new FuncionarioMapping());
            modelBuilder.ApplyConfiguration(new EmpresaMapping());
            modelBuilder.ApplyConfiguration(new CargoMapping());
        }
    }
}