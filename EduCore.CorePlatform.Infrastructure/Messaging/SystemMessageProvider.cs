using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Abstractions.Authentication;
using EduCore.CorePlatform.Domain.Specifications.System.SystemMessages;
using EduCore.CorePlatform.Domain.Common.Constants.Localization;
using EduCore.CorePlatform.Domain.Common.Constants.System;
using EduCore.CorePlatform.Domain.Entities.System.Messages;
using EduCore.CorePlatform.Application.Abstractions.Persistence;
using EduCore.CorePlatform.Application.Abstractions.Localization;

namespace EduCore.CorePlatform.Infrastructure.Messaging;

public sealed class SystemMessageProvider : ISystemMessageProvider
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public SystemMessageProvider(IUnitOfWork unitOfWork, 
                                ICurrentUser currentUser,
                                IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<string> GetAsync(string code, string? languageCode = null,
                                        CancellationToken cancellationToken = default)
    {
        var language = ResolveLanguage(languageCode);

        var spec = new SystemMessageByCodeWithLanguageSpec(code);

        var message = await _unitOfWork.Repository<SystemMessage>()
                                        .FirstOrDefaultAsync(spec, cancellationToken);

        if (message == null)
            return code;

        var translation = message.Translations
                                .FirstOrDefault(t => t.LanguageCode == language);

        return translation?.Content ?? code;
    }
    private string ResolveLanguage(string? languageCode)
    {
        // 1️ Ưu tiên languageCode truyền vào
        if (!string.IsNullOrWhiteSpace(languageCode))
            return languageCode;

        // 2️ Accept-Language header
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext is null)
            return string.Empty;

        // 1️ Ưu tiên Accept-Language header
        var headerLang = httpContext?.Request.Headers[RequestHeaderTypeConstants.AcceptLanguage].FirstOrDefault();

        if (!string.IsNullOrWhiteSpace(headerLang))
        {
            return headerLang.Split(',').First().Split('-').First();
        }

        // 3️ Language của user
        if (!string.IsNullOrWhiteSpace(_currentUser.LanguageCode))
            return _currentUser.LanguageCode;

        // 4️ Default system language
        return LanguageCodeConstants.Vi;
    }
}
