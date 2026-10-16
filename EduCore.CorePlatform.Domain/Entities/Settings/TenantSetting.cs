using EduCore.CorePlatform.Domain.Common.Constants;
using EduCore.CorePlatform.Domain.Common.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace EduCore.CorePlatform.Domain.Entities.Settings;

public class TenantSetting : MultiTenantEntity<Guid, Guid>
{
    public string Key { get; private set; } = string.Empty;
    public string? Value { get; private set; }
    public string? DataType { get; private set; }
    public string? Description { get; private set; }
    public bool IsEncrypted { get; private set; }
    public bool IsReadOnly { get; private set; }
    public bool IsVisible { get; private set; } = true;
    public string? Category { get; private set; }
    public int DisplayOrder { get; private set; }

    // Custom fields
    //public Dictionary<string, object> CustomFields { get; set; } = new();

    protected TenantSetting() { }

    public TenantSetting(Guid tenantId, string key, string? value, string? dataType = "string",
                         string? description = null, bool isEncrypted = false, bool isReadOnly = false,
                         string? category = null, int displayOrder = 0)
        : base(Guid.NewGuid(), tenantId)
    {
        Key = key;
        Value = value;
        DataType = dataType;
        Description = description;
        IsEncrypted = isEncrypted;
        IsReadOnly = isReadOnly;
        Category = category;
        DisplayOrder = displayOrder;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateValue(string? value)
    {
        if (IsReadOnly)
            throw new InvalidOperationException("Cannot update read-only setting");

        Value = value;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(string? description, string? category, int displayOrder, bool isVisible)
    {
        Description = description;
        Category = category;
        DisplayOrder = displayOrder;
        IsVisible = isVisible;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsEncrypted()
    {
        IsEncrypted = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsReadOnly()
    {
        IsReadOnly = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public T? GetValue<T>()
    {
        if (string.IsNullOrEmpty(Value))
            return default;

        try
        {
            return JsonSerializer.Deserialize<T>(Value);
        }
        catch
        {
            return default;
        }
    }

    public void SetValue<T>(T value)
    {
        Value = JsonSerializer.Serialize(value);
        UpdatedAt = DateTime.UtcNow;
    }
}