using TalabeyahTaskApi.Domain.Common;

namespace TalabeyahTaskApi.Domain.Catalog;

public class District : AuditableEntity<int>, IAggregateRoot
{
    public string Name { get; private set; }
    public int CityId { get; private set; }

    public City City { get; private set; }

    public District(string name, int cityId)
    {
        Name = name;
        CityId = cityId;
    }

    public District Update(string name, int cityId)
    {
        if (name is not null && Name?.Equals(name) is not true) Name = name;
        if (cityId > 0 && CityId.Equals(cityId) is not true) CityId = cityId;
        return this;
    }
}