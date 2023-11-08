using System.Collections.ObjectModel;

namespace TalabeyahTaskApi.Shared.Authorization;
public static class FSHRoles
{
    public const string Admin = nameof(Admin);
    public const string Basic = nameof(Basic);
    public const string Employer = nameof(Employer);
    public const string Applicant = nameof(Applicant);

    public static IReadOnlyList<string> DefaultRoles { get; } = new ReadOnlyCollection<string>(new[]
    {
        Admin,
        Basic,
        Employer,
        Applicant
    });

    public static bool IsDefault(string roleName) => DefaultRoles.Any(r => r == roleName);
}