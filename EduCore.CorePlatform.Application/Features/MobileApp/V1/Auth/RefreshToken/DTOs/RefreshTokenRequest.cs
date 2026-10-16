using EduCore.CorePlatform.Domain.Common.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.MobileApp.V1.Auth.RefreshToken.DTOs;

public sealed class RefreshTokenRequest
{
    public string RefreshToken { get; init; } = null!;
    public string? DeviceId { get; init; }
}