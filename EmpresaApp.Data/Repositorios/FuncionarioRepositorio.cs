using EmpresaApp.Domain.Entitys;
using EmpresaApp.Domain.Interfaces.Repositorios;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmpresaApp.Data.Repositorios
{
    public class FuncionarioRepositorio : Repository<int, Funcionario>, IFuncionarioRepositorio
    {
        private readonly DataContext _context;
        public FuncionarioRepositorio(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Funcionario>> ObterListaFuncionarioComEmpresaECargo()
        {
            return await _context.Funcionarios
                .Include("Empresa")
                .Include("Cargo")
                .ToListAsync();
        }

        public Task<Funcionario> ObterPorNomeAsync(string nome)
        {
            return _context.Funcionarios.FirstOrDefaultAsync(s => s.Nome.ToUpper().Trim() == nome.ToUpper().Trim());
        }
    }
}
