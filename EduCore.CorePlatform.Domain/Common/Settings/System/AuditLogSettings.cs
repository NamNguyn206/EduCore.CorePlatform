using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace EduCore.CorePlatform.Domain.Common.Settings.System;

public class AuditLogSettings
{
    [JsonPropertyName("Enabled")]
    public bool Enabled { get; set; } = true;

    [JsonPropertyName("SaveToDatabase")]
    public bool SaveToDatabase { get; set; } = true;

    [JsonPropertyName("SaveToFile")]
    public bool SaveToFile { get; set; } = false;

    [JsonPropertyName("FilePath")]
    public string FilePath { get; set; } = "logs/audit";

    [JsonPropertyName("LogSuccessfulRequests")]
    public bool LogSuccessfulRequests { get; set; } = true;

    [JsonPropertyName("LogFailedRequests")]
    public bool LogFailedRequests { get; set; } = true;

    [JsonPropertyName("MaskSensitiveData")]
    public bool MaskSensitiveData { get; set; } = true;

    [JsonPropertyName("SensitiveFields")]
    public List<string> SensitiveFields { get; set; } = new()
        {
            "password",
            "token",
            "secret",
            "creditcard",
            "cvv"
        };
}