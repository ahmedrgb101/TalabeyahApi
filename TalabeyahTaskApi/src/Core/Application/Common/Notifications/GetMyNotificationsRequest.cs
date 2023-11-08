using TalabeyahTaskApi.Application.Common;
using TalabeyahTaskApi.Application.Common.Persistence;
using TalabeyahTaskApi.Application.Identity.Users;
using System.Linq;

namespace TalabeyahTaskApi.Application.Auditing;

public class GetMyNotificationsRequest : PaginationFilter, IRequest<PaginationResponse<NotificationDto>>
{
    public string? UserId { get; set; }
}

public class GetMyNotificationsRequestHandler : IRequestHandler<GetMyNotificationsRequest, PaginationResponse<NotificationDto>>
{
    private readonly IRepository<UserNotification> _repository;
    private readonly IUserService _userService;

    public GetMyNotificationsRequestHandler(IUserService userService, IRepository<UserNotification> repository) =>
        (_userService, _repository) = (userService, repository);

    public async Task<PaginationResponse<NotificationDto>> Handle(GetMyNotificationsRequest request, CancellationToken cancellationToken)
    {
        return await _repository.Entities.Where(x => x.UserId == request.UserId).OrderByDescending(x => x.CreatedOn).Select(x => new NotificationDto
        {
            Id = x.Id,
            Text = x.Notification.Text,
            IsRead = x.IsRead,
            CreatedOn = x.CreatedOn,
        }).ToPaginatedListAsync(0, int.MaxValue);
    }
}