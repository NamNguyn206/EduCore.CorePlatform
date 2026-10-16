using EduCore.CorePlatform.Application.DTOs.Role;
using EduCore.CorePlatform.Domain.Common.Enums.MultiTenants;
using EduCore.CorePlatform.Domain.Entities.Identity;
using EduCore.CorePlatform.Domain.Entities.MultiTenants.Identities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Common.Services.Security;

public interface IUserAuthorizationService
{
    UserTenant? GetUserTenant(
        User user,
        Guid tenantId);

    IReadOnlyCollection<Role> GetRoles(
        User user,
        Guid tenantId);

    IReadOnlyCollection<Permission> GetPermissions(
        User user,
        Guid tenantId);

    IReadOnlyCollection<string> GetRoleCodes(
        User user,
        Guid tenantId);

    IReadOnlyCollection<string> GetPermissionCodes(
        User user,
        Guid tenantId);

    bool HasRole(
        User user,
        Guid tenantId,
        string roleCode);

    bool HasPermission(
        User user,
        Guid tenantId,
        string permissionCode);
}
public sealed class UserAuthorizationService
    : IUserAuthorizationService
{
    public UserTenant? GetUserTenant(
        User user,
        Guid tenantId)
    {
        return user.UserTenants
            .FirstOrDefault(x =>
                x.TenantId == tenantId &&
                x.LeftAt == null);
    }

    public IReadOnlyCollection<Role> GetRoles(
        User user,
        Guid tenantId)
    {
        var userTenant =
            GetUserTenant(user, tenantId);

        if (userTenant == null)
            return [];

        var tenantRoles = userTenant.Roles
            .Where(x =>
                x.Status ==
                PartnershipStatus.Approved)
            .Select(x => x.Role);

        var systemRoles = user.SystemRoles
            .Select(x => x.Role);

        return tenantRoles
            .Concat(systemRoles)
            .DistinctBy(x => x.Id)
            .ToList();
    }

    public IReadOnlyCollection<Permission> GetPermissions(
        User user,
        Guid tenantId)
    {
        return GetRoles(user, tenantId)
            .SelectMany(x => x.RolePermissions)
            .Select(x => x.Permission)
            .DistinctBy(x => x.Id)
            .ToList();
    }

    public IReadOnlyCollection<string> GetRoleCodes(
        User user,
        Guid tenantId)
    {
        return GetRoles(user, tenantId)
            .Select(x => x.Code)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    public IReadOnlyCollection<string> GetPermissionCodes(
        User user,
        Guid tenantId)
    {
        return GetPermissions(user, tenantId)
            .Select(x => x.Code)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    public bool HasRole(
        User user,
        Guid tenantId,
        string roleCode)
    {
        return GetRoles(user, tenantId)
            .Any(x =>
                x.Code.Equals(
                    roleCode,
                    StringComparison.OrdinalIgnoreCase));
    }

    public bool HasPermission(
        User user,
        Guid tenantId,
        string permissionCode)
    {
        return GetPermissions(user, tenantId)
            .Any(x =>
                x.Code.Equals(
                    permissionCode,
                    StringComparison.OrdinalIgnoreCase));
    }
}