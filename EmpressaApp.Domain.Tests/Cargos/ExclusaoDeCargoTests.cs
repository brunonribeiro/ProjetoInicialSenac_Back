using Bogus;
using EmpresaApp.Domain.Entitys;
using EmpresaApp.Domain.Interfaces.Gerais;
using EmpresaApp.Domain.Interfaces.Repositorios;
using EmpresaApp.Domain.Notifications;
using EmpresaApp.Domain.Services.Exclusoes;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace EmpressaApp.Domain.Tests.Cargos
{
    public class ExclusaoDeCargoTests
    {
        private readonly Mock<ICargoRepositorio> _cargoRepositorioMock;
        private readonly Mock<IDomainNotificationHandlerAsync<DomainNotification>> _notificacaoDeDominioMock;
        private readonly ExclusaoDeCargo _exclusaoDeCargo;
        private const int CargoId = 1;

        public ExclusaoDeCargoTests()
        {
            _notificacaoDeDominioMock = new Mock<IDomainNotificationHandlerAsync<DomainNotification>>();
            _cargoRepositorioMock = new Mock<ICargoRepositorio>();
            _exclusaoDeCargo = new ExclusaoDeCargo(_cargoRepositorioMock.Object, _notificacaoDeDominioMock.Object);
        }

        [Fact]
        public async Task DeveExcluirCargo()
        {
            var cargo = CargoBuilder.Novo().Build();
            _cargoRepositorioMock.Setup(r => r.ObterPorIdAsync(CargoId)).Returns(Task.FromResult(cargo));

            await _exclusaoDeCargo.Excluir(CargoId);

            _cargoRepositorioMock.Verify(r => r.Remover(cargo));
        }

        [Fact]
        public async Task NaoDeveRemoverSeExistirNotificacaoDeErro()
        {
            _notificacaoDeDominioMock.Setup(o => o.HasNotifications()).Returns(true);

            await _exclusaoDeCargo.Excluir(CargoId);

            _cargoRepositorioMock.Verify(r => r.Remover(It.IsAny<Cargo>()), Times.Never);
        }
    }
}
