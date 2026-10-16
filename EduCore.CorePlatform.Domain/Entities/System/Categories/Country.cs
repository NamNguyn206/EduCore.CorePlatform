using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Domain.Common.Entities;


namespace EduCore.CorePlatform.Domain.Entities.System.Categories;

public class Country: AggregateRoot<Guid>
{
    public string Name { get; private set; } = default!;

    protected Country() { }

    public Country(string name)
    {
        Name = name;
    }

    public void Update(string name)
    {
        Name = name;
        UpdatedAt = DateTime.UtcNow;
    }
}
