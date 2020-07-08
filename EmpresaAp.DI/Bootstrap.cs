using EmpresaApp.Data;
using EmpresaApp.Data.Repositorios;
using EmpresaApp.Domain.Interfaces.Armazenadores;
using EmpresaApp.Domain.Interfaces.Gerais;
using EmpresaApp.Domain.Interfaces.Repositorios;
using EmpresaApp.Domain.Notifications;
using EmpresaApp.Domain.Services.Armazenadores;
using EmpresaApp.Domain.Services.Exclusoes;
using EmpresaApp.Domain.Services.Armazenadores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EmpresaApp.DI
{
    public class Bootstrap
    {
        public static void Configure(IServiceCollection services, string conection)
        {
            services.AddDbContext<DataContext>(options => options.UseSqlServer(conection));
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddScoped(typeof(IDomainNotificationHandlerAsync<DomainNotification>), typeof(DomainNotificationHandlerAsync));

            services.AddScoped(typeof(IArmazenadorDeFuncionario), typeof(ArmazenadorDeFuncionario));
            services.AddScoped(typeof(IArmazenadorDeCargo), typeof(ArmazenadorDeCargo));
            services.AddScoped(typeof(IArmazenadorDeEmpresa), typeof(ArmazenadorDeEmpresa));

            services.AddScoped(typeof(IFuncionarioRepositorio), typeof(FuncionarioRepositorio));
            services.AddScoped(typeof(ICargoRepositorio), typeof(CargoRepositorio));
            services.AddScoped(typeof(IEmpresaRepositorio), typeof(EmpresaRepositorio));

            services.AddScoped(typeof(ExclusaoDeFuncionario));
            services.AddScoped(typeof(ExclusaoDeEmpresa));
            services.AddScoped(typeof(ExclusaoDeCargo));

            services.AddScoped(typeof(IConsultaBase<,>), typeof(ConsultaBase<,>));

        }
    }
}
