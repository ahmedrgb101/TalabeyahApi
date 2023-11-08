using TalabeyahTaskApi.Application.TicketManager;
using TalabeyahTaskApi.Application.Common;
using System.Security.Claims;

namespace TalabeyahTaskApi.Host.Controllers.Catalog;
public class TicketController : VersionedApiController
{
    [HttpPost("search")]
    [MustHavePermission(FSHAction.Search, FSHResource.Tickets)]
    [OpenApiOperation("Search Ticket using available filters.", "")]
    public Task<PaginationResponse<TicketDto>> SearchAsync(SearchTicketsRequest request)
    {
        request.UserId = new Guid(User.GetUserId());
        return Mediator.Send(request);
    }

    [HttpPatch("handleTicket/{id:guid}")]
    [MustHavePermission(FSHAction.ApplyToTicket, FSHResource.Tickets)]
    [OpenApiOperation("Handle Ticket.", "")]
    public Task<Guid> HandleTicketAsync(Guid id)
    {
        HandleTicketRequest request = new HandleTicketRequest() { Id = id };

        return Mediator.Send(request);
    }

    [HttpPatch("unHandleTicket/{id:guid}")]
    [MustHavePermission(FSHAction.ApplyToTicket, FSHResource.Tickets)]
    [OpenApiOperation("unhandle ticket.", "")]
    public Task<Guid> UnHandleTicketAsync(Guid id)
    {
        UnHandleTicketRequest request = new UnHandleTicketRequest() { Id = id };

        return Mediator.Send(request);
    }

    [HttpGet("{id:guid}")]
    [MustHavePermission(FSHAction.View, FSHResource.Tickets)]
    [OpenApiOperation("Get Ticket details.", "")]
    public Task<TicketDto> GetAsync(Guid id)
    {
        return Mediator.Send(new GetTicketRequest(id));
    }

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Tickets)]
    [OpenApiOperation("Create a new Ticket.", "")]
    public Task<Guid> CreateAsync(CreateTicketRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPut("{id:guid}")]
    [MustHavePermission(FSHAction.Update, FSHResource.Tickets)]
    [OpenApiOperation("Update a Ticket.", "")]
    public async Task<ActionResult<Guid>> UpdateAsync(UpdateTicketRequest request, Guid id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    }

    [HttpDelete("{id:guid}")]
    [MustHavePermission(FSHAction.Delete, FSHResource.Tickets)]
    [OpenApiOperation("Delete a Ticket.", "")]
    public Task<Guid> DeleteAsync(Guid id)
    {
        return Mediator.Send(new DeleteTicketRequest(id));
    }

    [HttpPost("export")]
    [MustHavePermission(FSHAction.Export, FSHResource.Tickets)]
    [OpenApiOperation("Export Tickets.", "")]
    public async Task<FileResult> ExportAsync(ExportTicketsRequest filter)
    {
        var result = await Mediator.Send(filter);
        return File(result, "application/octet-stream", "Tickets");
    }
}