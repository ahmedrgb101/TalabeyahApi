using TalabeyahTaskApi.Domain.Common;

namespace TalabeyahTaskApi.Domain.Catalog;

public class City : AuditableEntity<int>, IAggregateRoot
{
    public string Name { get; private set; }
    public int GovernorateId { get; private set; }

    public virtual Governorate Governorate { get; private set; }

    public City(string name, int governorateId)
    {
        Name = name;
        GovernorateId = governorateId;
    }

    public City Update(string name, int governorateId)
    {
        if (name is not null && Name?.Equals(name) is not true) Name = name;
        if (governorateId > 0 && GovernorateId.Equals(governorateId) is not true) GovernorateId = governorateId;
        return this;
    }
}