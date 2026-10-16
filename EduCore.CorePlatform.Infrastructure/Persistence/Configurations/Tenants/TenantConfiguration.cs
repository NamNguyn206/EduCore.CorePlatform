using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Domain.Entities.Localization;
using EduCore.CorePlatform.Domain.Entities.MultiTenants;

namespace EduCore.CorePlatform.Infrastructure.Persistence.Configurations.Tenants;

public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.ToTable("Tenants");

        builder.HasKey(e => e.Id);
        builder.HasIndex(x => x.Code).IsUnique();

        builder.HasOne(x => x.DefaultLanguage)
            .WithMany(x => x.Tenants)
            .HasForeignKey(x => x.DefaultLanguageId);
    }
}