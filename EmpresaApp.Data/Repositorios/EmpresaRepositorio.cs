using EmpresaApp.Domain.AutoMapper;
using EmpresaApp.Domain.Dto;
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

        public Task<EmpresaDto> ObterPorNomeAsync(string nome)
        {
            var result = _context.Empresas.FirstOrDefaultAsync(s => s.Nome.ToUpper().Trim() == nome.ToUpper().Trim());
            return result.MapTo<Task<EmpresaDto>>();
        }
    }
}