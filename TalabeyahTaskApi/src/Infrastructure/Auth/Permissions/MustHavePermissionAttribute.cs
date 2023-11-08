using Microsoft.AspNetCore.Authorization;
using TalabeyahTaskApi.Shared.Authorization;

namespace TalabeyahTaskApi.Infrastructure.Auth.Permissions;
public class MustHavePermissionAttribute : AuthorizeAttribute
{
    public MustHavePermissionAttribute(string action, string resource) =>
        Policy = FSHPermission.NameFor(action, resource);
}