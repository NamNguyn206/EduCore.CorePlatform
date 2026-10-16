using EduCore.CorePlatform.Domain.Entities.System.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Infrastructure.Persistence.Configurations.System;

public class SystemMessageConfiguration : IEntityTypeConfiguration<SystemMessage>
{
    public void Configure(EntityTypeBuilder<SystemMessage> builder)
    {
        builder.ToTable("SystemMessages");

        builder.HasKey(e => e.Id);
        builder.HasIndex(x => x.Code).IsUnique();

    }
}
