using TalabeyahTaskApi.Domain.Common;

namespace TalabeyahTaskApi.Domain.Catalog;

public class Governorate : AuditableEntity<int>, IAggregateRoot
{
    public string Name { get; private set; }
    public Governorate(string name)
    {
        Name = name;
    }

    public Governorate Update(string name)
    {
        if (name is not null && Name?.Equals(name) is not true) Name = name;
        return this;
    }
}