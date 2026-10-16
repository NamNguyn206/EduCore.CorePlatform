using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.DTOs.Identity;

namespace EduCore.CorePlatform.Application.Features.MobileApp.V1.Auth.Login.DTOs;

public sealed class LoginResponseDto
{
    public string AccessToken { get; init; } = default!;
    public string RefreshToken { get; init; } = default!;
    public DateTime ExpiresAt { get; init; }

    public UserProfileDto User { get; set; } = default!;
    public List<UserTenantDto> Tenants { get; set; } = new();
}
