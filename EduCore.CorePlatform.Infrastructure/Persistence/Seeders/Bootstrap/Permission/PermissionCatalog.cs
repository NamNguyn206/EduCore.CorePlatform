using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using EduCore.CorePlatform.Domain.Common.Constants.Auth;
using EduCore.CorePlatform.Domain.Common.Enums.Identities;

namespace EduCore.CorePlatform.Infrastructure.Persistence.Seeders.Bootstrap.Permission;

public static class PermissionCatalog
{
    public static IReadOnlyCollection<PermissionDefinition> All
        => _all.Value;

    private static readonly Lazy<IReadOnlyCollection<PermissionDefinition>> _all
        = new(BuildPermissions);

    private static IReadOnlyCollection<PermissionDefinition> BuildPermissions()
    {
        var permissions = new List<PermissionDefinition>();

        var rootType = typeof(PermissionConstants);

        // 1. Scan root-level const strings
        permissions.AddRange(
            ScanConstStrings(rootType, RoleScope.System));

        // 2. Scan nested static classes
        foreach (var nestedType in rootType.GetNestedTypes(
                     BindingFlags.Public | BindingFlags.Static))
        {
            var scope = InferScopeFromPermission(nestedType.Name);

            permissions.AddRange(
                ScanConstStrings(nestedType, scope));
        }

        return permissions;
    }

    private static IEnumerable<PermissionDefinition> ScanConstStrings(
        Type type,
        RoleScope scope)
    {
        return type
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(f => f.IsLiteral && !f.IsInitOnly && f.FieldType == typeof(string))
            .Select(f =>
            {
                var code = (string)f.GetRawConstantValue()!;

                return new PermissionDefinition(
                    Code: code,
                    Name: ToHumanReadable(f.Name),
                    Description: code,
                    Scope: scope
                );
            });
    }

    // ===============================
    // HELPERS
    // ===============================

    private static RoleScope InferScopeFromPermission(string groupName)
    {
        // SYSTEM SCOPE
        if (groupName.Equals(nameof(PermissionConstants.System), StringComparison.OrdinalIgnoreCase)
            || groupName.Equals(nameof(PermissionConstants.Roles), StringComparison.OrdinalIgnoreCase)
            || groupName.Equals(nameof(PermissionConstants.Permission), StringComparison.OrdinalIgnoreCase))
        {
            return RoleScope.System;
        }

        // TENANT SCOPE (DEFAULT)
        return RoleScope.Tenant;
    }

    private static string ToHumanReadable(string name)
    {
        // ViewProfile → View Profile
        return string.Concat(
            name.Select((c, i) =>
                i > 0 && char.IsUpper(c) ? " " + c : c.ToString()));
    }
}
