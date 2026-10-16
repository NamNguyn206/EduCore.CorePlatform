using EduCore.CorePlatform.Domain.Events.Domain.CustomField;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using EduCore.CorePlatform.Domain.Common.Events;
using EduCore.CorePlatform.Domain.Common.Entities;
using EduCore.CorePlatform.Domain.Common.Enums.System;

namespace EduCore.CorePlatform.Domain.Entities.System.CustomFields;

public class CustomFieldValue : MultiTenantEntity<Guid, Guid>
{
    private readonly List<DomainEvent> _domainEvents = new();
    public Guid FieldDefinitionId { get; private set; }
    public string EntityType { get; private set; } = string.Empty;
    public Guid EntityId { get; private set; }
    public string? StringValue { get; private set; }
    public int? IntValue { get; private set; }
    public decimal? DecimalValue { get; private set; }
    public bool? BoolValue { get; private set; }
    public DateTime? DateTimeValue { get; private set; }
    public string? JsonValue { get; private set; }
    public Guid? ReferenceValue { get; private set; }
    public string? MultiOptionValue { get; private set; } // JSON array for multi-select

    // Navigation properties
    public virtual CustomFieldDefinition FieldDefinition { get; private set; } = null!;

    protected CustomFieldValue() { }

    public CustomFieldValue(Guid tenantId, Guid fieldDefinitionId, string entityType, Guid entityId)
        : base(Guid.NewGuid(), tenantId)
    {
        FieldDefinitionId = fieldDefinitionId;
        EntityType = entityType;
        EntityId = entityId;
        CreatedAt = DateTime.UtcNow;
    }

    public void SetValue(object? value)
    {
        ClearAllValues();

        if (value == null)
            return;

        var fieldDefinition = FieldDefinition;

        switch (fieldDefinition.DataType)
        {
            case FieldDataType.String:
            case FieldDataType.Email:
            case FieldDataType.Phone:
            case FieldDataType.Url:
            case FieldDataType.Color:
            case FieldDataType.RichText:
                StringValue = value.ToString();
                break;

            case FieldDataType.Integer:
                IntValue = Convert.ToInt32(value);
                break;

            case FieldDataType.Decimal:
                DecimalValue = Convert.ToDecimal(value);
                break;

            case FieldDataType.Boolean:
                BoolValue = Convert.ToBoolean(value);
                break;

            case FieldDataType.DateTime:
            case FieldDataType.Date:
            case FieldDataType.Time:
                DateTimeValue = Convert.ToDateTime(value);
                break;

            case FieldDataType.Json:
                JsonValue = JsonSerializer.Serialize(value);
                break;

            case FieldDataType.Reference:
                ReferenceValue = (Guid)value;
                break;

            case FieldDataType.Option:
                StringValue = value.ToString();
                break;

            case FieldDataType.MultiOption:
                MultiOptionValue = JsonSerializer.Serialize(value);
                break;

            case FieldDataType.File:
            case FieldDataType.Image:
                StringValue = value.ToString(); // Store file path or URL
                break;
        }

        UpdatedAt = DateTime.UtcNow;

        RaiseDomainEvent(new CustomFieldValueChangedEvent(
            Id, EntityType, EntityId, fieldDefinition.FieldCode, TenantId));
    }

    public object? GetValue()
    {
        var fieldDefinition = FieldDefinition;

        return fieldDefinition.DataType switch
        {
            FieldDataType.String or FieldDataType.Email or FieldDataType.Phone
            or FieldDataType.Url or FieldDataType.Color or FieldDataType.RichText
            or FieldDataType.Option => StringValue,

            FieldDataType.Integer => IntValue,
            FieldDataType.Decimal => DecimalValue,
            FieldDataType.Boolean => BoolValue,
            FieldDataType.DateTime or FieldDataType.Date or FieldDataType.Time => DateTimeValue,
            FieldDataType.Json => JsonValue != null ?
                JsonDocument.Parse(JsonValue) : null,
            FieldDataType.Reference => ReferenceValue,
            FieldDataType.MultiOption => MultiOptionValue != null ?
                JsonSerializer.Deserialize<string[]>(MultiOptionValue) : null,
            FieldDataType.File or FieldDataType.Image => StringValue,
            _ => null
        };
    }

    private void ClearAllValues()
    {
        StringValue = null;
        IntValue = null;
        DecimalValue = null;
        BoolValue = null;
        DateTimeValue = null;
        JsonValue = null;
        ReferenceValue = null;
        MultiOptionValue = null;
    }

    // Domain events
    public void RaiseDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void RemoveDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}