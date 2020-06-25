using System;

namespace EmpresaApp.Domain.Notifications
{
    public abstract class Event : Message
    {
        public DateTime Timestamp { get; }

        protected Event()
        {
            Timestamp = DateTime.Now;
        }
    }
}