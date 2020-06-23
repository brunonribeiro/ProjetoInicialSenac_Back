using EmpresaApp.Data;
using EmpresaApp.Domain.Interfaces;
using EmpresaApp.Domain.Services.Armazenadores;
using EmpresaApp.Domain.Services.Exclusoes;
using FuncionarioApp.Domain.Services.Armazenadores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EmpresaApp.DI
{
    public class Bootstrap
    {
        public static void Configure(IServiceCollection services, string conection)
        {
            services.AddDbContext<DataContext>(options => options.UseSqlServer(conection));
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork));

            services.AddTransient(typeof(IArmazenadorDeFuncionario), typeof(ArmazenadorDeFuncionario));
            services.AddTransient(typeof(IArmazenadorDeCargo), typeof(ArmazenadorDeCargo));
            services.AddTransient(typeof(IArmazenadorDeEmpresa), typeof(ArmazenadorDeEmpresa));

            services.AddTransient(typeof(ExclusaoDeCargo));
            services.AddTransient(typeof(ExclusaoDeEmpresa));
            services.AddTransient(typeof(ExclusaoDeFuncionario));

        }
    }
}
