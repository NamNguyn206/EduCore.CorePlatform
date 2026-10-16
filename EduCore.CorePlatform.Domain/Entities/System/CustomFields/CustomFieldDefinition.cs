using EduCore.CorePlatform.Domain.Events.Domain.CustomField;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using EduCore.CorePlatform.Domain.Common.Events;
using EduCore.CorePlatform.Domain.Common.Entities;
using EduCore.CorePlatform.Domain.Common.Enums.System;
using EduCore.CorePlatform.Domain.Common.Constants.System;

namespace EduCore.CorePlatform.Domain.Entities.System.CustomFields;

public class CustomFieldDefinition : MultiTenantEntity<Guid, Guid>
{
    private readonly List<DomainEvent> _domainEvents = new();
    public string FieldCode { get; private set; } = string.Empty;
    public string DisplayName { get; private set; } = string.Empty;
    public FieldDataType DataType { get; private set; }
    public string? Description { get; private set; }
    public string EntityType { get; private set; } = string.Empty;
    public bool IsRequired { get; private set; }
    public bool IsUnique { get; private set; }
    public bool IsActive { get; private set; } = true;
    public bool IsSystemField { get; private set; }
    public string? DefaultValue { get; private set; }
    public string? ValidationRules { get; private set; }
    public int DisplayOrder { get; private set; }
    public string? GroupName { get; private set; }
    public string? Options { get; private set; } // JSON array of options for Option/MultiOption types
    public int? MinLength { get; private set; }
    public int? MaxLength { get; private set; }
    public decimal? MinValue { get; private set; }
    public decimal? MaxValue { get; private set; }
    public string? RegularExpression { get; private set; }
    public string? ErrorMessage { get; private set; }
    public string? ReferenceEntity { get; private set; }
    public string? ReferenceField { get; private set; }

    // Navigation properties
    public virtual ICollection<CustomFieldOption> FieldOptions { get; private set; } = new List<CustomFieldOption>();
    public virtual ICollection<CustomFieldValue> FieldValues { get; private set; } = new List<CustomFieldValue>();

    // Domain events
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected CustomFieldDefinition() { }

    public CustomFieldDefinition(Guid tenantId, string fieldCode, string displayName,
                               FieldDataType dataType, string entityType, string? description = null)
        : base(Guid.NewGuid(), tenantId)
    {
        FieldCode = fieldCode.ToLowerInvariant().Replace(" ", "_");
        DisplayName = displayName;
        DataType = dataType;
        EntityType = entityType;
        Description = description;
        CreatedAt = DateTime.UtcNow;

        RaiseDomainEvent(new CustomFieldDefinitionCreatedEvent(
            Id, FieldCode, EntityType, TenantId));
    }

    public void Update(string displayName, string? description, bool isRequired, bool isUnique,
                      string? defaultValue, string? validationRules, int displayOrder, string? groupName)
    {
        DisplayName = displayName;
        Description = description;
        IsRequired = isRequired;
        IsUnique = isUnique;
        DefaultValue = defaultValue;
        ValidationRules = validationRules;
        DisplayOrder = displayOrder;
        GroupName = groupName;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetValidationRules(int? minLength, int? maxLength, decimal? minValue, decimal? maxValue,
                                  string? regularExpression, string? errorMessage)
    {
        MinLength = minLength;
        MaxLength = maxLength;
        MinValue = minValue;
        MaxValue = maxValue;
        RegularExpression = regularExpression;
        ErrorMessage = errorMessage;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetReference(string referenceEntity, string referenceField)
    {
        ReferenceEntity = referenceEntity;
        ReferenceField = referenceField;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetOptions(List<CustomFieldOption> options)
    {
        FieldOptions.Clear();
        foreach (var option in options)
        {
            option.SetFieldDefinition(this);
            FieldOptions.Add(option);
        }
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

    public void MarkAsSystemField()
    {
        IsSystemField = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public bool ValidateValue(object? value)
    {
        if (IsRequired && (value == null || (value is string str && string.IsNullOrWhiteSpace(str))))
            return false;

        if (value == null)
            return true;

        // Type validation
        try
        {
            switch (DataType)
            {
                case FieldDataType.Integer:
                    Convert.ToInt32(value);
                    break;
                case FieldDataType.Decimal:
                    Convert.ToDecimal(value);
                    break;
                case FieldDataType.Boolean:
                    Convert.ToBoolean(value);
                    break;
                case FieldDataType.DateTime:
                case FieldDataType.Date:
                    Convert.ToDateTime(value);
                    break;
                case FieldDataType.Email:
                    var email = value.ToString();
                    if (!string.IsNullOrEmpty(email) && !global::System.Text.RegularExpressions.Regex.IsMatch(
                        email, ValidationConstants.EmailRegex))
                        return false;
                    break;
            }
        }
        catch
        {
            return false;
        }

        // Length validation
        if (value is string stringValue)
        {
            if (MinLength.HasValue && stringValue.Length < MinLength.Value)
                return false;
            if (MaxLength.HasValue && stringValue.Length > MaxLength.Value)
                return false;
        }

        // Value range validation
        if (MinValue.HasValue || MaxValue.HasValue)
        {
            decimal numericValue;
            if (value is decimal dec)
                numericValue = dec;
            else if (value is int i)
                numericValue = i;
            else if (value is decimal d)
                numericValue = (decimal)d;
            else if (value is float f)
                numericValue = (decimal)f;
            else
                return true;

            if (MinValue.HasValue && numericValue < MinValue.Value)
                return false;
            if (MaxValue.HasValue && numericValue > MaxValue.Value)
                return false;
        }

        // Regex validation
        if (!string.IsNullOrEmpty(RegularExpression) && value is string regexValue)
        {
            if (!global::System.Text.RegularExpressions.Regex.IsMatch(regexValue, RegularExpression))
                return false;
        }

        return true;
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