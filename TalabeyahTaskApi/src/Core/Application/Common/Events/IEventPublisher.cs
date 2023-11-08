using TalabeyahTaskApi.Shared.Events;

namespace TalabeyahTaskApi.Application.Common.Events;
public interface IEventPublisher : ITransientService
{
    Task PublishAsync(IEvent @event);
}