using TalabeyahTaskApi.Domain.Common;
using System.Xml.Linq;

namespace TalabeyahTaskApi.Domain.Catalog;

public class Ticket : AuditableEntity, IAggregateRoot
{
    public string PhoneNumber { get; private set; }
    public int GovernorateId { get; private set; }
    public int CityId { get; private set; }
    public int DistrictId { get; private set; }
    public bool IsHandled { get; private set; }

    public virtual Governorate Governorate { get; private set; }
    public virtual City City { get; private set; }
    public virtual District District { get; private set; }
    public Ticket(string phoneNumber, int governorateId, int cityId, int districtId)
    {
        PhoneNumber = phoneNumber;
        GovernorateId = governorateId;
        CityId = cityId;
        DistrictId = districtId;
    }

    public Ticket Update(string phoneNumber, int governorateId, int cityId, int districtId)
    {
        if (phoneNumber is not null && PhoneNumber?.Equals(phoneNumber) is not true) PhoneNumber = phoneNumber;
        if (governorateId > 0) GovernorateId = governorateId;
        if (cityId > 0) CityId = cityId;
        if (districtId > 0) DistrictId = districtId;
        return this;
    }

    public Ticket Update(bool isHandled)
    {
        IsHandled = isHandled;
        return this;
    }
}