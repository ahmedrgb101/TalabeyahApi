using TalabeyahTaskApi.Application.Common;
using TalabeyahTaskApi.Application.TicketManager;
using System.Security.Claims;

namespace TalabeyahTaskApi.Host.Controllers.Catalog;
public class GovernorateController : VersionedApiController
{
    [HttpPost("search")]
    [AllowAnonymous]
    [OpenApiOperation("Search Governorate using available filters.", "")]
    public Task<PaginationResponse<GovernorateDto>> SearchAsync(SearchGovernorateRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPost("searchAdmin")]
    [MustHavePermission(FSHAction.Create, FSHResource.Tickets)]
    [OpenApiOperation("Search Governorate for admin using available filters.", "")]
    public Task<PaginationResponse<GovernorateDto>> SearchAdminAsync(SearchGovernorateRequest request)
    {
        request.IsAdmin = true;
        return Mediator.Send(request);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    [OpenApiOperation("Get Governorate details.", "")]
    public Task<GovernorateDto> GetAsync(int id)
    {
        return Mediator.Send(new GetGovernorateRequest(id));
    }

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Tickets)]
    [OpenApiOperation("Create a new Governorate.", "")]
    public Task<int> CreateAsync(CreateGovernorateRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPut("{id:int}")]
    [MustHavePermission(FSHAction.Update, FSHResource.Tickets)]
    [OpenApiOperation("Update a Governorate.", "")]
    public async Task<ActionResult<int>> UpdateAsync(UpdateGovernorateRequest request, int id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    } 

    [HttpDelete("{id:int}")]
    [MustHavePermission(FSHAction.Delete, FSHResource.Tickets)]
    [OpenApiOperation("Delete a Governorate.", "")]
    public Task<int> DeleteAsync(int id)
    {
        return Mediator.Send(new DeleteGovernorateRequest(id));
    }
}