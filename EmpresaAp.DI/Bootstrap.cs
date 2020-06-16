using EmpresaApp.Data;
using EmpresaApp.Domain.Interfaces;
using EmpresaApp.Domain.Services;
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

            services.AddTransient(typeof(EmpresaService));
            services.AddTransient(typeof(FuncionarioService));
            services.AddTransient(typeof(CargoService));
        }
    }
}
