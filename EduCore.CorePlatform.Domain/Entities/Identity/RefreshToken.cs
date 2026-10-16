using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Domain.Common.Entities;

namespace EduCore.CorePlatform.Domain.Entities.Identity;

public sealed class RefreshToken : AuditableEntity<Guid>
{
    public Guid UserId { get; private set; }
    public string Token { get; private set; } = null!;
    public DateTime ExpiresAt { get; private set; }
    public bool IsRevoked { get; private set; }
    public DateTime? RevokedAt { get; private set; }    

    public string ClientType { get; private set; } = null!;
    public string? DeviceId { get; private set; }

    public bool IsExpired => !IsRevoked && ExpiresAt > DateTime.UtcNow;

    public User User { get; private set; } = null!;
    private RefreshToken() { }

    public RefreshToken(Guid userId, string token, DateTime expiresAt,
                        string clientType, string? deviceId)
    {
        UserId = userId;
        Token = token;
        ExpiresAt = expiresAt;
        ClientType = clientType;
        DeviceId = deviceId;
        IsRevoked = false;
    }
    public void Revoke()
    {
        IsRevoked = true;
        UpdatedAt = DateTime.UtcNow;
        RevokedAt = DateTime.UtcNow;
    }
}
