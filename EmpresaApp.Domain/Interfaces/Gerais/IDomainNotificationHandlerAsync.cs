using EmpresaApp.Domain.Notifications;
using System.Collections.Generic;

namespace EmpresaApp.Domain.Interfaces.Gerais
{
    public interface IDomainNotificationHandlerAsync<T> : IHandlerAsync<T> where T : Message
    {
        bool HasNotifications();
        List<T> GetNotifications();
        void Clean();
    }
}
