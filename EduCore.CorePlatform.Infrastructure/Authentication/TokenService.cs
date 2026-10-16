using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using EduCore.CorePlatform.Application.DTOs.Auth;
using EduCore.CorePlatform.Domain.Entities.Identity;
using EduCore.CorePlatform.Domain.Common.Enums.Identities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using EduCore.CorePlatform.Domain.Common.Constants.Auth;
using EduCore.CorePlatform.Domain.Common.Settings.Auth;
using EduCore.CorePlatform.Application.Abstractions.Authentication;

namespace EduCore.CorePlatform.Infrastructure.Authentication;

public sealed class TokenService : ITokenService
{
    private readonly JwtSettings _jwtSettings;

    public TokenService(IOptions<JwtSettings> options)
    {
        _jwtSettings = options.Value;
    }

    public AccessTokenResult GenerateAccessToken(User user)
    {
        var claims = BuildBaseClaims(user);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenMinutes);

        var tokenDescriptor = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials
        );

        var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

        return new AccessTokenResult
        {
            Token = token,
            ExpiresAt = expiresAt
        };
    }
    private static List<Claim> BuildBaseClaims(User user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(CustomClaimTypeConstants.UserType, user.UserType.ToString()),
        };

        if (user.UserType == UserType.SuperAdmin)
        {
            claims.Add(new Claim(CustomClaimTypeConstants.IsSuperAdmin, "true"));
        }

        return claims;
    }
    // =========================
    // REFRESH TOKEN
    // =========================
    public RefreshToken GenerateRefreshToken(
        Guid userId,
        string clientType,
        string? deviceId)
    {
        return new RefreshToken(
            userId,
            GenerateSecureToken(),
            DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenDays),
            clientType,
            deviceId
        );
    }

    private static string GenerateSecureToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);

        return Convert.ToBase64String(randomBytes);
    }
}
