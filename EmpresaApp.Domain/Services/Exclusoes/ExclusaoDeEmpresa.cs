using EmpresaApp.Domain.Base;
using EmpresaApp.Domain.Entitys;
using EmpresaApp.Domain.Interfaces;

namespace EmpresaApp.Domain.Services.Exclusoes
{
    public class ExclusaoDeEmpresa : DomainService
    {
        private readonly IRepository<Empresa> _repository;

        public ExclusaoDeEmpresa(IRepository<Empresa> repository)
        {
            _repository = repository;
        }

        public void Excluir(int id)
        {
            var empresa = _repository.GetById(id);
            _repository.Remove(empresa);
        }
    }
}
