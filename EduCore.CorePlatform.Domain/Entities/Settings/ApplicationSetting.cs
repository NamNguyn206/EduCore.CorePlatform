using EduCore.CorePlatform.Domain.Common.Constants;
using EduCore.CorePlatform.Domain.Common.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace EduCore.CorePlatform.Domain.Entities.Settings;

public class DomainSetting : MultiTenantEntity<Guid, Guid>
{
    public string Key { get; private set; } = string.Empty;    
    public string Value { get; private set; } = string.Empty;
    public string? Type { get; private set; }
    public string? Description { get; private set; }
    public bool IsPublic { get; private set; }
    public bool IsReadOnly { get; private set; }
    public bool IsEncrypted { get; private set; }
    public string? Group { get; private set; }
    public int Order { get; private set; }

    // Custom fields
    public Dictionary<string, object> CustomFields { get; set; } = new();

    protected DomainSetting() { }

    public DomainSetting(Guid tenantId, string key, string value, string? type = "string",
                             string? description = null, bool isPublic = false, bool isReadOnly = false,
                             string? group = null, int order = 0)
        : base(Guid.NewGuid(), tenantId)
    {
        Key = key;
        Value = value;
        Type = type;
        Description = description;
        IsPublic = isPublic;
        IsReadOnly = isReadOnly;
        Group = group;
        Order = order;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateValue(string value)
    {
        if (IsReadOnly)
            throw new InvalidOperationException("Cannot update read-only setting");

        Value = value;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(string? description, string? group, int order, bool isPublic)
    {
        Description = description;
        Group = group;
        Order = order;
        IsPublic = isPublic;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsEncrypted()
    {
        IsEncrypted = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public T? GetValue<T>()
    {
        if (IsEncrypted)
            throw new InvalidOperationException("Value is encrypted, decrypt first");

        try
        {
            if (typeof(T) == typeof(string))
                return (T)(object)Value;

            return JsonSerializer.Deserialize<T>(Value);
        }
        catch
        {
            return default;
        }
    }
}