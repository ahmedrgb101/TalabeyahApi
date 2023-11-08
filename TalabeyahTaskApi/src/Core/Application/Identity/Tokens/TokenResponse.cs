using TalabeyahTaskApi.Application.Identity.Users;

namespace TalabeyahTaskApi.Application.Identity.Tokens;

public record TokenResponse(string Token, string RefreshToken, List<string> Permissions, IList<string> Roles, DateTime RefreshTokenExpiryTime);