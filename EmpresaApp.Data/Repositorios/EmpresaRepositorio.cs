using EmpresaApp.Domain.Entitys;
using EmpresaApp.Domain.Interfaces.Repositorios;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EmpresaApp.Data.Repositorios
{
    public class EmpresaRepositorio : Repository<int, Empresa>, IEmpresaRepositorio
    {
        private readonly DataContext _context;
        public EmpresaRepositorio(DataContext context) : base(context)
        {
            _context = context;
        }

        public Task<Empresa> ObterPorNomeAsync(string nome)
        {
            return _context.Empresas.FirstOrDefaultAsync(s => s.Nome.ToUpper().Trim() == nome.ToUpper().Trim());
        }
    }
}