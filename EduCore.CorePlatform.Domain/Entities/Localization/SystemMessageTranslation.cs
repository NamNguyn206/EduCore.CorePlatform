using EduCore.CorePlatform.Domain.Common.Entities;
using EduCore.CorePlatform.Domain.Common.Enums;
using EduCore.CorePlatform.Domain.Entities.System.Messages;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace EduCore.CorePlatform.Domain.Entities.Localization;

public class SystemMessageTranslation : BaseEntity<Guid>
{
    public Guid SystemMessageId { get; private set; }
    public string LanguageCode { get; private set; } = default!;
    public string Content { get; private set; } = default!;

    public SystemMessage SystemMessage { get; private set; } = default!;
    private SystemMessageTranslation() { }

    public SystemMessageTranslation(Guid systemMessageId, string languageCode, string content)
    {
        SystemMessageId = systemMessageId;
        LanguageCode = languageCode;
        Content = content;
    }
    public static SystemMessageTranslation Create(Guid systemMessageId, string languageCode, string content)
    {
        return new SystemMessageTranslation
        {
            SystemMessageId = systemMessageId,
            LanguageCode = languageCode,
            Content = content
        };
    }

    /// <summary>
    /// Cập nhật nội dung message (DDD chuẩn)
    /// </summary>
    public void Update(string content)
    {
        Content = content;
    }
}
