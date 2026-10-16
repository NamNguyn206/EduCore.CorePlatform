using EduCore.CorePlatform.Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Abstractions.Authentication;

public interface ICurrentUser
{
    Guid UserId { get; }
    Guid? TenantId { get; }
    Guid? UserTenantId { get; }      // 👈 BẮT BUỘC cho authorization
    bool IsAuthenticated { get; }
    bool IsSuperAdmin { get; }
    string? LanguageCode { get; }

    //Task<bool> HasPermissionAsync(string permission);
    //Task<bool> HasAnyPermissionAsync(IEnumerable<string> permissions);
    //Task<bool> HasAllPermissionsAsync(IEnumerable<string> permissions);
    //Task<List<string>> GetPermissionsAsync();
    //Task<bool> IsInRoleAsync(string role);
    //Task<bool> HasAccessToTenantAsync(Guid tenantId);
}