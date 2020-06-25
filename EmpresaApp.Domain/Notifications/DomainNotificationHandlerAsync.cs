﻿using EmpresaApp.Domain.Interfaces.Gerais;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmpresaApp.Domain.Notifications
{
    public class DomainNotificationHandlerAsync : IDomainNotificationHandlerAsync<DomainNotification>, IHandlerAsync<DomainNotification>
    {
        private List<DomainNotification> _notifications;

        public DomainNotificationHandlerAsync()
        {
            _notifications = new List<DomainNotification>();
        }

        public Task HandleAsync(DomainNotification message)
        {
            _notifications.Add(message);
            return Task.CompletedTask;
        }

        public List<DomainNotification> GetNotifications()
        {
            return _notifications;
        }

        public bool HasNotifications()
        {
            return GetNotifications().Any();
        }

        public void Clean()
        {
            _notifications = new List<DomainNotification>();
        }

        public void Dispose()
        {
            Clean();
        }
    }
}
