namespace TalabeyahTaskApi.Application.TicketManager;
public class DistrictDto : IDto
{
    public int Id { get; set; }
    public int CityId { get; set; }
    public string? Name { get; set; }
}