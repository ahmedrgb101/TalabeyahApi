using TalabeyahTaskApi.Application.Common;
using TalabeyahTaskApi.Application.Common.Persistence;
using TalabeyahTaskApi.Application.Identity.Users;
using TalabeyahTaskApi.Domain.Common;

namespace TalabeyahTaskApi.Application.Auditing;

public class MarkNotificationsAsReadRequest : IRequest<bool>
{
    public Guid Id { get; set; }
    public string? UserId { get; set; }
}

public class MarkNotificationsAsReadRequestHandler : IRequestHandler<MarkNotificationsAsReadRequest, bool>
{
    private readonly IRepository<UserNotification> _repository;
    private readonly IUserService _userService;

    public MarkNotificationsAsReadRequestHandler(IUserService userService, IRepository<UserNotification> repository) =>
        (_userService, _repository) = (userService, repository);

    public async Task<bool> Handle(MarkNotificationsAsReadRequest request, CancellationToken cancellationToken)
    {
        var notification = _repository.Entities.Where(x => x.Id == request.Id).FirstOrDefault();

        notification.Update(true);
        await _repository.UpdateAsync(notification);

        return true;
    }
}