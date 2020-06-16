using System.Threading.Tasks;

namespace EmpresaApp.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        Task Commit();
    }
}
