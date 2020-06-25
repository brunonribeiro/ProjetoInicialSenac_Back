using EmpresaApp.Domain.Notifications;
using System.Threading.Tasks;

namespace EmpresaApp.Domain.Interfaces.Gerais
{
    public interface IHandlerAsync<in T> where T : Message
    {
        Task HandleAsync(T message);
    }
}
