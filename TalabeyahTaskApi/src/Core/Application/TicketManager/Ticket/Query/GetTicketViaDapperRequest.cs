using Mapster;

namespace TalabeyahTaskApi.Application.TicketManager;
public class GetTicketViaDapperRequest : IRequest<TicketDto>
{
    public Guid Id { get; set; }

    public GetTicketViaDapperRequest(Guid id) => Id = id;
}

public class GetTicketViaDapperRequestHandler : IRequestHandler<GetTicketViaDapperRequest, TicketDto>
{
    private readonly IDapperRepository _repository;
    private readonly IStringLocalizer<GetTicketViaDapperRequestHandler> _localizer;

    public GetTicketViaDapperRequestHandler(IDapperRepository repository, IStringLocalizer<GetTicketViaDapperRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<TicketDto> Handle(GetTicketViaDapperRequest request, CancellationToken cancellationToken)
    {
        var Ticket = await _repository.QueryFirstOrDefaultAsync<Ticket>(
            $"SELECT * FROM public.\"Tickets\" WHERE \"Id\"  = '{request.Id}' AND \"Tenant\" = '@tenant'", cancellationToken: cancellationToken);

        _ = Ticket ?? throw new NotFoundException(string.Format(_localizer["Ticket.notfound"], request.Id));

        return Ticket.Adapt<TicketDto>();
    }
}