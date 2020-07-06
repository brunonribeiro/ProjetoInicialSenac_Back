using EmpresaApp.Domain.Entitys;
using EmpressaApp.Domain.Tests.Comum;

namespace EmpressaApp.Domain.Tests.Cargos
{
    public class CargoBuilder : BuilderBase
    {
        private string _descricao;
        private int _id;

        public static CargoBuilder Novo()
        {
            var fake = FakerBuilder.Novo().Build();

            return new CargoBuilder
            {
                _descricao = fake.Lorem.Sentence()
            };
        }

        public CargoBuilder ComId(int id)
        {
            _id = id;
            return this;
        }

        public CargoBuilder ComDescricao(string descricao)
        {
            _descricao = descricao;
            return this;
        }

        public Cargo Build()
        {
            var cargo = new Cargo(_descricao);
            AtribuirId(_id, cargo);
            return cargo;
        }
    }
}
