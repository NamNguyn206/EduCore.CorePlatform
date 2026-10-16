using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Domain.Common.Entities;


namespace EduCore.CorePlatform.Domain.Entities.MultiTenants.Categories;

public class Manufacturer : MultiTenantEntity<Guid, Guid>
{
    public string Name { get; private set; } = default!;

    protected Manufacturer() { }

    public Manufacturer(Guid tenantId, string name)
    {
        Name = name;
        SetTenant(tenantId);
    }

    public void Update(string name)
    {
        Name = name;
        UpdatedAt = DateTime.UtcNow;
    }
}
