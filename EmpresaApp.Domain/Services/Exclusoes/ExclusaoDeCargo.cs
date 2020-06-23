using EmpresaApp.Domain.Base;
using EmpresaApp.Domain.Entitys;
using EmpresaApp.Domain.Interfaces;

namespace EmpresaApp.Domain.Services.Exclusoes
{
    public class ExclusaoDeCargo : DomainService
    {
        private readonly IRepository<Cargo> _repository;

        public void Excluir(int id)
        {
            var cargo = _repository.GetById(id);
            _repository.Remove(cargo);
        }
    }
}
