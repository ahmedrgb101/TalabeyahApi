using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using TalabeyahTaskApi.Application.Common.Caching;
using TalabeyahTaskApi.Application.Common.Exceptions;
using TalabeyahTaskApi.Application.Common.Interfaces;
using TalabeyahTaskApi.Application.Identity.Users;
using TalabeyahTaskApi.Domain.Common;
using TalabeyahTaskApi.Shared.Authorization;
using TalabeyahTaskApi.Shared.Notifications;

namespace TalabeyahTaskApi.Infrastructure.Identity;
internal partial class UserService
{
    public async Task<List<string>> GetPermissionsAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId);

        _ = user ?? throw new NotFoundException(_localizer["User Not Found."]);

        var userRoles = await _userManager.GetRolesAsync(user);
        var permissions = new List<string>();
        foreach (var role in await _roleManager.Roles
            .Where(r => userRoles.Contains(r.Name))
            .ToListAsync(cancellationToken))
        {
            permissions.AddRange(await _db.RoleClaims
                .Where(rc => rc.RoleId == role.Id && rc.ClaimType == FSHClaims.Permission)
                .Select(rc => rc.ClaimValue)
                .ToListAsync(cancellationToken));
        }

        return permissions.Distinct().ToList();
    }

    public async Task<List<string>> GetUsersHasPermissionAsync(List<string> permission, CancellationToken cancellationToken)
    {
        var roles = await _db.RoleClaims
                .Where(rc => permission.Contains(rc.ClaimValue) && rc.ClaimType == FSHClaims.Permission)
                .Select(rc => rc.RoleId)
                .Distinct()
                .ToListAsync(cancellationToken);

        return await _db.UserRoles.Where(x => roles.Contains(x.RoleId)).Select(x => x.UserId).Distinct().ToListAsync(cancellationToken);
    }

    public async Task SendNotificationAsync(string message, List<string> permissions, CancellationToken cancellationToken)
    {
        var notification = await _db.Notification.AddAsync(new Notification(message));

        var usersId = await GetUsersHasPermissionAsync(permissions, cancellationToken);

        foreach (string uid in usersId)
        {
            await _db.UserNotification.AddAsync(new UserNotification(uid, notification.Entity.Id));
        }

        await _db.SaveChangesAsync();

        await _notifications.SendToUsersAsync(
          new BasicNotification()
          {
              Message = "New",
              Label = BasicNotification.LabelType.Success,
          }, usersId,
          cancellationToken);
    }

    public async Task<bool> HasPermissionAsync(string userId, string permission, CancellationToken cancellationToken)
    {
        var permissions = await _cache.GetOrSetAsync(
            _cacheKeys.GetCacheKey(FSHClaims.Permission, userId),
            () => GetPermissionsAsync(userId, cancellationToken),
            cancellationToken: cancellationToken);

        return permissions?.Contains(permission) ?? false;
    }

    public Task InvalidatePermissionCacheAsync(string userId, CancellationToken cancellationToken) =>
        _cache.RemoveAsync(_cacheKeys.GetCacheKey(FSHClaims.Permission, userId), cancellationToken);
}