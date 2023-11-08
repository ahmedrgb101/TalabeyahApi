using Mapster;
using TalabeyahTaskApi.Application.Common;
using TalabeyahTaskApi.Application.Common.Models;
using TalabeyahTaskApi.Application.Identity.Users;
using System.Linq;

namespace TalabeyahTaskApi.Application.TicketManager;

public class SearchTicketsRequest : PaginationFilter, IRequest<PaginationResponse<TicketDto>>
{
    public Guid UserId { get; set; }
}

public class SearchTicketsRequestHandler : IRequestHandler<SearchTicketsRequest, PaginationResponse<TicketDto>>
{
    private readonly IReadRepository<Ticket> _repository;
    private readonly IUserService _userService;

    public SearchTicketsRequestHandler(IReadRepository<Ticket> repository, IUserService userService)
    {
        _repository = repository;
        _userService = userService;
    }

    public async Task<PaginationResponse<TicketDto>> Handle(SearchTicketsRequest request, CancellationToken cancellationToken)
    {
        var list = await _repository.Entities
            .Where(x => x.CreatedBy == request.UserId)
            .Select(x => new TicketDto
            {
                Id = x.Id,
                PhoneNumber = x.PhoneNumber,
                Governrate = x.Governorate.Name,
                City = x.City.Name,
                District = x.District.Name,
                IsHandled = x.IsHandled,
                CreateDateTime = x.CreatedOn,
                CityId = x.CityId,
                DistrictId = x.DistrictId,
                GovernrateId = x.GovernorateId,
                Color = DateTime.Now.Subtract(x.CreatedOn).TotalHours >= 1 ? "Red" :
                (DateTime.Now.Subtract(x.CreatedOn).TotalMinutes >= 45 ? "Blue" : (DateTime.Now.Subtract(x.CreatedOn).TotalMinutes >= 30 ? "Green" : string.Empty)),
            })
             .ToPaginatedListAsync(request.PageNumber, request.PageSize);
        return list;
    }
}