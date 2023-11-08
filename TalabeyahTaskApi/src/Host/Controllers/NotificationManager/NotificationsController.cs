using TalabeyahTaskApi.Application.Auditing;
using TalabeyahTaskApi.Application.Common;
using System.Security.Claims;

namespace TalabeyahTaskApi.Host.Controllers.Catalog;
public class NotificationsController : VersionedApiController
{
    [HttpGet("notifications")]
    [OpenApiOperation("Get notifications of currently logged in user.", "")]
    public Task<PaginationResponse<NotificationDto>> GetNotificationsAsync()
    {
        return Mediator.Send(new GetMyNotificationsRequest() { UserId = User.GetUserId() });
    }

    [HttpPatch("markAllAsRead")]
    [OpenApiOperation("Mark All  notifications AsRead for currently logged in user.", "")]
    public Task<bool> MarkAllAsReadAsync()
    {
        return Mediator.Send(new MarkAllMyNotificationsAsReadRequest() { UserId = User.GetUserId() });
    }

    [HttpPut("markAllAsRead/{id}")]
    [OpenApiOperation("Mark All  notifications AsRead for currently logged in user.", "")]
    public Task<bool> MarkAsReadAsync(Guid id)
    {
        return Mediator.Send(new MarkNotificationsAsReadRequest() { UserId = User.GetUserId(), Id = id });
    }
}