namespace TalabeyahTaskApi.Application.TicketManager;
public class TicketDto : IDto
{
    public Guid Id { get; set; }
    public string? PhoneNumber { get; set; }
    public int GovernrateId { get; set; }
    public int CityId { get; set; }
    public int DistrictId { get; set; }
    public string? Governrate { get; set; }
    public string? City { get; set; }
    public string? District { get; set; }
    public DateTime? CreateDateTime { get; set; }
    public bool IsHandled { get; set; }
    public string Color { get; set; }
}