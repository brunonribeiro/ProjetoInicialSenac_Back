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

namespace EmpressaApp.Domain.Tests.Empresas
{
    public class ArmazenadorDeEmpresaTests
    {
        private readonly EmpresaDto _empresaDto;
        private readonly ArmazenadorDeEmpresa _armazenadorDeEmpresa;
        private readonly Mock<IEmpresaRepositorio> _empresaRepositorioMock;
        private readonly Mock<IDomainNotificationHandlerAsync<DomainNotification>> _notificacaoDeDominioMock;
        private readonly Faker _faker;

        public ArmazenadorDeEmpresaTests()
        {
            _faker = FakerBuilder.Novo().Build();
            _empresaDto = new EmpresaDto
            {
                Nome = _faker.Lorem.Sentence(),
                Cnpj = _faker.Company.Cnpj(true)
            };

            _notificacaoDeDominioMock = new Mock<IDomainNotificationHandlerAsync<DomainNotification>>();
            _empresaRepositorioMock = new Mock<IEmpresaRepositorio>();
            _armazenadorDeEmpresa = new ArmazenadorDeEmpresa(_empresaRepositorioMock.Object, _notificacaoDeDominioMock.Object);
        }

        [Fact]
        public async Task DeveAdicionarUmaNovaEmpresa()
        {
            await _armazenadorDeEmpresa.Armazenar(_empresaDto);

            _empresaRepositorioMock.Verify(repositorio =>
                repositorio.AdicionarAsync(
                    It.Is<Empresa>(s => s.Nome == _empresaDto.Nome)
                  ));
        }

        [Fact]
        public async Task NaoDeveAdicionarUmaEmpresaInvalida()
        {
            var empresaDto = new EmpresaDto();
            _notificacaoDeDominioMock.Setup(notificacao => notificacao.HasNotifications()).Returns(true);

            await _armazenadorDeEmpresa.Armazenar(empresaDto);

            _empresaRepositorioMock.Verify(repositorio =>
                repositorio.AdicionarAsync(It.IsAny<Empresa>()), Times.Never());
        }

        [Fact]
        public async Task DeveNotificarErroQuandoExistirUmaOutroEmpresaComMesmoNome()
        {
            const int idDaOutroEmpresa = 1050;
            var erroEsperado = string.Format(CommonResources.MsgDominioComMesmoNomeNoFeminino, CommonResources.EmpresaDominio);
            var empresaSalva = EmpresaBuilder.Novo().ComId(idDaOutroEmpresa).Build();
            
            _empresaRepositorioMock.Setup(repositorio =>
                repositorio.ObterPorNomeAsync(_empresaDto.Nome)).Returns(Task.FromResult(empresaSalva));

            await _armazenadorDeEmpresa.Armazenar(_empresaDto);

            _notificacaoDeDominioMock.Verify(notificacao =>
                notificacao.HandleAsync(It.Is<DomainNotification>(d => d.Value == erroEsperado)));
        }

        [Fact]
        public async Task DeveNotificarErrosDeDominioQuandoExistir()
        {
            var empresaDto = new EmpresaDto();

            await _armazenadorDeEmpresa.Armazenar(empresaDto);

            _notificacaoDeDominioMock.Verify(notificacao =>
                notificacao.HandleAsync(It.Is<DomainNotification>(d => d.Key == TipoDeNotificacao.ErroDeDominio.ToString())));
        }

        [Fact]
        public async Task DeveEditarEmpresa()
        {
            CriarCenarioParaEditarEmpresa(out var empresaParaEdicao, out var empresaDto);

            await _armazenadorDeEmpresa.Armazenar(empresaDto);

            Assert.Equal(empresaDto.Nome, empresaParaEdicao.Nome);
            Assert.Equal(empresaDto.Cnpj.RemoverMascaraCpfCnpj(), empresaParaEdicao.Cnpj);
        }

        [Fact]
        public async Task QuandoEditarEmpresaNaoDeveAdicionarNovamenteNoRepositorio()
        {
            CriarCenarioParaEditarEmpresa(out var empresaParaEdicao, out var empresaDto);

            await _armazenadorDeEmpresa.Armazenar(empresaDto);

            _empresaRepositorioMock.Verify(repositorio =>
               repositorio.AdicionarAsync(It.IsAny<Empresa>()), Times.Never);
        }

        private void CriarCenarioParaEditarEmpresa(out Empresa empresaParaEdicao, out EmpresaDto empresaDto)
        {
            const int empresaId = 1;
            empresaParaEdicao = EmpresaBuilder.Novo().ComId(empresaId).Build();
            empresaDto = new EmpresaDto
            {
                Id = empresaId,
                Nome = _faker.Lorem.Sentence(),
                Cnpj = _faker.Company.Cnpj(true)
            };
            _empresaRepositorioMock.Setup(r => r.ObterPorIdAsync(empresaId))
                .ReturnsAsync(empresaParaEdicao);
        }
    }
}
