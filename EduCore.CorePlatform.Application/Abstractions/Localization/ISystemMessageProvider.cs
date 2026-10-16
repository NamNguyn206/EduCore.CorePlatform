using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Abstractions.Localization;

public interface ISystemMessageProvider
{
    Task<string> GetAsync(string code, string? languageCode = null, CancellationToken cancellationToken = default);
}
