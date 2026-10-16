using EduCore.CorePlatform.Domain.Common.Constants;
using EduCore.CorePlatform.Domain.Common.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace EduCore.CorePlatform.Domain.Entities.System.MobileApp;

public class MobileDeviceInfo : MultiTenantEntity<Guid, Guid>
{    
    public Guid UserId { get; private set; }
    public string DeviceId { get; private set; } = string.Empty;
    public string DeviceName { get; private set; } = string.Empty;
    public string DeviceModel { get; private set; } = string.Empty;
    public string Platform { get; private set; } = string.Empty; // iOS, Android
    public string OsVersion { get; private set; } = string.Empty;
    public string AppVersion { get; private set; } = string.Empty;
    public string? PushToken { get; private set; }
    public bool IsActive { get; private set; } = true;
    public bool IsPushEnabled { get; private set; }
    public DateTime LastActiveAt { get; private set; }
    public DateTime RegisteredAt { get; private set; }
    public DateTime? UnregisteredAt { get; private set; }
    public string? DeviceProperties { get; private set; } // JSON additional properties

    // Navigation properties
    public virtual Identity.User User { get; private set; } = null!;

    protected MobileDeviceInfo() { }

    public MobileDeviceInfo(Guid tenantId, Guid userId, string deviceId, string deviceName,
                           string deviceModel, string platform, string osVersion, string appVersion)
        : base(Guid.NewGuid(), tenantId)
    {
        UserId = userId;
        DeviceId = deviceId;
        DeviceName = deviceName;
        DeviceModel = deviceModel;
        Platform = platform;
        OsVersion = osVersion;
        AppVersion = appVersion;
        RegisteredAt = DateTime.UtcNow;
        LastActiveAt = DateTime.UtcNow;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateLastActive()
    {
        LastActiveAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateAppVersion(string appVersion)
    {
        AppVersion = appVersion;
        LastActiveAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateOsVersion(string osVersion)
    {
        OsVersion = osVersion;
        LastActiveAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetPushToken(string pushToken)
    {
        PushToken = pushToken;
        IsPushEnabled = !string.IsNullOrEmpty(pushToken);
        UpdatedAt = DateTime.UtcNow;
    }

    public void ClearPushToken()
    {
        PushToken = null;
        IsPushEnabled = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        UnregisteredAt = null;
        LastActiveAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UnregisteredAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetDeviceProperties(Dictionary<string, object> properties)
    {
        DeviceProperties = JsonSerializer.Serialize(properties);
        UpdatedAt = DateTime.UtcNow;
    }

    public bool IsSameDevice(string deviceId, string platform)
    {
        return DeviceId == deviceId && Platform == platform;
    }
}