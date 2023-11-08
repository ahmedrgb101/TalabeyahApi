using TalabeyahTaskApi.Application.Common;
using TalabeyahTaskApi.Application.TicketManager;
using System.Security.Claims;

namespace TalabeyahTaskApi.Host.Controllers.Catalog;
public class CityController : VersionedApiController
{
    [HttpPost("search")]
    [AllowAnonymous]
    [OpenApiOperation("Search City using available filters.", "")]
    public Task<PaginationResponse<CityDto>> SearchAsync(SearchCityRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    [OpenApiOperation("Get City details.", "")]
    public Task<CityDto> GetAsync(int id)
    {
        return Mediator.Send(new GetCityRequest(id));
    }

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Tickets)]
    [OpenApiOperation("Create a new City.", "")]
    public Task<int> CreateAsync(CreateCityRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPut("{id:int}")]
    [MustHavePermission(FSHAction.Update, FSHResource.Tickets)]
    [OpenApiOperation("Update a City.", "")]
    public async Task<ActionResult<int>> UpdateAsync(UpdateCityRequest request, int id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    } 

    [HttpDelete("{id:int}")]
    [MustHavePermission(FSHAction.Delete, FSHResource.Tickets)]
    [OpenApiOperation("Delete a City.", "")]
    public Task<int> DeleteAsync(int id)
    {
        return Mediator.Send(new DeleteCityRequest(id));
    }
}