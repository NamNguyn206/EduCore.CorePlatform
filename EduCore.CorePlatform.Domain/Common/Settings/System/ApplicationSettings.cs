using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using EduCore.CorePlatform.Domain.Common.Constants;
using EduCore.CorePlatform.Domain.Common.Settings.Auth;

namespace EduCore.CorePlatform.Domain.Common.Settings.System;

public class ApplicationSettings
{
    [JsonPropertyName("DomainName")]
    public string DomainName { get; set; } = string.Empty;

    [JsonPropertyName("Version")]
    public string Version { get; set; } = "1.0.0";

    [JsonPropertyName("Environment")]
    public string Environment { get; set; } = "Development";

    [JsonPropertyName("DefaultLanguage")]
    public string DefaultLanguage { get; set; } = "vi";

    [JsonPropertyName("DefaultTimeZone")]
    public string DefaultTimeZone { get; set; } = "SE Asia Standard Time";

    [JsonPropertyName("DefaultPageSize")]
    public int DefaultPageSize { get; set; } = 10;

    [JsonPropertyName("MaxPageSize")]
    public int MaxPageSize { get; set; } = 100;

    [JsonPropertyName("EnableDetailedErrors")]
    public bool EnableDetailedErrors { get; set; } = false;

    [JsonPropertyName("Jwt")]
    public JwtSettings Jwt { get; set; } = new JwtSettings();

    [JsonPropertyName("FileStorage")]
    public FileStorageSettings FileStorage { get; set; } = new FileStorageSettings();

    [JsonPropertyName("AuditLog")]
    public AuditLogSettings AuditLog { get; set; } = new AuditLogSettings();

    [JsonPropertyName("DataTypeFormatSettings")]
    public DataTypeFormatSettings DataTypeFormatSettings { get; set; } = new DataTypeFormatSettings();
}
