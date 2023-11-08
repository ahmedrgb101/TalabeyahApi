using TalabeyahTaskApi.Application.Common;
using System.Security.Claims;
using TalabeyahTaskApi.Application.TicketManager;

namespace TalabeyahTaskApi.Host.Controllers.Catalog;
public class DistrictController : VersionedApiController
{
    [HttpPost("search")]
    [AllowAnonymous]
    [OpenApiOperation("Search District using available filters.", "")]
    public Task<PaginationResponse<DistrictDto>> SearchAsync(SearchDistrictRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPost("searchAdmin")]
    [MustHavePermission(FSHAction.Create, FSHResource.Tickets)]
    [OpenApiOperation("Search District for admin using available filters.", "")]
    public Task<PaginationResponse<DistrictDto>> SearchAdminAsync(SearchDistrictRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    [OpenApiOperation("Get District details.", "")]
    public Task<DistrictDto> GetAsync(int id)
    {
        return Mediator.Send(new GetDistrictRequest(id));
    }

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Tickets)]
    [OpenApiOperation("Create a new District.", "")]
    public Task<int> CreateAsync(CreateDistrictRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPut("{id:int}")]
    [MustHavePermission(FSHAction.Update, FSHResource.Tickets)]
    [OpenApiOperation("Update a District.", "")]
    public async Task<ActionResult<int>> UpdateAsync(UpdateDistrictRequest request, int id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    } 

    [HttpDelete("{id:int}")]
    [MustHavePermission(FSHAction.Delete, FSHResource.Tickets)]
    [OpenApiOperation("Delete a District.", "")]
    public Task<int> DeleteAsync(int id)
    {
        return Mediator.Send(new DeleteDistrictRequest(id));
    }
}