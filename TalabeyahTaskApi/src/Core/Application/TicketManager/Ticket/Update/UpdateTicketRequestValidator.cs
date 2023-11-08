using FluentValidation;
using MediatR;

namespace TalabeyahTaskApi.Application.TicketManager;

public class UpdateTicketRequestValidator : CustomValidator<UpdateTicketRequest>
{
    public UpdateTicketRequestValidator(IReadRepository<Ticket> TicketRepo, IStringLocalizer<UpdateTicketRequestValidator> localizer)
    {
        //RuleFor(p => p.Description)
        //    .NotEmpty()
        //    .MaximumLength(75)
        //    .MustAsync(async (Ticket, name, ct) =>
        //            await TicketRepo.GetBySpecAsync(new TicketByNameSpec(name), ct)
        //                is not Ticket existingTicket || existingTicket.Id == Ticket.Id)
        //        .WithMessage((_, name) => string.Format(localizer["Ticket.alreadyexists"], name));

        RuleFor(p => p.PhoneNumber)
            .NotEmpty();
    }
}