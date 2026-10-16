using EduCore.CorePlatform.Domain.Common.Constants;
using EduCore.CorePlatform.Domain.Common.Entities;
using EduCore.CorePlatform.Domain.Common.Enums.Notifications;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace EduCore.CorePlatform.Domain.Entities.Notifications;

public class NotificationTemplate : MultiTenantEntity<Guid, Guid>
{
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public string Subject { get; private set; } = string.Empty;
    public string Body { get; private set; } = string.Empty;
    public string? BodyHtml { get; private set; }    
    public NotificationType Type { get; private set; }
    public string Channel { get; private set; } = "Email"; // Email, SMS, Push, InApp
    public string? Variables { get; private set; } // JSON array of variable names
    public bool IsActive { get; private set; } = true;
    public bool IsSystemTemplate { get; private set; }
    public string? LanguageCode { get; private set; } = "vi-VN";

    // Navigation properties
    public virtual ICollection<Notification> Notifications { get; private set; } = new List<Notification>();

    protected NotificationTemplate() { }

    public NotificationTemplate(Guid tenantId, string code, string name, string subject,
                               string body, NotificationType type, string channel = "Email",
                               string? description = null, string? bodyHtml = null,
                               string? languageCode = "vi-VN")
        : base(Guid.NewGuid(), tenantId)
    {
        Code = code;
        Name = name;
        Subject = subject;
        Body = body;
        BodyHtml = bodyHtml;
        Type = type;
        Channel = channel;
        Description = description;
        LanguageCode = languageCode;
        CreatedAt = DateTime.UtcNow;
        IsActive = true;
    }

    public void Update(string name, string subject, string body, string? bodyHtml,
                      string? description, string? languageCode)
    {
        Name = name;
        Subject = subject;
        Body = body;
        BodyHtml = bodyHtml;
        Description = description;
        LanguageCode = languageCode;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetVariables(List<string> variableNames)
    {
        Variables = JsonSerializer.Serialize(variableNames);
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsSystemTemplate()
    {
        IsSystemTemplate = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public string FormatBody(Dictionary<string, string> variables)
    {
        var formattedBody = Body;

        foreach (var variable in variables)
        {
            formattedBody = formattedBody.Replace($"{{{variable.Key}}}", variable.Value);
        }

        return formattedBody;
    }

    public string FormatSubject(Dictionary<string, string> variables)
    {
        var formattedSubject = Subject;

        foreach (var variable in variables)
        {
            formattedSubject = formattedSubject.Replace($"{{{variable.Key}}}", variable.Value);
        }

        return formattedSubject;
    }

    public List<string> GetVariables()
    {
        if (string.IsNullOrEmpty(Variables))
            return new List<string>();

        return JsonSerializer.Deserialize<List<string>>(Variables) ?? new List<string>();
    }
}