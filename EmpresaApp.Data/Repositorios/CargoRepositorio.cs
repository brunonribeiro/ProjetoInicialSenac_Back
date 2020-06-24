using EmpresaApp.Domain.Entitys;
using EmpresaApp.Domain.Interfaces.Repositorios;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EmpresaApp.Data.Repositorios
{
    public class CargoRepositorio : Repository<int, Cargo>, ICargoRepositorio
    {
        private readonly DataContext _context;

        public CargoRepositorio(DataContext context) : base(context)
        {
            _context = context;
        }

        public Task<Cargo> ObterPorDescricaoAsync(string descricao)
        {
            return _context.Cargos.FirstOrDefaultAsync(s => s.Descricao.ToUpper().Trim() == descricao.ToUpper().Trim());
        }
    }
}
