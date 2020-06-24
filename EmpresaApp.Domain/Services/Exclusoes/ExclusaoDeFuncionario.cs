using EmpresaApp.Domain.Base;
using EmpresaApp.Domain.Entitys;
using EmpresaApp.Domain.Interfaces.Gerais;
using EmpresaApp.Domain.Interfaces.Repositorios;
using System.Threading.Tasks;

namespace EmpresaApp.Domain.Services.Exclusoes
{
    public class ExclusaoDeFuncionario : DomainService
    {
        private readonly IFuncionarioRepositorio _funcionarioRepositorio;

        public ExclusaoDeFuncionario(IFuncionarioRepositorio funcionarioRepositorio)
        {
            _funcionarioRepositorio = funcionarioRepositorio;
        }

        public async Task Excluir(int id)
        {
            var funcionario = await _funcionarioRepositorio.ObterPorIdAsync(id);
            _funcionarioRepositorio.Remover(funcionario);
        }
    }
}
