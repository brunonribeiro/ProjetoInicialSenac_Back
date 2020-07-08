using Bogus;
using Bogus.Extensions.Brazil;
using EmpresaApp.Domain.Entitys;
using EmpressaApp.Domain.Tests.Comum;
using Xunit;

namespace EmpressaApp.Domain.Tests.Funcionarios
{
    public class FuncionarioTests
    {
        private readonly Faker _faker;
        private readonly string _nome;
        private readonly string _cpf;
        private readonly int _tamanhoDeEspacos = 5;

        public FuncionarioTests()
        {
            _faker = FakerBuilder.Novo().Build();
            _nome = _faker.Lorem.Paragraph();
            _cpf = _faker.Person.Cpf(false);
        }

        [Fact]
        public void DeveCriarFuncionario()
        {
            var funcionarioEsperado = FuncionarioBuilder.Novo().Build();

            var funcionario = new Funcionario(
                funcionarioEsperado.Nome,
                funcionarioEsperado.Cpf
             );

            Assert.Equal(funcionarioEsperado.Nome, funcionario.Nome);
            Assert.Equal(funcionarioEsperado.Cpf, funcionario.Cpf);
        }

        [Fact]
        public void NaoDeveCriarFuncionarioComEspacosAntesDoNome()
        {
            var funcionario = FuncionarioBuilder.Novo().ComNome(_nome.PadLeft(_tamanhoDeEspacos)).Build();
            Assert.Equal(_nome, funcionario.Nome);
        }

        [Fact]
        public void NaoDeveCriarFuncionarioComEspacosDepoisDoNome()
        {
            var funcionario = FuncionarioBuilder.Novo().ComNome(_nome.PadRight(_tamanhoDeEspacos)).Build();
            Assert.Equal(_nome, funcionario.Nome);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void DeveValidarNomeObrigatorioQuandoCriar(string nomeInvalido)
        {
            var funcionario = FuncionarioBuilder.Novo().ComNome(nomeInvalido).Build();

            var resultado = funcionario.Validar();

            Assert.False(resultado);
        }

        [Theory]
        [InlineData(101)]
        public void DeveValidarNomeTamanhoMaximoExcedido(int tamanhoMaximo)
        {
            var nomeInvalido = _faker.Random.AlphaNumeric(tamanhoMaximo);
            var funcionario = FuncionarioBuilder.Novo()
                .ComNome(nomeInvalido)
                .Build();

            var resultado = funcionario.Validar();

            Assert.False(resultado);
        }

        [Theory]
        [InlineData(50)]
        public void NaoDeveValidarNomeTamanhoMaximoExcedido(int tamanhoMaximo)
        {
            var nomeInvalido = _faker.Random.AlphaNumeric(tamanhoMaximo);
            var funcionario = FuncionarioBuilder.Novo()
                .ComNome(nomeInvalido)
                .Build();

            var resultado = funcionario.Validar();

            Assert.True(resultado);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void DeveValidarCpfComoObrigatorio(string cpfInvalido)
        {
            var empresa = FuncionarioBuilder.Novo()
                .ComCpf(cpfInvalido)
                .Build();

            var resultado = empresa.Validar();

            Assert.False(resultado);
        }

        [Fact]
        public void NaoDeveCriarfuncionarioComEspacosAntesDoCpf()
        {
            var cpfComEspacoAntes = _cpf.PadLeft(_tamanhoDeEspacos);
            var funcionario = FuncionarioBuilder.Novo()
                .ComCpf(cpfComEspacoAntes)
                .Build();

            Assert.Equal(_cpf, funcionario.Cpf);
        }

        [Fact]
        public void NaoDeveCriarfuncionarioComEspacosDepoisDoCpf()
        {
            var cpfComEspacoDepois = _cpf.PadRight(_tamanhoDeEspacos);
            var funcionario = FuncionarioBuilder.Novo()
                .ComCpf(cpfComEspacoDepois)
                .Build();

            Assert.Equal(_cpf, funcionario.Cpf);
        }

        [Fact]
        public void NaoDeveCriarfuncionarioComMascaraNoCpf()
        {
            var cpfComMascara = _faker.Person.Cpf(true);
            var funcionario = FuncionarioBuilder.Novo()
                .ComCpf(cpfComMascara)
                .Build();

            Assert.Equal(cpfComMascara.Replace(".", "").Replace("-", ""), funcionario.Cpf);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(12)]
        public void DeveValidarCnpjTamanhoInvalido(int tamanho)
        {
            var cpfInvalido = _faker.Random.AlphaNumeric(tamanho);
            var funcionario = FuncionarioBuilder.Novo()
                .ComCpf(cpfInvalido)
                .Build();

            var resultado = funcionario.Validar();

            Assert.False(resultado);
        }

        [Fact]
        public void DeveAlterarNome()
        {
            var nome = _faker.Random.Word();
            var funcionario = FuncionarioBuilder.Novo().Build();

            funcionario.AlterarNome(nome);

            Assert.Equal(nome, funcionario.Nome);
        }

        [Fact]
        public void NaoDeveEditarFuncionarioComEspacosDepoisEAntesDoNome()
        {
            var funcionario = FuncionarioBuilder.Novo().Build();

            funcionario.AlterarNome(VariaveisDeTeste.TextoComEspacoAntesEDepois);

            Assert.Equal(VariaveisDeTeste.TextoComEspacoAntesEDepois.Trim(), funcionario.Nome);
        }

        [Fact]
        public void DeveAlterarCpf()
        {
            var cpf = _faker.Person.Cpf(false);
            var funcionario = FuncionarioBuilder.Novo().Build();

            funcionario.AlterarCpf(cpf);

            Assert.Equal(cpf, funcionario.Cpf);
        }

        [Fact]
        public void DeveAlterarDataContratacao()
        {
            var data = _faker.Date.Past();
            var funcionario = FuncionarioBuilder.Novo().Build();

            funcionario.AlterarDataContratacao(data.ToShortDateString());

            Assert.Equal(data.Date, funcionario.DataContratacao.Value.Date);
        }

        [Fact]
        public void DeveAlterarEmpresa()
        {
            var empresaId = _faker.Random.Number(100);
            var funcionario = FuncionarioBuilder.Novo().Build();

            funcionario.AlterarEmpresa(empresaId);

            Assert.Equal(empresaId, funcionario.EmpresaId);
        }

        [Fact]
        public void DeveAlterarCargo()
        {
            var cargoId = _faker.Random.Number(100);
            var funcionario = FuncionarioBuilder.Novo().Build();

            funcionario.AlterarCargo(cargoId);

            Assert.Equal(cargoId, funcionario.CargoId);
        }
    }
}
