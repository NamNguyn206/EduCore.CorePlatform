using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.DTOs.Auth;

public sealed class AccessTokenResult
{
    public string Token { get; init; } = null!;
    public DateTime ExpiresAt { get; init; }
}
