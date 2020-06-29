using EmpresaApp.Domain.Base;
using EmpresaApp.Domain.Entitys;

namespace EmpresaApp.Domain.Specification
{
    public class ListarFuncionarioSpecificationBuilder : SpecificationBuilder<Funcionario>
    {
        public static ListarFuncionarioSpecificationBuilder Novo()
        {
            return new ListarFuncionarioSpecificationBuilder();
        }

        public override Specification<Funcionario> Build()
        {
            return new Specification<Funcionario>(
                    s =>
                        string.IsNullOrEmpty(Nome) || s.Nome.ToLower().Contains(Nome.ToLower())                   
                )
                .OrderBy(OrdenarPor)
                .Sorting(Ordem)
                .Paging(Pagina)
                .PageSize(TamanhoDaPagina);
        }
    }
}
