using System.Threading.Tasks;

namespace EmpresaApp.Domain.Interfaces.Gerais
{
    public interface IUnitOfWork
    {
        Task Commit();
    }
}
