using EduCore.CorePlatform.Domain.Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace EduCore.CorePlatform.Domain.Entities.MultiTenants.Categories;

public class SupplyCategory : MultiTenantEntity<Guid, Guid>
{
    public string Name { get; private set; } = default!;
    public ICollection<Supplier>? Suppliers { get; private set; }

    protected SupplyCategory() { }

    public SupplyCategory(Guid tenantId, string name)
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
