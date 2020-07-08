using Bogus;
using Bogus.Extensions.Brazil;
using EmpresaApp.Domain.Entitys;
using EmpresaApp.Domain.Utils;
using EmpressaApp.Domain.Tests.Comum;
using Xunit;

namespace EmpressaApp.Domain.Tests.Empresas
{
    public class EmpresaTests
    {
        private readonly Faker _faker;
        private readonly string _nome;
        private readonly string _cnpj;
        private readonly int _tamanhoDeEspacos = 5;


        public EmpresaTests()
        {
            _faker = FakerBuilder.Novo().Build();
            _nome = _faker.Random.AlphaNumeric(Constantes.QuantidadeDeCaracteres100);
            _cnpj = _faker.Company.Cnpj(false);
        }

        [Fact]
        public void DeveCriarEmpresa()
        {
            var empresaEsperada = EmpresaBuilder.Novo().Build();

            var empresa = new Empresa(
              empresaEsperada.Nome,
              empresaEsperada.Cnpj
            );

            Assert.Equal(empresaEsperada.Nome, empresa.Nome);
            Assert.Equal(empresaEsperada.Cnpj, empresa.Cnpj);
        }

        [Fact]
        public void NaoDeveCriarEmpresaComEspacosAntesDoNome()
        {
            var nomeComEspacoAntes = _nome.PadLeft(_tamanhoDeEspacos);
            var empresa = EmpresaBuilder.Novo()
                .ComNome(nomeComEspacoAntes)
                .Build();

            Assert.Equal(_nome, empresa.Nome);
        }

        [Fact]
        public void NaoDeveCriarEmpresaComEspacosDepoisDoNome()
        {
            var nomeComEspacoDepois = _nome.PadRight(_tamanhoDeEspacos);
            var empresa = EmpresaBuilder.Novo()
                .ComNome(nomeComEspacoDepois)
                .Build();

            Assert.Equal(_nome, empresa.Nome);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void DeveValidarNomeComoObrigatorio(string nomeInvalido)
        {
            var empresa = EmpresaBuilder.Novo()
                .ComNome(nomeInvalido)
                .Build();

            var resultado = empresa.Validar();

            Assert.False(resultado);
        }

        [Theory]
        [InlineData(101)]
        public void DeveValidarNomeTamanhoMaximoExcedido(int tamanhoMaximo)
        {
            var nomeInvalido = _faker.Random.AlphaNumeric(tamanhoMaximo);
            var empresa = EmpresaBuilder.Novo()
                .ComNome(nomeInvalido)
                .Build();

            var resultado = empresa.Validar();

            Assert.False(resultado);
        }

        [Theory]
        [InlineData(50)]
        public void NaoDeveValidarNomeTamanhoMaximoExcedido(int tamanhoMaximo)
        {
            var nomeValido = _faker.Random.AlphaNumeric(tamanhoMaximo);
            var empresa = EmpresaBuilder.Novo()
                .ComNome(nomeValido)
                .Build();

            var resultado = empresa.Validar();

            Assert.True(resultado);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void DeveValidarCnpjComoObrigatorio(string cnpjInvalido)
        {
            var empresa = EmpresaBuilder.Novo()
                .ComCnpj(cnpjInvalido)
                .Build();

            var resultado = empresa.Validar();

            Assert.False(resultado);
        }

        [Fact]
        public void NaoDeveCriarEmpresaComEspacosAntesDoCnpj()
        {
            var cnpjComEspacoAntes = _cnpj.PadLeft(_tamanhoDeEspacos);
            var empresa = EmpresaBuilder.Novo()
                .ComCnpj(cnpjComEspacoAntes)
                .Build();

            Assert.Equal(_cnpj, empresa.Cnpj);
        }

        [Fact]
        public void NaoDeveCriarEmpresaComEspacosDepoisDoCnpj()
        {
            var cnpjComEspacoDepois = _cnpj.PadRight(_tamanhoDeEspacos);
            var empresa = EmpresaBuilder.Novo()
                .ComCnpj(cnpjComEspacoDepois)
                .Build();

            Assert.Equal(_cnpj, empresa.Cnpj);
        }

        [Fact]
        public void NaoDeveCriarEmpresaComMascaraNoCnpj()
        {
            var cnpjComMascara = _faker.Company.Cnpj(true);
            var empresa = EmpresaBuilder.Novo()
                .ComCnpj(cnpjComMascara)
                .Build();

            Assert.Equal(cnpjComMascara.Replace(".", "").Replace("-", "").Replace("/", ""), empresa.Cnpj);
        }

        [Theory]
        [InlineData(13)]
        [InlineData(15)]
        public void DeveValidarCnpjTamanhoInvalido(int tamanho)
        {
            var cnpjInvalido = _faker.Random.AlphaNumeric(tamanho);
            var empresa = EmpresaBuilder.Novo()
                .ComCnpj(cnpjInvalido)
                .Build();

            var resultado = empresa.Validar();

            Assert.False(resultado);
        }

        [Fact]
        public void DeveAlterarNome()
        {
            var nome = _faker.Random.Word();
            var empresa = EmpresaBuilder.Novo().Build();

            empresa.AlterarNome(nome);

            Assert.Equal(nome, empresa.Nome);
        }

        [Fact]
        public void NaoDeveEditarFuncionarioComEspacosDepoisEAntesDoNome()
        {
            var funcionario = EmpresaBuilder.Novo().Build();

            funcionario.AlterarNome(VariaveisDeTeste.TextoComEspacoAntesEDepois);

            Assert.Equal(VariaveisDeTeste.TextoComEspacoAntesEDepois.Trim(), funcionario.Nome);
        }

        [Fact]
        public void DeveAlterarCnpj()
        {
            var cnpj = _faker.Company.Cnpj(false);
            var empresa = EmpresaBuilder.Novo().Build();

            empresa.AlterarCnpj(cnpj);

            Assert.Equal(cnpj, empresa.Cnpj);
        }

        [Fact]
        public void DeveAlterarDataFundacao()
        {
            var data = _faker.Date.Past();
            var empresa = EmpresaBuilder.Novo().Build();

            empresa.AlterarDataFundacao(data.ToShortDateString());

            Assert.Equal(data.Date, empresa.DataFundacao.Value.Date);
        }
    }
}
