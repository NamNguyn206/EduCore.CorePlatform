using EduCore.CorePlatform.Domain.Entities.MultiTenants.Identities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Infrastructure.Persistence.Configurations.MultiTenants.Identities;

public class UserTenantRoleConfiguration : IEntityTypeConfiguration<UserTenantRole>
{
    public void Configure(EntityTypeBuilder<UserTenantRole> builder)
    {
        builder.ToTable("UserTenantRoles");
        builder.HasKey(x => x.Id);

      
        builder.HasOne(x => x.Role)
            .WithMany(x => x.UserTenantRoles)
            .HasForeignKey(x => x.RoleId);

        builder.HasOne(x => x.UserTenant)
            .WithMany(x => x.Roles)
            .HasForeignKey(x => x.UserTenantId);
    }
}