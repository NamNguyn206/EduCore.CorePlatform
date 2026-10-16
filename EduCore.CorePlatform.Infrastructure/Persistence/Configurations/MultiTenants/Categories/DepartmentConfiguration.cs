using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EduCore.CorePlatform.Domain.Entities.MultiTenants.Categories;

namespace EduCore.CorePlatform.Infrastructure.Persistence.Configurations.MultiTenants.Categories;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("Departments");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.Address)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasIndex(x => new { x.TenantId, x.Name, x.Address })
            .IsUnique();

    }
}