using EduCore.CorePlatform.Domain.Entities.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Infrastructure.Persistence.Configurations.Localizztion;

public class SystemMessageTranslationConfiguration : IEntityTypeConfiguration<SystemMessageTranslation>
{
    public void Configure(EntityTypeBuilder<SystemMessageTranslation> builder)
    {
        builder.ToTable("SystemMessageTranslations");

        builder.HasKey(e => e.Id);

        builder.HasOne(x => x.SystemMessage).WithMany(x => x.Translations).HasForeignKey(x => x.SystemMessageId);
    }
}
