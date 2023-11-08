namespace TalabeyahTaskApi.Domain.Common;

public class UserNotification : AuditableEntity, IAggregateRoot
{
    public string UserId { get; private set; }
    public Guid NotificationId { get; private set; }
    public bool IsRead { get; private set; }

    public virtual Notification Notification { get; private set; }

    public UserNotification(string userId, Guid notificationId)
    {
        UserId = userId;
        NotificationId = notificationId;
    }

    public UserNotification Update(bool isRead)
    {
        IsRead = isRead;
        return this;
    }
}