namespace TalabeyahTaskApi.Application.TicketManager;

public class GetTicketRequest : IRequest<TicketDto>
{
    public Guid Id { get; set; }

    public GetTicketRequest(Guid id) => Id = id;
}

public class GetTicketRequestHandler : IRequestHandler<GetTicketRequest, TicketDto>
{
    private readonly IRepository<Ticket> _repository;
    private readonly IStringLocalizer<GetTicketRequestHandler> _localizer;

    public GetTicketRequestHandler(IRepository<Ticket> repository, IStringLocalizer<GetTicketRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<TicketDto> Handle(GetTicketRequest request, CancellationToken cancellationToken) =>
        _repository.Entities.Select(x => new TicketDto
        {
            Id = x.Id,
            PhoneNumber = x.PhoneNumber,
            IsHandled = x.IsHandled,
            CityId = x.CityId,
            DistrictId = x.DistrictId,
            GovernrateId = x.GovernorateId,
        })
            .FirstOrDefault(p => p.Id == request.Id)
        ?? throw new NotFoundException(string.Format(_localizer["Ticket.notfound"], request.Id));
}