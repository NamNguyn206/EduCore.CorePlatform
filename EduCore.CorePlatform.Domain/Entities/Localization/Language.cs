using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Text;
using EduCore.CorePlatform.Domain.Common.Entities;
using EduCore.CorePlatform.Domain.Common.Enums;
using EduCore.CorePlatform.Domain.Entities.Identity;
using EduCore.CorePlatform.Domain.Entities.MultiTenants;
using EduCore.CorePlatform.Domain.Events.Domain.Localization;

namespace EduCore.CorePlatform.Domain.Entities.Localization;

public class Language : AggregateRoot<Guid>
{
    public string Code { get; private set; } = default!;
    public string Name { get; private set; } = default!;
    public bool IsDefault { get; private set; }

    //Navigation properties
    public virtual ICollection<Tenant>? Tenants { get; private set; } = new List<Tenant>();
    public virtual ICollection<User>? Users { get; private set; } = new List<User>();

    private Language() { }

    public Language(string code, string name, bool isDefault = false, Guid? createdByUserId = null)
    {
        Code = code;
        Name = name;
        IsDefault = isDefault;
        CreatedBy = createdByUserId;

        RaiseDomainEvent(new LanguageCreatedEvent(Id, code, name, createdByUserId));
    }
    public void Update(
        string code, 
        string name, 
        bool isDefault = false, 
        Guid? updatedByUserId = null
    )
    {
        Code = code;
        Name = name;
        UpdatedBy = updatedByUserId;
        UpdatedAt = DateTime.UtcNow;

        RaiseDomainEvent(new LanguageUpdatedEvent(Id, updatedByUserId));
    }
}
