using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.MobileApp.V1.Auth.Logout.DTOs;

public sealed class LogoutRequest
{
    public string RefreshToken { get; init; } = default!;
}
