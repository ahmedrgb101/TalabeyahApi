using Microsoft.AspNetCore.Identity;
using TalabeyahTaskApi.Domain.Common;

namespace TalabeyahTaskApi.Infrastructure.Identity;
public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
    public UserType Type { get; set; }

    public string? ObjectId { get; set; }
}