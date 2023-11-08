namespace TalabeyahTaskApi.Application.TicketManager;

public class TicketByPhoneSpec : Specification<Ticket>, ISingleResultSpecification
{
    public TicketByPhoneSpec(string phone) =>
        Query.Where(p => p.PhoneNumber.Contains(phone));
}