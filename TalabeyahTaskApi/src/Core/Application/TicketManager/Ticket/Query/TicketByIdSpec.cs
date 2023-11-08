namespace TalabeyahTaskApi.Application.TicketManager;
using System.Linq;

public class TicketByIdSpec : Specification<Ticket, TicketDto>, ISingleResultSpecification
{
    public TicketByIdSpec(Guid Id)
    {
        Query
            .Select(x => new TicketDto
            {
                Id = x.Id,
                PhoneNumber = x.PhoneNumber,
                IsHandled = x.IsHandled,
            })
            .Where(p => p.Id == Id);
    }
}