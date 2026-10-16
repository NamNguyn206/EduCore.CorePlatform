using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Domain.Entities.System.Files;

namespace EduCore.CorePlatform.Infrastructure.Persistence.Configurations.System;

public class FileStorageConfiguration : IEntityTypeConfiguration<FileStorage>
{
    public void Configure(EntityTypeBuilder<FileStorage> builder)
    {
        builder.ToTable("FileStorages");

        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Notification)
                .WithMany(x => x.FileAttachments)
                .HasForeignKey(x => x.NotificationId);
    }
}