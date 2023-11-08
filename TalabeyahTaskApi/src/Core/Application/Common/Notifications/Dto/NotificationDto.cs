namespace TalabeyahTaskApi.Application.Common;

public class NotificationDto : IDto
{
    public Guid Id { get; set; }
    public string Text { get; set; } = default!;
    public bool IsRead { get; set; }
    public DateTime CreatedOn { get; set; }
}