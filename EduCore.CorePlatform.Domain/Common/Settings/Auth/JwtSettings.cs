using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace EduCore.CorePlatform.Domain.Common.Settings.Auth;

public class JwtSettings
{
    public const string SectionName = "Jwt";
    public string Issuer { get; init; } = null!;        
    public string Audience { get; init; } = null!;
    public string SecretKey { get; init; } = null!;
    public int AccessTokenMinutes { get; init; } = 60;
    public int RefreshTokenDays { get; init; } = 7;
}