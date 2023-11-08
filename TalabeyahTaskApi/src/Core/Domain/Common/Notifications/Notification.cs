namespace TalabeyahTaskApi.Domain.Common;

public class Notification : AuditableEntity, IAggregateRoot
{
    public string Text { get; private set; }

    public Notification(string text)
    {
        Text = text;
    }
}