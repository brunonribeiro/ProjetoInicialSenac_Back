using EmpresaApp.Domain.Base;
using EmpresaApp.Domain.Interfaces.Gerais;
using EmpresaApp.Domain.Interfaces.Repositorios;
using EmpresaApp.Domain.Notifications;
using System.Threading.Tasks;

namespace EmpresaApp.Domain.Services.Exclusoes
{
    public class ExclusaoDeCargo : DomainService
    {
        private readonly ICargoRepositorio _cargoRepositorio;

        public ExclusaoDeCargo(ICargoRepositorio cargoRepositorio, 
            IDomainNotificationHandlerAsync<DomainNotification> notificacaoDeDominio)
            : base(notificacaoDeDominio)
        {
            _cargoRepositorio = cargoRepositorio;
        }

        public async Task Excluir(int id)
        {
            var cargo = await _cargoRepositorio.ObterPorIdAsync(id);
            _cargoRepositorio.Remover(cargo);
        }
    }
}
