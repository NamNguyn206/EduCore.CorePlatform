using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Abstractions.Authentication;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Domain.Common.Constants.Localization;
using EduCore.CorePlatform.Domain.Common.Constants.System;

namespace EduCore.CorePlatform.Infrastructure.Localization;

public sealed class CurrentLanguage : ICurrentLanguage
{
    public string LanguageCode { get; }

    public CurrentLanguage(
        IHttpContextAccessor httpContextAccessor,
        ICurrentUser currentUser
    )
    {
        var context = httpContextAccessor.HttpContext;

        // 1. Ưu tiên Accept-Language header
        var headerLang = context?.Request
                                 .Headers[RequestHeaderTypeConstants.AcceptLanguage]
                                 .FirstOrDefault();

        if (!string.IsNullOrWhiteSpace(headerLang))
        {
            LanguageCode = headerLang
                .Split(',')
                .First()
                .Split('-')
                .First();

            return;
        }

        // 2. Language từ user
        if (!string.IsNullOrWhiteSpace(currentUser.LanguageCode))
        {
            LanguageCode = currentUser.LanguageCode!;
            return;
        }

        // 3. Default system language
        LanguageCode = LanguageCodeConstants.Vi;
    }
}