using EduCore.CorePlatform.Domain.Common.Constants;
using EduCore.CorePlatform.Domain.Common.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EduCore.CorePlatform.Domain.Entities.Settings;

public class LanguageText : BaseEntity<Guid>
{
    public string LanguageCode { get; private set; } = string.Empty;
    public string Key { get; private set; } = string.Empty;
    public string Value { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public string? Module { get; private set; }
    public string? Category { get; private set; }
    public bool IsActive { get; private set; } = true;
    public bool IsSystemText { get; private set; }
    public Guid? TenantId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public Guid? CreatedBy { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public Guid? UpdatedBy { get; private set; }

    protected LanguageText() { }

    public LanguageText(string languageCode, string key, string value, string? description = null,
                       string? module = null, string? category = null, bool isSystemText = false,
                       Guid? tenantId = null, Guid? createdBy = null)
    {
        Id = Guid.NewGuid();
        LanguageCode = languageCode;
        Key = key;
        Value = value;
        Description = description;
        Module = module;
        Category = category;
        IsSystemText = isSystemText;
        TenantId = tenantId;
        CreatedAt = DateTime.UtcNow;
        CreatedBy = createdBy;
    }

    public void Update(string value, string? description)
    {
        Value = value;
        Description = description;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsSystemText()
    {
        IsSystemText = true;
        UpdatedAt = DateTime.UtcNow;
    }
}