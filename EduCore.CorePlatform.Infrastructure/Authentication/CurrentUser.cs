using EduCore.CorePlatform.Application.Abstractions.Authentication;
using EduCore.CorePlatform.Domain.Common.Constants.Auth;
using EduCore.CorePlatform.Domain.Common.Constants.System;
using EduCore.CorePlatform.Domain.Common.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace EduCore.CorePlatform.Infrastructure.Authentication;

public sealed class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    private HttpContext? Context => _httpContextAccessor.HttpContext;
    private ClaimsPrincipal? User => Context?.User;

    public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;

    public Guid UserId =>
        Guid.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    public Guid? TenantId =>
        Guid.TryParse(
            _httpContextAccessor.HttpContext!.Request.Headers[RequestHeaderTypeConstants.XTenantId],
            out var tenantId)
            ? tenantId
            : null;

    public Guid? UserTenantId =>
        Context?.Items.TryGetValue(HttpContextItemTypeConstants.UserTenantId, out var value) == true
            ? value as Guid?
            : null;

    public bool IsSuperAdmin =>
        User?.HasClaim(CustomClaimTypeConstants.IsSuperAdmin, "true") ?? false;
    public string? LanguageCode =>
        User?.FindFirst("language")?.Value;
}
