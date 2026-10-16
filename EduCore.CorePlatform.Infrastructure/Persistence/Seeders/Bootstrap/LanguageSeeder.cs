using DocumentFormat.OpenXml.InkML;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Domain.Common.Enums;
using EduCore.CorePlatform.Domain.Common.Settings;
using EduCore.CorePlatform.Domain.Entities.Identity;
using EduCore.CorePlatform.Domain.Entities.Localization;
using EduCore.CorePlatform.Domain.Entities.MultiTenants;
using EduCore.CorePlatform.Infrastructure.Persistence.DbContext;

namespace EduCore.CorePlatform.Infrastructure.Persistence.Seeders.Bootstrap;

public sealed class LanguageSeeder : IDataSeeder
{
    public int Order => 4;
    private readonly ApplicationDbContext _db;
    private readonly ILogger<LanguageSeeder> _logger;

    public LanguageSeeder(
        ApplicationDbContext db,
        ILogger<LanguageSeeder> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        var languages = new List<(string Code, string Name, bool IsDefault)>
        {
            ("vi-VN", "Vietnamese", true),
            ("en-US", "English", false),
            ("fr-FR", "French", false)
        };

        foreach (var lang in languages)
        {
            var existing = await _db.Languages.FirstOrDefaultAsync(x => x.Code == lang.Code);

            if (existing == null)
            {
                _db.Languages.Add(new Language(
                    lang.Code,
                    lang.Name,
                    lang.IsDefault
                ));
            }
            else
            {
                existing.Update(
                    lang.Code,
                    lang.Name, 
                    lang.IsDefault
                );
            }
        }

        await _db.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Languages bootstrap completed successfully");
    }
}
