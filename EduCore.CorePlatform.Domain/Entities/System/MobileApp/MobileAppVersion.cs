using EduCore.CorePlatform.Domain.Common.Enums.System.MobileApp;
using EduCore.CorePlatform.Domain.Common.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace EduCore.CorePlatform.Domain.Entities.System.MobileApp;

public class MobileAppVersion : BaseEntity<Guid>
{
    public string Platform { get; private set; } = string.Empty; // iOS, Android
    public string Version { get; private set; } = string.Empty;
    public MobileAppVersionStatus Status { get; private set; } = MobileAppVersionStatus.Active;    
    public bool IsRequired { get; private set; }
    public string? ReleaseNotes { get; private set; }
    public string? DownloadUrl { get; private set; }
    public DateTime ReleaseDate { get; private set; }
    public DateTime? EndOfSupportDate { get; private set; }
    public int BuildNumber { get; private set; }
    public string? Metadata { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    protected MobileAppVersion() { }

    public MobileAppVersion(string platform, string version, int buildNumber,
                           bool isRequired = false, string? releaseNotes = null,
                           string? downloadUrl = null)
    {
        Id = Guid.NewGuid();
        Platform = platform;
        Version = version;
        BuildNumber = buildNumber;
        IsRequired = isRequired;
        ReleaseNotes = releaseNotes;
        DownloadUrl = downloadUrl;
        ReleaseDate = DateTime.UtcNow;
        Status = MobileAppVersionStatus.Active;
        CreatedAt = DateTime.UtcNow;
    }

    public void MarkAsDeprecated()
    {
        Status = MobileAppVersionStatus.Deprecated;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsInactive()
    {
        Status = MobileAppVersionStatus.Inactive;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetRequired(bool isRequired)
    {
        IsRequired = isRequired;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetEndOfSupportDate(DateTime endDate)
    {
        EndOfSupportDate = endDate;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetMetadata(Dictionary<string, object> metadata)
    {
        Metadata = JsonSerializer.Serialize(metadata);
        UpdatedAt = DateTime.UtcNow;
    }

    public bool IsNewerThan(string otherVersion)
    {
        var currentVersion = new Version(Version);
        var other = new Version(otherVersion);
        return currentVersion > other;
    }

    public bool IsUpdateRequired(string currentVersion)
    {
        if (!IsRequired)
            return false;

        var current = new Version(currentVersion);
        var required = new Version(Version);
        return current < required;
    }
}

