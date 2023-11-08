using TalabeyahTaskApi.Shared.Events;

namespace TalabeyahTaskApi.Domain.Common.Contracts;
public abstract class DomainEvent : IEvent
{
    public DateTime TriggeredOn { get; protected set; } = DateTime.UtcNow;
}