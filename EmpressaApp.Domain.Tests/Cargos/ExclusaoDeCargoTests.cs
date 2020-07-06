using Bogus;
using EmpresaApp.Domain.Base;
using EmpresaApp.Domain.Entitys;
using EmpresaApp.Domain.Interfaces.Gerais;
using EmpresaApp.Domain.Interfaces.Repositorios;
using EmpresaApp.Domain.Notifications;
using EmpresaApp.Domain.Services.Exclusoes;
using EmpresaApp.Domain.Utils;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace EmpressaApp.Domain.Tests.Cargos
{
    public class ExclusaoDeCargoTests
    {
        private readonly Mock<ICargoRepositorio> _cargoRepositorioMock;
        private readonly Mock<IFuncionarioRepositorio> _funcionarioRepositorioMock;
        private readonly Mock<IDomainNotificationHandlerAsync<DomainNotification>> _notificacaoDeDominioMock;
        private readonly ExclusaoDeCargo _exclusaoDeCargo;
        private const int CargoId = 1;

        public ExclusaoDeCargoTests()
        {
            _notificacaoDeDominioMock = new Mock<IDomainNotificationHandlerAsync<DomainNotification>>();
            _cargoRepositorioMock = new Mock<ICargoRepositorio>();
            _funcionarioRepositorioMock = new Mock<IFuncionarioRepositorio>();

            _exclusaoDeCargo = new ExclusaoDeCargo(
                _cargoRepositorioMock.Object, 
                _funcionarioRepositorioMock.Object, 
                _notificacaoDeDominioMock.Object
            );
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

        [Fact]
        public async Task NaoDeveRemoverQuandoCargoNaoExistir()
        {
            _cargoRepositorioMock.Setup(rep => rep.ObterPorIdAsync(CargoId)).Returns(Task.FromResult<Cargo>(null));

            await _exclusaoDeCargo.Excluir(CargoId);

            _notificacaoDeDominioMock.Verify(notificacao =>
                notificacao.HandleAsync(It.Is<DomainNotification>(
                         d => d.Value == string.Format(CommonResources.MsgDominioNaoCadastradoNoMasculino, CommonResources.CargoDominio)
                    ))
              );
        }

        [Fact]
        public async Task NaoDeveRemoverQuandoCargoVinculadoAFuncionario()
        {
            var cargo = CargoBuilder.Novo().Build();
            _cargoRepositorioMock.Setup(rep => rep.ObterPorIdAsync(CargoId)).Returns(Task.FromResult(cargo));

            var funcionario = new Funcionario("nome", "12345678909");
            _funcionarioRepositorioMock.Setup(rep => rep.ObterPorCargoIdAsync(cargo.Id)).Returns(Task.FromResult(funcionario));

            await _exclusaoDeCargo.Excluir(CargoId);

            _notificacaoDeDominioMock.Verify(notificacao =>
                notificacao.HandleAsync(It.Is<DomainNotification>(
                         d => d.Value == CommonResources.MsgCargoEstaVinculadoComFuncionario
                    ))
              );
        }
    }
}
