namespace EmpresaApp.Domain.Notifications
{
    public class DomainNotification : Event
    {
        public string Key { get; }
        public string Value { get; }

        public DomainNotification(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
