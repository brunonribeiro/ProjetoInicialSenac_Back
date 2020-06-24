using EmpresaApp.Domain.Base;
using EmpresaApp.Domain.Entitys;
using EmpresaApp.Domain.Interfaces;

namespace EmpresaApp.Domain.Services.Exclusoes
{
    public class ExclusaoDeFuncionario : DomainService
    {
        private readonly IRepository<Funcionario> _repository;

        public ExclusaoDeFuncionario(IRepository<Funcionario> repository)
        {
            _repository = repository;
        }

        public void Excluir(int id)
        {
            var funcionario = _repository.GetById(id);
            _repository.Remove(funcionario);
        }
    }
}
