using TalabeyahTaskApi.Application.Common;
using TalabeyahTaskApi.Domain.Catalog;
using TalabeyahTaskApi.Domain.Common;
using TalabeyahTaskApi.Domain.Common.Events;

namespace TalabeyahTaskApi.Application.TicketManager;
public class UpdateTicketRequest : IRequest<Guid>
{
    public Guid Id { get; set; }
    public string PhoneNumber { get; set; }
    public int GovernorateId { get; set; }
    public int CityId { get; set; }
    public int DistrictId { get; set; }
}

public class UpdateTicketRequestHandler : IRequestHandler<UpdateTicketRequest, Guid>
{
    private readonly IRepository<Ticket> _repository;
    private readonly IStringLocalizer<UpdateTicketRequestHandler> _localizer;
    private readonly IFileStorageService _file;

    public UpdateTicketRequestHandler(IRepository<Ticket> repository, IStringLocalizer<UpdateTicketRequestHandler> localizer, IFileStorageService file) =>
        (_repository, _localizer, _file) = (repository, localizer, file);

    public async Task<Guid> Handle(UpdateTicketRequest request, CancellationToken cancellationToken)
    {
        var vacancie = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = vacancie ?? throw new NotFoundException(string.Format(_localizer["Ticket.notfound"], request.Id));

        vacancie.Update(request.PhoneNumber, request.GovernorateId, request.CityId, request.DistrictId);

        // Add Domain Events to be raised after the commit
        vacancie.DomainEvents.Add(EntityUpdatedEvent.WithEntity(vacancie));

        await _repository.UpdateAsync(vacancie, cancellationToken);

        return request.Id;
    }
}