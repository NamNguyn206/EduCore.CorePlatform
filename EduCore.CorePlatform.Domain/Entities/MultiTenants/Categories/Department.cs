using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Domain.Common.Entities;

namespace EduCore.CorePlatform.Domain.Entities.MultiTenants.Categories;

public class Department : MultiTenantEntity<Guid, Guid>
{
    public string Name { get; private set; } = default!;
    public string? Address { get; private set; }

    //navigational properties
    
    protected Department() { }

    public Department(Guid tenantId, string name, string address)
    {
        Name = name;
        Address = address;
        SetTenant(tenantId);        
    }

    public void Update(string name, string address)
    {
        Name = name;
        Address = address;
        UpdatedAt = DateTime.UtcNow;
    }
}