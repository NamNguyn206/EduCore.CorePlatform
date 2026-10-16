using EduCore.CorePlatform.Domain.Common.Entities;
using EduCore.CorePlatform.Domain.Common.Enums.System;
using EduCore.CorePlatform.Domain.Entities.Notifications;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EduCore.CorePlatform.Domain.Entities.System.Files;

public class FileStorage : MultiTenantEntity<Guid, Guid>
{
    public StorageType? StorageType { get; private set; }
    public string FileName { get; private set; } = default!;
    public string FileUrl { get; set; } = default!;
    public string ContentType { get; set; } = default!;
    public long FileSize { get; set; }
    public string? Description { get; private set; }
    public int DownloadCount { get; private set; }
    public DateTime? LastDownloadedAt { get; private set; }
    public Guid? NotificationId { get; private set; }

    // Navigation properties
    public virtual Notification? Notification { get; private set; }

    protected FileStorage() { }

    public FileStorage(Guid tenantId, StorageType storageType, string fileName, string fileUrl,
                        string contentType, long fileSize)
        : base(Guid.NewGuid(), tenantId)
    {
        StorageType = storageType;
        FileName = fileName;
        FileUrl = fileUrl;
        ContentType = contentType;
        FileSize = fileSize;

        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateStorage(StorageType storageType, string fileName, string fileUrl,
                                string contentType, long fileSize)
    {
        StorageType = storageType;
        FileName = fileName;
        FileUrl = fileUrl;
        ContentType = contentType;
        FileSize = fileSize;

        UpdatedAt = DateTime.UtcNow;
    }
    public string GetFileExtension()
    {
        return Path.GetExtension(FileName).ToLowerInvariant();
    }

    public bool IsImage()
    {
        var extension = GetFileExtension();
        return new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" }.Contains(extension);
    }

    public bool IsDocument()
    {
        var extension = GetFileExtension();
        return new[] { ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".txt" }.Contains(extension);
    }
}