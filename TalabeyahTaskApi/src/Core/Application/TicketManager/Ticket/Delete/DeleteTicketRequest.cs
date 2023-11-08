using TalabeyahTaskApi.Domain.Common.Events;

namespace TalabeyahTaskApi.Application.TicketManager;
public class DeleteTicketRequest : IRequest<Guid>
{
    public Guid Id { get; set; }

    public DeleteTicketRequest(Guid id) => Id = id;
}

public class DeleteTicketRequestHandler : IRequestHandler<DeleteTicketRequest, Guid>
{
    private readonly IRepository<Ticket> _repository;
    private readonly IStringLocalizer<DeleteTicketRequestHandler> _localizer;

    public DeleteTicketRequestHandler(IRepository<Ticket> repository,IStringLocalizer<DeleteTicketRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<Guid> Handle(DeleteTicketRequest request, CancellationToken cancellationToken)
    {
        var vacancie = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = vacancie ?? throw new NotFoundException(_localizer["Ticket.notfound"]);

        // Add Domain Events to be raised after the commit
        vacancie.DomainEvents.Add(EntityDeletedEvent.WithEntity(vacancie));

        await _repository.DeleteAsync(vacancie, cancellationToken);

        return request.Id;
    }
}