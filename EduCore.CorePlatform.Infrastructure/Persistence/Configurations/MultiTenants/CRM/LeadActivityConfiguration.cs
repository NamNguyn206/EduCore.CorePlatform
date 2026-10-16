using EduCore.CorePlatform.Domain.Entities.MultiTenants.CRM;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Infrastructure.Persistence.Configurations.MultiTenants.CRM;

public class LeadActivityConfiguration : IEntityTypeConfiguration<LeadActivity>
{
    public void Configure(EntityTypeBuilder<LeadActivity> builder)
    {
        builder.ToTable("LeadActivity");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.LeadId)
            .IsRequired();

        builder.Property(x => x.ActivityType)
            .IsRequired();

        builder.Property(x => x.Details)
            .HasMaxLength(255);
        
        builder.Property(x => x.OccurredAt)
            .IsRequired();

        builder.HasOne(x => x.Lead)
            .WithMany(x => x.Activities)
            .HasForeignKey(x => x.LeadId);
    }
}