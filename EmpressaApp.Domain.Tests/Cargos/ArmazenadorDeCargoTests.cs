using Bogus;
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

namespace EmpressaApp.Domain.Tests.Cargos
{
    public class ArmazenadorDeCargoTests
    {
        private readonly CargoDto _cargoDto;
        private readonly ArmazenadorDeCargo _armazenadorDeCargo;
        private readonly Mock<ICargoRepositorio> _cargoRepositorioMock;
        private readonly Mock<IDomainNotificationHandlerAsync<DomainNotification>> _notificacaoDeDominioMock;
        private readonly Faker _faker;

        public ArmazenadorDeCargoTests()
        {
            _faker = FakerBuilder.Novo().Build();
            _cargoDto = new CargoDto
            {
                Descricao = _faker.Lorem.Sentence()
            };
            _notificacaoDeDominioMock = new Mock<IDomainNotificationHandlerAsync<DomainNotification>>();
            _cargoRepositorioMock = new Mock<ICargoRepositorio>();
            _armazenadorDeCargo = new ArmazenadorDeCargo(_cargoRepositorioMock.Object, _notificacaoDeDominioMock.Object);
        }

        [Fact]
        public async Task DeveAdicionarUmNovoCargo()
        {
            await _armazenadorDeCargo.Armazenar(_cargoDto);

            _cargoRepositorioMock.Verify(repositorio =>
                repositorio.AdicionarAsync(
                    It.Is<Cargo>(s => s.Descricao == _cargoDto.Descricao)));
        }

        [Fact]
        public async Task NaoDeveAdicionarUmCargoInvalido()
        {
            var cargoDto = new CargoDto();
            _notificacaoDeDominioMock.Setup(notificacao => notificacao.HasNotifications()).Returns(true);

            await _armazenadorDeCargo.Armazenar(cargoDto);

            _cargoRepositorioMock.Verify(repositorio =>
                repositorio.AdicionarAsync(It.IsAny<Cargo>()), Times.Never());
        }

        [Fact]
        public async Task DeveNotificarErroQuandoExistirUmOutroCargoComMesmoNome()
        {
            const int idDoOutroCargo = 1050;
            var erroEsperado = string.Format(CommonResources.MsgDominioComMesmoNomeNoMasculino, CommonResources.CargoDominio);
            var cargoSalvo = CargoBuilder.Novo().ComId(idDoOutroCargo).Build();
            _cargoRepositorioMock.Setup(repositorio =>
                repositorio.ObterPorDescricaoAsync(_cargoDto.Descricao)).Returns(Task.FromResult(cargoSalvo));

            await _armazenadorDeCargo.Armazenar(_cargoDto);

            _notificacaoDeDominioMock.Verify(notificacao =>
                notificacao.HandleAsync(It.Is<DomainNotification>(d => d.Value == erroEsperado)));
        }

        [Fact]
        public async Task DeveNotificarErrosDeDominioQuandoExistir()
        {
            var cargoDto = new CargoDto();

            await _armazenadorDeCargo.Armazenar(cargoDto);

            _notificacaoDeDominioMock.Verify(notificacao =>
                notificacao.HandleAsync(It.Is<DomainNotification>(d => d.Key == TipoDeNotificacao.ErroDeDominio.ToString())));
        }

        [Fact]
        public async Task DeveEditarCargo()
        {
            CriarCenarioParaEditarCargo(out var cargoParaEdicao, out var cargoDto);

            await _armazenadorDeCargo.Armazenar(cargoDto);

            Assert.Equal(cargoDto.Descricao, cargoParaEdicao.Descricao);
        }

        [Fact]
        public async Task QuandoEditarCargoNaoDeveAdicionarNovamenteNoRepositorio()
        {
            CriarCenarioParaEditarCargo(out var cargoParaEdicao, out var cargoDto);

            await _armazenadorDeCargo.Armazenar(cargoDto);

            _cargoRepositorioMock.Verify(repositorio =>
               repositorio.AdicionarAsync(It.IsAny<Cargo>()), Times.Never);
        }


        private void CriarCenarioParaEditarCargo(out Cargo cargoParaEdicao, out CargoDto cargoDto)
        {
            const int cargoId = 1;
            cargoParaEdicao = CargoBuilder.Novo().ComId(cargoId).Build();
            cargoDto = new CargoDto
            {
                Id = cargoId,
                Descricao = _faker.Lorem.Sentence(),
            };
            _cargoRepositorioMock.Setup(r => r.ObterPorIdAsync(cargoId))
                .ReturnsAsync(cargoParaEdicao);
        }

    }
}
