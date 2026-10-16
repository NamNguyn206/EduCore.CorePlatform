using EduCore.CorePlatform.Application.DTOs.Auth;
using EduCore.CorePlatform.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Abstractions.Authentication;

public interface ITokenService
{
    AccessTokenResult GenerateAccessToken(User user);
    RefreshToken GenerateRefreshToken(Guid userId, string clientType, string? deviceId);
}
