using EduCore.CorePlatform.Domain.Entities.MultiTenants.Identities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Infrastructure.Persistence.Configurations.MultiTenants.Identities;

public sealed class UserTenantConfiguration : IEntityTypeConfiguration<UserTenant>
{
    public void Configure(EntityTypeBuilder<UserTenant> builder)
    {
        builder.ToTable("UserTenants");

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.TenantId)
            .IsRequired();

        builder.HasIndex(x => new { x.UserId, x.TenantId })
            .IsUnique();

        builder.HasOne(x => x.User)
            .WithMany(x => x.UserTenants)
            .HasForeignKey(x => x.UserId);

        builder.HasOne(x => x.Tenant)
            .WithMany(x => x.UserTenants)
            .HasForeignKey(x => x.TenantId);
    }
}
