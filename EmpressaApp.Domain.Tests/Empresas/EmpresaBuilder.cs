using Bogus.Extensions.Brazil;
using EmpresaApp.Domain.Entitys;
using EmpresaApp.Domain.Utils;
using EmpressaApp.Domain.Tests.Comum;

namespace EmpressaApp.Domain.Tests.Empresas
{
    public class EmpresaBuilder : BuilderBase
    {
        private int _id;
        private string _nome;
        private string _cnpj;
        private string _dataFundacao;

        public static EmpresaBuilder Novo()
        {
            var faker = FakerBuilder.Novo().Build();

            return new EmpresaBuilder
            {
                _nome = faker.Random.AlphaNumeric(Constantes.QuantidadeDeCaracteres100),
                _cnpj = faker.Company.Cnpj(false),
            };
        }

        public EmpresaBuilder ComId(int id)
        {
            _id = id;
            return this;
        }

        public EmpresaBuilder ComNome(string nome)
        {
            _nome = nome;
            return this;
        }

        public EmpresaBuilder ComCnpj(string cnpj)
        {
            _cnpj = cnpj;
            return this;
        }

        public EmpresaBuilder ComDataFundacao(string dataFundacao)
        { 
            _dataFundacao = dataFundacao;
            return this;
        }

        public Empresa Build()
        {
            var empresa = new Empresa(_nome, _cnpj);

            AtribuirId(_id, empresa);

            if (string.IsNullOrEmpty(_dataFundacao))
                empresa.AlterarDataFundacao(_dataFundacao);

            return empresa;
        }
    }
}
