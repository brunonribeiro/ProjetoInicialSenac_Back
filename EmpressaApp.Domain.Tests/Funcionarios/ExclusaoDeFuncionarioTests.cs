using EmpresaApp.Domain.Entitys;
using EmpresaApp.Domain.Interfaces.Gerais;
using EmpresaApp.Domain.Interfaces.Repositorios;
using EmpresaApp.Domain.Notifications;
using EmpresaApp.Domain.Services.Exclusoes;
using EmpresaApp.Domain.Utils;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace EmpressaApp.Domain.Tests.Funcionarios
{
    public class ExclusaoDeFuncionarioTests
    {
        private readonly Mock<IFuncionarioRepositorio> _funcionarioRepositorioMock;
        private readonly Mock<IDomainNotificationHandlerAsync<DomainNotification>> _notificacaoDeDominioMock;
        private readonly ExclusaoDeFuncionario _exclusaoDeFuncionario;
        private const int FuncionarioId = 1;

        public ExclusaoDeFuncionarioTests()
        {
            _notificacaoDeDominioMock = new Mock<IDomainNotificationHandlerAsync<DomainNotification>>();
            _funcionarioRepositorioMock = new Mock<IFuncionarioRepositorio>();
            _funcionarioRepositorioMock = new Mock<IFuncionarioRepositorio>();

            _exclusaoDeFuncionario = new ExclusaoDeFuncionario(
                _funcionarioRepositorioMock.Object,
                _notificacaoDeDominioMock.Object
            );
        }

        [Fact]
        public async Task DeveExcluirFuncionario()
        {
            var funcionario = FuncionarioBuilder.Novo().Build();
            _funcionarioRepositorioMock.Setup(r => r.ObterPorIdAsync(FuncionarioId)).Returns(Task.FromResult(funcionario));

            await _exclusaoDeFuncionario.Excluir(FuncionarioId);

            _funcionarioRepositorioMock.Verify(r => r.Remover(funcionario));
        }

        [Fact]
        public async Task NaoDeveRemoverSeExistirNotificacaoDeErro()
        {
            _notificacaoDeDominioMock.Setup(o => o.HasNotifications()).Returns(true);

            await _exclusaoDeFuncionario.Excluir(FuncionarioId);

            _funcionarioRepositorioMock.Verify(r => r.Remover(It.IsAny<Funcionario>()), Times.Never);
        }
    }
}
