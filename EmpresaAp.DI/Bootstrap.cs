using EmpresaApp.Data;
using EmpresaApp.Data.Repositorios;
using EmpresaApp.Domain.Interfaces.Armazenadores;
using EmpresaApp.Domain.Interfaces.Gerais;
using EmpresaApp.Domain.Interfaces.Repositorios;
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
            services.AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork));

            services.AddTransient(typeof(IArmazenadorDeFuncionario), typeof(ArmazenadorDeFuncionario));
            services.AddTransient(typeof(IArmazenadorDeCargo), typeof(ArmazenadorDeCargo));
            services.AddTransient(typeof(IArmazenadorDeEmpresa), typeof(ArmazenadorDeEmpresa));

            services.AddTransient(typeof(IFuncionarioRepositorio), typeof(FuncionarioRepositorio));
            services.AddTransient(typeof(ICargoRepositorio), typeof(CargoRepositorio));
            services.AddTransient(typeof(IEmpresaRepositorio), typeof(EmpresaRepositorio));

            services.AddTransient(typeof(ExclusaoDeFuncionario));
            services.AddTransient(typeof(ExclusaoDeEmpresa));
            services.AddTransient(typeof(ExclusaoDeCargo));

        }
    }
}
