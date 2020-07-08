using EmpresaApp.Domain.Entitys;
using EmpresaApp.Domain.Interfaces.Gerais;
using EmpresaApp.Domain.Interfaces.Repositorios;
using EmpresaApp.Domain.Notifications;
using EmpresaApp.Domain.Services.Exclusoes;
using EmpresaApp.Domain.Utils;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace EmpressaApp.Domain.Tests.Empresas
{
    public class ExclusaoDeEmpresaTests
    {
        private readonly Mock<IEmpresaRepositorio> _empresaRepositorioMock;
        private readonly Mock<IFuncionarioRepositorio> _funcionarioRepositorioMock;
        private readonly Mock<IDomainNotificationHandlerAsync<DomainNotification>> _notificacaoDeDominioMock;
        private readonly ExclusaoDeEmpresa _exclusaoDeEmpresa;
        private const int EmpresaId = 1;

        public ExclusaoDeEmpresaTests()
        {
            _notificacaoDeDominioMock = new Mock<IDomainNotificationHandlerAsync<DomainNotification>>();
            _empresaRepositorioMock = new Mock<IEmpresaRepositorio>();
            _funcionarioRepositorioMock = new Mock<IFuncionarioRepositorio>();

            _exclusaoDeEmpresa = new ExclusaoDeEmpresa(
                _empresaRepositorioMock.Object,
                _funcionarioRepositorioMock.Object,
                _notificacaoDeDominioMock.Object
            );
        }

        [Fact]
        public async Task DeveExcluirEmpresa()
        {
            var empresa = EmpresaBuilder.Novo().Build();
            _empresaRepositorioMock.Setup(r => r.ObterPorIdAsync(EmpresaId)).Returns(Task.FromResult(empresa));

            await _exclusaoDeEmpresa.Excluir(EmpresaId);

            _empresaRepositorioMock.Verify(r => r.Remover(empresa));
        }

        [Fact]
        public async Task NaoDeveRemoverSeExistirNotificacaoDeErro()
        {
            _notificacaoDeDominioMock.Setup(o => o.HasNotifications()).Returns(true);

            await _exclusaoDeEmpresa.Excluir(EmpresaId);

            _empresaRepositorioMock.Verify(r => r.Remover(It.IsAny<Empresa>()), Times.Never);
        }

        [Fact]
        public async Task NaoDeveRemoverQuandoEmpresaNaoExistir()
        {
            _empresaRepositorioMock.Setup(rep => rep.ObterPorIdAsync(EmpresaId)).Returns(Task.FromResult<Empresa>(null));

            await _exclusaoDeEmpresa.Excluir(EmpresaId);

            _notificacaoDeDominioMock.Verify(notificacao =>
                notificacao.HandleAsync(It.Is<DomainNotification>(
                         d => d.Value == string.Format(CommonResources.MsgDominioNaoCadastradoNoFeminino, CommonResources.EmpresaDominio)
                    ))
              );
        }

        [Fact]
        public async Task NaoDeveRemoverQuandoEmpresaVinculadoAFuncionario()
        {
            var empresa = EmpresaBuilder.Novo().Build();
            _empresaRepositorioMock.Setup(rep => rep.ObterPorIdAsync(EmpresaId)).Returns(Task.FromResult(empresa));

            var funcionario = new Funcionario("nome", "12345678909");
            _funcionarioRepositorioMock.Setup(rep => rep.ObterPorEmpresaIdAsync(empresa.Id)).Returns(Task.FromResult(funcionario));

            await _exclusaoDeEmpresa.Excluir(EmpresaId);

            _notificacaoDeDominioMock.Verify(notificacao =>
                notificacao.HandleAsync(It.Is<DomainNotification>(
                         d => d.Value == CommonResources.MsgEmpresaEstaVinculadoComFuncionario
                    ))
              );
        }
    }
}
