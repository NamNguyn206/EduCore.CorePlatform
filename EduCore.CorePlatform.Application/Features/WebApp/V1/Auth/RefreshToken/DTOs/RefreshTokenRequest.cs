using EduCore.CorePlatform.Domain.Common.Constants.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.Auth.RefreshToken.DTOs;

public sealed class RefreshTokenRequest
{
    public string RefreshToken { get; init; } = null!;
}