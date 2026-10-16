using EduCore.CorePlatform.Domain.Common;
using EduCore.CorePlatform.Domain.Common.Entities;
using EduCore.CorePlatform.Domain.Common.Enums.System;
using EduCore.CorePlatform.Domain.Entities.Localization;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace EduCore.CorePlatform.Domain.Entities.System.Messages;

public class SystemMessage : BaseEntity<Guid>
{
    public string Code { get; private set; } = default!; 
    public SystemMessageType Type { get; private set; }

    private readonly List<SystemMessageTranslation> _translations = new();
    public IReadOnlyCollection<SystemMessageTranslation> Translations => _translations;

    protected SystemMessage() { }

    public SystemMessage(string code, SystemMessageType type)
    {
        Code = code;
        Type = type;
    }
    public static SystemMessage Create(
            string code,
            SystemMessageType type)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Code is required", nameof(code));

   
        return new SystemMessage(code.Trim(), type);
    }

    // =======================
    // BEHAVIORS
    // =======================
    public void ChangeType(SystemMessageType newType)
    {
        if (Type == newType)
            return;

        Type = newType;
    }
    public void AddTranslation(string languageCode, string content)
    {
        _translations.Add(SystemMessageTranslation.Create(Id, languageCode, content));
    }

    public void UpsertTranslation(string languageCode, string content)
    {
        var existing = _translations.FirstOrDefault(x => x.LanguageCode == languageCode);
        if (existing == null)
            AddTranslation(languageCode, content);
        else
            existing.Update(content);
    }
}
