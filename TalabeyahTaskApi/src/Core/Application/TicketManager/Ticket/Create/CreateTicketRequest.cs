using TalabeyahTaskApi.Application.Common.Persistence;
using TalabeyahTaskApi.Domain.Common.Events;
using System.Linq;

namespace TalabeyahTaskApi.Application.TicketManager;
public class CreateTicketRequest : IRequest<Guid>
{
    public string PhoneNumber { get; set; }
    public int GovernorateId { get; set; }
    public int CityId { get; set; }
    public int DistrictId { get; set; }
}

public class CreateTicketRequestHandler : IRequestHandler<CreateTicketRequest, Guid>
{
    private readonly IRepository<Ticket> _repository;
    private readonly IStringLocalizer<CreateTicketRequestHandler> _localizer;

    public CreateTicketRequestHandler(IRepository<Ticket> repository, IStringLocalizer<CreateTicketRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<Guid> Handle(CreateTicketRequest request, CancellationToken cancellationToken)
    {
        var vacancie = new Ticket(request.PhoneNumber, request.GovernorateId, request.CityId, request.DistrictId);

        // Add Domain Events to be raised after the commit
        vacancie.DomainEvents.Add(EntityCreatedEvent.WithEntity(vacancie));

        await _repository.AddAsync(vacancie, cancellationToken);

        return vacancie.Id;
    }
}