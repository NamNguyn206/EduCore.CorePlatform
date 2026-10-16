using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Domain.Entities.MultiTenants.Categories;

namespace EduCore.CorePlatform.Infrastructure.Persistence.Configurations.MultiTenants.Categories;

public class SupplierConfiguration
    : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
        builder.ToTable("Suppliers");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasIndex(x => new { x.Name, x.TenantId })
            .IsUnique();

        builder.HasOne(x => x.SupplyCategory)
            .WithMany(x => x.Suppliers)
            .HasForeignKey(x => x.SupplyCategoryId);

        builder.HasOne(x => x.Bank)
            .WithMany(x => x.Suppliers)
            .HasForeignKey(x => x.BankId);
    }
}
