namespace TalabeyahTaskApi.Application.TicketManager;
public class CityDto : IDto
{
    public int Id { get; set; }
    public int GovernorateId { get; set; }
    public string? Name { get; set; }
}