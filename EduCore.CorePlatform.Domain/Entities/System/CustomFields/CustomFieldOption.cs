using EduCore.CorePlatform.Domain.Common.Constants;
using EduCore.CorePlatform.Domain.Common.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EduCore.CorePlatform.Domain.Entities.System.CustomFields;

public class CustomFieldOption : BaseEntity<Guid>
{    
    public Guid FieldDefinitionId { get; private set; }
    public string Value { get; private set; } = string.Empty;
    public string DisplayText { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public int DisplayOrder { get; private set; }
    public bool IsActive { get; private set; } = true;
    public bool IsDefault { get; private set; }
    public string? Color { get; private set; }
    public string? Icon { get; private set; }

    // Navigation properties
    public virtual CustomFieldDefinition FieldDefinition { get; private set; } = null!;

    protected CustomFieldOption() { }

    public CustomFieldOption(Guid fieldDefinitionId, string value, string displayText,
                           int displayOrder = 0, string? description = null, bool isDefault = false,
                           string? color = null, string? icon = null)
    {
        Id = Guid.NewGuid();
        FieldDefinitionId = fieldDefinitionId;
        Value = value;
        DisplayText = displayText;
        DisplayOrder = displayOrder;
        Description = description;
        IsDefault = isDefault;
        Color = color;
        Icon = icon;
    }

    internal void SetFieldDefinition(CustomFieldDefinition fieldDefinition)
    {
        FieldDefinition = fieldDefinition;
        FieldDefinitionId = fieldDefinition.Id;
    }

    public void Update(string displayText, string? description, int displayOrder,
                      string? color, string? icon)
    {
        DisplayText = displayText;
        Description = description;
        DisplayOrder = displayOrder;
        Color = color;
        Icon = icon;
    }

    public void MarkAsDefault()
    {
        IsDefault = true;
    }

    public void UnmarkAsDefault()
    {
        IsDefault = false;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}