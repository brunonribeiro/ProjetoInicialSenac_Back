using Bogus;
using Bogus.Extensions.Brazil;
using EmpresaApp.Domain.Base;
using EmpresaApp.Domain.Dto;
using EmpresaApp.Domain.Entitys;
using EmpresaApp.Domain.Interfaces.Gerais;
using EmpresaApp.Domain.Interfaces.Repositorios;
using EmpresaApp.Domain.Notifications;
using EmpresaApp.Domain.Services.Armazenadores;
using EmpresaApp.Domain.Utils;
using EmpressaApp.Domain.Tests.Comum;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace EmpressaApp.Domain.Tests.Funcionarios
{
    public class ArmazenadorDeFuncionarioTests
    {
        private readonly int _idFuncionario;
        private readonly int _idEmpresa;
        private readonly int _idCargo;

        private readonly FuncionarioDto _funcionarioDto;
        private readonly ArmazenadorDeFuncionario _armazenadorDeFuncionario;

        private readonly Mock<IFuncionarioRepositorio> _funcionarioRepositorioMock;
        private readonly Mock<IEmpresaRepositorio> _empresaRepositorioMock;
        private readonly Mock<ICargoRepositorio> _cargoRepositorioMock;
        private readonly Mock<IDomainNotificationHandlerAsync<DomainNotification>> _notificacaoDeDominioMock;

        private readonly Faker _faker;

        public ArmazenadorDeFuncionarioTests()
        {
            _faker = FakerBuilder.Novo().Build();
            _funcionarioDto = new FuncionarioDto
            {
                Nome = _faker.Lorem.Sentence(),
                Cpf = _faker.Person.Cpf(true)
            };

            _idFuncionario = _faker.Random.Number(100);
            _idEmpresa = _faker.Random.Number(100);
            _idCargo = _faker.Random.Number(100);

            _notificacaoDeDominioMock = new Mock<IDomainNotificationHandlerAsync<DomainNotification>>();
            _funcionarioRepositorioMock = new Mock<IFuncionarioRepositorio>();
            _empresaRepositorioMock = new Mock<IEmpresaRepositorio>();
            _cargoRepositorioMock = new Mock<ICargoRepositorio>();
            _armazenadorDeFuncionario = new ArmazenadorDeFuncionario(
                _funcionarioRepositorioMock.Object,
                _empresaRepositorioMock.Object,
                _cargoRepositorioMock.Object,
                _notificacaoDeDominioMock.Object
             );
        }

        [Fact]
        public async Task DeveAdicionarUmNovoFuncionario()
        {
            await _armazenadorDeFuncionario.Armazenar(_funcionarioDto);

            _funcionarioRepositorioMock.Verify(repositorio =>
                repositorio.AdicionarAsync(
                    It.Is<Funcionario>(s => s.Nome == _funcionarioDto.Nome)
                  ));
        }

        [Fact]
        public async Task NaoDeveAdicionarUmFuncionarioInvalido()
        {
            var funcionarioDto = new FuncionarioDto();
            _notificacaoDeDominioMock.Setup(notificacao => notificacao.HasNotifications()).Returns(true);

            await _armazenadorDeFuncionario.Armazenar(funcionarioDto);

            _funcionarioRepositorioMock.Verify(repositorio =>
                repositorio.AdicionarAsync(It.IsAny<Funcionario>()), Times.Never());
        }

        [Fact]
        public async Task DeveNotificarErroQuandoExistirUmOutroFuncionarioComMesmoNome()
        {
            const int idDoOutroFuncionario = 1050;
            var erroEsperado = string.Format(CommonResources.MsgDominioComMesmoNomeNoMasculino, CommonResources.FuncionarioDominio);
            var funcionarioSalva = FuncionarioBuilder.Novo().ComId(idDoOutroFuncionario).Build();

            _funcionarioRepositorioMock.Setup(repositorio =>
                repositorio.ObterPorNomeAsync(_funcionarioDto.Nome)).Returns(Task.FromResult(funcionarioSalva));

            await _armazenadorDeFuncionario.Armazenar(_funcionarioDto);

            _notificacaoDeDominioMock.Verify(notificacao =>
                notificacao.HandleAsync(It.Is<DomainNotification>(d => d.Value == erroEsperado)));
        }

        [Fact]
        public async Task DeveNotificarErrosDeDominioQuandoExistir()
        {
            var funcionarioDto = new FuncionarioDto();

            await _armazenadorDeFuncionario.Armazenar(funcionarioDto);

            _notificacaoDeDominioMock.Verify(notificacao =>
                notificacao.HandleAsync(It.Is<DomainNotification>(d => d.Key == TipoDeNotificacao.ErroDeDominio.ToString())));
        }

        [Fact]
        public async Task DeveEditarFuncionario()
        {
            CriarCenarioParaEditarFuncionario(out var funcionarioParaEdicao, out var funcionarioDto);

            await _armazenadorDeFuncionario.Armazenar(funcionarioDto);

            Assert.Equal(funcionarioDto.Nome, funcionarioParaEdicao.Nome);
            Assert.Equal(funcionarioDto.Cpf.RemoverMascaraCpfCnpj(), funcionarioParaEdicao.Cpf);
        }

        [Fact]
        public async Task QuandoEditarFuncionarioNaoDeveAdicionarNovamenteNoRepositorio()
        {
            CriarCenarioParaEditarFuncionario(out var funcionarioParaEdicao, out var funcionarioDto);

            await _armazenadorDeFuncionario.Armazenar(funcionarioDto);

            _funcionarioRepositorioMock.Verify(repositorio =>
               repositorio.AdicionarAsync(It.IsAny<Funcionario>()), Times.Never);
        }

        [Fact]
        public async Task DeveAdicionarUmaEmpresaAoFuncionario()
        {
            CriarCenarioParaEditarFuncionario(out var funcionarioParaEdicao, out _);

            await _armazenadorDeFuncionario.AdicionarEmpresa(funcionarioParaEdicao.Id, _idEmpresa);

            Assert.Equal(funcionarioParaEdicao.EmpresaId, _idEmpresa);
        }

        [Fact]
        public async Task NaoDeveAdicionarUmaEmpresaAoFuncionarioQuandoTiverErro()
        {
            _notificacaoDeDominioMock.Setup(notificacao => notificacao.HasNotifications()).Returns(true);

            CriarCenarioParaEditarFuncionario(out var funcionarioParaEdicao, out _);

            await _armazenadorDeFuncionario.AdicionarEmpresa(funcionarioParaEdicao.Id, _idEmpresa);

            Assert.NotEqual(funcionarioParaEdicao.EmpresaId, _idEmpresa);
        }

        [Fact]
        public async Task DeveNotificarErroAoVincularUmaEmpresaQuandoFuncionarioNaoCadastrado()
        {
            _notificacaoDeDominioMock.Setup(notificacao => notificacao.HasNotifications()).Returns(true);
            await _armazenadorDeFuncionario.AdicionarEmpresa(_funcionarioDto.Id, _idEmpresa);

            _notificacaoDeDominioMock.Verify(notificacao =>
              notificacao.HandleAsync(It.Is<DomainNotification>(d => 
                    d.Value == string.Format(CommonResources.MsgDominioNaoCadastradoNoMasculino, CommonResources.FuncionarioDominio))));
        }

        [Fact]
        public async Task DeveNotificarErroQuandoEmpresaNaoCadastrada()
        {
            CriarCenarioParaEditarFuncionario(out var funcionarioParaEdicao, out _);

            _notificacaoDeDominioMock.Setup(notificacao => notificacao.HasNotifications()).Returns(true);
            var empresaNaoCadastrada = 0;
            await _armazenadorDeFuncionario.AdicionarEmpresa(_funcionarioDto.Id, empresaNaoCadastrada);

            _notificacaoDeDominioMock.Verify(notificacao =>
              notificacao.HandleAsync(It.Is<DomainNotification>(d =>
                    d.Value == string.Format(CommonResources.MsgDominioNaoCadastradoNoFeminino, CommonResources.EmpresaDominio))));
        }

        [Fact]
        public async Task DeveAdicionarUmaCargoAoFuncionario()
        {
            CriarCenarioParaEditarFuncionario(out var funcionarioParaEdicao, out _, _idEmpresa);

            await _armazenadorDeFuncionario.AdicionarCargo(funcionarioParaEdicao.Id, _idCargo);

            Assert.Equal(funcionarioParaEdicao.CargoId, _idCargo);
        }

        [Fact]
        public async Task NaoDeveAdicionarUmCargoAoFuncionarioQuandoTiverErro()
        {
            _notificacaoDeDominioMock.Setup(notificacao => notificacao.HasNotifications()).Returns(true);

            CriarCenarioParaEditarFuncionario(out var funcionarioParaEdicao, out _);

            await _armazenadorDeFuncionario.AdicionarCargo(funcionarioParaEdicao.Id, _idCargo);

            Assert.NotEqual(funcionarioParaEdicao.CargoId, _idCargo);
        }

        [Fact]
        public async Task DeveNotificarErroAoVincularUmCargoQuandoFuncionarioNaoCadastrado()
        {
            _notificacaoDeDominioMock.Setup(notificacao => notificacao.HasNotifications()).Returns(true);
            await _armazenadorDeFuncionario.AdicionarCargo(_funcionarioDto.Id, _idCargo);

            _notificacaoDeDominioMock.Verify(notificacao =>
              notificacao.HandleAsync(It.Is<DomainNotification>(d =>
                    d.Value == string.Format(CommonResources.MsgDominioNaoCadastradoNoMasculino, CommonResources.FuncionarioDominio))));
        }

        [Fact]
        public async Task DeveNotificarErroQuandoFuncionarioNaoTiverEmpresaVinculada()
        {
            CriarCenarioParaEditarFuncionario(out var funcionarioParaEdicao, out _);
            _notificacaoDeDominioMock.Setup(notificacao => notificacao.HasNotifications()).Returns(true);

            await _armazenadorDeFuncionario.AdicionarCargo(funcionarioParaEdicao.Id, _idCargo);

            _notificacaoDeDominioMock.Verify(notificacao =>
              notificacao.HandleAsync(It.Is<DomainNotification>(d =>
                    d.Value == CommonResources.MsgFuncionarioNaoPossuiEmpresaVinculada)));
        }

        [Fact]
        public async Task DeveNotificarErroQuandoCargoNaoCadastrado()
        {
            _notificacaoDeDominioMock.Setup(notificacao => notificacao.HasNotifications()).Returns(true);
            var cargoNaoCadastrada = 0;
            await _armazenadorDeFuncionario.AdicionarCargo(_funcionarioDto.Id, cargoNaoCadastrada);

            _notificacaoDeDominioMock.Verify(notificacao =>
              notificacao.HandleAsync(It.Is<DomainNotification>(d =>
                    d.Value == string.Format(CommonResources.MsgDominioNaoCadastradoNoMasculino, CommonResources.CargoDominio))));
        }

        private void CriarCenarioParaEditarFuncionario(out Funcionario funcionarioParaEdicao, out FuncionarioDto funcionarioDto, int? idEmpresa = null)
        {
            var builder = FuncionarioBuilder.Novo().ComId(_idFuncionario);
            
            if (idEmpresa.HasValue)
            {
                builder.ComEmpresa(idEmpresa.Value);
            }

            funcionarioParaEdicao = builder.Build();
            funcionarioDto = new FuncionarioDto
            {
                Id = _idFuncionario,
                Nome = _faker.Lorem.Sentence(),
                Cpf = _faker.Person.Cpf(true)
            };

            _funcionarioRepositorioMock.Setup(r => r.ObterPorIdAsync(_idFuncionario))
                .ReturnsAsync(funcionarioParaEdicao);
        }
    }
}
