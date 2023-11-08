using TalabeyahTaskApi.Application.Common.Exporters;

namespace TalabeyahTaskApi.Application.TicketManager;
public class ExportTicketsRequest : BaseFilter, IRequest<Stream>
{
    public string? PhoneNumber { get;  set; }
}

public class ExportTicketsRequestHandler : IRequestHandler<ExportTicketsRequest, Stream>
{
    private readonly IReadRepository<Ticket> _repository;
    private readonly IExcelWriter _excelWriter;

    public ExportTicketsRequestHandler(IReadRepository<Ticket> repository, IExcelWriter excelWriter)
    {
        _repository = repository;
        _excelWriter = excelWriter;
    }

    public async Task<Stream> Handle(ExportTicketsRequest request, CancellationToken cancellationToken)
    {
        var spec = new ExportTicketsWithBrandsSpecification(request);

        var list = await _repository.ListAsync(spec, cancellationToken);

        return _excelWriter.WriteToStream(list);
    }
}

public class ExportTicketsWithBrandsSpecification : EntitiesByBaseFilterSpec<Ticket, TicketDto>
{
    public ExportTicketsWithBrandsSpecification(ExportTicketsRequest request)
        : base(request) =>
        Query
            .Where(p => p.PhoneNumber.Contains(request.PhoneNumber), request.PhoneNumber != null);
}