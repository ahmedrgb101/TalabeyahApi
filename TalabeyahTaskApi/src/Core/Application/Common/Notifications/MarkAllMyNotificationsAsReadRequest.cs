using TalabeyahTaskApi.Application.Common;
using TalabeyahTaskApi.Application.Common.Persistence;
using TalabeyahTaskApi.Application.Identity.Users;
using TalabeyahTaskApi.Domain.Common;

namespace TalabeyahTaskApi.Application.Auditing;

public class MarkAllMyNotificationsAsReadRequest : IRequest<bool>
{
    public string? UserId { get; set; }
}

public class MarkAllMyNotificationsAsReadRequestHandler : IRequestHandler<MarkAllMyNotificationsAsReadRequest, bool>
{
    private readonly IRepository<UserNotification> _repository;
    private readonly IUserService _userService;

    public MarkAllMyNotificationsAsReadRequestHandler(IUserService userService, IRepository<UserNotification> repository) =>
        (_userService, _repository) = (userService, repository);

    public async Task<bool> Handle(MarkAllMyNotificationsAsReadRequest request, CancellationToken cancellationToken)
    {
        var notifications = _repository.Entities.Where(x => x.UserId == request.UserId).ToList();

        foreach (var notification in notifications)
        {
            notification.Update(true);
            await _repository.UpdateAsync(notification);
        }

        return true;
    }
}