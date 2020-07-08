using Bogus;
using EmpresaApp.Domain.Entitys;
using EmpressaApp.Domain.Tests.Comum;
using Xunit;

namespace EmpressaApp.Domain.Tests.Cargos
{
    public class CargoTests
    {
        private readonly Faker _faker;
        private readonly string _descricao;
        private readonly int _tamanhoDeEspacos = 5;
        private readonly string _nomeComEspaco = VariaveisDeTeste.TextoComEspacoAntesEDepois;

        public CargoTests()
        {
            _faker = FakerBuilder.Novo().Build();
            _descricao = _faker.Lorem.Paragraph();
        }

        [Fact]
        public void DeveCriarCargo()
        {
            var cargoEsperado = CargoBuilder.Novo().Build();

            var cargo = new Cargo(cargoEsperado.Descricao);

            Assert.Equal(cargoEsperado.Descricao, cargo.Descricao);
        }

        [Fact]
        public void NaoDeveCriarCargoComEspacosAntesDaDescricao()
        {
            var cargo = CargoBuilder.Novo().ComDescricao(_descricao.PadLeft(_tamanhoDeEspacos)).Build();
            Assert.Equal(_descricao, cargo.Descricao);
        }

        [Fact]
        public void NaoDeveCriarCargoComEspacosDepoisDaDescricao()
        {
            var cargo = CargoBuilder.Novo().ComDescricao(_descricao.PadRight(_tamanhoDeEspacos)).Build();
            Assert.Equal(_descricao, cargo.Descricao);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void DeveValidarDescricaoObrigatoriaQuandoCriar(string descricaoInvalida)
        {
            var cargo = CargoBuilder.Novo().ComDescricao(descricaoInvalida).Build();

            var resultado = cargo.Validar();

            Assert.False(resultado);
        }

        [Fact]
        public void DeveAlterarDescricao()
        {
            var descricao = _faker.Random.Word();
            var cargo = CargoBuilder.Novo().Build();

            cargo.AlterarDescricao(descricao);

            Assert.Equal(descricao, cargo.Descricao);
        }

        [Fact]
        public void NaoDeveEditarCargoComEspacosDepoisEAntesDaDescricao()
        {
            var cargo = CargoBuilder.Novo().Build();

            cargo.AlterarDescricao(_nomeComEspaco);

            Assert.Equal(_nomeComEspaco.Trim(), cargo.Descricao);
        }
    }
}
