using EduCore.CorePlatform.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Infrastructure.Persistence.Configurations.Identities;

public class UserSystemRoleConfiguration : IEntityTypeConfiguration<UserSystemRole>
{
    public void Configure(EntityTypeBuilder<UserSystemRole> builder)
    {
        builder.ToTable("UserSystemRoles");

        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.User)
                .WithMany(x => x.SystemRoles)
                .HasForeignKey(x => x.UserId);
        
        builder.HasOne(x => x.Role)
                .WithMany(x => x.SystemRoles)
                .HasForeignKey(x => x.RoleId);

    }
}
