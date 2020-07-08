using Bogus.Extensions.Brazil;
using EmpressaApp.Domain.Tests.Comum;
using EmpresaApp.Domain.Entitys;
using EmpresaApp.Domain.Utils;

namespace EmpressaApp.Domain.Tests.Funcionarios
{
    public class FuncionarioBuilder : BuilderBase
    {
        private int _id;
        private string _nome;
        private string _cpf;
        private int? _idEmpresa;

        public static FuncionarioBuilder Novo()
        {
            var faker = FakerBuilder.Novo().Build();

            return new FuncionarioBuilder
            {
                _nome = faker.Random.AlphaNumeric(Constantes.QuantidadeDeCaracteres100),
                _cpf = faker.Person.Cpf(false)
            };
        }

        public FuncionarioBuilder ComId(int id)
        {
            _id = id;
            return this;
        }

        public FuncionarioBuilder ComNome(string nome)
        {
            _nome = nome;
            return this;
        }

        public FuncionarioBuilder ComCpf(string cpf)
        {
            _cpf = cpf;
            return this;
        }

        public FuncionarioBuilder ComEmpresa(int idEmpresa)
        {
            _idEmpresa = idEmpresa;
            return this;
        }

        public Funcionario Build()
        {
            var funcionario = new Funcionario(_nome, _cpf);

            AtribuirId(_id, funcionario);

            if (_idEmpresa.HasValue)
                funcionario.AlterarEmpresa(_idEmpresa.Value);

            return funcionario;
        }
    }
}
