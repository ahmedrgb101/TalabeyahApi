using TalabeyahTaskApi.Domain.Common.Events;

namespace TalabeyahTaskApi.Application.TicketManager;
public class UnHandleTicketRequest : IRequest<Guid>
{
    public Guid Id { get; set; }
}

public class UnHandleTicketRequestHandler : IRequestHandler<UnHandleTicketRequest, Guid>
{
    private readonly IRepository<Ticket> _repository;
    private readonly IStringLocalizer<UnHandleTicketRequestHandler> _localizer;
    private readonly IFileStorageService _file;

    public UnHandleTicketRequestHandler(IRepository<Ticket> repository, IStringLocalizer<UnHandleTicketRequestHandler> localizer, IFileStorageService file) =>
        (_repository, _localizer, _file) = (repository, localizer, file);

    public async Task<Guid> Handle(UnHandleTicketRequest request, CancellationToken cancellationToken)
    {
        var ticket = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = ticket ?? throw new NotFoundException(string.Format(_localizer["Ticket.notfound"], request.Id));

        var updatedTicket = ticket.Update(false);

        // Add Domain Events to be raised after the commit
        ticket.DomainEvents.Add(EntityUpdatedEvent.WithEntity(ticket));

        await _repository.UpdateAsync(updatedTicket, cancellationToken);

        return request.Id;
    }
}